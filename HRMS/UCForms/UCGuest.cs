using HRMS.Interfaces;
using HRMS.Models;
using HRMS.Services;
using HRMS.Helper;
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
    public partial class UCGuest : UserControl
    {
        private readonly IGuestService _guestService;

        private Guest _selectedGuest;
        private bool _isEditMode;

        public UCGuest()
        {
            InitializeComponent();
            _guestService = new GuestService();
            InitializeGuestControls();

            Load += UCGuest_Load;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.CellClick += dataGridView1_CellClick;
            button5.Click += button5_Click;
            textBox1.TextChanged += textBox1_TextChanged;

            RefreshGuestPage();
        }

        private void UCGuest_Load(object sender, EventArgs e)
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

        private void InitializeGuestControls()
        {
            // Initialize ID Type dropdown
            comboBox6.Items.AddRange(new object[] { "Passport", "Driver's License", "National ID", "Other" });
            comboBox6.SelectedIndex = 0;

            // Initialize Classification dropdown
            comboBox7.Items.AddRange(new object[] { "Regular", "VIP", "Corporate", "Family", "Group" });
            comboBox7.SelectedIndex = 0;
        }


        private void label19_Click(object sender, EventArgs e)
        {
            label19.Text = DateTime.Now.ToString("dddd, MMMM dd yyyy hh:mm:ss tt");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        public void LoadGuestData()
        {
            try
            {
                var guests = _guestService.GetAllGuests();

                BindGuestGrid(guests);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading guest data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindGuestGrid(IEnumerable<Guest> guests)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = guests;

            if (dataGridView1.Columns.Contains("GuestID"))
                dataGridView1.Columns["GuestID"].Visible = false;

            if (dataGridView1.Columns.Contains("FirstName"))
                dataGridView1.Columns["FirstName"].Visible = false;

            if (dataGridView1.Columns.Contains("LastName"))
                dataGridView1.Columns["LastName"].Visible = false;

            if (dataGridView1.Columns.Contains("Address"))
                dataGridView1.Columns["Address"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (dataGridView1.Columns.Contains("Email"))
                dataGridView1.Columns["Email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (dataGridView1.Columns.Contains("PhoneNumber"))
                dataGridView1.Columns["PhoneNumber"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (dataGridView1.Columns.Contains("Nationality"))
                dataGridView1.Columns["Nationality"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (dataGridView1.Columns.Contains("DateOfBirth"))
                dataGridView1.Columns["DateOfBirth"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (dataGridView1.Columns.Contains("IDType"))
                dataGridView1.Columns["IDType"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (dataGridView1.Columns.Contains("IDNumber"))
                dataGridView1.Columns["IDNumber"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (dataGridView1.Columns.Contains("Classification"))
                dataGridView1.Columns["Classification"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string keyword = (textBox1.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(keyword) || keyword.Equals("Search", StringComparison.OrdinalIgnoreCase))
            {
                LoadGuestData();
                return;
            }

            try
            {
                var guests = _guestService.SearchGuest(keyword);
                BindGuestGrid(guests);
                dataGridView1.ClearSelection();
                _selectedGuest = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching guest: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshGuestPage()
        {
            ClearGuestForm();
            LoadGuestData();
            dataGridView1.ClearSelection();
            LoadTotalGuestMetric();
        }

        private void LoadTotalGuestMetric()
        {
            try
            {
                int totalGuests = _guestService.GetAllGuests().Count();
                label5.Text = totalGuests.ToString();
            }
            catch
            {
                // Ignore metric failures
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                var row = dataGridView1.Rows[e.RowIndex];
                _selectedGuest = row.DataBoundItem as Guest;
            }
            catch
            {
                _selectedGuest = null;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (_selectedGuest == null)
            {
                MessageBox.Show("Please select a guest from the list first.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                textBox5.Text = _selectedGuest.FirstName ?? "";
                textBox8.Text = _selectedGuest.LastName ?? "";
                textBox7.Text = _selectedGuest.Email ?? "";
                textBox9.Text = _selectedGuest.PhoneNumber ?? "";
                textBox10.Text = _selectedGuest.Address ?? "";
                textBox11.Text = _selectedGuest.IDNumber ?? "";
                textBox12.Text = _selectedGuest.Nationality ?? "";
                dateTimePicker3.Value = _selectedGuest.DateOfBirth == default ? DateTime.Now : _selectedGuest.DateOfBirth;

                if (!string.IsNullOrWhiteSpace(_selectedGuest.IDType))
                {
                    comboBox6.SelectedItem = _selectedGuest.IDType;
                }

                if (!string.IsNullOrWhiteSpace(_selectedGuest.Classification))
                {
                    comboBox7.SelectedItem = _selectedGuest.Classification;
                }

                _isEditMode = true;
                button2.Text = "Save";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading guest for editing: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void ClearGuestForm()
        {
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

            _selectedGuest = null;
            _isEditMode = false;
            button2.Text = "Save";
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (!ValidateGuestFields())
                return;

            try
            {
                if (_isEditMode && _selectedGuest == null && dataGridView1.CurrentRow != null)
                {
                    _selectedGuest = dataGridView1.CurrentRow.DataBoundItem as Guest;
                }

                var guest = new Guest
                {
                    GuestID = _isEditMode ? _selectedGuest?.GuestID ?? 0 : 0,
                    FirstName = textBox5.Text.Trim(),
                    LastName = textBox8.Text.Trim(),
                    Email = textBox7.Text.Trim(),
                    PhoneNumber = textBox9.Text.Trim(),
                    Address = textBox10.Text.Trim(),
                    IDType = comboBox6.SelectedItem?.ToString() ?? "",
                    IDNumber = textBox11.Text.Trim(),
                    Nationality = textBox12.Text.Trim(),
                    DateOfBirth = dateTimePicker3.Value,
                    Classification = comboBox7.SelectedItem?.ToString() ?? ""
                };

                if (_isEditMode)
                {
                    if (guest.GuestID <= 0)
                    {
                        MessageBox.Show("Could not determine which guest to update. Please select a guest and click Edit again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _guestService.UpdateGuest(guest);
                    MessageBox.Show($"Guest '{guest.FullName}' has been successfully updated.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    int guestId = _guestService.AddGuest(guest);
                    if (guestId <= 0)
                    {
                        MessageBox.Show("Failed to save guest information. Please try again.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    MessageBox.Show($"Guest '{guest.FullName}' has been successfully saved with ID: {guestId}",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                RefreshGuestPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving guest: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateGuestFields()
        {
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

            return true;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            ClearGuestForm();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = dataGridView1.SelectedRows[0].DataBoundItem as Guest;
            int guestId = selected?.GuestID ?? 0;
            if (guestId <= 0)
            {
                MessageBox.Show("Could not determine which guest to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Confirm deletion
            DialogResult result = MessageBox.Show("Are you sure you want to delete this Guest?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (_guestService.HasReservationsForGuest(guestId))
                    {
                        MessageBox.Show("This guest cannot be deleted because they have existing reservations. Delete the guest's reservations first (or cancel them) before deleting the guest.",
                            "Cannot Delete Guest", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _guestService.DeleteGuest(guestId);

                    MessageBox.Show("Guest deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    RefreshGuestPage();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting Guest: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
