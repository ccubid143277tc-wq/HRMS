using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HRMS.UCForms
{
    public partial class AdminDashboard : UserControl
    {
        public AdminDashboard()
        {
            InitializeComponent();
        }

        private void label19_Click(object sender, EventArgs e)
        {
            label19.Text = DateTime.Now.ToString("dddd, MMMM dd yyyy hh:mm:ss tt");
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
