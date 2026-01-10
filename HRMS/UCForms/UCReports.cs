using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HRMS.Helper;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace HRMS.UCForms
{
    public partial class UCReports : UserControl
    {
        public UCReports()
        {
            InitializeComponent();

            Load += UCReports_Load;
        }

        private void ApplyCurrentUserToHeader()
        {
            try
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
            catch
            {
            }
        }

        private void EnsureDefaults()
        {
            if (cmbViewType.Items.Count > 0 && cmbViewType.SelectedIndex < 0)
            {
                cmbViewType.SelectedIndex = 0;
            }

            if (dateTimePicker2.Value.Date > dateTimePicker1.Value.Date)
            {
                var tmp = dateTimePicker2.Value;
                dateTimePicker2.Value = dateTimePicker1.Value;
                dateTimePicker1.Value = tmp;
            }
        }

        private (DateTime fromInclusive, DateTime toExclusive) GetDateRange()
        {
            DateTime from = dateTimePicker2.Value.Date;
            DateTime to = dateTimePicker1.Value.Date;
            if (from > to)
            {
                var tmp = from;
                from = to;
                to = tmp;
            }

            return (from, to.AddDays(1));
        }

        private string GetPaymentMethodFilter()
        {
            string raw = cmbViewType.SelectedItem?.ToString() ?? "";
            raw = raw.Trim();

            if (string.IsNullOrWhiteSpace(raw) || raw.Equals("All payment method", StringComparison.OrdinalIgnoreCase))
            {
                return "ALL";
            }

            if (raw.IndexOf("cash", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "CASH";
            }

            if (raw.IndexOf("g-cash", StringComparison.OrdinalIgnoreCase) >= 0
                || raw.IndexOf("gcash", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "GCASH";
            }

            if (raw.IndexOf("card", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "CARD";
            }

            return "ALL";
        }

        private void LoadReport()
        {
            EnsureDefaults();
            var (fromInclusive, toExclusive) = GetDateRange();
            string paymentFilter = GetPaymentMethodFilter();

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();

                string reservationsQuery = @"SELECT
                                                COUNT(*) AS TotalBookings,
                                                COALESCE(SUM(COALESCE(r.numberOfNights, DATEDIFF(r.Check_OutDate, r.Check_InDate))), 0) AS TotalNights
                                            FROM reservations r
                                            WHERE r.Check_InDate >= @fromDate
                                              AND r.Check_InDate < @toDate";

                int totalBookings = 0;
                int totalNights = 0;
                using (var cmd = new MySqlCommand(reservationsQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@fromDate", fromInclusive);
                    cmd.Parameters.AddWithValue("@toDate", toExclusive);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int.TryParse(reader["TotalBookings"]?.ToString() ?? "0", out totalBookings);
                            int.TryParse(reader["TotalNights"]?.ToString() ?? "0", out totalNights);
                        }
                    }
                }

                string paymentsBaseWhere = @"p.Payment_Date >= @fromDate
                                            AND p.Payment_Date < @toDate
                                            AND (p.Payment_Status IS NULL OR p.Payment_Status <> 'Voided')";

                string paymentsFilterWhere = paymentFilter switch
                {
                    "CASH" => paymentsBaseWhere + " AND p.Payment_method = 'Cash'",
                    "GCASH" => paymentsBaseWhere + " AND (p.Payment_method = 'G-cash' OR p.Payment_method = 'GCash')",
                    "CARD" => paymentsBaseWhere + " AND (p.Payment_method LIKE '%Card%' OR p.Payment_method = 'Credit Card' OR p.Payment_method = 'Debit Card')",
                    _ => paymentsBaseWhere
                };

                string revenueQuery = $@"SELECT
                                            ROUND(COALESCE(SUM(CASE
                                                WHEN (p.Payment_method = 'Additional Service' OR p.Payment_Status = 'Charge') THEN p.amount
                                                ELSE 0
                                            END), 0), 2) AS AdditionalServicesRevenue,
                                            ROUND(COALESCE(SUM(CASE
                                                WHEN (p.Payment_method IS NULL OR p.Payment_method <> 'Additional Service')
                                                     AND (p.Payment_Status IS NULL OR p.Payment_Status <> 'Charge') THEN p.amount
                                                ELSE 0
                                            END), 0), 2) AS RoomRevenue,
                                            ROUND(COALESCE(SUM(p.amount), 0), 2) AS TotalRevenue,
                                            ROUND(COALESCE(SUM(CASE WHEN p.Payment_method = 'Cash' THEN p.amount ELSE 0 END), 0), 2) AS CashPayments,
                                            ROUND(COALESCE(SUM(CASE WHEN (p.Payment_method = 'G-cash' OR p.Payment_method = 'GCash') THEN p.amount ELSE 0 END), 0), 2) AS GcashPayments,
                                            ROUND(COALESCE(SUM(CASE WHEN (p.Payment_method LIKE '%Card%' OR p.Payment_method = 'Credit Card' OR p.Payment_method = 'Debit Card') THEN p.amount ELSE 0 END), 0), 2) AS CardPayments
                                        FROM payment p
                                        WHERE {paymentsFilterWhere}";

                decimal additionalServicesRevenue = 0m;
                decimal roomRevenue = 0m;
                decimal totalRevenue = 0m;
                decimal cashPayments = 0m;
                decimal gcashPayments = 0m;
                decimal cardPayments = 0m;

                using (var cmd = new MySqlCommand(revenueQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@fromDate", fromInclusive);
                    cmd.Parameters.AddWithValue("@toDate", toExclusive);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            decimal.TryParse(reader["AdditionalServicesRevenue"]?.ToString() ?? "0", NumberStyles.Any, CultureInfo.InvariantCulture, out additionalServicesRevenue);
                            decimal.TryParse(reader["RoomRevenue"]?.ToString() ?? "0", NumberStyles.Any, CultureInfo.InvariantCulture, out roomRevenue);
                            decimal.TryParse(reader["TotalRevenue"]?.ToString() ?? "0", NumberStyles.Any, CultureInfo.InvariantCulture, out totalRevenue);
                            decimal.TryParse(reader["CashPayments"]?.ToString() ?? "0", NumberStyles.Any, CultureInfo.InvariantCulture, out cashPayments);
                            decimal.TryParse(reader["GcashPayments"]?.ToString() ?? "0", NumberStyles.Any, CultureInfo.InvariantCulture, out gcashPayments);
                            decimal.TryParse(reader["CardPayments"]?.ToString() ?? "0", NumberStyles.Any, CultureInfo.InvariantCulture, out cardPayments);
                        }
                    }
                }

                string chartQuery = $@"SELECT
                                           DATE(p.Payment_Date) AS PayDay,
                                           ROUND(COALESCE(SUM(p.amount), 0), 2) AS Revenue
                                       FROM payment p
                                       WHERE {paymentsFilterWhere}
                                       GROUP BY DATE(p.Payment_Date)
                                       ORDER BY DATE(p.Payment_Date) ASC";

                var points = new List<(DateTime day, decimal revenue)>();
                using (var cmd = new MySqlCommand(chartQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@fromDate", fromInclusive);
                    cmd.Parameters.AddWithValue("@toDate", toExclusive);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime.TryParse(reader["PayDay"]?.ToString() ?? "", out DateTime day);
                            decimal.TryParse(reader["Revenue"]?.ToString() ?? "0", NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rev);
                            if (day != DateTime.MinValue)
                            {
                                points.Add((day.Date, rev));
                            }
                        }
                    }
                }

                label6.Text = totalBookings.ToString();
                label7.Text = totalNights.ToString();
                label9.Text = $"₱ {additionalServicesRevenue:0,0.00}";
                label11.Text = $"₱ {roomRevenue:0,0.00}";
                label24.Text = $"₱ {totalRevenue:0,0.00}";
                label25.Text = $"₱ {cashPayments:0,0.00}";
                label26.Text = $"₱ {gcashPayments:0,0.00}";
                label29.Text = $"₱ {cardPayments:0,0.00}";
                label3.Text = $"Last update : {DateTime.Now:MMM-dd-yyyy hh:mm tt}";

                PopulateSalesChart(points);
            }
        }

        private void PopulateSalesChart(List<(DateTime day, decimal revenue)> points)
        {
            chart2.Series.Clear();

            var series = new Series("Revenue")
            {
                ChartType = SeriesChartType.Column,
                XValueType = ChartValueType.Date,
                YValueType = ChartValueType.Double
            };

            foreach (var p in points.OrderBy(p => p.day))
            {
                int idx = series.Points.AddXY(p.day, (double)p.revenue);
                series.Points[idx].AxisLabel = p.day.ToString("MMM-dd");
            }

            chart2.Series.Add(series);

            if (chart2.ChartAreas.Count > 0)
            {
                var area = chart2.ChartAreas[0];
                area.AxisX.Interval = 1;
                area.AxisX.MajorGrid.Enabled = false;
                area.AxisY.MajorGrid.LineColor = Color.Gainsboro;
                area.RecalculateAxesScale();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                LoadReport();
            }
            catch
            {
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label31_Click(object sender, EventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void label35_Click(object sender, EventArgs e)
        {

        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void UCReports_Load(object sender, EventArgs e)
        {
            try
            {
                ApplyCurrentUserToHeader();
                EnsureDefaults();
                LoadReport();
            }
            catch
            {
            }
        }

        private void cmbViewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadReport();
            }
            catch
            {
            }
        }
    }
}
