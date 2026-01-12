using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HRMS.Helper;
using MySql.Data.MySqlClient;

namespace HRMS.WinForms
{
    public partial class GuestInformation : Form
    {
        private readonly int _reservationId;

        public GuestInformation()
        {
            InitializeComponent();
        }

        public GuestInformation(int reservationId)
        {
            InitializeComponent();
            _reservationId = reservationId;
            Load += GuestInformation_Load;
        }

        private void GuestInformation_Load(object? sender, EventArgs e)
        {
            try
            {
                LoadGuestInformation();
            }
            catch
            {
            }
        }

        private void LoadGuestInformation()
        {
            if (_reservationId <= 0)
            {
                return;
            }

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();

                string detailsQuery = @"SELECT
                                            r.ReservationID,
                                            COALESCE(r.BookingReferences, '') AS BookingReferences,
                                            COALESCE(r.ReservationType, '') AS ReservationType,
                                            COALESCE(r.NumAdult, 0) AS NumAdult,
                                            COALESCE(r.NumChildren, 0) AS NumChildren,
                                            COALESCE(rt.RoomType, '') AS RoomType,
                                            CONCAT(g.FirstName, ' ', g.LastName) AS GuestName,
                                            COALESCE(g.PhoneNumber, '') AS PhoneNumber,
                                            COALESCE(g.Email, '') AS Email,
                                            COALESCE(g.Address, '') AS Address,
                                            COALESCE(g.IDTYPE, '') AS IDTYPE,
                                            COALESCE(g.IDNumber, '') AS IDNumber,
                                            COALESCE(g.Nationality, '') AS Nationality,
                                            g.BirthDate AS BirthDate
                                        FROM reservations r
                                        LEFT JOIN Guest g ON r.GuestID = g.GuestID
                                        LEFT JOIN ReservationRooms rr ON r.ReservationID = rr.ReservationID
                                        LEFT JOIN Rooms rm ON rm.RoomID = COALESCE(rr.RoomID, r.RoomID)
                                        LEFT JOIN RoomType rt ON rm.RoomTypeID = rt.RoomTypeID
                                        WHERE r.ReservationID = @ReservationID
                                        LIMIT 1";

                string totalPaidQuery = @"SELECT
                                            COALESCE(SUM(amount), 0) AS TotalPaid
                                        FROM payment
                                        WHERE ReservationID = @ReservationID
                                          AND (Payment_Status IS NULL OR Payment_Status <> 'Voided')";

                string guestName = "";
                string phone = "";
                string email = "";
                string address = "";
                string idType = "";
                string idNumber = "";
                string nationality = "";
                DateTime dob = DateTime.MinValue;
                int adults = 0;
                int children = 0;
                string roomType = "";
                string bookingRef = "";
                string reservationType = "";

                using (var cmd = new MySqlCommand(detailsQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ReservationID", _reservationId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            guestName = reader["GuestName"]?.ToString() ?? "";
                            phone = reader["PhoneNumber"]?.ToString() ?? "";
                            email = reader["Email"]?.ToString() ?? "";
                            address = reader["Address"]?.ToString() ?? "";
                            idType = reader["IDTYPE"]?.ToString() ?? "";
                            idNumber = reader["IDNumber"]?.ToString() ?? "";
                            nationality = reader["Nationality"]?.ToString() ?? "";
                            DateTime.TryParse(reader["BirthDate"]?.ToString() ?? "", out dob);

                            int.TryParse(reader["NumAdult"]?.ToString() ?? "0", out adults);
                            int.TryParse(reader["NumChildren"]?.ToString() ?? "0", out children);
                            roomType = reader["RoomType"]?.ToString() ?? "";
                            bookingRef = reader["BookingReferences"]?.ToString() ?? "";
                            reservationType = reader["ReservationType"]?.ToString() ?? "";
                        }
                    }
                }

                decimal totalPaid = 0m;
                using (var cmd = new MySqlCommand(totalPaidQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ReservationID", _reservationId);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        decimal.TryParse(result.ToString(), out totalPaid);
                    }
                }

                if (label15 != null) label15.Text = guestName;
                if (label16 != null) label16.Text = phone;
                if (label17 != null) label17.Text = email;
                if (label18 != null) label18.Text = address;
                if (label19 != null) label19.Text = idType;
                if (label20 != null) label20.Text = idNumber;
                if (label21 != null) label21.Text = nationality;
                if (label22 != null) label22.Text = dob == DateTime.MinValue ? "" : dob.ToString("MMMM dd, yyyy");
                if (label23 != null) label23.Text = adults.ToString();
                if (label24 != null) label24.Text = children.ToString();
                if (label25 != null) label25.Text = roomType;
                if (label26 != null) label26.Text = bookingRef;
                if (label27 != null) label27.Text = reservationType;
                if (label28 != null) label28.Text = MoneyHelper.Format(totalPaid);
            }
        }
    }
}
