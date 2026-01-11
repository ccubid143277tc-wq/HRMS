using HRMS.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HRMS.Services;  

namespace HRMS.UCForms
{
    public partial class UCRooms : UserControl
    {
        private readonly RoomService _roomService;
        public UCRooms()
        {
            InitializeComponent();
            _roomService = new RoomService();
            button1.Click += button1_Click;


        }

        private void BtnAddNewRoom_Click(object sender, EventArgs e)
        {
            AddRoom AR = new AddRoom();
            AR.Show();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a room to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the RoomID from the selected row (make sure your query includes RoomID)
            int roomId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["RoomID"].Value);

            // Open the EditRoom form and pass the RoomID
            EditRoom editForm = new EditRoom(roomId);
            editForm.ShowDialog();


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void UCRooms_Load(object sender, EventArgs e)
        {

            var roomData = _roomService.GetRoomGridData();


            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();


            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = roomData;
            dataGridView1.Columns["RoomID"].Visible = false;
            dataGridView1.Columns["RoomNumber"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["RoomType"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["BedConfiguration"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["MaximumOccupancy"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["RoomFloor"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["RoomStatus"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["ViewType"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["RoomRate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["Amenities"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            var counts = _roomService.GetRoomStatusCounts();

            label3.Text = counts["Total"].ToString();
            label5.Text = counts["Available"].ToString();
            label9.Text = counts["Occupied"].ToString();
            label7.Text = counts["Maintenance"].ToString();
            LoadRoomStatusCounts();



        }
        private void LoadRoomStatusCounts()
        {
            var counts = _roomService.GetRoomStatusCounts();

            label3.Text = counts["Total"].ToString();
            label5.Text = counts["Available"].ToString();
            label9.Text = counts["Occupied"].ToString();
            label7.Text = counts["Maintenance"].ToString();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a room to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get RoomID from selected row
            int roomId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["RoomID"].Value);

            // Confirm deletion
            DialogResult result = MessageBox.Show("Are you sure you want to delete this room?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Delete room and its amenities
                    _roomService.DeleteRoom(roomId);

                    MessageBox.Show("Room deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh the Room page
                    // or ucRooms.RefreshRooms() if you're calling from outside
                    LoadRoomStatusCounts(); // optional: refresh dashboard
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting room: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string keyword = txtSearchBar.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Please enter a search term.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Call your service method
            var results = _roomService.SearchRooms(keyword);

            // Bind results to DataGridView
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();


            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = results.ToList();
            // IEnumerable → List

            // Hide RoomID
            if (dataGridView1.Columns.Contains("RoomID"))
                dataGridView1.Columns["RoomID"].Visible = false;

            // Adjust column widths
            var roomNumberCol = dataGridView1.Columns["RoomNumber"];
            if (roomNumberCol != null) roomNumberCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            var roomTypeNameCol = dataGridView1.Columns["RoomTypeName"];
            if (roomTypeNameCol != null) roomTypeNameCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            var bedConfigurationCol = dataGridView1.Columns["BedConfiguration"];
            if (bedConfigurationCol != null) bedConfigurationCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            var maximumOccupancyCol = dataGridView1.Columns["MaximumOccupancy"];
            if (maximumOccupancyCol != null) maximumOccupancyCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            var roomFloorCol = dataGridView1.Columns["RoomFloor"];
            if (roomFloorCol != null) roomFloorCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            var roomStatusNameCol = dataGridView1.Columns["RoomStatusName"];
            if (roomStatusNameCol != null) roomStatusNameCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            var viewTypeCol = dataGridView1.Columns["ViewType"];
            if (viewTypeCol != null) viewTypeCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            var roomRateCol = dataGridView1.Columns["RoomRate"];
            if (roomRateCol != null) roomRateCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            var amenitiesStringCol = dataGridView1.Columns["AmenitiesString"];
            if (amenitiesStringCol != null) amenitiesStringCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
    }
}
