using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HRMS.Models;
using HRMS.Interfaces;
using HRMS.Services;
using HRMS.UCForms;


namespace HRMS.WinForms
{

    public partial class AddRoom : Form
    {


        private readonly IRoomService _roomService;
        private readonly RoomTypeService _roomTypeService;
        private readonly RoomStatusService _roomStatusService;

        public AddRoom()
        {
            InitializeComponent();
            _roomService = new RoomService();
            _roomTypeService = new RoomTypeService();
            _roomStatusService = new RoomStatusService();

        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            int occupancy = int.Parse(txtMaximumOccupancy.Text);
            int floor = int.Parse(txtRoomFloor.Text);
            decimal rate = decimal.Parse(textBox1.Text);

            try
            {

                var room = new Room
                {
                    RoomNumber = txtRoomNumber.Text.Trim(),
                    BedConfiguration = cmbBedConfiguration.SelectedItem?.ToString(),
                    RoomType = Convert.ToInt32(cmbRoomType.SelectedValue),
                    MaximumOccupancy = occupancy,
                    RoomFloor = floor,
                    RoomRate = rate,
                    RoomStatusID = Convert.ToInt32(cmbRoomStatus.SelectedValue),
                    ViewType = cmbViewType.SelectedItem?.ToString()
                };


                var selectedAmenities = new List<int>();
                if (chckWifi.Checked) selectedAmenities.Add(1);
                if (chckAirConditioning.Checked) selectedAmenities.Add(2);
                if (chckRoomService.Checked) selectedAmenities.Add(5);
                if (chckSafe.Checked) selectedAmenities.Add(7);
                if (chckTV.Checked) selectedAmenities.Add(3);
                if (chckMinibar.Checked) selectedAmenities.Add(4);
                if (chckHairDryer.Checked) selectedAmenities.Add(6);
                if (chckCoffeeMaker.Checked) selectedAmenities.Add(8);


                int newRoomId = _roomService.AddRoom(room);


                foreach (int amenityId in selectedAmenities)
                {
                    _roomService.AddRoomAmenity(newRoomId, amenityId);
                }

                MessageBox.Show("Room added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearForm();
            

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding room: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void AddRoom_Load(object sender, EventArgs e)
        {
            LoadRoomTypes();
            LoadRoomStatuses();

        }
        private void LoadRoomTypes()
        {
            var roomTypes = _roomTypeService.GetAllRoomTypes(); // SELECT * FROM RoomType
            cmbRoomType.DataSource = roomTypes;
            cmbRoomType.DisplayMember = "RoomType";     // shows "Single", "Double", etc.
            cmbRoomType.ValueMember = "RoomTypeID";     // gives the ID (1,2,3...)
            cmbRoomType.SelectedIndex = -1;             // no default selection
        }
        private void LoadRoomStatuses()
        {
            var statuses = _roomStatusService.GetAllRoomStatuses(); // SELECT * FROM RoomStatus
            cmbRoomStatus.DataSource = statuses;
            cmbRoomStatus.DisplayMember = "RoomStatus";     // shows "Available", "Occupied", etc.
            cmbRoomStatus.ValueMember = "RoomStatusID";     // gives the ID
            cmbRoomStatus.SelectedIndex = -1;
        }
        private void ClearForm()
        {
            // Reset textboxes
            txtRoomNumber.Clear();
            txtMaximumOccupancy.Clear();
            txtRoomFloor.Clear();
            textBox1.Clear();

            // Reset comboboxes
            cmbBedConfiguration.SelectedIndex = -1;
            cmbRoomType.SelectedIndex = -1;
            cmbRoomStatus.SelectedIndex = -1;
            cmbViewType.SelectedIndex = -1;

            // Reset checkboxes
            chckWifi.Checked = false;
            chckAirConditioning.Checked = false;
            chckRoomService.Checked = false;
            chckSafe.Checked = false;
            chckTV.Checked = false;
            chckMinibar.Checked = false;
            chckHairDryer.Checked = false;
            chckCoffeeMaker.Checked = false;
        }
      
    }
}

