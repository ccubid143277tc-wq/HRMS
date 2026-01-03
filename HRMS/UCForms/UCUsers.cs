using HRMS.Interfaces;
using HRMS.Models;
using HRMS.Services;
using HRMS.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HRMS.UCForms
{
    public partial class UCUsers : UserControl
    {
        private readonly IUserService _userService;

        public UCUsers()
        {
            InitializeComponent();
            _userService = new UserService();
            this.Load += UCUsers_Load;
        }

        private void UCUsers_Load(object sender, EventArgs e)
        {
            RefreshUsersGrid();
            LoadUserStatusCounts();
        }

        private void RefreshUsersGrid()
        {
            try
            {
                var userData = _userService.GetUserGridData();

                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = userData;

                // Hide internal columns
                dataGridView1.Columns["UserID"].Visible = false;
                dataGridView1.Columns["PasswordHash"].Visible = false;
                dataGridView1.Columns["RoleID"].Visible = false;

                // Set column widths
                dataGridView1.Columns["Username"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["FirstName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["LastName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["Email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["Phone"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["RoleName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["UserStatus"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["CreatedAt"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUserStatusCounts()
        {
            try
            {
                var counts = _userService.GetUserStatusCounts();

                // Update labels (assuming you have these labels on your form)
                if (label3 != null) label3.Text = counts["Total"].ToString();
                if (label5 != null) label5.Text = counts["Active"].ToString();
                if (label9 != null) label9.Text = counts["Inactive"].ToString();
                if (label7 != null) label7.Text = counts["Suspended"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user counts: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddNewUser_Click(object sender, EventArgs e)
        {
            AddUsers ad = new AddUsers();
            ad.Show();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshUsersGrid();
            LoadUserStatusCounts();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int userid = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["UserID"].Value);
            EditUser eu = new EditUser(userid);
            eu.ShowDialog();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int userId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["UserID"].Value);

            // Confirm deletion
            DialogResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Delete user
                    _userService.DeleteUser(userId);

                    MessageBox.Show("User deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh the grid and counts
                    RefreshUsersGrid();
                    LoadUserStatusCounts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

         
            var results = _userService.SearchUser(keyword);

            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();


            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = results.ToList();
           

          
            if (dataGridView1.Columns.Contains("UserID"))
                dataGridView1.Columns["UserID"].Visible = false;
            if (dataGridView1.Columns.Contains("PasswordHash"))
                dataGridView1.Columns["PasswordHash"].Visible = false;

            // Adjust column widths
            dataGridView1.Columns["Username"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["FirstName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["LastName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["Email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["Phone"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["RoleName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["UserStatus"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["CreatedAt"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
    }
}


