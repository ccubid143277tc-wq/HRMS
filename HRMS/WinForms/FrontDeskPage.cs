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
    public partial class FrontDeskPage : Form
    {
        public FrontDeskPage()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
        private void UserControl(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;
            this.panel5.Controls.Clear();
            this.panel5.Controls.Add(uc);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReceptionDashboard receptionDashboard = new ReceptionDashboard();
            UserControl(receptionDashboard);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UCreservation ucr = new UCreservation();
            UserControl(ucr);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UCGuest ucg = new UCGuest();
            UserControl(ucg);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Payment p = new Payment();
            UserControl(p);
        }

        private void receptionDashboard1_Load(object sender, EventArgs e)
        {

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
