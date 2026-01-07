using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HRMS.Helper;
using MySql.Data.MySqlClient;

namespace HRMS.UCForms
{
    public partial class Payment : UserControl
    {
        public Payment()
        {
            InitializeComponent();
            Load += Payment_Load;
        }

        private void Payment_Load(object sender, EventArgs e)
        {
            try
            {
                ApplyCurrentUserToHeader();
                LoadPaymentsGrid();
            }
            catch
            {
                // Intentionally ignore here to avoid crashing the page
            }
        }

        private void ApplyCurrentUserToHeader()
        {
            if (!string.IsNullOrWhiteSpace(UserSession.CurrentUserName))
            {
                label15.Text = UserSession.CurrentUserName;
            }

            if (!string.IsNullOrWhiteSpace(UserSession.CurrentUserRole))
            {
                label14.Text = UserSession.CurrentUserRole;
            }
        }

        private void LoadPaymentsGrid()
        {
            var table = GetPaymentsGridData();

            dataGridView1.AutoGenerateColumns = false;

            colpaymentID.DataPropertyName = "PaymentID";
            colBookingReference.DataPropertyName = "BookingReferences";
            ColPaymentDate.DataPropertyName = "Payment_Date";
            colGuestName.DataPropertyName = "GuestName";
            colAmountPaid.DataPropertyName = "amount";
            colPaymentMethod.DataPropertyName = "Payment_method";
            colPaymentStatus.DataPropertyName = "Payment_Status";
            colProcessedBy.DataPropertyName = "ProcessedBy";
            colTotalDue.DataPropertyName = "TotalDue";
            colTotalPaid.DataPropertyName = "TotalPaid";
            colBalance.DataPropertyName = "Balance";
            ColReferenceNo.DataPropertyName = "ReferenceNo";

            dataGridView1.DataSource = table;
        }

        private DataTable GetPaymentsGridData()
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();

                // Notes:
                // - TotalDue is calculated similar to Reservation Summary (sum room rates per night x nights + 5% tax)
                // - Multi-room is supported via ReservationRooms, with a fallback to reservations.RoomID
                // - TotalPaid includes all payments except those explicitly marked as 'Voided'
                string query = @"SELECT
                                    p.PaymentID,
                                    r.BookingReferences,
                                    p.Payment_Date,
                                    CONCAT(g.FirstName, ' ', g.LastName) AS GuestName,
                                    p.amount,
                                    p.Payment_method,
                                    p.Payment_Status,
                                    COALESCE(u.Username, '') AS ProcessedBy,
                                    ROUND(COALESCE(rtcalc.TotalDue, 0), 2) AS TotalDue,
                                    ROUND(COALESCE(pd.TotalPaid, 0), 2) AS TotalPaid,
                                    ROUND(COALESCE(rtcalc.TotalDue, 0) - COALESCE(pd.TotalPaid, 0), 2) AS Balance,
                                    '' AS ReferenceNo
                                 FROM payment p
                                 INNER JOIN reservations r ON p.ReservationID = r.ReservationID
                                 LEFT JOIN Guest g ON r.GuestID = g.GuestID
                                 LEFT JOIN Users u ON p.UserID = u.UserID
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
                                     WHERE Payment_Status IS NULL OR Payment_Status <> 'Voided'
                                     GROUP BY ReservationID
                                 ) pd ON pd.ReservationID = r.ReservationID
                                 ORDER BY p.Payment_Date DESC, p.PaymentID DESC";

                using (var cmd = new MySqlCommand(query, conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        private void label19_Click(object sender, EventArgs e)
        {
            label19.Text = DateTime.Now.ToString("dddd, MMMM dd yyyy hh:mm:ss tt");
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void label39_Click(object sender, EventArgs e)
        {

        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
