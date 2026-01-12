using HRMS.Helper;
using HRMS.Interfaces;
using HRMS.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace HRMS.DbContext
{
    public class MySqlGuestDbContext : IGuestService
    {
        public int AddGuest(Guest guest)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcAddGuest", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_FirstName", guest.FirstName);
                    cmd.Parameters.AddWithValue("p_LastName", guest.LastName);
                    cmd.Parameters.AddWithValue("p_Email", guest.Email);
                    cmd.Parameters.AddWithValue("p_PhoneNumber", guest.PhoneNumber);
                    cmd.Parameters.AddWithValue("p_BirthDate", guest.DateOfBirth);
                    cmd.Parameters.AddWithValue("p_Address", guest.Address);
                    cmd.Parameters.AddWithValue("p_Nationality", guest.Nationality);
                    cmd.Parameters.AddWithValue("p_IDType", guest.IDType);
                    cmd.Parameters.AddWithValue("p_IDNumber", guest.IDNumber);
                    cmd.Parameters.AddWithValue("p_Classification", guest.Classification);

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
                using (var cmd = new MySqlCommand("prcUpdateGuest", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_GuestID", guest.GuestID);
                    cmd.Parameters.AddWithValue("p_FirstName", guest.FirstName);
                    cmd.Parameters.AddWithValue("p_LastName", guest.LastName);
                    cmd.Parameters.AddWithValue("p_Email", guest.Email);
                    cmd.Parameters.AddWithValue("p_PhoneNumber", guest.PhoneNumber);
                    cmd.Parameters.AddWithValue("p_BirthDate", guest.DateOfBirth);
                    cmd.Parameters.AddWithValue("p_Address", guest.Address);
                    cmd.Parameters.AddWithValue("p_Nationality", guest.Nationality);
                    cmd.Parameters.AddWithValue("p_IDType", guest.IDType);
                    cmd.Parameters.AddWithValue("p_IDNumber", guest.IDNumber);
                    cmd.Parameters.AddWithValue("p_Classification", guest.Classification);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteGuest(int guestId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcDeleteGuest", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_GuestID", guestId);
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
                using (var cmd = new MySqlCommand("prcSearchGuests", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_SearchTerm", keyword);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            guests.Add(MapGuest(reader));
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
                using (var cmd = new MySqlCommand("prcGetGuestById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_GuestID", guestId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapGuest(reader);
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
                using (var cmd = new MySqlCommand("prcGetAllGuests", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            guests.Add(MapGuest(reader));
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
                using (var cmd = new MySqlCommand("prcGetGuestGridData", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            guests.Add(new Guest
                            {
                                GuestID = Convert.ToInt32(reader["GuestID"]),
                                FirstName = reader["GuestName"].ToString().Split(' ').FirstOrDefault() ?? "",
                                LastName = reader["GuestName"].ToString().Split(' ').Skip(1).FirstOrDefault() ?? "",
                                Email = reader["Email"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                IDType = reader["IDTYPE"].ToString(),
                                Classification = reader["Classification"].ToString()
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
                                    SUM(CASE WHEN Classification = 'VIP' THEN 1 ELSE 0 END) as Active,
                                    SUM(CASE WHEN Classification = 'Regular' THEN 1 ELSE 0 END) as Inactive,
                                    SUM(CASE WHEN Classification = 'Blacklisted' THEN 1 ELSE 0 END) as Blacklisted
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

        private Guest MapGuest(MySqlDataReader reader)
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
                Classification = reader["Classification"].ToString()
            };
        }
    }
}
