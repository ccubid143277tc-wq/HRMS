using HRMS.Helper;
using HRMS.Interfaces;
using HRMS.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace HRMS.Services
{
    public class GuestService : IGuestService
    {
        public int AddGuest(Guest guest)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO Guest
                                (FirstName, LastName, Email, PhoneNumber, BirthDate, Address, 
                                 Nationality, IDTYPE, IDNumber, Classification) 
                                VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @BirthDate, @Address, 
                                        @Nationality, @IDTYPE, @IDNumber, @Classification);
                                SELECT LAST_INSERT_ID();";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", guest.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", guest.LastName);
                    cmd.Parameters.AddWithValue("@Email", guest.Email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", guest.PhoneNumber);
                    cmd.Parameters.AddWithValue("@BirthDate", guest.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Address", guest.Address);
                    cmd.Parameters.AddWithValue("@Nationality", guest.Nationality);
                    cmd.Parameters.AddWithValue("@IDTYPE", guest.IDType);
                    cmd.Parameters.AddWithValue("@IDNumber", guest.IDNumber);
                    cmd.Parameters.AddWithValue("@Classification", guest.Classification);

                    int newGuestId = Convert.ToInt32(cmd.ExecuteScalar());
                    return newGuestId;
                }
            }
        }

        public void UpdateGuest(Guest guest)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"UPDATE Guest 
                                SET FirstName=@FirstName, LastName=@LastName, Email=@Email, PhoneNumber=@PhoneNumber, 
                                    BirthDate=@BirthDate, Address=@Address, Nationality=@Nationality, 
                                    IDTYPE=@IDTYPE, IDNumber=@IDNumber, Classification=@Classification
                                WHERE GuestID=@GuestID";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@GuestID", guest.GuestID);
                    cmd.Parameters.AddWithValue("@FirstName", guest.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", guest.LastName);
                    cmd.Parameters.AddWithValue("@Email", guest.Email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", guest.PhoneNumber);
                    cmd.Parameters.AddWithValue("@BirthDate", guest.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Address", guest.Address);
                    cmd.Parameters.AddWithValue("@Nationality", guest.Nationality);
                    cmd.Parameters.AddWithValue("@IDTYPE", guest.IDType);
                    cmd.Parameters.AddWithValue("@IDNumber", guest.IDNumber);
                    cmd.Parameters.AddWithValue("@Classification", guest.Classification);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteGuest(int guestId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Guest WHERE GuestID=@GuestID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@GuestID", guestId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool HasReservationsForGuest(int guestId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();

                string query = "SELECT COUNT(*) FROM reservations WHERE GuestID = @GuestID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@GuestID", guestId);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public IEnumerable<Guest> SearchGuest(string keyword)
        {
            var guests = new List<Guest>();

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"SELECT * FROM Guest 
                                 WHERE FirstName LIKE @Keyword 
                                    OR LastName LIKE @Keyword 
                                    OR Email LIKE @Keyword 
                                    OR PhoneNumber LIKE @Keyword 
                                    OR IDNumber LIKE @Keyword
                                ";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            guests.Add(new Guest
                            {
                                GuestID = Convert.ToInt32(reader["GuestID"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["BirthDate"]),
                                Address = reader["Address"].ToString(),
                                Nationality = reader["Nationality"].ToString(),
                                IDType = reader["IDTYPE"].ToString(),
                                IDNumber = reader["IDNumber"].ToString(),
                                Classification = reader["Classification"].ToString(),
                                
                            });
                        }
                    }
                }
            }

            return guests;
        }

        public Guest GetGuestById(int guestId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Guest WHERE GuestID=@GuestID";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@GuestID", guestId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Guest
                            {
                                GuestID = Convert.ToInt32(reader["GuestID"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["BirthDate"]),
                                Address = reader["Address"].ToString(),
                                Nationality = reader["Nationality"].ToString(),
                                IDType = reader["IDTYPE"].ToString(),
                                IDNumber = reader["IDNumber"].ToString(),
                                Classification = reader["Classification"].ToString(),
                              
                            };
                        }
                    }
                }
            }

            return null;
        }

        public IEnumerable<Guest> GetAllGuests()
        {
            var guests = new List<Guest>();

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Guest ";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            guests.Add(new Guest
                            {
                                GuestID = Convert.ToInt32(reader["GuestID"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["BirthDate"]),
                                Address = reader["Address"].ToString(),
                                Nationality = reader["Nationality"].ToString(),
                                IDType = reader["IDTYPE"].ToString(),
                                IDNumber = reader["IDNumber"].ToString(),
                                Classification = reader["Classification"].ToString(),
                             
                            });
                        }
                    }
                }
            }

            return guests;
        }

        public IEnumerable<Guest> GetGuestGridData()
        {
            var guests = new List<Guest>();

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"SELECT GuestID, FirstName, LastName, Email, PhoneNumber, 
                                       IDNumber
                                 FROM Guest 
                                ";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            guests.Add(new Guest
                            {
                                GuestID = Convert.ToInt32(reader["GuestID"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                IDNumber = reader["IDNumber"].ToString(),
                               
                            });
                        }
                    }
                }
            }

            return guests;
        }

        public Dictionary<string, int> GetGuestStatusCounts()
        {
            var counts = new Dictionary<string, int>
            {
                ["Total"] = 0,
                ["Active"] = 0,
                ["Inactive"] = 0,
                ["Blacklisted"] = 0
            };

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"SELECT 
                                    COUNT(*) as Total,
                                    SUM(CASE WHEN GuestStatus = 'Active' THEN 1 ELSE 0 END) as Active,
                                    SUM(CASE WHEN GuestStatus = 'Inactive' THEN 1 ELSE 0 END) as Inactive,
                                    SUM(CASE WHEN GuestStatus = 'Blacklisted' THEN 1 ELSE 0 END) as Blacklisted
                                 FROM Guest";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            counts["Total"] = Convert.ToInt32(reader["Total"]);
                            counts["Active"] = Convert.ToInt32(reader["Active"]);
                            counts["Inactive"] = Convert.ToInt32(reader["Inactive"]);
                            counts["Blacklisted"] = Convert.ToInt32(reader["Blacklisted"]);
                        }
                    }
                }
            }

            return counts;
        }
    }
}
