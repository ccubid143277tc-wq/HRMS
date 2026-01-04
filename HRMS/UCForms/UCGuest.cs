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
    public partial class UCGuest : UserControl
    {
        private readonly IGuestService _guestService;

        public UCGuest()
        {
            InitializeComponent();
            _guestService = new GuestService();
            InitializeGuestControls();
            LoadGuestData();
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

                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = guests;

                // Hide GuestID column
                if (dataGridView1.Columns.Contains("GuestID"))
                    dataGridView1.Columns["GuestID"].Visible = false;

                // Configure column widths and display
                if (dataGridView1.Columns.Contains("FirstName"))
                    dataGridView1.Columns["FirstName"].Visible = false;

                if (dataGridView1.Columns.Contains("LastName"))
                    dataGridView1.Columns["LastName"].Visible = false;

                // Create a combined GuestName column if needed, or use existing columns
                if (dataGridView1.Columns.Contains("FirstName") && dataGridView1.Columns.Contains("LastName"))
                {
                    // We'll handle this in the query or create a computed property
                }

                // Configure visible columns
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading guest data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!ValidateGuestFields())
                return;

            try
            {
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
                
                if (guestId > 0)
                {
                    MessageBox.Show($"Guest '{guest.FullName}' has been successfully saved with ID: {guestId}", 
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    ClearGuestForm();
                    LoadGuestData();
                }
                else
                {
                    MessageBox.Show("Failed to save guest information. Please try again.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving guest: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearGuestForm();
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
        }
    }
}
