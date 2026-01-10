using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using HRMS.Helper;
using HRMS.Services;
using MySql.Data.MySqlClient;

namespace HRMS.UCForms
{
    public partial class AdminDashboard : UserControl
    {
        private readonly RoomService _roomService;

        public AdminDashboard()
        {
            InitializeComponent();

            _roomService = new RoomService();
            Load += AdminDashboard_Load;
        }

        private void label19_Click(object sender, EventArgs e)
        {
            label19.Text = DateTime.Now.ToString("dddd, MMMM dd yyyy hh:mm:ss tt");
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            try
            {
                ApplyCurrentUserToHeader();
                LoadOccupancyRate();
                LoadRevenueToday();
                LoadCheckInOutMetrics();
                LoadRemainingMetrics();
                LoadRoomStatusPieChart();
                LoadBookingsByRoomTypeChart();
            }
            catch
            {
                
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

        private void LoadOccupancyRate()
        {
            var counts = _roomService.GetRoomStatusCounts();

            counts.TryGetValue("Total", out int totalRooms);
            if (totalRooms <= 0)
            {
                totalRooms = 1;
            }

            int occupiedToday = _roomService.GetOccupiedRoomCountByDate(DateTime.Today);
            double occupancyRate = (double)occupiedToday / totalRooms * 100.0;

            label2.Text = $"{occupancyRate:0}%";
            label29.Text = $"{occupiedToday} of {totalRooms} Rooms";
        }

        private void LoadRevenueToday()
        {
            decimal revenue = 0m;

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();

                string query = @"SELECT COALESCE(SUM(amount), 0)
                                 FROM payment
                                 WHERE DATE(Payment_Date) = @Today
                                   AND (Payment_Status IS NULL OR Payment_Status <> 'Voided')
                                   AND NOT (Payment_method = 'Additional Service' AND Payment_Status = 'Charge')";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Today", DateTime.Today);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        decimal.TryParse(result.ToString(), out revenue);
                    }
                }
            }

            label22.Text = $"₱ {revenue:0.00}";
        }

        private void LoadCheckInOutMetrics()
        {
            int checkInsToday = _roomService.GetExpectedArrivalsCount(DateTime.Today);
            int checkOutsToday = _roomService.GetExpectedDeparturesCount(DateTime.Today);

            label5.Text = checkInsToday.ToString();
            label27.Text = $"{checkInsToday} Pending Arrivals";

            label7.Text = checkOutsToday.ToString();
            label28.Text = $"{checkOutsToday} Pending Check-Out";
        }

        private void LoadRemainingMetrics()
        {
            try
            {
                var counts = _roomService.GetRoomStatusCounts();

                if (counts.TryGetValue("Available", out int availableRooms))
                {
                    label9.Text = availableRooms.ToString();
                }

                if (counts.TryGetValue("Maintenance", out int maintenanceRooms))
                {
                    label11.Text = maintenanceRooms.ToString();
                }

                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    // Pending reservations (awaiting confirmation)
                    string pendingReservationsQuery = @"SELECT COALESCE(COUNT(*), 0)
                                                     FROM reservations
                                                     WHERE ReservationStatus = 'Pending'";

                    using (var cmd = new MySqlCommand(pendingReservationsQuery, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        int pendingReservations = 0;
                        if (result != null && result != DBNull.Value)
                        {
                            int.TryParse(result.ToString(), out pendingReservations);
                        }

                        label21.Text = pendingReservations.ToString();
                        label23.Text = "Awaiting Confirmation";
                    }

                    // Outstanding payments = reservations with remaining balance > 0
                    string outstandingPaymentsQuery = @"SELECT COALESCE(COUNT(*), 0) FROM (
                                                            SELECT
                                                                r.ReservationID,
                                                                ROUND((COALESCE(rtcalc.TotalDue, 0) + COALESCE(svc.ServiceCharges, 0)) - COALESCE(pd.TotalPaid, 0), 2) AS Balance
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
                                                                  AND NOT (Payment_method = 'Additional Service' AND Payment_Status = 'Charge')
                                                                GROUP BY ReservationID
                                                            ) pd ON pd.ReservationID = r.ReservationID
                                                            LEFT JOIN (
                                                                SELECT
                                                                    ReservationID,
                                                                    COALESCE(SUM(amount), 0) AS ServiceCharges
                                                                FROM payment
                                                                WHERE (Payment_Status IS NULL OR Payment_Status <> 'Voided')
                                                                  AND (Payment_method = 'Additional Service' AND Payment_Status = 'Charge')
                                                                GROUP BY ReservationID
                                                            ) svc ON svc.ReservationID = r.ReservationID
                                                        ) x
                                                        WHERE x.Balance > 0";

                    using (var cmd = new MySqlCommand(outstandingPaymentsQuery, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        int outstandingPayments = 0;
                        if (result != null && result != DBNull.Value)
                        {
                            int.TryParse(result.ToString(), out outstandingPayments);
                        }

                        label13.Text = outstandingPayments.ToString();
                        label24.Text = $"{outstandingPayments} Pending Payments";
                    }
                }
            }
            catch
            {
                // Intentionally ignore here to avoid crashing the dashboard
            }
        }

        private void LoadRoomStatusPieChart()
        {
            var counts = _roomService.GetRoomStatusCounts();

            if (chart1.Series.Count == 0)
            {
                chart1.Series.Add(new Series("Series1"));
            }

            var series = chart1.Series[0];
            series.Points.Clear();
            series.ChartType = SeriesChartType.Pie;
            series.IsValueShownAsLabel = true;
            series.Label = "#PERCENT{P0}";
            series.LegendText = "#VALX (#VALY)";

            if (counts.TryGetValue("Available", out int available))
            {
                series.Points.AddXY("Available", available);
            }
            if (counts.TryGetValue("Occupied", out int occupied))
            {
                series.Points.AddXY("Occupied", occupied);
            }
            if (counts.TryGetValue("Maintenance", out int maintenance))
            {
                series.Points.AddXY("Maintenance", maintenance);
            }
            if (counts.TryGetValue("Reserved", out int reserved))
            {
                series.Points.AddXY("Reserved", reserved);
            }

            chart1.Titles.Clear();
        }

        private void LoadBookingsByRoomTypeChart()
        {
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    string query = @"SELECT
                                        rt.RoomType AS RoomType,
                                        COUNT(DISTINCT r.ReservationID) AS Bookings
                                     FROM reservations r
                                     LEFT JOIN ReservationRooms rr ON r.ReservationID = rr.ReservationID
                                     LEFT JOIN Rooms rm ON rm.RoomID = COALESCE(rr.RoomID, r.RoomID)
                                     LEFT JOIN RoomType rt ON rm.RoomTypeID = rt.RoomTypeID
                                     WHERE r.ReservationStatus NOT IN ('Cancelled')
                                     GROUP BY rt.RoomType
                                     ORDER BY Bookings DESC";

                    var table = new DataTable();
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(table);
                    }

                    if (chart2.Series.Count == 0)
                    {
                        chart2.Series.Add(new Series("Series1"));
                    }

                    var series = chart2.Series[0];
                    series.Points.Clear();
                    series.ChartType = SeriesChartType.Column;
                    series.IsValueShownAsLabel = true;
                    series.LegendText = "Bookings";

                    foreach (DataRow row in table.Rows)
                    {
                        string roomType = row["RoomType"] == DBNull.Value ? "Unknown" : row["RoomType"].ToString();
                        int bookings = 0;
                        int.TryParse(row["Bookings"]?.ToString() ?? "0", out bookings);
                        series.Points.AddXY(roomType, bookings);
                    }

                    if (chart2.ChartAreas.Count > 0)
                    {
                        var area = chart2.ChartAreas[0];
                        area.AxisX.Interval = 1;
                        area.AxisX.MajorGrid.Enabled = false;
                        area.AxisY.MajorGrid.LineColor = Color.LightGray;
                        area.AxisY.Minimum = 0;
                    }

                    chart2.Titles.Clear();
                }
            }
            catch
            {
                // Intentionally ignore here to avoid crashing the dashboard
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }
    }
}
