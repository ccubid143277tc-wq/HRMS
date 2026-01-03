using HRMS.Interfaces;
using HRMS.Models;
using HRMS.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HRMS.WinForms
{
    public partial class EditRoom : Form
    {
        private int _roomId;
        private readonly IRoomService _roomService;
        private readonly RoomTypeService _roomTypeService;
        private readonly RoomStatusService _roomStatusService;
        
        public EditRoom(int roomId)
        {
            InitializeComponent();
            _roomId = roomId;
            _roomService = new RoomService();
            _roomTypeService = new RoomTypeService();
            _roomStatusService = new RoomStatusService();

            LoadComboBoxes();
            
            // Load room details AFTER comboboxes are populated
            this.Load += (s, e) => LoadRoomDetails();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Build updated room object
                var room = new Room
                {
                    RoomID = _roomId, 
                    RoomNumber = txtRoomNumber.Text.Trim(),
                    BedConfiguration = cmbBedConfiguration.SelectedItem?.ToString(),
                    RoomType = Convert.ToInt32(cmbRoomType.SelectedValue),
                    MaximumOccupancy = int.Parse(txtMaximumOccupancy.Text),
                    RoomFloor = int.Parse(txtRoomFloor.Text),
                    RoomRate = decimal.Parse(textBox1.Text),
                    RoomStatusID = Convert.ToInt32(cmbRoomStatus.SelectedValue),
                    ViewType = cmbViewType.SelectedItem?.ToString(),
                    Amenities = GetSelectedAmenities() 
                };

               
                _roomService.UpdateRoom(room);

                MessageBox.Show("Room updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close(); // close the edit form
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating room: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private List<string> GetSelectedAmenities()
        {
            var amenities = new List<string>();

            if (chckWifi.Checked) amenities.Add("Wifi");
            if (chckAirConditioning.Checked) amenities.Add("Air Conditioning");
            if (chckRoomService.Checked) amenities.Add("Room Service");
            if (chckSafe.Checked) amenities.Add("Safe");
            if (chckTV.Checked) amenities.Add("TV");
            if (chckMinibar.Checked) amenities.Add("Minibar");
            if (chckHairDryer.Checked) amenities.Add("Hair Dryer");
            if (chckCoffeeMaker.Checked) amenities.Add("Coffee Maker");

            return amenities;
        }

        private void LoadComboBoxes()
        {
            try
            {
                // Load Room Types
                var roomTypes = _roomTypeService.GetAllRoomTypes();
                cmbRoomType.DataSource = roomTypes;
                cmbRoomType.DisplayMember = "RoomTypeName";
                cmbRoomType.ValueMember = "RoomTypeID";
                cmbRoomType.SelectedIndex = -1;

                // Load Room Statuses
                var statuses = _roomStatusService.GetAllRoomStatuses();
                cmbRoomStatus.DataSource = statuses;
                cmbRoomStatus.DisplayMember = "RoomStatusName";
                cmbRoomStatus.ValueMember = "RoomStatusID";
                cmbRoomStatus.SelectedIndex = -1;

             
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading comboboxes: " + ex.Message);
            }
        }

        private void LoadRoomDetails()
        {
            try
            {
                var room = _roomService.GetRoomById(_roomId);

                if (room != null)
                {
                    txtRoomNumber.Text = room.RoomNumber;
                    
                    // Set BedConfiguration
                    if (!string.IsNullOrEmpty(room.BedConfiguration))
                    {
                        cmbBedConfiguration.SelectedItem = room.BedConfiguration;
                    }
                    
                    // Set RoomType by value
                    if (cmbRoomType.Items.Count > 0)
                    {
                        cmbRoomType.SelectedValue = room.RoomType;
                    }
                    
                    txtMaximumOccupancy.Text = room.MaximumOccupancy.ToString();
                    txtRoomFloor.Text = room.RoomFloor.ToString();
                    textBox1.Text = room.RoomRate.ToString("F2");
                    
                    // Set RoomStatus by value
                    if (cmbRoomStatus.Items.Count > 0)
                    {
                        cmbRoomStatus.SelectedValue = room.RoomStatusID;
                    }
                    
                    // Set ViewType
                    if (!string.IsNullOrEmpty(room.ViewType))
                    {
                        cmbViewType.SelectedItem = room.ViewType;
                    }

                    // Load room amenities from database
                    LoadRoomAmenities(_roomId);
                }
                else
                {
                    MessageBox.Show("Room not found!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading room details: " + ex.Message);
            }
        }

        private void LoadRoomAmenities(int roomId)
        {
            try
            {
                // Get amenities for this room from RoomAmenities table
                // You'll need to add a method to get room amenities
                var roomAmenities = _roomService.GetRoomAmenities(roomId);
                
                // Reset all checkboxes first
                chckWifi.Checked = false;
                chckAirConditioning.Checked = false;
                chckRoomService.Checked = false;
                chckSafe.Checked = false;
                chckTV.Checked = false;
                chckMinibar.Checked = false;
                chckHairDryer.Checked = false;
                chckCoffeeMaker.Checked = false;

                // Check the appropriate boxes based on room amenities
                foreach (var amenity in roomAmenities)
                {
                    switch (amenity)
                    {
                        case 1: chckWifi.Checked = true; break;
                        case 2: chckAirConditioning.Checked = true; break;
                        case 3: chckTV.Checked = true; break;
                        case 4: chckMinibar.Checked = true; break;
                        case 5: chckRoomService.Checked = true; break;
                        case 6: chckHairDryer.Checked = true; break;
                        case 7: chckSafe.Checked = true; break;
                        case 8: chckCoffeeMaker.Checked = true; break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading amenities: " + ex.Message);
            }
        }

    }
}
