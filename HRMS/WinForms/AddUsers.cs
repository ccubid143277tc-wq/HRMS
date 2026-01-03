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
using System.Linq;


namespace HRMS.WinForms
{
    public partial class AddUsers : Form
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        
        public AddUsers()
        {
            InitializeComponent();
            _userService = new UserService();
            _roleService = new RoleService();
            LoadRoles();
            LoadStatuses();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Username is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Password is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                var newUser = new User
                {
                    Username = txtUsername.Text.Trim(),
                    PasswordHash = HashPassword(txtPassword.Text.Trim()), // implement hashing
                    FirstName = txtFirstname.Text.Trim(),
                    LastName = txtLastname.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtPhoneNumber.Text.Trim(),
                    RoleID = Convert.ToInt32(cmbRole.SelectedValue),
                    UserStatus = cmbStatus.SelectedItem.ToString()
                };

                int userId = _userService.AddUser(newUser);
                MessageBox.Show($"User added successfully! ID: {userId}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearForm()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtFirstname.Clear();
            txtLastname.Clear();
            txtEmail.Clear();
            txtPhoneNumber.Clear();
            cmbRole.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
        }

        private void LoadRoles()
        {
            try
            {
                var roles = _roleService.GetAllRoles();
                cmbRole.DataSource = roles;
                cmbRole.DisplayMember = "RoleName";
                cmbRole.ValueMember = "RoleID";
                cmbRole.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading roles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStatuses()
        {
            try
            {
                // Load user statuses (you can hardcode these or load from database)
                cmbStatus.Items.Clear();
                cmbStatus.Items.AddRange(new[] { "Active", "Inactive", "Suspended" });
                cmbStatus.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading statuses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
