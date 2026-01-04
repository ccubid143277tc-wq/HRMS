using HRMS.Helper;
using HRMS.Interfaces;
using HRMS.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace HRMS.Services
{
    public class RoomTypeService : IRoomTypeService
    {
        public IEnumerable<RoomType> GetAllRoomTypes()
        {
            var roomTypes = new List<RoomType>();

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT RoomTypeID, RoomType FROM RoomType";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roomTypes.Add(new RoomType
                        {
                            RoomTypeID = reader.GetInt32("RoomTypeID"),
                            RoomTypeName = reader.GetString("RoomType")
                        });
                    }
                }
            }

            return roomTypes;
        }

        public RoomType GetRoomTypeById(int roomTypeId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT RoomTypeID, RoomType FROM RoomType WHERE RoomTypeID = @RoomTypeID";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoomTypeID", roomTypeId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new RoomType
                            {
                                RoomTypeID = Convert.ToInt32(reader["RoomTypeID"]),
                                RoomTypeName = reader["RoomType"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}