using HRMS.UCForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
            label5.Text = DateTime.Now.ToString("dddd, MMMM dd yyyy hh:mm:ss tt");
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
    }
}
