using System;
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
        }
        private void button1_Click_1(object sender, EventArgs e)
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
                UserSession.CurrentUserRole = string.IsNullOrWhiteSpace(user.RoleName) ? "" : user.RoleName;

                Form next = CreateNextFormByRole(UserSession.CurrentUserRole);

                next.Show();
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Form CreateNextFormByRole(string roleName)
        {
            string role = (roleName ?? "").Trim();

            if (string.Equals(role, "Hotel Manager", StringComparison.OrdinalIgnoreCase))
            {
                return new AdminPage();
            }

            if (string.Equals(role, "Receptionist", StringComparison.OrdinalIgnoreCase))
            {
                return new FrontDeskPage();
            }

            // Fallback: if role not recognized, send to front desk (safer default)
            return new FrontDeskPage();
        }

        private void chckShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chckShowPassword.Checked)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }
    }
}
