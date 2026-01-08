namespace HRMS.WinForms
{
    partial class LoginAdmin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginAdmin));
            panel1 = new Panel();
            label2 = new Label();
            label1 = new Label();
            label3 = new Label();
            label4 = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            label5 = new Label();
            chckShowPassword = new CheckBox();
            button1 = new Button();
            chckRememberMe = new CheckBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(42, 93, 159);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(698, 82);
            panel1.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(12, 50);
            label2.Name = "label2";
            label2.Size = new Size(160, 20);
            label2.TabIndex = 1;
            label2.Text = "Hotel Manager - Login";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 27);
            label1.Name = "label1";
            label1.Size = new Size(327, 23);
            label1.TabIndex = 0;
            label1.Text = "Hotel Reservation Management System";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 22.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(279, 146);
            label3.Name = "label3";
            label3.Size = new Size(113, 50);
            label3.TabIndex = 1;
            label3.Text = "Login";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(169, 233);
            label4.Name = "label4";
            label4.Size = new Size(87, 23);
            label4.TabIndex = 2;
            label4.Text = "Username";
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUsername.Location = new Point(169, 259);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(349, 30);
            txtUsername.TabIndex = 3;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPassword.Location = new Point(169, 340);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(349, 30);
            txtPassword.TabIndex = 5;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(169, 314);
            label5.Name = "label5";
            label5.Size = new Size(80, 23);
            label5.TabIndex = 4;
            label5.Text = "Password";
            // 
            // chckShowPassword
            // 
            chckShowPassword.AutoSize = true;
            chckShowPassword.BackColor = Color.Transparent;
            chckShowPassword.Location = new Point(386, 376);
            chckShowPassword.Name = "chckShowPassword";
            chckShowPassword.Size = new Size(132, 24);
            chckShowPassword.TabIndex = 6;
            chckShowPassword.Text = "Show Password";
            chckShowPassword.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(42, 93, 159);
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Location = new Point(169, 406);
            button1.Name = "button1";
            button1.Size = new Size(349, 45);
            button1.TabIndex = 7;
            button1.Text = "Login";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click_1;
            button1.Enter += button1_Click_1;
            // 
            // chckRememberMe
            // 
            chckRememberMe.AutoSize = true;
            chckRememberMe.BackColor = Color.Transparent;
            chckRememberMe.Location = new Point(169, 457);
            chckRememberMe.Name = "chckRememberMe";
            chckRememberMe.Size = new Size(129, 24);
            chckRememberMe.TabIndex = 8;
            chckRememberMe.Text = "Remember me";
            chckRememberMe.UseVisualStyleBackColor = false;
            // 
            // LoginAdmin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(698, 614);
            Controls.Add(chckRememberMe);
            Controls.Add(button1);
            Controls.Add(chckShowPassword);
            Controls.Add(txtPassword);
            Controls.Add(label5);
            Controls.Add(txtUsername);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "LoginAdmin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LoginAdmin";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label label2;
        private Label label1;
        private Label label3;
        private Label label4;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Label label5;
        private CheckBox chckShowPassword;
        private Button button1;
        private CheckBox chckRememberMe;
    }
}