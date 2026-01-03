using HRMS.Helper;
using HRMS.Interfaces;
using HRMS.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace HRMS.Services
{
    public class RoleService : IRoleService
    {
        public IEnumerable<Role> GetAllRoles()
        {
            var roles = new List<Role>();

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT RoleID, RoleName FROM Roles ORDER BY RoleName";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(new Role
                            {
                                RoleID = Convert.ToInt32(reader["RoleID"]),
                                RoleName = reader["RoleName"].ToString()
                            });
                        }
                    }
                }
            }

            return roles;
        }

        public Role GetRoleById(int roleId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT RoleID, RoleName FROM Roles WHERE RoleID = @RoleID";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoleID", roleId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Role
                            {
                                RoleID = Convert.ToInt32(reader["RoleID"]),
                                RoleName = reader["RoleName"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}
