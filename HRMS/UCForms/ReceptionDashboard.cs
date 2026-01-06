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

namespace HRMS.UCForms
{
    public partial class ReceptionDashboard : UserControl
    {
        private readonly RoomService _roomService;

        public ReceptionDashboard()
        {
            InitializeComponent();

            _roomService = new RoomService();
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
            }
            catch
            {
                // Intentionally ignore here to avoid crashing the dashboard
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
