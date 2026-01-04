using HRMS.Interfaces;
using HRMS.Models;
using HRMS.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HRMS.UCForms
{
    public partial class UCreservation : UserControl
    {
        private readonly IReservationService _reservationService;
        private readonly IGuestService _guestService;
        private readonly IRoomService _roomService;
        private Room _selectedRoom;
        private Reservation _selectedReservation;

        public UCreservation()
        {
            InitializeComponent();
            _guestService = new GuestService();
            _reservationService = new ReservationService();
            _roomService = new RoomService();
            InitializeGuestControls();
        }

        private void label19_Click(object sender, EventArgs e)
        {
            label19.Text = DateTime.Now.ToString("dddd, MMMM dd yyyy hh:mm:ss tt");
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAvailableRooms();
        }

        private void LoadRoomTypes()
        {
            try
            {
                var roomTypes = _roomService.GetRoomTypes();
                comboBox5.Items.Clear();
                
                foreach (var roomType in roomTypes)
                {
                    comboBox5.Items.Add(roomType.RoomTypeName);
                }
                
                if (comboBox5.Items.Count > 0)
                    comboBox5.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading room types: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // Fallback to default options
                comboBox5.Items.AddRange(new object[] { "Standard", "Deluxe", "Suite", "Executive", "Presidential" });
                if (comboBox5.Items.Count > 0)
                    comboBox5.SelectedIndex = 0;
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                
                _selectedRoom = new Room
                {
                    RoomID = Convert.ToInt32(row.Cells["RoomNumber"].Tag),
                    RoomNumber = row.Cells["RoomNumber"].Value.ToString(),
                    RoomTypeName = row.Cells["RoomType"].Value.ToString(),
                    RoomStatusName = row.Cells["RoomStatus"].Value.ToString(),
                    RoomFloor = Convert.ToInt32(row.Cells["RoomFloor"].Value),
                    ViewType = row.Cells["ViewType"].Value.ToString()
                };

                // Show selected room info
                MessageBox.Show($"Selected Room: {_selectedRoom.RoomNumber}\nType: {_selectedRoom.RoomTypeName}\nFloor: {_selectedRoom.RoomFloor}", 
                    "Room Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadAvailableRooms()
        {
            try
            {
                string selectedRoomType = comboBox5.SelectedItem?.ToString();
                
                // Always clear the datagrid first
                dataGridView2.AutoGenerateColumns = false;
                dataGridView2.DataSource = null;
                dataGridView2.Rows.Clear();
                
                if (string.IsNullOrEmpty(selectedRoomType))
                {
                    return;
                }

                var availableRooms = _roomService.GetAvailableRoomsByType(selectedRoomType);
                
                // Add room data manually to match designer columns
                foreach (var room in availableRooms)
                {
                    int rowIndex = dataGridView2.Rows.Add(
                        room.RoomNumber,        // RoomNumber
                        room.RoomTypeName,      // RoomType
                        room.RoomStatusName,    // RoomStatus
                        room.RoomFloor.ToString(), // RoomFloor
                        room.ViewType           // ViewType
                    );
                    
                    // Store RoomID in the Tag property of the first cell for later retrieval
                    dataGridView2.Rows[rowIndex].Cells["RoomNumber"].Tag = room.RoomID;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading available rooms: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      

        private void button1_Click(object sender, EventArgs e)
        {
            ClearAllFields();
        }

        public void LoadReservationData()
        {
            try
            {
                var reservations = _reservationService.GetReservationGridData();

                // Clear existing rows and turn off auto-column generation
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                
                // Add reservation data manually to match designer columns
                foreach (var reservation in reservations)
                {
                    dataGridView1.Rows.Add(
                        reservation.GuestName,        // colGuestName
                        reservation.RoomNumber,      // ColRoomNumber
                        reservation.RoomTypeName,      // ColRoomType
                        reservation.TotalDays.ToString(), // ColNumberOfNights
                        reservation.TotalGuests.ToString(), // colNumberOfOccupants
                        reservation.SpecialRequest,    // colSpecialRequest
                        reservation.ReservationStatus   // colReservationStatus
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading reservation data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeGuestControls()
        {
            // Initialize ID Type dropdown
            comboBox6.Items.AddRange(new object[] { "Passport", "Driver's License", "National ID", "Other" });
            comboBox6.SelectedIndex = 0;

            // Initialize Classification dropdown
            comboBox7.Items.AddRange(new object[] { "Regular", "VIP", "Corporate", "Family", "Group" });
            comboBox7.SelectedIndex = 0;

            // Initialize Adults and Children dropdowns
            comboBox3.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8" });
            comboBox3.SelectedIndex = 0;

            comboBox4.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8" });
            comboBox4.SelectedIndex = 0;

            // Initialize Room Type dropdown with actual room types from database
            LoadRoomTypes();

            // Load reservation data
            LoadReservationData();
            
            // Load initial available rooms
            LoadAvailableRooms();
        }

        private bool ValidateReservationInformation()
        {
            // Validate Guest Information
            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please enter First Name", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox5.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox8.Text))
            {
                MessageBox.Show("Please enter Last Name", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox8.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox7.Text))
            {
                MessageBox.Show("Please enter Email Address", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox7.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox9.Text))
            {
                MessageBox.Show("Please enter Phone Number", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox9.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox10.Text))
            {
                MessageBox.Show("Please enter Address", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox10.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox11.Text))
            {
                MessageBox.Show("Please enter ID Number", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox11.Focus();
                return false;
            }

            // Validate Reservation Details
            if (dateTimePicker1.Value >= dateTimePicker2.Value)
            {
                MessageBox.Show("Check-Out date must be after Check-In date", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimePicker2.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please enter Number of Nights", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus();
                return false;
            }

            // Validate Room Selection
            if (_selectedRoom == null)
            {
                MessageBox.Show("Please select a room from the Room Available list", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dataGridView2.Focus();
                return false;
            }

            return true;
        }

        private void ClearAllFields()
        {
            // Clear Guest Information
            textBox5.Clear();
            textBox8.Clear();
            textBox7.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            comboBox6.SelectedIndex = 0;
            comboBox7.SelectedIndex = 0;
            dateTimePicker3.Value = DateTime.Now;

            // Clear Reservation Details
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now.AddDays(1);
            textBox3.Clear();
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            textBox4.Clear();

            // Clear selected room
            _selectedRoom = null;
            dataGridView2.ClearSelection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!ValidateReservationInformation())
                return;

            try
            {
                // First, create and save the guest
                var guest = new Guest
                {
                    FirstName = textBox5.Text.Trim(),
                    LastName = textBox8.Text.Trim(),
                    Email = textBox7.Text.Trim(),
                    PhoneNumber = textBox9.Text.Trim(),
                    Address = textBox10.Text.Trim(),
                    IDType = comboBox6.SelectedItem.ToString(),
                    IDNumber = textBox11.Text.Trim(),
                    Nationality = textBox12.Text.Trim(),
                    DateOfBirth = dateTimePicker3.Value,
                    Classification = comboBox7.SelectedItem.ToString()
                };

                int guestId = _guestService.AddGuest(guest);
                
                if (guestId <= 0)
                {
                    MessageBox.Show("Failed to save guest information. Please try again.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Then, create and save the reservation
                var reservation = new Reservation
                {
                    GuestID = guestId,
                    Check_InDate = dateTimePicker1.Value,
                    Check_OutDate = dateTimePicker2.Value,
                    NumAdult = Convert.ToInt32(comboBox3.SelectedItem),
                    NumChild = Convert.ToInt32(comboBox4.SelectedItem),
                    SpecialRequest = textBox4.Text.Trim(),
                    ReservationStatus = "Confirmed",
                    RoomID = _selectedRoom?.RoomID ?? 1, // Use selected room or default
                    GuestName = guest.FullName,
                    RoomNumber = _selectedRoom?.RoomNumber ?? "TBD",
                    RoomTypeName = _selectedRoom?.RoomTypeName ?? comboBox5.SelectedItem.ToString()
                };

                int reservationId = _reservationService.AddReservation(reservation);
                
                if (reservationId > 0)
                {
                    MessageBox.Show($"Reservation created successfully!\nGuest: {guest.FullName}\nReservation ID: {reservationId}\nCheck-In: {dateTimePicker1.Value:yyyy-MM-dd}\nCheck-Out: {dateTimePicker2.Value:yyyy-MM-dd}", 
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    ClearAllFields();
                    
                    // Refresh both datagrids
                    RefreshGuestData();
                    LoadReservationData();
                }
                else
                {
                    MessageBox.Show("Failed to create reservation. Please try again.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while creating reservation: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshGuestData()
        {
            // Find UCGuest control and refresh its data
            var parentForm = this.FindForm();
            if (parentForm != null)
            {
                var ucGuest = parentForm.Controls.Find("UCGuest", true).FirstOrDefault() as UCGuest;
                if (ucGuest != null)
                {
                    ucGuest.LoadGuestData();
                }
            }
        }
    }
}
