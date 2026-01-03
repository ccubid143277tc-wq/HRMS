namespace HRMS
{
    partial class Welcome
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Welcome));
            label1 = new Label();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            button1 = new Button();
            BtnStaff = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(42, 93, 159);
            label1.Location = new Point(109, 281);
            label1.Name = "label1";
            label1.Size = new Size(718, 54);
            label1.TabIndex = 0;
            label1.Text = "Hotel Reservation Management System";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(424, 335);
            label2.Name = "label2";
            label2.Size = new Size(64, 20);
            label2.TabIndex = 1;
            label2.Text = "Login as";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(281, 82);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(367, 211);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(42, 93, 159);
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Location = new Point(281, 389);
            button1.Name = "button1";
            button1.Size = new Size(129, 46);
            button1.TabIndex = 3;
            button1.Text = "Admin";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // BtnStaff
            // 
            BtnStaff.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnStaff.ForeColor = Color.FromArgb(42, 93, 159);
            BtnStaff.Location = new Point(519, 389);
            BtnStaff.Name = "BtnStaff";
            BtnStaff.Size = new Size(129, 46);
            BtnStaff.TabIndex = 4;
            BtnStaff.Text = "Staff";
            BtnStaff.UseVisualStyleBackColor = true;
            // 
            // Welcome
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(935, 548);
            Controls.Add(BtnStaff);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Welcome";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Welcome";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private PictureBox pictureBox1;
        private Button button1;
        private Button BtnStaff;
    }
}
