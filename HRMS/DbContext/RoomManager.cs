using HRMS.Helper;
using HRMS.Interfaces;
using HRMS.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HRMS.DbContext
{
    public class MySqlRoomDbContext : IRoomService
    {
        public int AddRoom(Room room)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcAddRoom", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_RoomNumber", room.RoomNumber);
                    cmd.Parameters.AddWithValue("p_RoomTypeID", room.RoomType);
                    cmd.Parameters.AddWithValue("p_BedConfiguration", room.BedConfiguration);
                    cmd.Parameters.AddWithValue("p_MaximumOccupancy", room.MaximumOccupancy);
                    cmd.Parameters.AddWithValue("p_RoomFloor", room.RoomFloor);
                    cmd.Parameters.AddWithValue("p_RoomStatusID", room.RoomStatusID);
                    cmd.Parameters.AddWithValue("p_ViewType", room.ViewType);
                    cmd.Parameters.AddWithValue("p_RoomRate", room.RoomRate);

                    int newRoomId = Convert.ToInt32(cmd.ExecuteScalar());
                    return newRoomId;
                }
            }
        }

        public void UpdateRoom(Room room)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcUpdateRoom", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_RoomID", room.RoomID);
                    cmd.Parameters.AddWithValue("p_RoomNumber", room.RoomNumber);
                    cmd.Parameters.AddWithValue("p_RoomTypeID", room.RoomType);
                    cmd.Parameters.AddWithValue("p_BedConfiguration", room.BedConfiguration);
                    cmd.Parameters.AddWithValue("p_MaximumOccupancy", room.MaximumOccupancy);
                    cmd.Parameters.AddWithValue("p_RoomFloor", room.RoomFloor);
                    cmd.Parameters.AddWithValue("p_RoomStatusID", room.RoomStatusID);
                    cmd.Parameters.AddWithValue("p_ViewType", room.ViewType);
                    cmd.Parameters.AddWithValue("p_RoomRate", room.RoomRate);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteRoom(int roomId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcDeleteRoom", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_RoomID", roomId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Room GetRoomById(int roomId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetRoomById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_RoomID", roomId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapRoom(reader);
                        }
                    }
                }
            }
            return null;
        }

        public IEnumerable<Room> SearchRooms(string keyword)
        {
            var rooms = new List<Room>();
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();

                using (var cmd = new MySqlCommand("prcSearchRooms", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_SearchTerm", keyword);
                    cmd.Parameters.AddWithValue("p_RoomTypeID", 0);
                    cmd.Parameters.AddWithValue("p_RoomStatusID", 0);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rooms.Add(new Room
                            {
                                RoomID = Convert.ToInt32(reader["RoomID"]),
                                RoomNumber = reader["RoomNumber"].ToString(),
                                BedConfiguration = reader["BedConfiguration"].ToString(),
                                MaximumOccupancy = Convert.ToInt32(reader["MaximumOccupancy"]),
                                RoomFloor = Convert.ToInt32(reader["RoomFloor"]),
                                RoomRate = Convert.ToDecimal(reader["RoomRate"]),
                                ViewType = reader["ViewType"].ToString(),
                                RoomTypeName = reader["RoomType"].ToString(),
                                RoomStatusName = reader["RoomStatus"].ToString(),
                                AmenitiesString = reader["Amenities"].ToString()
                            });
                        }
                    }
                }
            }
            return rooms;
        }

        public IEnumerable<Room> FilterRooms(int? roomTypeId, int? roomStatusId, decimal? minRate, decimal? maxRate)
        {
            var rooms = new List<Room>();
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcFilterRooms", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_RoomTypeID", roomTypeId ?? 0);
                    cmd.Parameters.AddWithValue("p_RoomStatusID", roomStatusId ?? 0);
                    cmd.Parameters.AddWithValue("p_MinRate", minRate.HasValue ? (object)minRate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("p_MaxRate", maxRate.HasValue ? (object)maxRate.Value : DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rooms.Add(MapRoom(reader));
                        }
                    }
                }
            }
            return rooms;
        }

        public IEnumerable<Room> GetAllRooms()
        {
            var rooms = new List<Room>();
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetAllRoomsDetails", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rooms.Add(MapRoom(reader));
                        }
                    }
                }
            }
            return rooms;
        }

        public void AddRoomAmenity(int roomId, int amenityId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO RoomAmenities (RoomID, AmenitiesID) VALUES (@RoomID, @AmenitiesID)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoomID", roomId);
                    cmd.Parameters.AddWithValue("@AmenitiesID", amenityId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<int> GetRoomAmenities(int roomId)
        {
            var amenities = new List<int>();
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT AmenitiesID FROM RoomAmenities WHERE RoomID = @RoomID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoomID", roomId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            amenities.Add(Convert.ToInt32(reader["AmenitiesID"]));
                        }
                    }
                }
            }
            return amenities;
        }

        public DataTable GetRoomGridData()
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetRoomGridData", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        var table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }

        public IEnumerable<RoomType> GetRoomTypes()
        {
            var roomTypes = new List<RoomType>();

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetRoomTypes", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roomTypes.Add(new RoomType
                            {
                                RoomTypeID = Convert.ToInt32(reader["RoomTypeID"]),
                                RoomTypeName = reader["RoomType"].ToString(),
                            });
                        }
                    }
                }
            }

            return roomTypes;
        }

        public IEnumerable<Room> GetAvailableRoomsByType(string roomTypeName)
        {
            var rooms = new List<Room>();

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetAvailableRoomsByType", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_RoomTypeName", roomTypeName);

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

        public bool IsRoomAvailable(int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcIsRoomAvailable", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_RoomID", roomId);
                    cmd.Parameters.AddWithValue("p_CheckInDate", checkInDate);
                    cmd.Parameters.AddWithValue("p_CheckOutDate", checkOutDate);

                    int conflictCount = Convert.ToInt32(cmd.ExecuteScalar());
                    return conflictCount == 0;
                }
            }
        }

        public bool UpdateRoomStatus(int roomId, string status)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcUpdateRoomStatus", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_RoomID", roomId);
                    cmd.Parameters.AddWithValue("p_StatusName", status);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public Dictionary<string, int> GetRoomStatusCounts()
        {
            var result = new Dictionary<string, int>();

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();

                var cmdTotal = new MySqlCommand("SELECT COUNT(*) FROM Rooms", conn);
                result["Total"] = Convert.ToInt32(cmdTotal.ExecuteScalar());

                var cmdAvailable = new MySqlCommand("SELECT COUNT(*) FROM Rooms WHERE RoomStatusID = 1", conn);
                result["Available"] = Convert.ToInt32(cmdAvailable.ExecuteScalar());

                var cmdOccupied = new MySqlCommand("SELECT COUNT(*) FROM Rooms WHERE RoomStatusID = 2", conn);
                result["Occupied"] = Convert.ToInt32(cmdOccupied.ExecuteScalar());

                var cmdMaintenance = new MySqlCommand("SELECT COUNT(*) FROM Rooms WHERE RoomStatusID = 3", conn);
                result["Maintenance"] = Convert.ToInt32(cmdMaintenance.ExecuteScalar());

                var cmdReserved = new MySqlCommand(@"SELECT COUNT(*) FROM Rooms WHERE RoomStatusID = 4", conn);
                result["Reserved"] = Convert.ToInt32(cmdReserved.ExecuteScalar());
            }

            return result;
        }

        public int GetOccupiedRoomCountByDate(DateTime date)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetOccupiedRoomCountByDate", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_Date", date.Date);
                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                    {
                        return 0;
                    }
                    return Convert.ToInt32(result);
                }
            }
        }

        public Dictionary<DateTime, int> GetWeeklyOccupiedRoomCounts(DateTime startDate, int days)
        {
            var result = new Dictionary<DateTime, int>();
            for (int i = 0; i < days; i++)
            {
                var day = startDate.Date.AddDays(i);
                result[day] = GetOccupiedRoomCountByDate(day);
            }
            return result;
        }

        public int GetExpectedArrivalsCount(DateTime date)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetCheckInCountByDate", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_Date", date.Date);
                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                    {
                        return 0;
                    }
                    return Convert.ToInt32(result);
                }
            }
        }

        public int GetExpectedDeparturesCount(DateTime date)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetCheckOutCountByDate", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_Date", date.Date);
                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                    {
                        return 0;
                    }
                    return Convert.ToInt32(result);
                }
            }
        }

        private Room MapRoom(MySqlDataReader reader)
        {
            return new Room
            {
                RoomID = Convert.ToInt32(reader["RoomID"]),
                RoomNumber = reader["RoomNumber"].ToString(),
                RoomType = Convert.ToInt32(reader["RoomTypeID"]),
                BedConfiguration = reader["BedConfiguration"].ToString(),
                MaximumOccupancy = Convert.ToInt32(reader["MaximumOccupancy"]),
                RoomFloor = Convert.ToInt32(reader["RoomFloor"]),
                RoomStatusID = Convert.ToInt32(reader["RoomStatusID"]),
                ViewType = reader["ViewType"].ToString(),
                RoomRate = Convert.ToDecimal(reader["RoomRate"])
            };
        }
    }
}
