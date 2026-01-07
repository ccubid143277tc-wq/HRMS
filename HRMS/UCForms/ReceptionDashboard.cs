using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using HRMS.Services;
using HRMS.Interfaces;

namespace HRMS.UCForms
{
    public partial class ReceptionDashboard : UserControl
    {
        private readonly RoomService _roomService;
        private readonly IReservationService _reservationService;

        public ReceptionDashboard()
        {
            InitializeComponent();

            _roomService = new RoomService();
            _reservationService = new ReservationService(new RoomService(), new GuestService(), new RoomTypeService());
            Load += ReceptionDashboard_Load;
        }

        private void ReceptionDashboard_Load(object sender, EventArgs e)
        {
            try
            {
                LoadOccupancyRate();
                LoadAvailabilityMetrics();
                LoadRoomStatusPieChart();
                LoadWeeklyOccupancyTrendChart();
                LoadExpectedArrivalsTodayGrid();
                LoadExpectedDeparturesTodayGrid();
            }
            catch
            {
                // Intentionally ignore here to avoid crashing the dashboard
            }
        }

        private void LoadExpectedArrivalsTodayGrid()
        {
            var table = _reservationService.GetExpectedArrivalsGridData(DateTime.Today);

            dataGridView1.AutoGenerateColumns = false;

            colReservationID.DataPropertyName = "ReservationID";
            colGuestName.DataPropertyName = "GuestName";
            colRoomNumber.DataPropertyName = "RoomNumbers";
            colRoomType.DataPropertyName = "RoomType";
            colCheckInDate.DataPropertyName = "Check_InDate";
            ColCheckOutDate.DataPropertyName = "Check_OutDate";
            colOccupants.DataPropertyName = "Occupants";
            ColReservationStatus.DataPropertyName = "ReservationStatus";

            dataGridView1.DataSource = table;
        }

        private void LoadExpectedDeparturesTodayGrid()
        {
            var table = _reservationService.GetExpectedDeparturesGridData(DateTime.Today);

            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.Columns.Clear();

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Reservation ID", DataPropertyName = "ReservationID", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Guest", DataPropertyName = "GuestName", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Room(s)", DataPropertyName = "RoomNumbers", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Room Type", DataPropertyName = "RoomType", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Check-In", DataPropertyName = "Check_InDate", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Check-Out", DataPropertyName = "Check_OutDate", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Occupants", DataPropertyName = "Occupants", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Status", DataPropertyName = "ReservationStatus", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            dataGridView2.DataSource = table;
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
            label20.Text = $"{occupancyRate:0}%";

            if (counts.TryGetValue("Available", out int availableRooms))
            {
                label21.Text = availableRooms.ToString();
            }

            if (counts.TryGetValue("Maintenance", out int maintenanceRooms))
            {
                label25.Text = maintenanceRooms.ToString();
            }
        }

        private void LoadAvailabilityMetrics()
        {
            int arrivalsToday = _roomService.GetExpectedArrivalsCount(DateTime.Today);
            int departuresToday = _roomService.GetExpectedDeparturesCount(DateTime.Today);

            label24.Text = arrivalsToday.ToString();
            label26.Text = departuresToday.ToString();
        }

        private void LoadRoomStatusPieChart()
        {
            var counts = _roomService.GetRoomStatusCounts();

            if (chart2.Series.Count == 0)
            {
                chart2.Series.Add(new Series("Series1"));
            }

            var series = chart2.Series[0];
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

            chart2.Titles.Clear();
        }

        private void LoadWeeklyOccupancyTrendChart()
        {
            var counts = _roomService.GetRoomStatusCounts();
            counts.TryGetValue("Total", out int totalRooms);
            if (totalRooms <= 0)
            {
                totalRooms = 1;
            }

            DateTime startDate = DateTime.Today.AddDays(-6);
            var occupiedByDay = _roomService.GetWeeklyOccupiedRoomCounts(startDate, 7);

            if (chart1.Series.Count == 0)
            {
                chart1.Series.Add(new Series("Series1"));
            }

            var series = chart1.Series[0];
            series.Points.Clear();
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 3;
            series.MarkerStyle = MarkerStyle.Circle;
            series.MarkerSize = 6;
            series.LegendText = "Occupancy %";

            foreach (var day in occupiedByDay.OrderBy(k => k.Key))
            {
                double percent = (double)day.Value / totalRooms * 100.0;
                string label = day.Key.ToString("ddd");
                series.Points.AddXY(label, percent);
            }

            if (chart1.ChartAreas.Count > 0)
            {
                var area = chart1.ChartAreas[0];
                area.AxisX.Interval = 1;
                area.AxisY.Minimum = 0;
                area.AxisY.Maximum = 100;
            }
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {
            label19.Text = DateTime.Now.ToString("dddd, MMMM dd yyyy hh:mm:ss tt");
        }
    }
}
