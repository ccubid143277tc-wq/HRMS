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
    public partial class EditUser : Form
    {
        private int _userId;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public EditUser(int userId)
        {
            InitializeComponent();
            _userId = userId;
            _userService = new UserService();
            _roleService = new RoleService();

            LoadComboBoxes();

            // Load user details AFTER comboboxes are populated
            this.Load += (s, e) => LoadUserDetails();
        }

       
        private void LoadComboBoxes()
        {
            try
            {
                // Load Roles
                var roles = _roleService.GetAllRoles();
                cmbRole.DataSource = roles;
                cmbRole.DisplayMember = "RoleName";
                cmbRole.ValueMember = "RoleID";
                cmbRole.SelectedIndex = -1;

                // Load Statuses
                cmbStatus.Items.Clear();
                cmbStatus.Items.AddRange(new[] { "Active", "Inactive", "Suspended" });
                cmbStatus.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading comboboxes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUserDetails()
        {
            try
            {
                var user = _userService.GetUserById(_userId);
                if (user != null)
                {
                    txtUsername.Text = user.Username;
                    txtFirstname.Text = user.FirstName;
                    txtLastname.Text = user.LastName;
                    txtEmail.Text = user.Email;
                    txtPhoneNumber.Text = user.Phone;

                    // Set role selection
                    cmbRole.SelectedValue = user.RoleID;

                    // Set status selection
                    cmbStatus.SelectedItem = user.UserStatus;
                }
                else
                {
                    MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private string HashPassword(string password)
        {
            // Simple hashing implementation - in production, use a proper hashing library like BCrypt.Net
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private void BtnUpdate_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Username is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtFirstname.Text))
                {
                    MessageBox.Show("First Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtLastname.Text))
                {
                    MessageBox.Show("Last Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show("Email is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbRole.SelectedValue == null)
                {
                    MessageBox.Show("Please select a role.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbStatus.SelectedItem == null)
                {
                    MessageBox.Show("Please select a status.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Build updated user object
                var user = new User
                {
                    UserID = _userId,
                    Username = txtUsername.Text.Trim(),
                    FirstName = txtFirstname.Text.Trim(),
                    LastName = txtLastname.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtPhoneNumber.Text.Trim(),
                    RoleID = Convert.ToInt32(cmbRole.SelectedValue),
                    UserStatus = cmbStatus.SelectedItem.ToString()
                };

                // Only update password if a new one is provided
                if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    user.PasswordHash = HashPassword(txtPassword.Text.Trim());
                }

                _userService.UpdateUser(user);
                MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
 }

