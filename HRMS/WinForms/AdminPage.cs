using HRMS.UCForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HRMS.Helper;

namespace HRMS.WinForms
{
    public partial class AdminPage : Form
    {
        public AdminPage()
        {
            InitializeComponent();
        }
        private void UserControl(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;
            this.panel3.Controls.Clear();
            this.panel3.Controls.Add(uc);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UCRooms UCR = new UCRooms();
            UserControl(UCR);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UCUsers UCU = new UCUsers();
            UserControl(UCU);
        }

        private void AdminPage_Load(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminDashboard Ad = new AdminDashboard();
            UserControl(Ad);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UCGuest uCGuest = new UCGuest();
            UserControl(uCGuest);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UCreservation uCReservation = new UCreservation();
            UserControl(uCReservation);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Payment p = new Payment();
            UserControl(p);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            UCReports UCR = new UCReports();
            UserControl(UCR);  
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
            {
                return;
            }

            UserSession.CurrentUserId = 0;
            UserSession.CurrentUserName = "";
            UserSession.CurrentUserRole = "";

            Form? login = Application.OpenForms.OfType<LoginAdmin>().FirstOrDefault();
            if (login == null)
            {
                login = Application.OpenForms.OfType<StaffLogin>().FirstOrDefault();
            }

            login ??= new LoginAdmin();

            login.Show();
            login.BringToFront();

            Close();
        }
    }
}
