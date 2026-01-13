using HRMS.Interfaces;
using HRMS.Models;
using HRMS.Services;
using HRMS.Helper;
using MySql.Data.MySqlClient;
using HRMS.DbContext;
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
        private readonly IRoomTypeService _roomTypeService;
        private readonly IGuestService _guestService;
        private readonly IRoomService _roomService;
        private readonly RoomManager _roomManager;
        private readonly GuestManager _guestManager;
        private Room? _selectedRoom;
        private List<Room> _selectedRooms = new List<Room>(); // Track multiple selected rooms
        private Reservation? _selectedReservation;
        private object? _selectedReservationData;
        private bool _isEditMode = false;

        private int _selectedReservationIdForSummary = 0;
        private decimal _lastSummaryTotalAmount = 0m;

        public UCreservation()
        {
            InitializeComponent();
            _guestService = new MySqlGuestDbContext();
            _guestManager = new GuestManager(_guestService);
            _roomService = new MySqlRoomDbContext();
            _roomManager = new RoomManager(_roomService);
            _roomTypeService = new RoomTypeService();
            _reservationService = new MySqlReservationDbContext();
            InitializeGuestControls();

            if (radioButton7 != null) radioButton7.CheckedChanged += PaymentOption_Changed;
            if (radioButton8 != null) radioButton8.CheckedChanged += PaymentOption_Changed;
            if (radioButton9 != null) radioButton9.CheckedChanged += PaymentOption_Changed;
            if (textBox6 != null) textBox6.TextChanged += PaymentOption_Changed;

            Load += UCreservation_Load;
        }

        private void PaymentOption_Changed(object? sender, EventArgs e)
        {
            try
            {
                UpdateAdvancePaymentPreview();
            }
            catch
            {
                // Intentionally ignore here to avoid crashing the page
            }
        }

        private void UpdateAdvancePaymentPreview()
        {
            if (_selectedReservationIdForSummary <= 0)
            {
                return;
            }

            decimal totalDue = GetTotalDueForReservation(_selectedReservationIdForSummary);
            if (totalDue <= 0m)
            {
                totalDue = MoneyHelper.Parse(label22.Text);
            }

            decimal alreadyPaid = GetTotalPaidForReservation(_selectedReservationIdForSummary);
            decimal balance = totalDue - alreadyPaid;
            if (balance < 0m)
            {
                balance = 0m;
            }

            if (radioButton9.Checked) // Full Payment
            {
                textBox6.Enabled = false;
                textBox6.Text = balance.ToString("0.00");
                label39.Text = $"Advance Payment (optional) - Remaining: ₱{balance:0.00}";
                return;
            }

            if (radioButton8.Checked) // 50% Payment
            {
                decimal targetPaid = Math.Round(totalDue * 0.5m, 2);
                decimal amountToPay = Math.Round(targetPaid - alreadyPaid, 2);
                if (amountToPay < 0m)
                {
                    amountToPay = 0m;
                }
                if (amountToPay > balance)
                {
                    amountToPay = balance;
                }

                decimal remainingAfter = Math.Round(balance - amountToPay, 2);
                if (remainingAfter < 0m)
                {
                    remainingAfter = 0m;
                }

                textBox6.Enabled = false;
                textBox6.Text = amountToPay.ToString("0.00");
                label39.Text = $"Advance Payment (optional) - Remaining: ₱{remainingAfter:0.00}";
                return;
            }

            // Custom Payment
            textBox6.Enabled = true;
            string raw = (textBox6.Text ?? "").Trim();
            decimal custom = MoneyHelper.Parse(raw);
            if (custom < 0m)
            {
                custom = 0m;
            }
            if (custom > balance)
            {
                custom = balance;
            }
            decimal remaining = Math.Round(balance - custom, 2);
            if (remaining < 0m)
            {
                remaining = 0m;
            }
            label39.Text = $"Advance Payment (optional) - Remaining: ₱{remaining:0.00}";
        }

        private void UCreservation_Load(object? sender, EventArgs e)
        {
            try
            {
                ApplyCurrentUserToHeader();
            }
            catch
            {
                // Intentionally ignore here to avoid crashing the page
            }
        }

        private decimal GetTotalDueForReservation(int reservationId)
        {
            if (reservationId <= 0)
            {
                return 0m;
            }

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();

                string query = @"SELECT
                                    ROUND((COALESCE(SUM(DISTINCT rm.RoomRate), 0)
                                          * COALESCE(r.numberOfNights, DATEDIFF(r.Check_OutDate, r.Check_InDate)))
                                         * 1.05, 2) AS TotalDue
                                 FROM reservations r
                                 LEFT JOIN ReservationRooms rr ON r.ReservationID = rr.ReservationID
                                 LEFT JOIN Rooms rm ON rm.RoomID = COALESCE(rr.RoomID, r.RoomID)
                                 WHERE r.ReservationID = @ReservationID
                                 GROUP BY r.ReservationID, r.numberOfNights, r.Check_InDate, r.Check_OutDate";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ReservationID", reservationId);
                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                    {
                        return 0m;
                    }

                    if (decimal.TryParse(result.ToString(), out decimal totalDue))
                    {
                        return totalDue;
                    }

                    return Convert.ToDecimal(result);
                }
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

        private void label19_Click(object sender, EventArgs e)
        {
            label19.Text = DateTime.Now.ToString("dddd, MMMM dd yyyy hh:mm:ss tt");
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            CalculateNumberOfNights();
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            CalculateNumberOfNights();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAvailableRooms();
        }

        private void LoadRoomTypes()
        {
            try
            {
                var roomTypes = _roomManager.GetRoomTypes();
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Store selected reservation data
                _selectedReservationData = new
                {
                    GuestName = row.Cells["colGuestName"].Value?.ToString() ?? "",
                    BookingReference = row.Cells["colBookingReference"].Value?.ToString() ?? "",
                    ReservationType = row.Cells["colReservationType"].Value?.ToString() ?? "",
                    RoomNumber = row.Cells["ColRoomNumber"].Value?.ToString() ?? "",
                    RoomType = row.Cells["ColRoomType"].Value?.ToString() ?? "",
                    NumberOfNights = row.Cells["ColNumberOfNights"].Value?.ToString() ?? "0",
                    NumberOfOccupants = row.Cells["colNumberOfOccupants"].Value?.ToString() ?? "0",
                    SpecialRequest = row.Cells["colSpecialRequest"].Value?.ToString() ?? "",
                    Status = row.Cells["colReservationStatus"].Value?.ToString() ?? "",
                    NumberOfRooms = row.Cells["colNumberOfRooms"].Value?.ToString() ?? "1"
                };
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                var room = new Room
                {
                    RoomID = Convert.ToInt32(row.Cells["RoomNumber"].Tag),
                    RoomNumber = row.Cells["RoomNumber"].Value.ToString(),
                    RoomTypeName = row.Cells["RoomType"].Value.ToString(),
                    RoomStatusName = row.Cells["RoomStatus"].Value.ToString(),
                    RoomFloor = Convert.ToInt32(row.Cells["RoomFloor"].Value),
                    ViewType = row.Cells["ViewType"].Value.ToString()
                };

                // For backward compatibility, keep _selectedRoom as the first selected room
                _selectedRoom = room;
                
                // Add to multiple rooms list if not already present
                if (!_selectedRooms.Any(r => r.RoomID == room.RoomID))
                {
                    _selectedRooms.Add(room);
                }
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

                var availableRooms = _roomManager.GetAvailableRoomsByType(selectedRoomType);

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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate all required information
                if (!ValidateReservationInformation())
                {
                    return;
                }

                // Create guest object
                var guest = new Guest
                {
                    FirstName = textBox5.Text.Trim(),
                    LastName = textBox8.Text.Trim(),
                    Email = textBox7.Text.Trim(),
                    PhoneNumber = textBox9.Text.Trim(),
                    Address = textBox10.Text.Trim(),
                    IDNumber = textBox11.Text.Trim(),
                    Nationality = textBox12.Text.Trim(),
                    DateOfBirth = dateTimePicker3.Value,
                    IDType = comboBox6.SelectedItem?.ToString() ?? "",
                    Classification = comboBox7.SelectedItem?.ToString() ?? ""
                };

                // Save or update guest
                int guestId;
                if (_isEditMode && _selectedReservation != null)
                {
                    // Update existing guest
                    var existingGuest = _guestManager.GetGuestById(_selectedReservation.GuestID);
                    if (existingGuest != null)
                    {
                        guest.GuestID = existingGuest.GuestID;
                        _guestManager.UpdateGuest(guest);
                        guestId = existingGuest.GuestID;
                    }
                    else
                    {
                        guestId = _guestManager.AddGuest(guest);
                    }
                }
                else
                {
                    // Add new guest
                    guestId = _guestManager.AddGuest(guest);
                }

                if (guestId <= 0)
                {
                    MessageBox.Show("Failed to save guest information. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Create reservation object
                var reservation = new Reservation
                {
                    GuestID = guestId,
                    Check_InDate = dateTimePicker1.Value,
                    Check_OutDate = dateTimePicker2.Value,
                    NumAdult = Convert.ToInt32(comboBox3.SelectedItem?.ToString() ?? "1"),
                    NumChild = Convert.ToInt32(comboBox4.SelectedItem?.ToString() ?? "0"),
                    SpecialRequest = BuildSpecialRequestFromCheckboxes(),
                    ReservationStatus = comboBox2.SelectedItem?.ToString() ?? "Confirmed",
                    ReservationType = comboBox8.SelectedItem?.ToString() ?? "Walk-in",
                    BookingReferences = GenerateBookingReference(), // This is the key fix!
                    RoomID = _selectedRooms.FirstOrDefault()?.RoomID ?? 0,
                    RoomTypeID = _selectedRooms.FirstOrDefault()?.RoomType ?? 0
                };

                int reservationId;
                if (_isEditMode && _selectedReservation != null)
                {
                    // Update existing reservation
                    reservation.ReservationID = _selectedReservation.ReservationID;
                    _reservationService.UpdateReservation(reservation);
                    reservationId = _selectedReservation.ReservationID;
                }
                else
                {
                    // Add new reservation
                    reservationId = _reservationService.AddReservation(reservation);
                }

                if (reservationId > 0)
                {
                    if (!_isEditMode)
                    {
                        var roomIds = _selectedRooms
                            .Select(r => r.RoomID)
                            .Where(id => id > 0)
                            .Distinct()
                            .ToList();

                        if (roomIds.Count > 0)
                        {
                            _reservationService.AddReservationRooms(reservationId, roomIds);
                        }
                    }

                    // Update room statuses to "Occupied"
                    foreach (var room in _selectedRooms)
                    {
                        _roomManager.UpdateRoomStatus(room.RoomID, "Occupied");
                    }

                    MessageBox.Show($"Reservation created successfully!\n\nGuest: {guest.FullName}\nBooking Reference: {reservation.BookingReferences}\nReservation ID: {reservationId}\nNumber of Rooms: {_selectedRooms.Count}\nCheck-In: {dateTimePicker1.Value:yyyy-MM-dd}\nCheck-Out: {dateTimePicker2.Value:yyyy-MM-dd}",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearAllFields();

                    // Refresh both datagrids
                    LoadReservationData();
                    LoadAvailableRooms(); // Refresh room list to show updated statuses
                }
                else
                {
                    MessageBox.Show("Failed to create reservation. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving reservation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    // reservation.RoomNumber now holds a comma-separated room list (e.g. "101, 102")
                    string displayRoomNumbers = reservation.RoomNumber;
                    string numberOfRooms = "1"; // Default

                    if (!string.IsNullOrWhiteSpace(reservation.RoomNumber))
                    {
                        var roomNumbers = reservation.RoomNumber
                            .Split(',')
                            .Select(r => r.Trim())
                            .Where(r => !string.IsNullOrWhiteSpace(r))
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .ToList();

                        numberOfRooms = roomNumbers.Count.ToString();
                        displayRoomNumbers = string.Join(", ", roomNumbers);
                    }
                    
                    dataGridView1.Rows.Add(
                        reservation.GuestName,        // colGuestName
                        reservation.BookingReferences ?? "", // colBookingReference
                        reservation.ReservationType ?? "", // colReservationType
                        displayRoomNumbers,          // ColRoomNumber
                        reservation.RoomTypeName,      // ColRoomType
                        reservation.TotalDays.ToString(), // ColNumberOfNights
                        reservation.TotalGuests.ToString(), // colNumberOfOccupants
                        reservation.SpecialRequest,    // colSpecialRequest
                        reservation.ReservationStatus,   // colReservationStatus
                        numberOfRooms                // colNumberOfRooms
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

            comboBox8.Items.AddRange(new object[] { "Walk-in", "Online", "Phone", "Agent" });
            comboBox8.SelectedIndex = 0;

            comboBox2.Items.AddRange(new object[] { "Confirmed", "Checked-In", "Checked-Out", "Cancelled" });
            comboBox2.SelectedIndex = 0;

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

            // Calculate initial number of nights
            CalculateNumberOfNights();
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

            if (string.IsNullOrWhiteSpace(textBox3.Text) || !int.TryParse(textBox3.Text, out _))
            {
                MessageBox.Show("Number of Nights is required and must be valid", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimePicker1.Focus();
                return false;
            }

            // Validate Room Selection (only required for new reservations)
            if (!_isEditMode && _selectedRooms.Count == 0)
            {
                MessageBox.Show("Please select at least one room from the Room Available list", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dataGridView2.Focus();
                return false;
            }

            if (comboBox8.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a reservation type", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox8.Focus();
                return false;
            }

            if (comboBox2.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a reservation status", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox2.Focus();
                return false;
            }

            return true;
        }

        private void ClearAllFields()
        {
            try
            {
                textBox5.Clear();
                textBox8.Clear();
                textBox7.Clear();
                textBox9.Clear();
                textBox10.Clear();
                textBox11.Clear();

                textBox3.Clear();
                

                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox1.Checked = false;


                comboBox6.SelectedIndex = -1;
                comboBox7.SelectedIndex = -1;
                comboBox8.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;

                if (comboBox3.Items.Count > 0) comboBox3.SelectedIndex = 0;
                if (comboBox4.Items.Count > 0) comboBox4.SelectedIndex = 0;

                _selectedRoom = null;
                _selectedRooms.Clear();
                _selectedReservation = null;
                _selectedReservationData = null;
                _isEditMode = false;

                // Clear available rooms selection
                dataGridView2.ClearSelection();
            }
            catch
            {
            }
        }

        private string BuildSpecialRequestFromCheckboxes()
        {
            var selected = new List<string>();

            if (checkBox3.Checked) selected.Add(checkBox3.Text);
            if (checkBox4.Checked) selected.Add(checkBox4.Text);
            if (checkBox5.Checked) selected.Add(checkBox5.Text);
            if (checkBox6.Checked) selected.Add(checkBox6.Text);
            if (checkBox7.Checked) selected.Add(checkBox7.Text);
            if (checkBox1.Checked) selected.Add(checkBox1.Text);

            return string.Join(", ", selected);
        }

        private void ApplySpecialRequestToCheckboxes(string? specialRequest)
        {
            string src = (specialRequest ?? string.Empty).Trim();

            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox1.Checked = false;

            if (string.IsNullOrWhiteSpace(src))
            {
                return;
            }

            var parts = src
                .Split(',')
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (parts.Contains(checkBox3.Text)) checkBox3.Checked = true;
            if (parts.Contains(checkBox4.Text)) checkBox4.Checked = true;
            if (parts.Contains(checkBox5.Text)) checkBox5.Checked = true;
            if (parts.Contains(checkBox6.Text)) checkBox6.Checked = true;
            if (parts.Contains(checkBox7.Text)) checkBox7.Checked = true;
            if (parts.Contains(checkBox1.Text)) checkBox1.Checked = true;
        }

        private void ResetEditMode()
        {
            _isEditMode = false;
            _selectedReservation = null;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (_selectedReservationData == null)
            {
                MessageBox.Show("Please select a reservation from the list first", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get reservation details for confirmation
            var guestName = _selectedReservationData.GetType().GetProperty("GuestName")?.GetValue(_selectedReservationData)?.ToString() ?? "";
            var bookingReference = _selectedReservationData.GetType().GetProperty("BookingReference")?.GetValue(_selectedReservationData)?.ToString() ?? "";
            var roomNumber = _selectedReservationData.GetType().GetProperty("RoomNumber")?.GetValue(_selectedReservationData)?.ToString() ?? "";

            // Confirm deletion
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete this reservation?\n\n" +
                $"Guest: {guestName}\n" +
                $"Booking Reference: {bookingReference}\n" +
                $"Room: {roomNumber}\n\n" +
                "This action cannot be undone.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Extract ReservationID from the selected row
                    int reservationId = 0;
                    var selectedRow = dataGridView1.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault();
                    if (selectedRow != null)
                    {
                        // Try to get ReservationID from the row's DataBoundItem or from the database
                        var reservation = _reservationService.GetReservationGridData()
                            .FirstOrDefault(r => r.GuestName == guestName && 
                                           r.BookingReferences == bookingReference && 
                                           r.RoomNumber == roomNumber);
                        if (reservation != null)
                        {
                            reservationId = reservation.ReservationID;
                        }
                    }

                    if (reservationId > 0)
                    {
                        _reservationService.DeleteReservation(reservationId);
                        
                        MessageBox.Show("Reservation deleted successfully!", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refresh the datagrid
                        LoadReservationData();

                        // Refresh the room list to reflect status updates
                        LoadAvailableRooms();
                        
                        // Clear selection
                        _selectedReservationData = null;
                    }
                    else
                    {
                        MessageBox.Show("Could not find the reservation to delete.", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting reservation: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (_selectedReservationData == null)
            {
                MessageBox.Show("Please select a reservation from the list first", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Get reservation data using reflection
                var guestName = _selectedReservationData.GetType().GetProperty("GuestName")?.GetValue(_selectedReservationData)?.ToString() ?? "";
                var bookingReference = _selectedReservationData.GetType().GetProperty("BookingReference")?.GetValue(_selectedReservationData)?.ToString() ?? "";
                var roomNumber = _selectedReservationData.GetType().GetProperty("RoomNumber")?.GetValue(_selectedReservationData)?.ToString() ?? "";
                var roomType = _selectedReservationData.GetType().GetProperty("RoomType")?.GetValue(_selectedReservationData)?.ToString() ?? "";
                var numberOfNights = _selectedReservationData.GetType().GetProperty("NumberOfNights")?.GetValue(_selectedReservationData)?.ToString() ?? "0";
                var numberOfOccupants = _selectedReservationData.GetType().GetProperty("NumberOfOccupants")?.GetValue(_selectedReservationData)?.ToString() ?? "0";
                var specialRequest = _selectedReservationData.GetType().GetProperty("SpecialRequest")?.GetValue(_selectedReservationData)?.ToString() ?? "";
                var status = _selectedReservationData.GetType().GetProperty("Status")?.GetValue(_selectedReservationData)?.ToString() ?? "";
                var numberOfRooms = _selectedReservationData.GetType().GetProperty("NumberOfRooms")?.GetValue(_selectedReservationData)?.ToString() ?? "1";

                // If some cells are blank (common when the grid was refreshed), re-fetch from DB by booking reference.
                if (!string.IsNullOrWhiteSpace(bookingReference) && (string.IsNullOrWhiteSpace(roomNumber) || string.IsNullOrWhiteSpace(roomType)))
                {
                    var dbReservation = _reservationService.GetReservationGridData()
                        .FirstOrDefault(r => string.Equals(r.BookingReferences, bookingReference, StringComparison.OrdinalIgnoreCase));

                    if (dbReservation != null)
                    {
                        guestName = string.IsNullOrWhiteSpace(guestName) ? (dbReservation.GuestName ?? "") : guestName;
                        roomNumber = string.IsNullOrWhiteSpace(roomNumber) ? (dbReservation.RoomNumber ?? "") : roomNumber;
                        roomType = string.IsNullOrWhiteSpace(roomType) ? (dbReservation.RoomTypeName ?? "") : roomType;

                        if (string.IsNullOrWhiteSpace(numberOfNights) || numberOfNights == "0")
                        {
                            numberOfNights = dbReservation.TotalDays.ToString();
                        }

                        if (string.IsNullOrWhiteSpace(numberOfOccupants) || numberOfOccupants == "0")
                        {
                            numberOfOccupants = dbReservation.TotalGuests.ToString();
                        }

                        specialRequest = string.IsNullOrWhiteSpace(specialRequest) ? (dbReservation.SpecialRequest ?? "") : specialRequest;
                        status = string.IsNullOrWhiteSpace(status) ? (dbReservation.ReservationStatus ?? "") : status;

                        if (string.IsNullOrWhiteSpace(numberOfRooms) || numberOfRooms == "0")
                        {
                            var roomCount = (dbReservation.RoomNumber ?? "")
                                .Split(',')
                                .Select(r => r.Trim())
                                .Where(r => !string.IsNullOrWhiteSpace(r))
                                .Distinct(StringComparer.OrdinalIgnoreCase)
                                .Count();
                            numberOfRooms = Math.Max(1, roomCount).ToString();
                        }
                    }
                }

                // Update the summary panel (panel4)
                UpdateReservationSummary(guestName, roomNumber, roomType, numberOfNights, numberOfOccupants, specialRequest, status, bookingReference, numberOfRooms);

                MessageBox.Show("Reservation summary updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing reservation summary: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateBookingReference()
        {
            // Get current year
            string year = DateTime.Now.Year.ToString();

            try
            {
                // Get all existing booking references for this year
                var existingReferences = _reservationService.GetAllReservations()
                    .Where(r => r.BookingReferences != null && r.BookingReferences.StartsWith($"BKG-{year}"))
                    .Select(r => r.BookingReferences)
                    .ToList();

                // Extract the numeric part and find the highest number
                int maxNumber = 0;
                foreach (var reference in existingReferences)
                {
                    // Extract the last 4 digits from reference like "BKG-2026-0001"
                    string numberPart = reference.Substring(reference.Length - 4);
                    if (int.TryParse(numberPart, out int number))
                    {
                        maxNumber = Math.Max(maxNumber, number);
                    }
                }

                // Generate the next number (increment by 1)
                int nextNumber = maxNumber + 1;

                // Format as 4-digit number with leading zeros
                string formattedNumber = nextNumber.ToString("D4");

                // Return the complete booking reference
                return $"BKG-{year}-{formattedNumber}";
            }
            catch (Exception ex)
            {
                // Fallback to timestamp-based reference if database query fails
                return $"BKG-{year}-{DateTime.Now.ToString("HHmmss")}";
            }
        }

        private void UpdateReservationSummary(string guestName, string roomNumber, string roomType, string numberOfNights, string numberOfOccupants, string specialRequest, string status, string bookingReference, string numberOfRooms)
        {
            try
            {
                // Get total per-night room rate from database (supports comma-separated room numbers)
                decimal totalRoomRatePerNight = GetTotalRoomRateFromDatabase(roomNumber);
                decimal subtotal = totalRoomRatePerNight * Convert.ToInt32(numberOfNights);
                decimal tax = subtotal * 0.05m; // 5% tax
                decimal total = subtotal + tax;

                _lastSummaryTotalAmount = total;
                _selectedReservationIdForSummary = GetReservationIdByBookingReference(bookingReference);

                // Update panel4 labels
                label6.Text = $"₱{totalRoomRatePerNight:F2}"; // Room Rate (per night) for all selected rooms
                label7.Text = numberOfNights; // Number of Nights
                label37.Text = bookingReference; // Booking Reference
                label8.Text = $"₱{subtotal:F2}"; // Room Subtotal = room rate × number of nights
                label21.Text = $"₱{tax:F2}"; // Tax
                label22.Text = $"₱{total:F2}"; // Total Amount
                label51.Text = numberOfRooms; // Number of Rooms
                
                // You can also update other labels if needed
                // For example, update special requests or guest info in other areas

                UpdateAdvancePaymentPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating summary: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetReservationIdByBookingReference(string bookingReference)
        {
            if (string.IsNullOrWhiteSpace(bookingReference))
            {
                return 0;
            }

            try
            {
                var reservation = _reservationService.GetAllReservations()
                    .FirstOrDefault(r => string.Equals(r.BookingReferences, bookingReference, StringComparison.OrdinalIgnoreCase));
                return reservation?.ReservationID ?? 0;
            }
            catch
            {
                return 0;
            }
        }

        private decimal GetTotalPaidForReservation(int reservationId)
        {
            if (reservationId <= 0)
            {
                return 0m;
            }

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = @"SELECT COALESCE(SUM(p.amount), 0)
                                 FROM payment p
                                 WHERE p.ReservationID = @ReservationID
                                   AND (p.Payment_Status IS NULL OR p.Payment_Status <> 'Voided')
                                   AND (p.Payment_method IS NULL OR p.Payment_method <> 'Additional Service')
                                   AND (p.Payment_Status IS NULL OR p.Payment_Status <> 'Charge')";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ReservationID", reservationId);
                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                    {
                        return 0m;
                    }
                    return Convert.ToDecimal(result);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (_selectedReservationIdForSummary <= 0)
            {
                MessageBox.Show("Please select a reservation and open the summary first.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBox1.SelectedItem == null || string.IsNullOrWhiteSpace(comboBox1.SelectedItem.ToString()) || comboBox1.Text == "Select Payment")
            {
                MessageBox.Show("Please select a payment method.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (UserSession.CurrentUserId <= 0)
            {
                MessageBox.Show("Please login first before processing payments.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                decimal totalDue = GetTotalDueForReservation(_selectedReservationIdForSummary);
                if (totalDue <= 0m)
                {
                    totalDue = _lastSummaryTotalAmount;
                    if (totalDue <= 0m)
                    {
                        totalDue = MoneyHelper.Parse(label22.Text);
                    }
                }

                decimal alreadyPaid = GetTotalPaidForReservation(_selectedReservationIdForSummary);
                decimal balance = totalDue - alreadyPaid;
                if (balance <= 0m)
                {
                    MessageBox.Show("This reservation is already fully paid.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                decimal amountToPay;
                string paymentStatus;
                if (radioButton9.Checked) // Full Payment
                {
                    amountToPay = balance;
                    paymentStatus = "Paid";
                }
                else if (radioButton8.Checked) // 50% Payment
                {
                    // Pay enough so that TotalPaid becomes 50% of TotalDue.
                    // Example: TotalDue=1000, alreadyPaid=200 => target=500 => pay 300.
                    decimal targetPaid = Math.Round(totalDue * 0.5m, 2);
                    amountToPay = Math.Round(targetPaid - alreadyPaid, 2);

                    if (amountToPay <= 0m)
                    {
                        MessageBox.Show("This reservation already has at least 50% paid.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Never allow paying more than the remaining balance
                    if (amountToPay > balance)
                    {
                        amountToPay = balance;
                    }

                    paymentStatus = "Pending";
                }
                else if (radioButton7.Checked) // Custom Payment
                {
                    string customText = (textBox6.Text ?? "").Trim();
                    if (string.IsNullOrWhiteSpace(customText) || customText.Equals("Enter Amount", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Please enter a custom payment amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!decimal.TryParse(customText, out amountToPay))
                    {
                        MessageBox.Show("Invalid amount. Please enter a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    paymentStatus = "Pending";
                }
                else
                {
                    MessageBox.Show("Please choose a payment option (Full / 50% / Custom).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                amountToPay = Math.Round(amountToPay, 2);
                if (amountToPay <= 0m)
                {
                    MessageBox.Show("Payment amount must be greater than 0.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (amountToPay > balance)
                {
                    MessageBox.Show($"Payment amount cannot exceed the remaining balance.\n\nBalance: ₱{balance:F2}", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string method = comboBox1.SelectedItem.ToString();

                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string insert = @"INSERT INTO payment (amount, ReservationID, Payment_method, Payment_Status, UserID, Payment_Date)
                                      VALUES (@amount, @ReservationID, @Payment_method, @Payment_Status, @UserID, @Payment_Date)";

                    using (var cmd = new MySqlCommand(insert, conn))
                    {
                        cmd.Parameters.AddWithValue("@amount", amountToPay);
                        cmd.Parameters.AddWithValue("@ReservationID", _selectedReservationIdForSummary);
                        cmd.Parameters.AddWithValue("@Payment_method", method);
                        cmd.Parameters.AddWithValue("@Payment_Status", paymentStatus);
                        cmd.Parameters.AddWithValue("@UserID", UserSession.CurrentUserId);
                        cmd.Parameters.AddWithValue("@Payment_Date", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }

                decimal updatedPaid = GetTotalPaidForReservation(_selectedReservationIdForSummary);
                decimal updatedBalance = totalDue - updatedPaid;
                if (updatedBalance < 0m)
                {
                    updatedBalance = 0m;
                }

                MessageBox.Show($"Payment recorded successfully!\n\nPaid Now: ₱{amountToPay:F2}\nTotal Paid: ₱{updatedPaid:F2}\nBalance: ₱{updatedBalance:F2}",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reset advance payment UI
                radioButton7.Checked = false;
                radioButton8.Checked = false;
                radioButton9.Checked = false;
                textBox6.Text = "Enter Amount";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing payment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private decimal GetTotalRoomRateFromDatabase(string roomNumbers)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roomNumbers))
                {
                    return 0m;
                }

                var requestedRoomNumbers = roomNumbers
                    .Split(',')
                    .Select(r => r.Trim())
                    .Where(r => !string.IsNullOrWhiteSpace(r))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (requestedRoomNumbers.Count == 0)
                {
                    return 0m;
                }

                var allRooms = _roomManager.GetAllRooms().ToList();
                decimal totalRate = 0m;

                foreach (var rn in requestedRoomNumbers)
                {
                    var room = allRooms.FirstOrDefault(r => string.Equals(r.RoomNumber, rn, StringComparison.OrdinalIgnoreCase));
                    if (room != null)
                    {
                        totalRate += room.RoomRate;
                    }
                }

                return totalRate;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching room rate: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0m;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (_selectedReservationData == null)
            {
                MessageBox.Show("Please select a reservation from the list first", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Get reservation data using reflection
                var guestName = _selectedReservationData.GetType().GetProperty("GuestName")?.GetValue(_selectedReservationData)?.ToString() ?? "";
                var roomNumber = _selectedReservationData.GetType().GetProperty("RoomNumber")?.GetValue(_selectedReservationData)?.ToString() ?? "";
                var roomType = _selectedReservationData.GetType().GetProperty("RoomType")?.GetValue(_selectedReservationData)?.ToString() ?? "";
                var numberOfNights = _selectedReservationData.GetType().GetProperty("NumberOfNights")?.GetValue(_selectedReservationData)?.ToString() ?? "0";
                var numberOfOccupants = _selectedReservationData.GetType().GetProperty("NumberOfOccupants")?.GetValue(_selectedReservationData)?.ToString() ?? "0";
                var specialRequest = _selectedReservationData.GetType().GetProperty("SpecialRequest")?.GetValue(_selectedReservationData)?.ToString() ?? "";
                var status = _selectedReservationData.GetType().GetProperty("Status")?.GetValue(_selectedReservationData)?.ToString() ?? "";

                var reservationType = _selectedReservationData.GetType().GetProperty("ReservationType")?.GetValue(_selectedReservationData)?.ToString() ?? "";

                // Parse guest name to get first and last name (assuming format "FirstName LastName")
                string[] nameParts = guestName.Split(new[] { ' ' }, 2);
                string firstName = nameParts.Length > 0 ? nameParts[0] : "";
                string lastName = nameParts.Length > 1 ? nameParts[1] : "";

                // Get the reservation ID from the database using booking reference
                string bookingReference = _selectedReservationData.GetType().GetProperty("BookingReference")?.GetValue(_selectedReservationData)?.ToString() ?? "";
                var existingReservation = _reservationService.GetAllReservations()
                    .FirstOrDefault(r => r.BookingReferences == bookingReference);

                if (existingReservation != null)
                {
                    _selectedReservation = existingReservation;
                    _isEditMode = true;
                }

                if (_selectedReservation == null)
                {
                    MessageBox.Show("Could not load the reservation for editing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var guest = _guestManager.GetGuestById(_selectedReservation.GuestID);

                // Make sure the guest information panel is visible and brought to front
                panel10.Visible = true;
                panel10.BringToFront();

                // Populate guest fields
                textBox5.Text = guest?.FirstName ?? firstName;
                textBox8.Text = guest?.LastName ?? lastName;
                textBox7.Text = guest?.Email ?? "";
                textBox9.Text = guest?.PhoneNumber ?? "";
                textBox10.Text = guest?.Address ?? "";
                textBox11.Text = guest?.IDNumber ?? "";
                textBox12.Text = guest?.Nationality ?? "";
                dateTimePicker3.Value = guest?.DateOfBirth ?? DateTime.Now;

                if (!string.IsNullOrWhiteSpace(guest?.IDType))
                {
                    comboBox6.SelectedItem = guest.IDType;
                }

                if (!string.IsNullOrWhiteSpace(guest?.Classification))
                {
                    comboBox7.SelectedItem = guest.Classification;
                }

                // Populate reservation fields
                dateTimePicker1.Value = _selectedReservation.Check_InDate;
                dateTimePicker2.Value = _selectedReservation.Check_OutDate;
                textBox3.Text = _selectedReservation.TotalDays.ToString();

                ApplySpecialRequestToCheckboxes(_selectedReservation.SpecialRequest);

                comboBox3.SelectedItem = _selectedReservation.NumAdult.ToString();
                comboBox4.SelectedItem = _selectedReservation.NumChild.ToString();

                textBox13.Text = _selectedReservation.BookingReferences ?? bookingReference;
                textBox13.ReadOnly = true;

                // Reservation type + status (prefer actual values if loaded)
                if (!string.IsNullOrWhiteSpace(_selectedReservation.ReservationType))
                {
                    comboBox8.SelectedItem = _selectedReservation.ReservationType;
                }
                else if (!string.IsNullOrWhiteSpace(reservationType))
                {
                    comboBox8.SelectedItem = reservationType;
                }

                if (!string.IsNullOrWhiteSpace(_selectedReservation.ReservationStatus))
                {
                    comboBox2.SelectedItem = _selectedReservation.ReservationStatus;
                }
                else if (!string.IsNullOrWhiteSpace(status))
                {
                    comboBox2.SelectedItem = status;
                }

                // Set room type
                for (int i = 0; i < comboBox5.Items.Count; i++)
                {
                    if (comboBox5.Items[i].ToString() == roomType)
                    {
                        comboBox5.SelectedIndex = i;
                        break;
                    }
                }

                // Load available rooms for the selected room type
                LoadAvailableRooms();

                // Only select a room if it's different from current reservation room
                // or if no room is currently selected
                string currentRoomNumber = _selectedReservation?.RoomNumber ?? "";
                if (!string.IsNullOrEmpty(currentRoomNumber) || currentRoomNumber != roomNumber)
                {
                    // Select the specific room in the room list
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        if (row.Cells["RoomNumber"].Value?.ToString() == roomNumber)
                        {
                            row.Selected = true;
                            dataGridView2.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                    }
                }
                else
                {
                    // Keep the existing room selection, don't force re-selection
                    _selectedRoom = _roomManager.GetAllRooms()
                        .FirstOrDefault(r => r.RoomNumber == currentRoomNumber);
                }

                MessageBox.Show("Guest information loaded for editing. Make your changes and click 'Save Reservation' to update.",
                    "Edit Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading reservation for editing: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void CalculateNumberOfNights()
        {
            try
            {
                DateTime checkIn = dateTimePicker1.Value.Date;
                DateTime checkOut = dateTimePicker2.Value.Date;

                if (checkOut > checkIn)
                {
                    int nights = (int)(checkOut - checkIn).TotalDays;
                    textBox3.Text = nights.ToString();
                }
                else
                {
                    textBox3.Text = "";
                }
            }
            catch (Exception ex)
            {
                textBox3.Text = "";
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

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            // Get all selected rooms
            var selectedRooms = new List<Room>();
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                if (row.Cells["RoomNumber"].Tag != null)
                {
                    int roomId = Convert.ToInt32(row.Cells["RoomNumber"].Tag);
                    var room = _roomManager.GetAllRooms().FirstOrDefault(r => r.RoomID == roomId);
                    if (room != null)
                    {
                        selectedRooms.Add(room);
                    }
                }
            }

            if (selectedRooms.Count == 0)
            {
                MessageBox.Show("Please select at least one room to update", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int updatedCount = 0;
                foreach (var room in selectedRooms)
                {
                    // Update room status in database
                    bool success = _roomManager.UpdateRoomStatus(room.RoomID, "Available");
                    
                    if (success)
                    {
                        updatedCount++;
                    }
                }

                if (updatedCount > 0)
                {
                    string roomNumbers = string.Join(", ", selectedRooms.Select(r => r.RoomNumber));
                    MessageBox.Show($"Successfully updated {updatedCount} room(s): {roomNumbers}",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh the room list
                    LoadAvailableRooms();

                    // Clear selection
                    _selectedRoom = null;
                    dataGridView2.ClearSelection();
                }
                else
                {
                    MessageBox.Show("Failed to update room status", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating room status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
