using HRMS.Helper;
using HRMS.Interfaces;
using HRMS.Models;
using MySql.Data.MySqlClient;


namespace HRMS.Services
{
    public class UserService : IUserService
    {
        public int AddUser(User user)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO Users 
                                (Username, PasswordHash, FirstName, LastName, Email, Phone, RoleID, User_Status, CreatedAt, UpdatedAt) 
                                VALUES (@Username, @PasswordHash, @FirstName, @LastName, @Email, @Phone, @RoleID, @User_Status, @CreatedAt, @UpdatedAt);
                                SELECT LAST_INSERT_ID();";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Phone", user.Phone);
                    cmd.Parameters.AddWithValue("@RoleID", user.RoleID);
                    cmd.Parameters.AddWithValue("@User_Status", user.UserStatus);
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                    int newUserId = Convert.ToInt32(cmd.ExecuteScalar());
                    return newUserId;
                }
            }
        }
      
        public void UpdateUser(User user)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"UPDATE Users 
                                SET FirstName=@FirstName, LastName=@LastName, Email=@Email, Phone=@Phone, 
                                    RoleID=@RoleID, User_Status=@User_Status, UpdatedAt=@UpdatedAt
                                WHERE UserID=@UserID";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Phone", user.Phone);
                    cmd.Parameters.AddWithValue("@RoleID", user.RoleID);
                    cmd.Parameters.AddWithValue("@User_Status", user.UserStatus);
                    cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(int userId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Users WHERE UserID=@UserID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public User GetUserById(int userId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"SELECT u.*, r.RoleName 
                                 FROM Users u 
                                 INNER JOIN Roles r ON u.RoleID = r.RoleID
                                 WHERE u.UserID=@UserID";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Username = reader["Username"].ToString(),
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
            }
            return null;
        }

        public IEnumerable<User> GetAllUsers()
        {
            var users = new List<User>();

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"SELECT u.*, r.RoleName 
                                 FROM Users u 
                                 INNER JOIN Roles r ON u.RoleID = r.RoleID";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Username = reader["Username"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                RoleID = Convert.ToInt32(reader["RoleID"]),
                                RoleName = reader["RoleName"].ToString(),
                                UserStatus = reader["User_Status"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                            });
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
                string query = @"SELECT u.*, r.RoleName 
                                 FROM Users u 
                                 INNER JOIN Roles r ON u.RoleID = r.RoleID
                                 ORDER BY u.CreatedAt DESC";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Username = reader["Username"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                RoleID = Convert.ToInt32(reader["RoleID"]),
                                RoleName = reader["RoleName"].ToString(),
                                UserStatus = reader["User_Status"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                            });
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
                string query = @"SELECT 
                                    COUNT(*) as Total,
                                    SUM(CASE WHEN User_Status = 'Active' THEN 1 ELSE 0 END) as Active,
                                    SUM(CASE WHEN User_Status = 'Inactive' THEN 1 ELSE 0 END) as Inactive,
                                    SUM(CASE WHEN User_Status = 'Suspended' THEN 1 ELSE 0 END) as Suspended
                                 FROM Users";

                using (var cmd = new MySqlCommand(query, conn))
                {
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
        public IEnumerable<User> SearchUser(string keyword)
        {
            var users = new List<User>();
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();

                string query = @"SELECT u.UserID,
                               u.Username,
                               u.FirstName,
                               u.LastName,
                               u.Email,
                               u.Phone,
                               u.User_Status,
                               u.CreatedAt,
                               u.UpdatedAt,
                               r.RoleName
                        FROM Users u
                        JOIN Roles r ON u.RoleID = r.RoleID
                        WHERE u.Username LIKE @Keyword
                           OR u.FirstName LIKE @Keyword
                           OR u.LastName LIKE @Keyword
                           OR u.Email LIKE @Keyword
                           OR u.Phone LIKE @Keyword
                           OR u.User_Status LIKE @Keyword
                           OR r.RoleName LIKE @Keyword
                        ORDER BY u.UserID;
                                    ";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Username = reader["Username"].ToString(),
                                FirstName = reader["Firstname"].ToString(),
                                LastName = reader["Lastname"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                RoleName = reader["RoleName"].ToString(),           
                                UserStatus = reader["User_Status"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])

                            });
                        }
                    }
                }
            }
            return users;
        }
    }
}


