using HRMS.Helper;
using HRMS.Interfaces;
using HRMS.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HRMS.Services
{
    public class RoomService: IRoomService
    {
        public int AddRoom(Room room)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO Rooms (RoomNumber, RoomTypeID, BedConfiguration, MaximumOccupancy, RoomFloor, RoomStatusID, ViewType, RoomRate)
                                 VALUES (@RoomNumber, @RoomTypeID, @BedConfiguration, @MaximumOccupancy, @RoomFloor, @RoomStatusID, @ViewType, @RoomRate);
                                 SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                    cmd.Parameters.AddWithValue("@RoomTypeID", room.RoomType);
                    cmd.Parameters.AddWithValue("@BedConfiguration", room.BedConfiguration);
                    cmd.Parameters.AddWithValue("@MaximumOccupancy", room.MaximumOccupancy);
                    cmd.Parameters.AddWithValue("@RoomFloor", room.RoomFloor);
                    cmd.Parameters.AddWithValue("@RoomStatusID", room.RoomStatusID);
                    cmd.Parameters.AddWithValue("@ViewType", room.ViewType);
                    cmd.Parameters.AddWithValue("@RoomRate", room.RoomRate);
                    
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
                string query = @"UPDATE Rooms SET
                                 RoomNumber=@RoomNumber, RoomTypeID=@RoomTypeID, BedConfiguration=@BedConfiguration,
                                 MaximumOccupancy=@MaximumOccupancy, RoomFloor=@RoomFloor, RoomStatusID=@RoomStatusID,
                                 ViewType=@ViewType, RoomRate=@RoomRate
                                 WHERE RoomID=@RoomID";
                using (var  cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoomID", room.RoomID);
                    cmd.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                    cmd.Parameters.AddWithValue("@RoomTypeID", room.RoomType);
                    cmd.Parameters.AddWithValue("@BedConfiguration", room.BedConfiguration);
                    cmd.Parameters.AddWithValue("@MaximumOccupancy", room.MaximumOccupancy);
                    cmd.Parameters.AddWithValue("@RoomFloor", room.RoomFloor);
                    cmd.Parameters.AddWithValue("@RoomStatusID", room.RoomStatusID);
                    cmd.Parameters.AddWithValue("@ViewType", room.ViewType);
                    cmd.Parameters.AddWithValue("@RoomRate", room.RoomRate);
                    cmd.ExecuteNonQuery();

                }
            }
        }
        public void DeleteRoom(int roomId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                // if you have RoomAmenities, delete those first to avoid FK constraint issues
                using (var cmdAmenities = new MySqlCommand("DELETE FROM RoomAmenities WHERE RoomID=@RoomID", conn))
                {
                    cmdAmenities.Parameters.AddWithValue("@RoomID", roomId);
                    cmdAmenities.ExecuteNonQuery();
                }

                using (var cmd = new MySqlCommand("DELETE FROM Rooms WHERE RoomID=@RoomID", conn))
                {
                    cmd.Parameters.AddWithValue("@RoomID", roomId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Room GetRoomById(int roomId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "Select * FROM Rooms WHERE  RoomID=@RoomID";
                using (var cmd = new MySqlCommand (query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoomID", roomId);
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

                string query = @"SELECT r.RoomID, r.RoomNumber, r.BedConfiguration, r.MaximumOccupancy,
                                r.RoomFloor, r.RoomRate, r.ViewType,
                                rt.RoomType, rs.RoomStatus,
                                GROUP_CONCAT(a.Amenities SEPARATOR ', ') AS Amenities
                         FROM Rooms r
                         JOIN RoomType rt ON r.RoomTypeID = rt.RoomTypeID
                         JOIN RoomStatus rs ON r.RoomStatusID = rs.RoomStatusID
                         LEFT JOIN RoomAmenities ra ON r.RoomID = ra.RoomID
                         LEFT JOIN Amenities a ON ra.AmenitiesID = a.AmenitiesID
                         WHERE r.RoomNumber LIKE @Keyword
                            OR r.BedConfiguration LIKE @Keyword
                            OR r.ViewType LIKE @Keyword
                            OR rt.RoomType LIKE @Keyword
                            OR rs.RoomStatus LIKE @Keyword
                            OR a.Amenities LIKE @Keyword
                         GROUP BY r.RoomID";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

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
                string query = "SELECT * FROM Rooms WHERE 1=1";
                if (roomTypeId.HasValue) query += " AND RoomTypeID=@RoomTypeID";
                if (roomStatusId.HasValue) query += " AND RoomStatusID=@RoomStatusID";
                if (minRate.HasValue) query += " AND RoomRate>=@MinRate";
                if (maxRate.HasValue) query += " AND RoomRate<=@MaxRate";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    if (roomTypeId.HasValue) cmd.Parameters.AddWithValue("@RoomTypeID", roomTypeId.Value);
                    if (roomStatusId.HasValue) cmd.Parameters.AddWithValue("@RoomStatusID", roomStatusId.Value);
                    if (minRate.HasValue) cmd.Parameters.AddWithValue("@MinRate", minRate.Value);
                    if (maxRate.HasValue) cmd.Parameters.AddWithValue("@MaxRate", maxRate.Value);

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
                string query = "SELECT * FROM Rooms";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rooms.Add(MapRoom(reader));
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
        public DataTable GetRoomGridData()
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string sql = @"SELECT 
                                r.RoomID,  
                                r.RoomNumber,
                                rt.RoomType,
                                r.BedConfiguration,
                                r.MaximumOccupancy,
                                r.RoomFloor,
                                rs.RoomStatus,
                                r.ViewType,
                                r.RoomRate,
                                GROUP_CONCAT(a.Amenities SEPARATOR ', ') AS Amenities
                            FROM Rooms r
                            INNER JOIN RoomType rt ON r.RoomTypeID = rt.RoomTypeID
                            INNER JOIN RoomStatus rs ON r.RoomStatusID = rs.RoomStatusID
                            LEFT JOIN RoomAmenities ra ON r.RoomID = ra.RoomID
                            LEFT JOIN Amenities a ON ra.AmenitiesID = a.AmenitiesID
                            GROUP BY r.RoomID;";

                using (var cmd = new MySqlCommand(sql, conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
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

                
                var cmdReserved = new MySqlCommand(@"SELECT COUNT(*)
                                                    FROM Rooms
                                                    WHERE RoomStatusID = (SELECT RoomStatusID FROM RoomStatus WHERE RoomStatus = 'Reserved' LIMIT 1)", conn);
                result["Reserved"] = Convert.ToInt32(cmdReserved.ExecuteScalar());
            }

            return result;
        }

        public int GetOccupiedRoomCountByDate(DateTime date)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"SELECT COUNT(DISTINCT roomId) FROM (
                                    SELECT rr.RoomID AS roomId
                                    FROM reservations r
                                    INNER JOIN ReservationRooms rr ON r.ReservationID = rr.ReservationID
                                    WHERE r.ReservationStatus NOT IN ('Cancelled', 'Checked-Out')
                                      AND r.Check_InDate <= @Date
                                      AND r.Check_OutDate > @Date
                                    UNION
                                    SELECT r.RoomID AS roomId
                                    FROM reservations r
                                    WHERE r.ReservationStatus NOT IN ('Cancelled', 'Checked-Out')
                                      AND r.Check_InDate <= @Date
                                      AND r.Check_OutDate > @Date
                                ) x";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Date", date.Date);
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

                string query = @"SELECT COUNT(DISTINCT r.ReservationID)
                                 FROM reservations r
                                 WHERE r.ReservationStatus NOT IN ('Cancelled', 'Checked-Out')
                                   AND r.Check_InDate = @Date";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Date", date.Date);
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

                string query = @"SELECT COUNT(DISTINCT r.ReservationID)
                                 FROM reservations r
                                 WHERE r.ReservationStatus NOT IN ('Cancelled', 'Checked-Out')
                                   AND r.Check_OutDate = @Date";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Date", date.Date);
                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                    {
                        return 0;
                    }
                    return Convert.ToInt32(result);
                }
            }
        }

        public IEnumerable<RoomType> GetRoomTypes()
        {
            var roomTypes = new List<RoomType>();

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM RoomType ORDER BY RoomType";

                using (var cmd = new MySqlCommand(query, conn))
                {
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
                string query = @"SELECT r.*, rt.RoomType, rs.RoomStatus
                                 FROM Rooms r
                                 INNER JOIN RoomType rt ON r.RoomTypeID = rt.RoomTypeID
                                 INNER JOIN RoomStatus rs ON r.RoomStatusID = rs.RoomStatusID
                                 WHERE rt.RoomType = @RoomTypeName 
                                 AND rs.RoomStatus = 'Available'
                                 ORDER BY r.RoomNumber";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoomTypeName", roomTypeName);

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
                string query = @"SELECT COUNT(*) FROM reservations 
                                 WHERE RoomID = @RoomID 
                                 AND ReservationStatus NOT IN ('Cancelled', 'Checked-Out')
                                 AND ((Check_InDate <= @CheckIn AND Check_OutDate > @CheckIn) 
                                      OR (Check_InDate < @CheckOut AND Check_OutDate >= @CheckOut)
                                      OR (Check_InDate >= @CheckIn AND Check_OutDate <= @CheckOut))";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoomID", roomId);
                    cmd.Parameters.AddWithValue("@CheckIn", checkInDate);
                    cmd.Parameters.AddWithValue("@CheckOut", checkOutDate);

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
                string query = @"UPDATE Rooms 
                                SET RoomStatusID = (SELECT RoomStatusID FROM RoomStatus WHERE RoomStatus = @RoomStatus)
                                WHERE RoomID = @RoomID";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoomID", roomId);
                    cmd.Parameters.AddWithValue("@RoomStatus", status);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }


    }

}
