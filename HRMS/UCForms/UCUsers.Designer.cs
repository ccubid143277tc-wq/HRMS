namespace HRMS.UCForms
{
    partial class UCUsers
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCUsers));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            panel1 = new Panel();
            label1 = new Label();
            BtnEdit = new Button();
            BtnDelete = new Button();
            dataGridView1 = new DataGridView();
            colUsername = new DataGridViewTextBoxColumn();
            colPassword = new DataGridViewTextBoxColumn();
            colFullname = new DataGridViewTextBoxColumn();
            colEmail = new DataGridViewTextBoxColumn();
            colPhoneNumber = new DataGridViewTextBoxColumn();
            colUserStatus = new DataGridViewTextBoxColumn();
            colRole = new DataGridViewTextBoxColumn();
            colCreateAt = new DataGridViewTextBoxColumn();
            colUpdatedAt = new DataGridViewTextBoxColumn();
            BtnAddNewUser = new Button();
            button1 = new Button();
            txtSearchBar = new TextBox();
            panel8 = new Panel();
            panel9 = new Panel();
            label8 = new Label();
            label9 = new Label();
            panel4 = new Panel();
            panel5 = new Panel();
            label4 = new Label();
            label5 = new Label();
            panel2 = new Panel();
            panel3 = new Panel();
            label2 = new Label();
            label3 = new Label();
            panel6 = new Panel();
            panel7 = new Panel();
            label6 = new Label();
            label7 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel8.SuspendLayout();
            panel9.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel6.SuspendLayout();
            panel7.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(42, 93, 159);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1924, 104);
            panel1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(25, 30);
            label1.Name = "label1";
            label1.Size = new Size(252, 38);
            label1.TabIndex = 0;
            label1.Text = "User Management";
            // 
            // BtnEdit
            // 
            BtnEdit.BackColor = Color.White;
            BtnEdit.FlatStyle = FlatStyle.Flat;
            BtnEdit.ForeColor = Color.FromArgb(42, 93, 159);
            BtnEdit.Image = (Image)resources.GetObject("BtnEdit.Image");
            BtnEdit.ImageAlign = ContentAlignment.MiddleLeft;
            BtnEdit.Location = new Point(1702, 205);
            BtnEdit.Name = "BtnEdit";
            BtnEdit.Padding = new Padding(0, 0, 10, 0);
            BtnEdit.Size = new Size(88, 29);
            BtnEdit.TabIndex = 14;
            BtnEdit.Text = "Edit";
            BtnEdit.TextAlign = ContentAlignment.MiddleRight;
            BtnEdit.UseVisualStyleBackColor = false;
            BtnEdit.Click += BtnEdit_Click;
            // 
            // BtnDelete
            // 
            BtnDelete.BackColor = Color.FromArgb(192, 0, 0);
            BtnDelete.FlatAppearance.BorderSize = 0;
            BtnDelete.FlatStyle = FlatStyle.Flat;
            BtnDelete.ForeColor = Color.White;
            BtnDelete.Image = (Image)resources.GetObject("BtnDelete.Image");
            BtnDelete.ImageAlign = ContentAlignment.MiddleLeft;
            BtnDelete.Location = new Point(1796, 205);
            BtnDelete.Name = "BtnDelete";
            BtnDelete.Size = new Size(88, 29);
            BtnDelete.TabIndex = 13;
            BtnDelete.Text = "Delete";
            BtnDelete.TextAlign = ContentAlignment.MiddleRight;
            BtnDelete.UseVisualStyleBackColor = false;
            BtnDelete.Click += BtnDelete_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(42, 93, 159);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { colUsername, colPassword, colFullname, colEmail, colPhoneNumber, colUserStatus, colRole, colCreateAt, colUpdatedAt });
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.Location = new Point(18, 240);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1866, 628);
            dataGridView1.TabIndex = 12;
            // 
            // colUsername
            // 
            colUsername.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colUsername.HeaderText = "Username";
            colUsername.MinimumWidth = 6;
            colUsername.Name = "colUsername";
            // 
            // colPassword
            // 
            colPassword.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colPassword.HeaderText = "Password";
            colPassword.MinimumWidth = 6;
            colPassword.Name = "colPassword";
            // 
            // colFullname
            // 
            colFullname.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colFullname.HeaderText = "Fullname";
            colFullname.MinimumWidth = 6;
            colFullname.Name = "colFullname";
            // 
            // colEmail
            // 
            colEmail.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colEmail.HeaderText = "Email";
            colEmail.MinimumWidth = 6;
            colEmail.Name = "colEmail";
            // 
            // colPhoneNumber
            // 
            colPhoneNumber.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colPhoneNumber.HeaderText = "Phone Number";
            colPhoneNumber.MinimumWidth = 6;
            colPhoneNumber.Name = "colPhoneNumber";
            // 
            // colUserStatus
            // 
            colUserStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colUserStatus.HeaderText = "Status";
            colUserStatus.MinimumWidth = 6;
            colUserStatus.Name = "colUserStatus";
            // 
            // colRole
            // 
            colRole.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colRole.HeaderText = "Role";
            colRole.MinimumWidth = 6;
            colRole.Name = "colRole";
            // 
            // colCreateAt
            // 
            colCreateAt.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colCreateAt.HeaderText = "CreatedAt";
            colCreateAt.MinimumWidth = 6;
            colCreateAt.Name = "colCreateAt";
            // 
            // colUpdatedAt
            // 
            colUpdatedAt.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colUpdatedAt.HeaderText = "UpdatedAt";
            colUpdatedAt.MinimumWidth = 6;
            colUpdatedAt.Name = "colUpdatedAt";
            // 
            // BtnAddNewUser
            // 
            BtnAddNewUser.BackColor = Color.FromArgb(42, 93, 159);
            BtnAddNewUser.FlatAppearance.BorderSize = 0;
            BtnAddNewUser.FlatStyle = FlatStyle.Flat;
            BtnAddNewUser.ForeColor = Color.White;
            BtnAddNewUser.Image = (Image)resources.GetObject("BtnAddNewUser.Image");
            BtnAddNewUser.ImageAlign = ContentAlignment.MiddleLeft;
            BtnAddNewUser.Location = new Point(372, 206);
            BtnAddNewUser.Name = "BtnAddNewUser";
            BtnAddNewUser.Size = new Size(144, 29);
            BtnAddNewUser.TabIndex = 11;
            BtnAddNewUser.Text = "Add New User";
            BtnAddNewUser.TextAlign = ContentAlignment.MiddleRight;
            BtnAddNewUser.UseVisualStyleBackColor = false;
            BtnAddNewUser.Click += BtnAddNewUser_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(42, 93, 159);
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Image = (Image)resources.GetObject("button1.Image");
            button1.Location = new Point(317, 206);
            button1.Name = "button1";
            button1.Size = new Size(49, 30);
            button1.TabIndex = 10;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // txtSearchBar
            // 
            txtSearchBar.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearchBar.Location = new Point(18, 206);
            txtSearchBar.Name = "txtSearchBar";
            txtSearchBar.Size = new Size(302, 30);
            txtSearchBar.TabIndex = 9;
            txtSearchBar.Text = "Search";
            // 
            // panel8
            // 
            panel8.BorderStyle = BorderStyle.FixedSingle;
            panel8.Controls.Add(panel9);
            panel8.Controls.Add(label9);
            panel8.Location = new Point(924, 120);
            panel8.Name = "panel8";
            panel8.Size = new Size(391, 65);
            panel8.TabIndex = 17;
            // 
            // panel9
            // 
            panel9.BackColor = Color.FromArgb(42, 93, 159);
            panel9.Controls.Add(label8);
            panel9.Dock = DockStyle.Top;
            panel9.Location = new Point(0, 0);
            panel9.Name = "panel9";
            panel9.Size = new Size(389, 32);
            panel9.TabIndex = 0;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.White;
            label8.Location = new Point(131, -1);
            label8.Name = "label8";
            label8.Size = new Size(127, 25);
            label8.TabIndex = 1;
            label8.Text = "Inactive Users";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(179, 35);
            label9.Name = "label9";
            label9.Size = new Size(22, 25);
            label9.TabIndex = 2;
            label9.Text = "0";
            // 
            // panel4
            // 
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Controls.Add(panel5);
            panel4.Controls.Add(label5);
            panel4.Location = new Point(615, 120);
            panel4.Name = "panel4";
            panel4.Size = new Size(310, 65);
            panel4.TabIndex = 16;
            // 
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(42, 93, 159);
            panel5.Controls.Add(label4);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(308, 32);
            panel5.TabIndex = 0;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(90, 1);
            label4.Name = "label4";
            label4.Size = new Size(114, 25);
            label4.TabIndex = 1;
            label4.Text = "Active Users";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(149, 35);
            label5.Name = "label5";
            label5.Size = new Size(22, 25);
            label5.TabIndex = 2;
            label5.Text = "0";
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(label3);
            panel2.Location = new Point(230, 120);
            panel2.Name = "panel2";
            panel2.Size = new Size(417, 65);
            panel2.TabIndex = 15;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(42, 93, 159);
            panel3.Controls.Add(label2);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(415, 32);
            panel3.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(135, 1);
            label2.Name = "label2";
            label2.Size = new Size(112, 25);
            label2.TabIndex = 1;
            label2.Text = "Total Users: ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(176, 35);
            label3.Name = "label3";
            label3.Size = new Size(22, 25);
            label3.TabIndex = 2;
            label3.Text = "0";
            // 
            // panel6
            // 
            panel6.BorderStyle = BorderStyle.FixedSingle;
            panel6.Controls.Add(panel7);
            panel6.Controls.Add(label7);
            panel6.Location = new Point(1304, 120);
            panel6.Name = "panel6";
            panel6.Size = new Size(392, 65);
            panel6.TabIndex = 18;
            // 
            // panel7
            // 
            panel7.BackColor = Color.FromArgb(42, 93, 159);
            panel7.Controls.Add(label6);
            panel7.Dock = DockStyle.Top;
            panel7.Location = new Point(0, 0);
            panel7.Name = "panel7";
            panel7.Size = new Size(390, 32);
            panel7.TabIndex = 0;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.White;
            label6.Location = new Point(117, 0);
            label6.Name = "label6";
            label6.Size = new Size(160, 25);
            label6.TabIndex = 1;
            label6.Text = "Suspended Users:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(177, 35);
            label7.Name = "label7";
            label7.Size = new Size(22, 25);
            label7.TabIndex = 2;
            label7.Text = "0";
            // 
            // UCUsers
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel6);
            Controls.Add(panel8);
            Controls.Add(panel4);
            Controls.Add(panel2);
            Controls.Add(BtnEdit);
            Controls.Add(BtnDelete);
            Controls.Add(dataGridView1);
            Controls.Add(BtnAddNewUser);
            Controls.Add(button1);
            Controls.Add(txtSearchBar);
            Controls.Add(panel1);
            Name = "UCUsers";
            Size = new Size(1924, 891);
            Load += UCUsers_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel8.ResumeLayout(false);
            panel8.PerformLayout();
            panel9.ResumeLayout(false);
            panel9.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Button BtnEdit;
        private Button BtnDelete;
        private DataGridView dataGridView1;
        private Button BtnAddNewUser;
        private Button button1;
        private TextBox txtSearchBar;
        private Panel panel8;
        private Panel panel9;
        private Label label8;
        private Label label9;
        private Panel panel4;
        private Panel panel5;
        private Label label4;
        private Label label5;
        private Panel panel2;
        private Panel panel3;
        private Label label2;
        private Label label3;
        private DataGridViewTextBoxColumn colUsername;
        private DataGridViewTextBoxColumn colPassword;
        private DataGridViewTextBoxColumn colFullname;
        private DataGridViewTextBoxColumn colEmail;
        private DataGridViewTextBoxColumn colPhoneNumber;
        private DataGridViewTextBoxColumn colUserStatus;
        private DataGridViewTextBoxColumn colRole;
        private DataGridViewTextBoxColumn colCreateAt;
        private DataGridViewTextBoxColumn colUpdatedAt;
        private Panel panel6;
        private Panel panel7;
        private Label label6;
        private Label label7;
    }
}
