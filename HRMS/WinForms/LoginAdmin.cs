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
            // Validate that both username and password are provided before attempting login.

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
                // stores user information in Usersession  
                    UserSession.CurrentUserId = user.UserID;
                UserSession.CurrentUserName = user.Username;
                UserSession.CurrentUserRole = string.IsNullOrWhiteSpace(user.RoleName) ? "Admin" : user.RoleName;
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
