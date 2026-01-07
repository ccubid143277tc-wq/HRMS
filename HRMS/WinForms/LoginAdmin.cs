using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HRMS.Helper;
using HRMS.Services;

namespace HRMS.WinForms
{
    public partial class LoginAdmin : Form
    {
        public LoginAdmin()
        {
            InitializeComponent();

            txtPassword.UseSystemPasswordChar = true;
            button1.Click += button1_Click;
            chckShowPassword.CheckedChanged += chckShowPassword_CheckedChanged;
            txtPassword.KeyDown += TxtPassword_KeyDown;
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void chckShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chckShowPassword.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text?.Trim() ?? "";
            string password = txtPassword.Text ?? "";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter username and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var userService = new UserService();
                var user = userService.AuthenticateUser(username, password);
                if (user == null)
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UserSession.CurrentUserId = user.UserID;
                UserSession.CurrentUserName = user.Username;
                UserSession.CurrentUserRole = string.IsNullOrWhiteSpace(user.RoleName) ? "Admin" : user.RoleName;

                // Prefer AdminPage if present; fallback to FrontDeskPage
                Form next;
                try
                {
                    next = new AdminPage();
                }
                catch
                {
                    next = new FrontDeskPage();
                }

                next.Show();
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
