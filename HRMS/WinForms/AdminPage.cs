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
    }
}
