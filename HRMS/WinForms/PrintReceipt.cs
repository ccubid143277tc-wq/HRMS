using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using HRMS.Helper;
using MySql.Data.MySqlClient;

namespace HRMS.WinForms
{
    public partial class PrintReceipt : Form
    {
        private readonly int _reservationId;
        private readonly int _paymentId;
        private readonly PrintDocument _printDocument;
        private readonly PrintPreviewDialog _printPreview;
        private Bitmap _receiptBitmap;

        public PrintReceipt(int reservationId, int paymentId)
        {
            InitializeComponent();

            _reservationId = reservationId;
            _paymentId = paymentId;

            _printDocument = new PrintDocument();
            _printDocument.PrintPage += PrintDocument_PrintPage;

            _printPreview = new PrintPreviewDialog
            {
                Document = _printDocument,
                Width = 1000,
                Height = 800
            };

            Load += PrintReceipt_Load;
            Activated += PrintReceipt_Activated;
        }

        private void PrintReceipt_Load(object sender, EventArgs e)
        {
            try
            {
                LoadReceiptData();
            }
            catch
            {
                // Intentionally ignore here to avoid crashing the receipt
            }
        }

        private void PrintReceipt_Activated(object sender, EventArgs e)
        {
            try
            {
                // Refresh amounts/dates if additional payments were recorded after the form opened.
                LoadReceiptData();
                _receiptBitmap?.Dispose();
                _receiptBitmap = null;
            }
            catch
            {
                // Intentionally ignore here to avoid crashing the receipt
            }
        }

        private void LoadReceiptData()
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();

                // Always resolve the latest payment for this reservation.
                // This avoids printing an outdated receipt header when multiple payments exist.
                int latestPaymentId = 0;
                DateTime latestPaymentDate = DateTime.Now;
                string latestPaymentQuery = @"SELECT p.PaymentID, p.Payment_Date
                                             FROM payment p
                                             WHERE p.ReservationID = @ReservationID
                                               AND (p.Payment_Status IS NULL OR p.Payment_Status <> 'Voided')
                                               AND (p.Payment_method IS NULL OR p.Payment_method <> 'Additional Service')
                                               AND (p.Payment_Status IS NULL OR p.Payment_Status <> 'Charge')
                                             ORDER BY p.Payment_Date DESC, p.PaymentID DESC
                                             LIMIT 1";

                using (var cmd = new MySqlCommand(latestPaymentQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ReservationID", _reservationId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int.TryParse(reader["PaymentID"]?.ToString() ?? "0", out latestPaymentId);
                            DateTime.TryParse(reader["Payment_Date"]?.ToString() ?? "", out latestPaymentDate);
                        }
                    }
                }

                // Reservation + guest details
                string reservationQuery = @"SELECT
                                                r.ReservationID,
                                                r.BookingReferences,
                                                r.Check_InDate,
                                                r.Check_OutDate,
                                                COALESCE(r.numberOfNights, DATEDIFF(r.Check_OutDate, r.Check_InDate)) AS numberOfNights,
                                                COALESCE(r.NumAdult, 0) AS NumAdult,
                                                COALESCE(r.NumChildren, 0) AS NumChildren,
                                                CONCAT(g.FirstName, ' ', g.LastName) AS GuestName,
                                                COALESCE(g.Address, '') AS Address,
                                                COALESCE(g.PhoneNumber, '') AS PhoneNumber,
                                                GROUP_CONCAT(DISTINCT rm.RoomNumber ORDER BY rm.RoomNumber SEPARATOR ', ') AS RoomNumbers,
                                                COALESCE(rt.RoomType, '') AS RoomType,
                                                COALESCE(SUM(DISTINCT rm.RoomRate), 0) AS NightlyRoomRateSum
                                             FROM reservations r
                                             LEFT JOIN Guest g ON r.GuestID = g.GuestID
                                             LEFT JOIN ReservationRooms rr ON r.ReservationID = rr.ReservationID
                                             LEFT JOIN Rooms rm ON rm.RoomID = COALESCE(rr.RoomID, r.RoomID)
                                             LEFT JOIN RoomType rt ON rm.RoomTypeID = rt.RoomTypeID
                                             WHERE r.ReservationID = @ReservationID
                                             GROUP BY r.ReservationID, r.BookingReferences, r.Check_InDate, r.Check_OutDate, r.numberOfNights,
                                                      r.NumAdult, r.NumChildren, g.FirstName, g.LastName, g.Address, g.PhoneNumber, rt.RoomType";

                string totalsQuery = @"SELECT
                                            ROUND(COALESCE(rtcalc.TotalDue, 0) + COALESCE(svc.ServiceCharges, 0), 2) AS TotalDue,
                                            ROUND(COALESCE(pd.TotalPaid, 0), 2) AS TotalPaid,
                                            ROUND((COALESCE(rtcalc.TotalDue, 0) + COALESCE(svc.ServiceCharges, 0)) - COALESCE(pd.TotalPaid, 0), 2) AS Balance,
                                            ROUND(COALESCE(svc.ServiceCharges, 0), 2) AS ServiceCharges
                                       FROM reservations r
                                       LEFT JOIN (
                                           SELECT
                                               r2.ReservationID,
                                               ((COALESCE(SUM(DISTINCT rm.RoomRate), 0) * COALESCE(r2.numberOfNights, 0)) * 1.05) AS TotalDue
                                           FROM reservations r2
                                           LEFT JOIN ReservationRooms rr ON r2.ReservationID = rr.ReservationID
                                           LEFT JOIN Rooms rm ON rm.RoomID = COALESCE(rr.RoomID, r2.RoomID)
                                           GROUP BY r2.ReservationID, r2.numberOfNights
                                       ) rtcalc ON rtcalc.ReservationID = r.ReservationID
                                       LEFT JOIN (
                                           SELECT
                                               ReservationID,
                                               COALESCE(SUM(amount), 0) AS TotalPaid
                                           FROM payment
                                           WHERE (Payment_Status IS NULL OR Payment_Status <> 'Voided')
                                             AND (Payment_method IS NULL OR Payment_method <> 'Additional Service')
                                             AND (Payment_Status IS NULL OR Payment_Status <> 'Charge')
                                           GROUP BY ReservationID
                                       ) pd ON pd.ReservationID = r.ReservationID
                                       LEFT JOIN (
                                           SELECT
                                               ReservationID,
                                               COALESCE(SUM(amount), 0) AS ServiceCharges
                                           FROM payment
                                           WHERE (Payment_Status IS NULL OR Payment_Status <> 'Voided')
                                             AND (Payment_method = 'Additional Service' OR Payment_Status = 'Charge')
                                           GROUP BY ReservationID
                                       ) svc ON svc.ReservationID = r.ReservationID
                                       WHERE r.ReservationID = @ReservationID";

                string receiptDateQuery = @"SELECT MAX(Payment_Date)
                                           FROM payment
                                           WHERE ReservationID = @ReservationID
                                             AND (Payment_Status IS NULL OR Payment_Status <> 'Voided')";

                string guestName = "";
                string address = "";
                string phone = "";
                string roomType = "";
                string roomNumbers = "";
                string bookingRef = "";
                DateTime checkIn = DateTime.MinValue;
                DateTime checkOut = DateTime.MinValue;
                int nights = 0;
                int adults = 0;
                int children = 0;
                decimal nightlyRoomRateSum = 0m;

                using (var cmd = new MySqlCommand(reservationQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ReservationID", _reservationId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bookingRef = reader["BookingReferences"]?.ToString() ?? "";
                            guestName = reader["GuestName"]?.ToString() ?? "";
                            address = reader["Address"]?.ToString() ?? "";
                            phone = reader["PhoneNumber"]?.ToString() ?? "";
                            roomType = reader["RoomType"]?.ToString() ?? "";
                            roomNumbers = reader["RoomNumbers"]?.ToString() ?? "";

                            DateTime.TryParse(reader["Check_InDate"]?.ToString() ?? "", out checkIn);
                            DateTime.TryParse(reader["Check_OutDate"]?.ToString() ?? "", out checkOut);
                            int.TryParse(reader["numberOfNights"]?.ToString() ?? "0", out nights);
                            int.TryParse(reader["NumAdult"]?.ToString() ?? "0", out adults);
                            int.TryParse(reader["NumChildren"]?.ToString() ?? "0", out children);
                            decimal.TryParse(reader["NightlyRoomRateSum"]?.ToString() ?? "0", out nightlyRoomRateSum);
                        }
                    }
                }

                decimal totalDue = 0m;
                decimal totalPaid = 0m;
                decimal balance = 0m;
                decimal serviceCharges = 0m;

                using (var cmd = new MySqlCommand(totalsQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ReservationID", _reservationId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            decimal.TryParse(reader["TotalDue"]?.ToString() ?? "0", out totalDue);
                            decimal.TryParse(reader["TotalPaid"]?.ToString() ?? "0", out totalPaid);
                            decimal.TryParse(reader["Balance"]?.ToString() ?? "0", out balance);
                            decimal.TryParse(reader["ServiceCharges"]?.ToString() ?? "0", out serviceCharges);
                        }
                    }
                }

                DateTime receiptDate = DateTime.Now;
                using (var cmd = new MySqlCommand(receiptDateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ReservationID", _reservationId);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        DateTime.TryParse(result.ToString(), out receiptDate);
                    }
                }

                // Prefer the latest payment details for receipt meta.
                int effectivePaymentId = latestPaymentId > 0 ? latestPaymentId : _paymentId;
                if (latestPaymentId > 0)
                {
                    receiptDate = latestPaymentDate;
                }

                // Header + billed to
                label6.Text = string.IsNullOrWhiteSpace(guestName) ? "Guest" : guestName;
                label7.Text = string.IsNullOrWhiteSpace(address) ? "" : address;
                label8.Text = string.IsNullOrWhiteSpace(phone) ? "" : phone;

                // Receipt meta
                string receiptNo = effectivePaymentId > 0 ? $"TMH - {effectivePaymentId:000}" : (string.IsNullOrWhiteSpace(bookingRef) ? "TMH - 000" : bookingRef);
                label11.Text = receiptNo;
                label12.Text = receiptDate.ToString("MMM-dd-yyyy");

                // Reservation details
                label26.Text = string.IsNullOrWhiteSpace(roomType) ? "" : roomType;
                label27.Text = string.IsNullOrWhiteSpace(roomNumbers) ? "" : roomNumbers;
                label23.Text = checkIn == DateTime.MinValue ? "" : checkIn.ToString("MMM-dd-yyyy");
                label24.Text = checkOut == DateTime.MinValue ? "" : checkOut.ToString("MMM-dd-yyyy");
                label25.Text = nights.ToString();

                string guestsText;
                if (children > 0 && adults > 0)
                {
                    guestsText = $"{children} Child, {adults} Adults";
                }
                else
                {
                    guestsText = (adults + children).ToString();
                }
                label28.Text = guestsText;

                // Charges breakdown
                decimal roomSubtotal = nightlyRoomRateSum * Math.Max(0, nights);

                label29.Text = $"Room Rate ({Math.Max(0, nights)} nights)";
                label30.Text = $"₱{roomSubtotal:0.00}";
                label31.Text = "Additional Services";
                label33.Text = $"₱{serviceCharges:0.00}";

                // Discount is not stored in DB (based on current code), so default to 0
                label36.Text = "₱0.00";
                label34.Text = $"₱{totalDue:0.00}";
                label39.Text = $"₱{totalPaid:0.00}";

                // Optional: show remaining balance on the empty label37 if present
                label37.Text = $"Remaining: ₱{balance:0.00}";
            }
        }

        private void ShowPrintPreview()
        {
            if (_printPreview.Visible)
            {
                return;
            }

            CreateReceiptBitmap();
            _printPreview.ShowDialog(this);
        }

        private void CreateReceiptBitmap()
        {
            _receiptBitmap?.Dispose();

            // Render the whole form surface to a bitmap.
            int w = Math.Max(1, Width);
            int h = Math.Max(1, Height);
            _receiptBitmap = new Bitmap(w, h);
            DrawToBitmap(_receiptBitmap, new Rectangle(0, 0, w, h));
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_receiptBitmap == null)
            {
                CreateReceiptBitmap();
            }

            if (_receiptBitmap == null)
            {
                e.HasMorePages = false;
                return;
            }

            Rectangle marginBounds = e.MarginBounds;
            float ratio = Math.Min((float)marginBounds.Width / _receiptBitmap.Width, (float)marginBounds.Height / _receiptBitmap.Height);
            int printWidth = (int)(_receiptBitmap.Width * ratio);
            int printHeight = (int)(_receiptBitmap.Height * ratio);

            var dest = new Rectangle(marginBounds.Left, marginBounds.Top, printWidth, printHeight);
            e.Graphics.DrawImage(_receiptBitmap, dest);
            e.HasMorePages = false;
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
