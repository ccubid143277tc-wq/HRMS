using HRMS.Helper;
using HRMS.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace HRMS.Services
{
    public class RoomTypeService
    {
        public List<RoomType> GetAllRoomTypes()
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
    }
}