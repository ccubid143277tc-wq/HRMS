using HRMS.Helper;
using HRMS.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace HRMS.Services
{
    public class RoomStatusService
    {
        public List<RoomStatus> GetAllRoomStatuses()
        {
            var statuses = new List<RoomStatus>();

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT RoomStatusID, RoomStatus FROM RoomStatus";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        statuses.Add(new RoomStatus
                        {
                            RoomStatusID = reader.GetInt32("RoomStatusID"),
                            RoomStatusName = reader.GetString("RoomStatus")
                        });
                    }
                }
            }

            return statuses;
        }
    }
}