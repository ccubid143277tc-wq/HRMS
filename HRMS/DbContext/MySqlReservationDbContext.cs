using HRMS.Helper;
using HRMS.Interfaces;
using HRMS.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace HRMS.DbContext
{
    public class MySqlReservationDbContext : IReservationService
    {
        public int AddReservation(Reservation reservation)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcAddReservation", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_GuestID", reservation.GuestID);
                    cmd.Parameters.AddWithValue("p_Check_InDate", reservation.Check_InDate);
                    cmd.Parameters.AddWithValue("p_Check_OutDate", reservation.Check_OutDate);
                    cmd.Parameters.AddWithValue("p_NumAdult", reservation.NumAdult);
                    cmd.Parameters.AddWithValue("p_NumChildren", reservation.NumChild);
                    cmd.Parameters.AddWithValue("p_SpecialRequest", reservation.SpecialRequest ?? "");
                    cmd.Parameters.AddWithValue("p_ReservationStatus", reservation.ReservationStatus ?? "Confirmed");
                    cmd.Parameters.AddWithValue("p_ReservationType", reservation.ReservationType ?? "");
                    cmd.Parameters.AddWithValue("p_RoomID", reservation.RoomID);
                    cmd.Parameters.AddWithValue("p_numberOfNights", (reservation.Check_OutDate - reservation.Check_InDate).Days);
                    cmd.Parameters.AddWithValue("p_BookingReferences", reservation.BookingReferences ?? "");

                    object result = cmd.ExecuteScalar();
                    return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
                }
            }
        }

        public void UpdateReservation(Reservation reservation)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcUpdateReservation", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_ReservationID", reservation.ReservationID);
                    cmd.Parameters.AddWithValue("p_GuestID", reservation.GuestID);
                    cmd.Parameters.AddWithValue("p_Check_InDate", reservation.Check_InDate);
                    cmd.Parameters.AddWithValue("p_Check_OutDate", reservation.Check_OutDate);
                    cmd.Parameters.AddWithValue("p_NumAdult", reservation.NumAdult);
                    cmd.Parameters.AddWithValue("p_NumChildren", reservation.NumChild);
                    cmd.Parameters.AddWithValue("p_SpecialRequest", reservation.SpecialRequest ?? "");
                    cmd.Parameters.AddWithValue("p_ReservationStatus", reservation.ReservationStatus ?? "Confirmed");
                    cmd.Parameters.AddWithValue("p_ReservationType", reservation.ReservationType ?? "");
                    cmd.Parameters.AddWithValue("p_RoomID", reservation.RoomID);
                    cmd.Parameters.AddWithValue("p_numberOfNights", (reservation.Check_OutDate - reservation.Check_InDate).Days);
                    cmd.Parameters.AddWithValue("p_BookingReferences", reservation.BookingReferences ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteReservation(int reservationId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcDeleteReservation", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_ReservationID", reservationId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CancelReservation(int reservationId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcCancelReservation", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_ReservationID", reservationId);
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
                using (var cmd = new MySqlCommand("prcSearchReservations", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_Keyword", keyword ?? "");

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
                using (var cmd = new MySqlCommand("prcGetReservationById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_ReservationID", reservationId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return null;
                        }

                        return MapReaderToReservation(reader);
                    }
                }
            }
        }

        public IEnumerable<Reservation> GetAllReservations()
        {
            var reservations = new List<Reservation>();
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetAllReservations", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
                using (var cmd = new MySqlCommand("prcGetReservationGridData", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
                using (var cmd = new MySqlCommand("prcGetExpectedArrivalsGridData", conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_Date", date.Date);
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public DataTable GetExpectedDeparturesGridData(DateTime date)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetExpectedDeparturesGridData", conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_Date", date.Date);
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public Dictionary<string, int> GetReservationStatusCounts()
        {
            var counts = new Dictionary<string, int>
            {
                ["Total"] = 0,
                ["Pending"] = 0,
                ["Confirmed"] = 0,
                ["Checked-In"] = 0,
                ["Checked-Out"] = 0,
                ["Cancelled"] = 0
            };

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetReservationStatusCounts", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
                using (var cmd = new MySqlCommand("prcCheckRoomAvailability", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_RoomID", roomId);
                    cmd.Parameters.AddWithValue("p_CheckInDate", checkIn);
                    cmd.Parameters.AddWithValue("p_CheckOutDate", checkOut);
                    object result = cmd.ExecuteScalar();
                    int conflictCount = result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
                    return conflictCount == 0;
                }
            }
        }

        public decimal CalculateReservationAmount(int roomId, DateTime checkIn, DateTime checkOut, int numAdults, int numChildren)
        {
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT RoomRate, MaximumOccupancy FROM Rooms WHERE RoomID = @RoomID";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RoomID", roomId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return 0m;
                            }

                            decimal roomRate = reader["RoomRate"] != DBNull.Value ? Convert.ToDecimal(reader["RoomRate"]) : 0m;
                            int maxOcc = reader["MaximumOccupancy"] != DBNull.Value ? Convert.ToInt32(reader["MaximumOccupancy"]) : 0;

                            int totalDays = (checkOut - checkIn).Days;
                            if (totalDays <= 0)
                            {
                                return 0m;
                            }

                            decimal total = roomRate * totalDays;

                            int totalGuests = numAdults + numChildren;
                            if (maxOcc > 0 && totalGuests > maxOcc)
                            {
                                int extraGuests = totalGuests - maxOcc;
                                total += extraGuests * 50m;
                            }

                            return total;
                        }
                    }
                }
            }
            catch
            {
                return 0m;
            }
        }

        public IEnumerable<Room> GetAvailableRoomsByType(int roomTypeId, DateTime checkIn, DateTime checkOut)
        {
            var rooms = new List<Room>();
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetAvailableRoomsByTypeIdDateRange", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_RoomTypeID", roomTypeId);
                    cmd.Parameters.AddWithValue("p_CheckInDate", checkIn);
                    cmd.Parameters.AddWithValue("p_CheckOutDate", checkOut);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rooms.Add(new Room
                            {
                                RoomID = Convert.ToInt32(reader["RoomID"]),
                                RoomNumber = reader["RoomNumber"].ToString(),
                                RoomType = Convert.ToInt32(reader["RoomTypeID"]),
                                BedConfiguration = reader["BedConfiguration"].ToString(),
                                MaximumOccupancy = Convert.ToInt32(reader["MaximumOccupancy"]),
                                RoomFloor = Convert.ToInt32(reader["RoomFloor"]),
                                RoomStatusID = Convert.ToInt32(reader["RoomStatusID"]),
                                ViewType = reader["ViewType"].ToString(),
                                RoomRate = Convert.ToDecimal(reader["RoomRate"]),
                                RoomTypeName = reader["RoomType"].ToString(),
                                RoomStatusName = reader["RoomStatus"].ToString()
                            });
                        }
                    }
                }
            }

            return rooms;
        }

        public IEnumerable<RoomType> GetAllRoomTypes()
        {
            var roomTypes = new List<RoomType>();
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetAllRoomTypesForReservation", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roomTypes.Add(new RoomType
                            {
                                RoomTypeID = Convert.ToInt32(reader["RoomTypeID"]),
                                RoomTypeName = reader["RoomType"].ToString()
                            });
                        }
                    }
                }
            }

            return roomTypes;
        }

        public void AddReservationRooms(int reservationId, List<int> roomIds)
        {
            if (roomIds == null || roomIds.Count == 0)
            {
                return;
            }

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                foreach (int roomId in roomIds)
                {
                    using (var cmd = new MySqlCommand("prcAddReservationRoom", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_ReservationID", reservationId);
                        cmd.Parameters.AddWithValue("p_RoomID", roomId);
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
                using (var cmd = new MySqlCommand("prcGetRoomIdsByReservation", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_ReservationID", reservationId);
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
                GuestName = HasColumn(reader, "FirstName") && HasColumn(reader, "LastName")
                    ? $"{reader["FirstName"]} {reader["LastName"]}".Trim()
                    : "",
                RoomNumber = roomNumberValue,
                RoomTypeName = roomTypeValue
            };
        }
    }
}
