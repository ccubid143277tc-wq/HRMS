using HRMS.Helper;
using HRMS.Interfaces;
using HRMS.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace HRMS.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IRoomService _roomService;
        private readonly IGuestService _guestService;
        private readonly IRoomTypeService _roomTypeService;

        public ReservationService(IRoomService roomService, IGuestService guestService, IRoomTypeService roomTypeService)
        {
            _roomService = roomService;
            _guestService = guestService;
            _roomTypeService = roomTypeService;
        }

        public int AddReservation(Reservation reservation)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO reservations 
                                (GuestID, Check_InDate, Check_OutDate, NumAdult, NumChildren, 
                                 SpecialRequest, ReservationStatus, ReservationType, RoomID, numberOfNights, BookingReferences) 
                                VALUES (@GuestID, @Check_InDate, @Check_OutDate, @NumAdult, @NumChildren, 
                                        @SpecialRequest, @ReservationStatus, @ReservationType, @RoomID, @numberOfNights, @BookingReferences);
                                SELECT LAST_INSERT_ID();";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@GuestID", reservation.GuestID);
                    cmd.Parameters.AddWithValue("@Check_InDate", reservation.Check_InDate);
                    cmd.Parameters.AddWithValue("@Check_OutDate", reservation.Check_OutDate);
                    cmd.Parameters.AddWithValue("@NumAdult", reservation.NumAdult);
                    cmd.Parameters.AddWithValue("@NumChildren", reservation.NumChild);
                    cmd.Parameters.AddWithValue("@SpecialRequest", reservation.SpecialRequest ?? "");
                    cmd.Parameters.AddWithValue("@ReservationStatus", reservation.ReservationStatus ?? "Confirmed");
                    cmd.Parameters.AddWithValue("@ReservationType", reservation.ReservationType ?? "");
                    cmd.Parameters.AddWithValue("@RoomID", reservation.RoomID);
                    cmd.Parameters.AddWithValue("@numberOfNights", (reservation.Check_OutDate - reservation.Check_InDate).Days);
                    cmd.Parameters.AddWithValue("@BookingReferences", reservation.BookingReferences ?? "");
                    int newReservationId = Convert.ToInt32(cmd.ExecuteScalar());
                    return newReservationId;
                }
            }
        }

        public DataTable GetExpectedDeparturesGridData(DateTime date)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();

                string query = @"SELECT
                                    r.ReservationID,
                                    r.BookingReferences,
                                    CONCAT(g.FirstName, ' ', g.LastName) AS GuestName,
                                    GROUP_CONCAT(DISTINCT rm.RoomNumber ORDER BY rm.RoomNumber SEPARATOR ', ') AS RoomNumbers,
                                    rt.RoomType AS RoomType,
                                    r.Check_InDate,
                                    r.Check_OutDate,
                                    r.NumAdult,
                                    r.NumChildren,
                                    (COALESCE(r.NumAdult, 0) + COALESCE(r.NumChildren, 0)) AS Occupants,
                                    r.ReservationStatus
                                 FROM reservations r
                                 LEFT JOIN Guest g ON r.GuestID = g.GuestID
                                 LEFT JOIN ReservationRooms rr ON r.ReservationID = rr.ReservationID
                                 LEFT JOIN Rooms rm ON rm.RoomID = COALESCE(rr.RoomID, r.RoomID)
                                 LEFT JOIN RoomType rt ON rm.RoomTypeID = rt.RoomTypeID
                                 WHERE r.ReservationStatus NOT IN ('Cancelled', 'Checked-Out')
                                   AND DATE(r.Check_OutDate) = @Date
                                 GROUP BY r.ReservationID, r.BookingReferences, g.FirstName, g.LastName, rt.RoomType,
                                          r.Check_InDate, r.Check_OutDate, r.NumAdult, r.NumChildren, r.ReservationStatus
                                 ORDER BY r.Check_OutDate, GuestName";

                using (var cmd = new MySqlCommand(query, conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@Date", date.Date);
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public void UpdateReservation(Reservation reservation)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"UPDATE reservations 
                                SET GuestID=@GuestID, Check_InDate=@Check_InDate, Check_OutDate=@Check_OutDate, 
                                    NumAdult=@NumAdult, NumChildren=@NumChildren, SpecialRequest=@SpecialRequest, 
                                    ReservationStatus=@ReservationStatus, ReservationType=@ReservationType, RoomID=@RoomID, numberOfNights=@numberOfNights
                                WHERE ReservationID=@ReservationID";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ReservationID", reservation.ReservationID);
                    cmd.Parameters.AddWithValue("@GuestID", reservation.GuestID);
                    cmd.Parameters.AddWithValue("@Check_InDate", reservation.Check_InDate);
                    cmd.Parameters.AddWithValue("@Check_OutDate", reservation.Check_OutDate);
                    cmd.Parameters.AddWithValue("@NumAdult", reservation.NumAdult);
                    cmd.Parameters.AddWithValue("@NumChildren", reservation.NumChild);
                    cmd.Parameters.AddWithValue("@SpecialRequest", reservation.SpecialRequest);
                    cmd.Parameters.AddWithValue("@ReservationStatus", reservation.ReservationStatus);
                    cmd.Parameters.AddWithValue("@ReservationType", reservation.ReservationType ?? "");
                    cmd.Parameters.AddWithValue("@RoomID", reservation.RoomID);
                    cmd.Parameters.AddWithValue("@numberOfNights", (reservation.Check_OutDate - reservation.Check_InDate).Days);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteReservation(int reservationId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Get associated rooms BEFORE deleting links
                        var roomIds = new List<int>();
                        string getRoomIdsQuery = "SELECT RoomID FROM ReservationRooms WHERE ReservationID=@ReservationID";
                        using (var cmd = new MySqlCommand(getRoomIdsQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ReservationID", reservationId);
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    roomIds.Add(System.Convert.ToInt32(reader["RoomID"]));
                                }
                            }
                        }

                        // Fallback for older data (if junction table has no rows)
                        if (roomIds.Count == 0)
                        {
                            string getSingleRoomIdQuery = "SELECT RoomID FROM reservations WHERE ReservationID=@ReservationID";
                            using (var cmd = new MySqlCommand(getSingleRoomIdQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@ReservationID", reservationId);
                                object result = cmd.ExecuteScalar();
                                if (result != null && result != System.DBNull.Value)
                                {
                                    roomIds.Add(System.Convert.ToInt32(result));
                                }
                            }
                        }

                        // Update room statuses back to Available
                        foreach (int roomId in roomIds.Distinct())
                        {
                            string updateRoomStatusQuery = @"UPDATE Rooms 
                                SET RoomStatusID = (SELECT RoomStatusID FROM RoomStatus WHERE RoomStatus = @RoomStatus)
                                WHERE RoomID = @RoomID";
                            using (var cmd = new MySqlCommand(updateRoomStatusQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@RoomStatus", "Available");
                                cmd.Parameters.AddWithValue("@RoomID", roomId);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Delete from junction table
                        string deleteJunctionQuery = "DELETE FROM ReservationRooms WHERE ReservationID=@ReservationID";
                        using (var cmd = new MySqlCommand(deleteJunctionQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ReservationID", reservationId);
                            cmd.ExecuteNonQuery();
                        }

                        // Then delete from main reservation table
                        string deleteReservationQuery = "DELETE FROM reservations WHERE ReservationID=@ReservationID";
                        using (var cmd = new MySqlCommand(deleteReservationQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ReservationID", reservationId);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void AddReservationRooms(int reservationId, List<int> roomIds)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                foreach (int roomId in roomIds)
                {
                    string query = "INSERT INTO ReservationRooms (ReservationID, RoomID) VALUES (@ReservationID, @RoomID)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ReservationID", reservationId);
                        cmd.Parameters.AddWithValue("@RoomID", roomId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public List<int> GetRoomIdsByReservation(int reservationId)
        {
            var roomIds = new List<int>();
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT RoomID FROM ReservationRooms WHERE ReservationID = @ReservationID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ReservationID", reservationId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roomIds.Add(Convert.ToInt32(reader["RoomID"]));
                        }
                    }
                }
            }
            return roomIds;
        }

        public void CancelReservation(int reservationId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "UPDATE reservations SET ReservationStatus='Cancelled' WHERE ReservationID=@ReservationID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ReservationID", reservationId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Reservation> SearchReservation(string keyword)
        {
            var reservations = new List<Reservation>();

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"SELECT r.*, g.FirstName, g.LastName, rm.RoomNumber
                                 FROM reservations r
                                 LEFT JOIN Guest g ON r.GuestID = g.GuestID
                                 LEFT JOIN Rooms rm ON r.RoomID = rm.RoomID
                                 WHERE g.FirstName LIKE @Keyword 
                                    OR g.LastName LIKE @Keyword 
                                    OR g.Email LIKE @Keyword 
                                    OR g.PhoneNumber LIKE @Keyword 
                                    OR rm.RoomNumber LIKE @Keyword
                                    OR r.ReservationStatus LIKE @Keyword
                                 ORDER BY r.Check_InDate DESC";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reservations.Add(MapReaderToReservation(reader));
                        }
                    }
                }
            }

            return reservations;
        }

        public DataTable GetExpectedArrivalsGridData(DateTime date)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();

                string query = @"SELECT
                                    r.ReservationID,
                                    r.BookingReferences,
                                    CONCAT(g.FirstName, ' ', g.LastName) AS GuestName,
                                    GROUP_CONCAT(DISTINCT rm.RoomNumber ORDER BY rm.RoomNumber SEPARATOR ', ') AS RoomNumbers,
                                    rt.RoomType AS RoomType,
                                    r.Check_InDate,
                                    r.Check_OutDate,
                                    r.NumAdult,
                                    r.NumChildren,
                                    (COALESCE(r.NumAdult, 0) + COALESCE(r.NumChildren, 0)) AS Occupants,
                                    r.ReservationStatus
                                 FROM reservations r
                                 LEFT JOIN Guest g ON r.GuestID = g.GuestID
                                 LEFT JOIN ReservationRooms rr ON r.ReservationID = rr.ReservationID
                                 LEFT JOIN Rooms rm ON rm.RoomID = COALESCE(rr.RoomID, r.RoomID)
                                 LEFT JOIN RoomType rt ON rm.RoomTypeID = rt.RoomTypeID
                                 WHERE r.ReservationStatus NOT IN ('Cancelled', 'Checked-Out')
                                   AND DATE(r.Check_InDate) = @Date
                                 GROUP BY r.ReservationID, r.BookingReferences, g.FirstName, g.LastName, rt.RoomType,
                                          r.Check_InDate, r.Check_OutDate, r.NumAdult, r.NumChildren, r.ReservationStatus
                                 ORDER BY r.Check_InDate, GuestName";

                using (var cmd = new MySqlCommand(query, conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@Date", date.Date);
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        // Get reservation by ID with guest and room details
        public Reservation GetReservationById(int reservationId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();        
                string query = @"SELECT r.*, g.FirstName, g.LastName, g.Email, g.PhoneNumber, rm.RoomNumber, rm.RoomRate
                                 FROM reservations r
                                 LEFT JOIN Guest g ON r.GuestID = g.GuestID
                                 LEFT JOIN Rooms rm ON r.RoomID = rm.RoomID
                                 WHERE r.ReservationID=@ReservationID";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ReservationID", reservationId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var reservation = MapReaderToReservation(reader);
                            reservation.GuestName = $"{reader["FirstName"]} {reader["LastName"]}";
                            reservation.TotalAmount = CalculateReservationAmount(
                                reservation.RoomID, 
                                reservation.Check_InDate, 
                                reservation.Check_OutDate, 
                                reservation.NumAdult, 
                                reservation.NumChild
                            );
                            return reservation;
                        }
                    }
                }
            }

            return null;
        }

        public IEnumerable<Reservation> GetAllReservations()
        {
            var reservations = new List<Reservation>();

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"SELECT r.*, g.FirstName, g.LastName, rm.RoomNumber
                                 FROM reservations r
                                 LEFT JOIN Guest g ON r.GuestID = g.GuestID
                                 LEFT JOIN Rooms rm ON r.RoomID = rm.RoomID
                                 ORDER BY r.Check_InDate DESC";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reservations.Add(MapReaderToReservation(reader));
                        }
                    }
                }
            }

            return reservations;
        }

        public IEnumerable<Reservation> GetReservationGridData()
        {
            var reservations = new List<Reservation>();

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"SELECT r.ReservationID, r.GuestID, r.Check_InDate, r.Check_OutDate, 
                                       r.NumAdult, r.NumChildren, r.SpecialRequest, r.ReservationStatus, r.RoomID,
                                       r.numberOfNights, r.BookingReferences, r.ReservationType,
                                       g.FirstName, g.LastName, 
                                       GROUP_CONCAT(DISTINCT rm.RoomNumber ORDER BY rm.RoomNumber SEPARATOR ', ') AS RoomNumbers,
                                       rt.RoomType
                                 FROM reservations r
                                 LEFT JOIN Guest g ON r.GuestID = g.GuestID
                                 LEFT JOIN ReservationRooms rr ON r.ReservationID = rr.ReservationID
                                 LEFT JOIN Rooms rm ON rr.RoomID = rm.RoomID
                                 LEFT JOIN RoomType rt ON rm.RoomTypeID = rt.RoomTypeID
                                 GROUP BY r.ReservationID, r.GuestID, r.Check_InDate, r.Check_OutDate, 
                                          r.NumAdult, r.NumChildren, r.SpecialRequest, r.ReservationStatus, r.RoomID,
                                          r.numberOfNights, r.BookingReferences, g.FirstName, g.LastName, rt.RoomType
                                 ORDER BY r.Check_InDate DESC";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reservations.Add(MapReaderToReservation(reader));
                        }
                    }
                }
            }

            return reservations;
        }

        public Dictionary<string, int> GetReservationStatusCounts()
        {
            var counts = new Dictionary<string, int>
            {
                ["Total"] = 0,
                ["Confirmed"] = 0,
                ["Checked-In"] = 0,
                ["Checked-Out"] = 0,
                ["Cancelled"] = 0
            };

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"SELECT 
                                    COUNT(*) as Total,
                                    SUM(CASE WHEN ReservationStatus = 'Confirmed' THEN 1 ELSE 0 END) as Confirmed,
                                    SUM(CASE WHEN ReservationStatus = 'Checked-In' THEN 1 ELSE 0 END) as CheckedIn,
                                    SUM(CASE WHEN ReservationStatus = 'Checked-Out' THEN 1 ELSE 0 END) as CheckedOut,
                                    SUM(CASE WHEN ReservationStatus = 'Cancelled' THEN 1 ELSE 0 END) as Cancelled
                                 FROM reservations";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            counts["Total"] = Convert.ToInt32(reader["Total"]);
                            counts["Confirmed"] = Convert.ToInt32(reader["Confirmed"]);
                            counts["Checked-In"] = Convert.ToInt32(reader["CheckedIn"]);
                            counts["Checked-Out"] = Convert.ToInt32(reader["CheckedOut"]);
                            counts["Cancelled"] = Convert.ToInt32(reader["Cancelled"]);
                        }
                    }
                }
            }

            return counts;
        }

        public bool CheckRoomAvailability(int roomId, DateTime checkIn, DateTime checkOut)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"SELECT COUNT(*) FROM reservations 
                                 WHERE RoomID = @RoomID 
                                 AND ReservationStatus NOT IN ('Cancelled', 'Checked-Out')
                                 AND ((Check_InDate <= @CheckIn AND Check_OutDate > @CheckIn) 
                                      OR (Check_InDate < @CheckOut AND Check_OutDate >= @CheckOut)
                                      OR (Check_InDate >= @CheckIn AND Check_OutDate <= @CheckOut))";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoomID", roomId);
                    cmd.Parameters.AddWithValue("@CheckIn", checkIn);
                    cmd.Parameters.AddWithValue("@CheckOut", checkOut);

                    int conflictingReservations = Convert.ToInt32(cmd.ExecuteScalar());
                    return conflictingReservations == 0;
                }
            }
        }

        public decimal CalculateReservationAmount(int roomId, DateTime checkIn, DateTime checkOut, int numAdults, int numChildren)
        {
            try
            {
                var room = _roomService.GetRoomById(roomId);
                if (room == null) return 0;

                int totalDays = (checkOut - checkIn).Days;
                if (totalDays <= 0) return 0;

                decimal dailyRate = room.RoomRate;
                decimal totalAmount = dailyRate * totalDays;

                // Additional charges for extra guests (example logic)
                int totalGuests = numAdults + numChildren;
                if (totalGuests > room.MaximumOccupancy)
                {
                    // Add extra guest fee
                    int extraGuests = totalGuests - room.MaximumOccupancy;
                    totalAmount += extraGuests * 50; // $50 per extra guest per night
                }

                return totalAmount;
            }
            catch
            {
                return 0;
            }
        }

        public IEnumerable<Room> GetAvailableRoomsByType(int roomTypeId, DateTime checkIn, DateTime checkOut)
        {
            var availableRooms = new List<Room>();
            
            // Get all rooms of the specified type
            var allRooms = _roomService.GetAllRooms();
            var roomsOfType = allRooms.Where(r => r.RoomType == roomTypeId).ToList();
            
            // Check availability for each room
            foreach (var room in roomsOfType)
            {
                if (CheckRoomAvailability(room.RoomID, checkIn, checkOut))
                {
                    availableRooms.Add(room);
                }
            }
            
            return availableRooms;
        }

        public IEnumerable<RoomType> GetAllRoomTypes()
        {
            return _roomTypeService.GetAllRoomTypes();
        }

        private static bool HasColumn(MySqlDataReader reader, string columnName)
        {
            try
            {
                return reader.GetOrdinal(columnName) >= 0;
            }
            catch
            {
                return false;
            }
        }

        private Reservation MapReaderToReservation(MySqlDataReader reader)
        {
            string roomNumberValue = "Unknown";
            if (HasColumn(reader, "RoomNumbers"))
            {
                roomNumberValue = reader["RoomNumbers"]?.ToString() ?? "Unknown";
            }
            else if (HasColumn(reader, "RoomNumber"))
            {
                roomNumberValue = reader["RoomNumber"]?.ToString() ?? "Unknown";
            }

            string roomTypeValue = "Unknown";
            if (HasColumn(reader, "RoomType"))
            {
                roomTypeValue = reader["RoomType"]?.ToString() ?? "Unknown";
            }

            return new Reservation
            {
                ReservationID = Convert.ToInt32(reader["ReservationID"]),
                GuestID = Convert.ToInt32(reader["GuestID"]),
                Check_InDate = Convert.ToDateTime(reader["Check_InDate"]),
                Check_OutDate = Convert.ToDateTime(reader["Check_OutDate"]),
                NumAdult = Convert.ToInt32(reader["NumAdult"]),
                NumChild = reader["NumChildren"] != DBNull.Value ? Convert.ToInt32(reader["NumChildren"]) : 0,
                SpecialRequest = reader["SpecialRequest"].ToString(),
                ReservationStatus = reader["ReservationStatus"].ToString(),
                ReservationType = reader["ReservationType"]?.ToString() ?? "",
                BookingReferences = reader["BookingReferences"]?.ToString() ?? "",
                RoomID = Convert.ToInt32(reader["RoomID"]),
                GuestName = reader["FirstName"] != DBNull.Value && reader["LastName"] != DBNull.Value 
                    ? $"{reader["FirstName"]} {reader["LastName"]}" 
                    : "Unknown Guest",
                RoomNumber = roomNumberValue,
                RoomTypeName = roomTypeValue
            };
        }
    }
}
