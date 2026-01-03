namespace HRMS.WinForms
{
    partial class EditUser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            label1 = new Label();
            BtnClear = new Button();
            BtnUpdate = new Button();
            groupBox1 = new GroupBox();
            cmbRole = new ComboBox();
            label7 = new Label();
            label6 = new Label();
            cmbStatus = new ComboBox();
            txtPhoneNumber = new TextBox();
            label11 = new Label();
            txtEmail = new TextBox();
            label10 = new Label();
            txtLastname = new TextBox();
            label5 = new Label();
            txtFirstname = new TextBox();
            label4 = new Label();
            txtPassword = new TextBox();
            label3 = new Label();
            txtUsername = new TextBox();
            label2 = new Label();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(42, 93, 159);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(685, 77);
            panel1.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 24);
            label1.Name = "label1";
            label1.Size = new Size(93, 28);
            label1.TabIndex = 0;
            label1.Text = "Edit User";
            // 
            // BtnClear
            // 
            BtnClear.Location = new Point(470, 456);
            BtnClear.Name = "BtnClear";
            BtnClear.Size = new Size(181, 45);
            BtnClear.TabIndex = 22;
            BtnClear.Text = "Clear";
            BtnClear.UseVisualStyleBackColor = true;
            // 
            // BtnUpdate
            // 
            BtnUpdate.Location = new Point(12, 456);
            BtnUpdate.Name = "BtnUpdate";
            BtnUpdate.Size = new Size(181, 45);
            BtnUpdate.TabIndex = 21;
            BtnUpdate.Text = "Update";
            BtnUpdate.UseVisualStyleBackColor = true;
            BtnUpdate.Click += BtnUpdate_Click_1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cmbRole);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(cmbStatus);
            groupBox1.Controls.Add(txtPhoneNumber);
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(txtEmail);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(txtLastname);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(txtFirstname);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(txtPassword);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtUsername);
            groupBox1.Controls.Add(label2);
            groupBox1.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(12, 83);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(653, 354);
            groupBox1.TabIndex = 20;
            groupBox1.TabStop = false;
            groupBox1.Text = "User Information";
            // 
            // cmbRole
            // 
            cmbRole.DisplayMember = "RoleName";
            cmbRole.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbRole.FormattingEnabled = true;
            cmbRole.Location = new Point(345, 288);
            cmbRole.Name = "cmbRole";
            cmbRole.Size = new Size(281, 31);
            cmbRole.TabIndex = 15;
            cmbRole.ValueMember = "RoleID";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(345, 262);
            label7.Name = "label7";
            label7.Size = new Size(43, 23);
            label7.TabIndex = 14;
            label7.Text = "Role";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(23, 262);
            label6.Name = "label6";
            label6.Size = new Size(56, 23);
            label6.TabIndex = 13;
            label6.Text = "Status";
            // 
            // cmbStatus
            // 
            cmbStatus.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbStatus.FormattingEnabled = true;
            cmbStatus.Items.AddRange(new object[] { "Active", "Inactive", "Suspended" });
            cmbStatus.Location = new Point(23, 288);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(281, 31);
            cmbStatus.TabIndex = 12;
            // 
            // txtPhoneNumber
            // 
            txtPhoneNumber.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPhoneNumber.Location = new Point(345, 214);
            txtPhoneNumber.Name = "txtPhoneNumber";
            txtPhoneNumber.Size = new Size(281, 30);
            txtPhoneNumber.TabIndex = 11;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label11.Location = new Point(345, 188);
            label11.Name = "label11";
            label11.Size = new Size(127, 23);
            label11.TabIndex = 10;
            label11.Text = "Phone Number";
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtEmail.Location = new Point(23, 214);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(281, 30);
            txtEmail.TabIndex = 9;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label10.Location = new Point(23, 188);
            label10.Name = "label10";
            label10.Size = new Size(51, 23);
            label10.TabIndex = 8;
            label10.Text = "Email";
            // 
            // txtLastname
            // 
            txtLastname.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLastname.Location = new Point(345, 142);
            txtLastname.Name = "txtLastname";
            txtLastname.Size = new Size(281, 30);
            txtLastname.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(345, 116);
            label5.Name = "label5";
            label5.Size = new Size(91, 23);
            label5.TabIndex = 6;
            label5.Text = "Last Name";
            // 
            // txtFirstname
            // 
            txtFirstname.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFirstname.Location = new Point(345, 73);
            txtFirstname.Name = "txtFirstname";
            txtFirstname.Size = new Size(281, 30);
            txtFirstname.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(345, 47);
            label4.Name = "label4";
            label4.Size = new Size(92, 23);
            label4.TabIndex = 4;
            label4.Text = "First Name";
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPassword.Location = new Point(23, 142);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(281, 30);
            txtPassword.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(30, 116);
            label3.Name = "label3";
            label3.Size = new Size(80, 23);
            label3.TabIndex = 2;
            label3.Text = "Password";
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUsername.Location = new Point(23, 73);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(281, 30);
            txtUsername.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(23, 47);
            label2.Name = "label2";
            label2.Size = new Size(87, 23);
            label2.TabIndex = 0;
            label2.Text = "Username";
            // 
            // EditUser
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(685, 533);
            Controls.Add(BtnClear);
            Controls.Add(BtnUpdate);
            Controls.Add(groupBox1);
            Controls.Add(panel1);
            Name = "EditUser";
            Text = "EditUser";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Button BtnClear;
        private Button BtnUpdate;
        private GroupBox groupBox1;
        private ComboBox cmbRole;
        private Label label7;
        private Label label6;
        private ComboBox cmbStatus;
        private TextBox txtPhoneNumber;
        private Label label11;
        private TextBox txtEmail;
        private Label label10;
        private TextBox txtLastname;
        private Label label5;
        private TextBox txtFirstname;
        private Label label4;
        private TextBox txtPassword;
        private Label label3;
        private TextBox txtUsername;
        private Label label2;
    }
}