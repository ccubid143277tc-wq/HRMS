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
        private int _currentReservationId;
        private decimal _currentBalance;

        public Payment()
        {
            InitializeComponent();
            dataGridView1.CellClick += dataGridView1_CellClick;
            button8.Click += button8_Click;
            Load += Payment_Load;

            textBox10.TextChanged += BillingInputs_Changed;
            textBox14.TextChanged += BillingInputs_Changed;
            textBox15.TextChanged += BillingInputs_Changed;
            textBox16.TextChanged += BillingInputs_Changed;
            textBox9.TextChanged += BillingInputs_Changed;
            textBox18.TextChanged += BillingInputs_Changed;

            checkBox3.CheckedChanged += BillingInputs_Changed;
            checkBox4.CheckedChanged += BillingInputs_Changed;
            checkBox5.CheckedChanged += BillingInputs_Changed;
            checkBox6.CheckedChanged += BillingInputs_Changed;
            checkBox7.CheckedChanged += BillingInputs_Changed;

            textBox27.TextChanged += PaymentInputs_Changed;
            comboBox1.SelectedIndexChanged += PaymentInputs_Changed;

            textBox26.ReadOnly = true;
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;

            SyncAdditionalServiceInputs();
            UpdatePaymentSection();
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

        private decimal GetSelectedAdditionalServicesSubtotal()
        {
            decimal additionalServices = 0m;
            if (checkBox3.Checked) additionalServices += ParseMoney(textBox10.Text);
            if (checkBox4.Checked) additionalServices += ParseMoney(textBox14.Text);
            if (checkBox5.Checked) additionalServices += ParseMoney(textBox15.Text);
            if (checkBox6.Checked) additionalServices += ParseMoney(textBox16.Text);
            if (checkBox7.Checked) additionalServices += ParseMoney(textBox9.Text);
            return additionalServices;
        }

        private void Payment_Load_1(object sender, EventArgs e)
        {
            Payment_Load(sender, e);
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

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                long? insertedPaymentId = null;

                if (_currentReservationId > 0 && UserSession.CurrentUserId > 0)
                {
                    decimal currentBalance = GetCurrentBalance();
                    decimal additionalServicesSubtotal = GetSelectedAdditionalServicesSubtotal();
                    decimal additionalServicesCharge = Math.Round(additionalServicesSubtotal * 1.05m, 2);
                    if (additionalServicesCharge < 0m)
                    {
                        additionalServicesCharge = 0m;
                    }

                    decimal effectiveBalance = currentBalance + additionalServicesCharge;

                    bool hasPaymentMethod = comboBox1.SelectedItem != null
                                            && !string.IsNullOrWhiteSpace(comboBox1.SelectedItem.ToString())
                                            && !string.Equals(comboBox1.SelectedItem.ToString(), "Select payment", StringComparison.OrdinalIgnoreCase);

                    decimal amountReceived = ParseMoney(textBox27.Text);
                    decimal amountToPay = 0m;
                    string method = hasPaymentMethod ? comboBox1.SelectedItem.ToString() : "";
                    string paymentStatus = "";

                    if (hasPaymentMethod)
                    {
                        amountToPay = Math.Round(Math.Min(amountReceived, effectiveBalance), 2);
                        if (amountToPay > 0m)
                        {
                            paymentStatus = amountToPay >= effectiveBalance ? "Paid" : "Unpaid";
                        }
                    }

                    if (additionalServicesCharge > 0m || amountToPay > 0m)
                    {
                        using (var conn = DBHelper.GetConnection())
                        {
                            conn.Open();
                            using (var tx = conn.BeginTransaction())
                            {
                                try
                                {
                                    string insert = @"INSERT INTO payment (amount, ReservationID, Payment_method, Payment_Status, UserID, Payment_Date)
                                                      VALUES (@amount, @ReservationID, @Payment_method, @Payment_Status, @UserID, @Payment_Date)";

                                    if (additionalServicesCharge > 0m)
                                    {
                                        using (var cmd = new MySqlCommand(insert, conn, tx))
                                        {
                                            cmd.Parameters.AddWithValue("@amount", additionalServicesCharge);
                                            cmd.Parameters.AddWithValue("@ReservationID", _currentReservationId);
                                            cmd.Parameters.AddWithValue("@Payment_method", "Additional Service");
                                            cmd.Parameters.AddWithValue("@Payment_Status", "Charge");
                                            cmd.Parameters.AddWithValue("@UserID", UserSession.CurrentUserId);
                                            cmd.Parameters.AddWithValue("@Payment_Date", DateTime.Now);
                                            cmd.ExecuteNonQuery();
                                            insertedPaymentId = cmd.LastInsertedId;
                                        }
                                    }

                                    if (amountToPay > 0m)
                                    {
                                        using (var cmd = new MySqlCommand(insert, conn, tx))
                                        {
                                            cmd.Parameters.AddWithValue("@amount", amountToPay);
                                            cmd.Parameters.AddWithValue("@ReservationID", _currentReservationId);
                                            cmd.Parameters.AddWithValue("@Payment_method", method);
                                            cmd.Parameters.AddWithValue("@Payment_Status", paymentStatus);
                                            cmd.Parameters.AddWithValue("@UserID", UserSession.CurrentUserId);
                                            cmd.Parameters.AddWithValue("@Payment_Date", DateTime.Now);
                                            cmd.ExecuteNonQuery();
                                            insertedPaymentId = cmd.LastInsertedId;
                                        }
                                    }

                                    tx.Commit();
                                }
                                catch
                                {
                                    tx.Rollback();
                                    throw;
                                }
                            }
                        }
                    }
                }

                LoadPaymentsGrid();

                if (insertedPaymentId.HasValue && insertedPaymentId.Value > 0)
                {
                    SelectPaymentRowById((int)insertedPaymentId.Value);
                }
                else
                {
                    PreserveCurrentSelectionByPaymentId();
                }
            }
            catch
            {
                // Intentionally ignore here to avoid crashing the page
            }
        }

        private void PreserveCurrentSelectionByPaymentId()
        {
            int? selectedPaymentId = null;
            if (dataGridView1.CurrentRow != null)
            {
                var rowView = dataGridView1.CurrentRow.DataBoundItem as DataRowView;
                if (rowView != null
                    && rowView.Row.Table.Columns.Contains("PaymentID")
                    && rowView["PaymentID"] != DBNull.Value
                    && int.TryParse(rowView["PaymentID"].ToString(), out int pid))
                {
                    selectedPaymentId = pid;
                }
            }

            if (selectedPaymentId.HasValue)
            {
                SelectPaymentRowById(selectedPaymentId.Value);
            }
        }

        private void SelectPaymentRowById(int paymentId)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var rv = row.DataBoundItem as DataRowView;
                if (rv == null)
                {
                    continue;
                }

                if (rv.Row.Table.Columns.Contains("PaymentID")
                    && rv["PaymentID"] != DBNull.Value
                    && int.TryParse(rv["PaymentID"].ToString(), out int pid)
                    && pid == paymentId)
                {
                    row.Selected = true;
                    dataGridView1.CurrentCell = row.Cells[0];

                    if (rv.Row.Table.Columns.Contains("Balance") && rv["Balance"] != DBNull.Value)
                    {
                        if (decimal.TryParse(rv["Balance"].ToString(), out decimal balance))
                        {
                            _currentBalance = balance;
                            label22.Text = $"Remaining Balance: ₱{balance:0.00}";
                        }
                    }

                    UpdatePaymentSection();
                    break;
                }
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
                                    r.ReservationID,
                                    r.BookingReferences,
                                    p.Payment_Date,
                                    CONCAT(g.FirstName, ' ', g.LastName) AS GuestName,
                                    p.amount,
                                    p.Payment_method,
                                    p.Payment_Status,
                                    COALESCE(u.Username, '') AS ProcessedBy,
                                    ROUND(COALESCE(rtcalc.TotalDue, 0) + COALESCE(svc.ServiceCharges, 0), 2) AS TotalDue,
                                    ROUND(COALESCE(pd.TotalPaid, 0), 2) AS TotalPaid,
                                    ROUND((COALESCE(rtcalc.TotalDue, 0) + COALESCE(svc.ServiceCharges, 0)) - COALESCE(pd.TotalPaid, 0), 2) AS Balance,
                                    '' AS ReferenceNo
                                 FROM reservations r
                                 LEFT JOIN Guest g ON r.GuestID = g.GuestID
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
                                 LEFT JOIN (
                                     SELECT
                                         ReservationID,
                                         MAX(PaymentID) AS LastPaymentID
                                     FROM payment
                                     WHERE (Payment_Status IS NULL OR Payment_Status <> 'Voided')
                                     GROUP BY ReservationID
                                 ) lp ON lp.ReservationID = r.ReservationID
                                 LEFT JOIN payment p ON p.PaymentID = lp.LastPaymentID
                                 LEFT JOIN Users u ON p.UserID = u.UserID
                                 ORDER BY COALESCE(p.Payment_Date, r.Check_InDate) DESC, COALESCE(p.PaymentID, 0) DESC";

                using (var cmd = new MySqlCommand(query, conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            try
            {
                var rowView = dataGridView1.Rows[e.RowIndex].DataBoundItem as DataRowView;
                if (rowView == null)
                {
                    return;
                }

                if (rowView.Row.Table.Columns.Contains("ReservationID")
                    && rowView["ReservationID"] != DBNull.Value
                    && int.TryParse(rowView["ReservationID"].ToString(), out int reservationId)
                    && reservationId > 0)
                {
                    _currentReservationId = reservationId;
                    LoadReservationDetailsAndCharges(reservationId);
                }

                if (rowView.Row.Table.Columns.Contains("Balance") && rowView["Balance"] != DBNull.Value)
                {
                    if (decimal.TryParse(rowView["Balance"].ToString(), out decimal balance))
                    {
                        _currentBalance = balance;
                        label22.Text = $"Remaining Balance: ₱{balance:0.00}";
                    }
                }

                RecalculateBillingSummary();
                UpdatePaymentSection();
            }
            catch
            {
                // Intentionally ignore here to avoid crashing the page
            }
        }

        private void PaymentInputs_Changed(object sender, EventArgs e)
        {
            try
            {
                UpdatePaymentSection();
            }
            catch
            {
                // Intentionally ignore here to avoid crashing the page
            }
        }

        private decimal GetCurrentBalance()
        {
            if (_currentBalance > 0m)
            {
                return _currentBalance;
            }

            string t = (label22.Text ?? "").Trim();
            int idx = t.IndexOf('₱');
            if (idx >= 0)
            {
                return ParseMoney(t.Substring(idx));
            }

            return 0m;
        }

        private void UpdatePaymentSection()
        {
            decimal balance = GetCurrentBalance();
            decimal additionalServicesSubtotal = GetSelectedAdditionalServicesSubtotal();
            decimal additionalServicesCharge = Math.Round(additionalServicesSubtotal * 1.05m, 2);
            if (additionalServicesCharge < 0m)
            {
                additionalServicesCharge = 0m;
            }

            decimal effectiveBalance = balance + additionalServicesCharge;
            decimal amountReceived = ParseMoney(textBox27.Text);

            decimal amountToPay = Math.Min(amountReceived, effectiveBalance);
            decimal change = amountReceived - amountToPay;
            if (change < 0m)
            {
                change = 0m;
            }

            textBox26.Text = Math.Round(change, 2).ToString("0.00");

            bool isPaid = effectiveBalance <= 0m || amountToPay >= effectiveBalance;
            checkBox1.Checked = isPaid;
            checkBox2.Checked = !isPaid;
        }

        private void BillingInputs_Changed(object sender, EventArgs e)
        {
            try
            {
                SyncAdditionalServiceInputs();
                RecalculateBillingSummary();
                UpdatePaymentSection();
            }
            catch
            {
                // Intentionally ignore here to avoid crashing the page
            }
        }

        private void SyncAdditionalServiceInputs()
        {
            // Enable the amount input only when the corresponding checkbox is checked.
            textBox10.Enabled = checkBox3.Checked;
            textBox14.Enabled = checkBox4.Checked;
            textBox15.Enabled = checkBox5.Checked;
            textBox16.Enabled = checkBox6.Checked;
            textBox9.Enabled = checkBox7.Checked;
        }

        private decimal ParseMoney(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0m;
            }

            string cleaned = value.Replace("₱", "").Replace(",", "").Trim();
            if (cleaned.Equals("Enter Amount", StringComparison.OrdinalIgnoreCase))
            {
                return 0m;
            }

            if (decimal.TryParse(cleaned, out decimal amount))
            {
                return amount;
            }

            return 0m;
        }

        private void RecalculateBillingSummary()
        {
            decimal roomCharges = ParseMoney(textBox6.Text);

            decimal additionalServices = 0m;

            if (checkBox3.Checked) additionalServices += ParseMoney(textBox10.Text);
            if (checkBox4.Checked) additionalServices += ParseMoney(textBox14.Text);
            if (checkBox5.Checked) additionalServices += ParseMoney(textBox15.Text);
            if (checkBox6.Checked) additionalServices += ParseMoney(textBox16.Text);
            if (checkBox7.Checked) additionalServices += ParseMoney(textBox9.Text);

            decimal discount = ParseMoney(textBox18.Text);

            decimal subtotalBeforeTax = roomCharges + additionalServices - discount;
            if (subtotalBeforeTax < 0m)
            {
                subtotalBeforeTax = 0m;
            }

            decimal tax = Math.Round(subtotalBeforeTax * 0.05m, 2);
            decimal totalAmount = Math.Round(subtotalBeforeTax + tax, 2);

            textBox21.Text = roomCharges.ToString("0.00");
            textBox20.Text = additionalServices.ToString("0.00");
            textBox19.Text = tax.ToString("0.00");
            textBox22.Text = subtotalBeforeTax.ToString("0.00");
        }

        private void LoadReservationDetailsAndCharges(int reservationId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();

                string query = @"SELECT
                                    r.ReservationID,
                                    r.BookingReferences,
                                    r.Check_InDate,
                                    r.Check_OutDate,
                                    COALESCE(r.numberOfNights, DATEDIFF(r.Check_OutDate, r.Check_InDate)) AS numberOfNights,
                                    CONCAT(g.FirstName, ' ', g.LastName) AS GuestName,
                                    GROUP_CONCAT(DISTINCT rm.RoomNumber ORDER BY rm.RoomNumber SEPARATOR ', ') AS RoomNumbers,
                                    COALESCE(rt.RoomType, '') AS RoomType,
                                    COUNT(DISTINCT rm.RoomID) AS NumberOfRooms,
                                    COALESCE(SUM(DISTINCT rm.RoomRate), 0) AS NightlyRoomRateSum
                                 FROM reservations r
                                 LEFT JOIN Guest g ON r.GuestID = g.GuestID
                                 LEFT JOIN ReservationRooms rr ON r.ReservationID = rr.ReservationID
                                 LEFT JOIN Rooms rm ON rm.RoomID = COALESCE(rr.RoomID, r.RoomID)
                                 LEFT JOIN RoomType rt ON rm.RoomTypeID = rt.RoomTypeID
                                 WHERE r.ReservationID = @ReservationID
                                 GROUP BY r.ReservationID, r.BookingReferences, r.Check_InDate, r.Check_OutDate, r.numberOfNights,
                                          g.FirstName, g.LastName, rt.RoomType";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ReservationID", reservationId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return;
                        }

                        string bookingRef = reader["BookingReferences"]?.ToString() ?? "";
                        string guestName = reader["GuestName"]?.ToString() ?? "";
                        string roomType = reader["RoomType"]?.ToString() ?? "";
                        string roomNumbers = reader["RoomNumbers"]?.ToString() ?? "";

                        int nights = 0;
                        int.TryParse(reader["numberOfNights"]?.ToString() ?? "0", out nights);

                        int numberOfRooms = 0;
                        int.TryParse(reader["NumberOfRooms"]?.ToString() ?? "0", out numberOfRooms);

                        decimal nightlyRoomRateSum = 0m;
                        decimal.TryParse(reader["NightlyRoomRateSum"]?.ToString() ?? "0", out nightlyRoomRateSum);

                        DateTime checkIn = DateTime.MinValue;
                        DateTime checkOut = DateTime.MinValue;
                        DateTime.TryParse(reader["Check_InDate"]?.ToString() ?? "", out checkIn);
                        DateTime.TryParse(reader["Check_OutDate"]?.ToString() ?? "", out checkOut);

                        // Reservation Details section
                        textBox1.Text = bookingRef;
                        textBox2.Text = guestName;
                        textBox4.Text = roomType;
                        textBox3.Text = roomNumbers;
                        textBox5.Text = checkIn == DateTime.MinValue ? "" : checkIn.ToString("yyyy-MM-dd");
                        textBox7.Text = checkOut == DateTime.MinValue ? "" : checkOut.ToString("yyyy-MM-dd");

                        // Room Charges section
                        textBox13.Text = nightlyRoomRateSum.ToString("0.00");
                        textBox12.Text = numberOfRooms.ToString();
                        textBox8.Text = nights.ToString();

                        decimal roomSubtotal = nightlyRoomRateSum * nights;
                        textBox6.Text = roomSubtotal.ToString("0.00");

                        SyncAdditionalServiceInputs();
                        RecalculateBillingSummary();
                    }
                }
            }
        }

        private void label19_Click(object sender, EventArgs e)
        {
            label19.Text = DateTime.Now.ToString("dddd, MMMM dd yyyy hh:mm:ss tt");
        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {
            BillingInputs_Changed(sender, e);
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            BillingInputs_Changed(sender, e);
        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {
            BillingInputs_Changed(sender, e);
        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            BillingInputs_Changed(sender, e);
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

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
