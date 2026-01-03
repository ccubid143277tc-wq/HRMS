using HRMS.WinForms;

namespace HRMS
{
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginAdmin loginAdmin = new LoginAdmin();
            loginAdmin.Show();
        }
    }
}
