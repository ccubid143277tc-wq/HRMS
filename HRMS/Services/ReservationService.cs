using HRMS.Helper;
using HRMS.Interfaces;
using HRMS.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

namespace HRMS.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IRoomService _roomService;
        private readonly IGuestService _guestService;
        private readonly IRoomTypeService _roomTypeService;

        public ReservationService()
        {
            _roomService = new RoomService();
            _guestService = new GuestService();
            _roomTypeService = new RoomTypeService();
        }

        public int AddReservation(Reservation reservation)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO reservations 
                                (GuestID, Check_InDate, Check_OutDate, NumAdult, NumChildren, 
                                 SpecialRequest, ReservationStatus, RoomID, numberOfNights) 
                                VALUES (@GuestID, @Check_InDate, @Check_OutDate, @NumAdult, @NumChildren, 
                                        @SpecialRequest, @ReservationStatus, @RoomID, @numberOfNights);
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
                    cmd.Parameters.AddWithValue("@RoomID", reservation.RoomID);
                    cmd.Parameters.AddWithValue("@numberOfNights", (reservation.Check_OutDate - reservation.Check_InDate).Days);

                    int newReservationId = Convert.ToInt32(cmd.ExecuteScalar());
                    return newReservationId;
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
                                    ReservationStatus=@ReservationStatus, RoomID=@RoomID, numberOfNights=@numberOfNights
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
                string query = "DELETE FROM reservations WHERE ReservationID=@ReservationID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ReservationID", reservationId);
                    cmd.ExecuteNonQuery();
                }
            }
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
                                       r.numberOfNights,
                                       g.FirstName, g.LastName, rm.RoomNumber, rt.RoomType
                                 FROM reservations r
                                 LEFT JOIN Guest g ON r.GuestID = g.GuestID
                                 LEFT JOIN Rooms rm ON r.RoomID = rm.RoomID
                                 LEFT JOIN RoomType rt ON rm.RoomTypeID = rt.RoomTypeID
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

        private Reservation MapReaderToReservation(MySqlDataReader reader)
        {
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
                RoomID = Convert.ToInt32(reader["RoomID"]),
                GuestName = reader["FirstName"] != DBNull.Value && reader["LastName"] != DBNull.Value 
                    ? $"{reader["FirstName"]} {reader["LastName"]}" 
                    : "Unknown Guest",
                RoomNumber = reader["RoomNumber"]?.ToString() ?? "Unknown",
                RoomTypeName = reader["RoomType"]?.ToString() ?? "Unknown"
            };
        }
    }
}
