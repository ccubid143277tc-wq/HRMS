using HRMS.Helper;
using HRMS.Interfaces;
using HRMS.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace HRMS.DbContext
{
    public class MySqlUserDbContext : IUserService
    {
        public int AddUser(User user)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcAddUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_Username", user.Username);
                    cmd.Parameters.AddWithValue("p_PasswordHash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("p_FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("p_LastName", user.LastName);
                    cmd.Parameters.AddWithValue("p_Email", user.Email);
                    cmd.Parameters.AddWithValue("p_Phone", user.Phone);
                    cmd.Parameters.AddWithValue("p_RoleID", user.RoleID);
                    cmd.Parameters.AddWithValue("p_User_Status", user.UserStatus);
                    cmd.Parameters.AddWithValue("p_CreatedAt", DateTime.Now);
                    cmd.Parameters.AddWithValue("p_UpdatedAt", DateTime.Now);

                    object result = cmd.ExecuteScalar();
                    return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
                }
            }
        }

        public void UpdateUser(User user)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();

                using (var cmd = new MySqlCommand("prcUpdateUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_UserID", user.UserID);
                    cmd.Parameters.AddWithValue("p_FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("p_LastName", user.LastName);
                    cmd.Parameters.AddWithValue("p_Email", user.Email);
                    cmd.Parameters.AddWithValue("p_Phone", user.Phone);
                    cmd.Parameters.AddWithValue("p_RoleID", user.RoleID);
                    cmd.Parameters.AddWithValue("p_User_Status", user.UserStatus);
                    cmd.Parameters.AddWithValue("p_UpdatedAt", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }

                if (!string.IsNullOrWhiteSpace(user.PasswordHash))
                {
                    using (var cmdPwd = new MySqlCommand("prcUpdateUserPassword", conn))
                    {
                        cmdPwd.CommandType = CommandType.StoredProcedure;
                        cmdPwd.Parameters.AddWithValue("p_UserID", user.UserID);
                        cmdPwd.Parameters.AddWithValue("p_PasswordHash", user.PasswordHash);
                        cmdPwd.Parameters.AddWithValue("p_UpdatedAt", DateTime.Now);
                        cmdPwd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void DeleteUser(int userId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcDeleteUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_UserID", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<User> SearchUser(string keyword)
        {
            var users = new List<User>();
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcSearchUsers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_Keyword", keyword ?? "");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(MapUser(reader));
                        }
                    }
                }
            }

            return users;
        }

        public User GetUserById(int userId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetUserById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_UserID", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapUser(reader);
                        }
                    }
                }
            }

            return null;
        }

        public IEnumerable<User> GetAllUsers()
        {
            var users = new List<User>();
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetAllUsers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(MapUser(reader));
                        }
                    }
                }
            }

            return users;
        }

        public IEnumerable<User> GetUserGridData()
        {
            var users = new List<User>();
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetUserGridData", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(MapUser(reader));
                        }
                    }
                }
            }

            return users;
        }

        public Dictionary<string, int> GetUserStatusCounts()
        {
            var counts = new Dictionary<string, int>
            {
                ["Total"] = 0,
                ["Active"] = 0,
                ["Inactive"] = 0,
                ["Suspended"] = 0
            };

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetUserStatusCounts", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            counts["Total"] = Convert.ToInt32(reader["Total"]);
                            counts["Active"] = Convert.ToInt32(reader["Active"]);
                            counts["Inactive"] = Convert.ToInt32(reader["Inactive"]);
                            counts["Suspended"] = Convert.ToInt32(reader["Suspended"]);
                        }
                    }
                }
            }

            return counts;
        }

        public User AuthenticateUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcAuthenticateUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_Username", username.Trim());
                    cmd.Parameters.AddWithValue("p_PasswordHash", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapUser(reader);
                        }
                    }
                }
            }

            return null;
        }

        private static User MapUser(MySqlDataReader reader)
        {
            return new User
            {
                UserID = Convert.ToInt32(reader["UserID"]),
                Username = reader["Username"].ToString(),
                PasswordHash = reader["PasswordHash"].ToString(),
                FirstName = reader["FirstName"].ToString(),
                LastName = reader["LastName"].ToString(),
                Email = reader["Email"].ToString(),
                Phone = reader["Phone"].ToString(),
                RoleID = Convert.ToInt32(reader["RoleID"]),
                RoleName = reader["RoleName"].ToString(),
                UserStatus = reader["User_Status"].ToString(),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
            };
        }
    }
}
