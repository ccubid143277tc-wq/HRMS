namespace HRMS.UCForms
{
    partial class Payment
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Payment));
            panel26 = new Panel();
            label14 = new Label();
            label15 = new Label();
            pictureBox16 = new PictureBox();
            label16 = new Label();
            label17 = new Label();
            label18 = new Label();
            label19 = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            panel26.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox16).BeginInit();
            SuspendLayout();
            // 
            // panel26
            // 
            panel26.BackColor = SystemColors.Window;
            panel26.BackgroundImageLayout = ImageLayout.None;
            panel26.BorderStyle = BorderStyle.FixedSingle;
            panel26.Controls.Add(label14);
            panel26.Controls.Add(label15);
            panel26.Controls.Add(pictureBox16);
            panel26.Controls.Add(label16);
            panel26.Controls.Add(label17);
            panel26.Controls.Add(label18);
            panel26.Controls.Add(label19);
            panel26.Dock = DockStyle.Top;
            panel26.Location = new Point(0, 0);
            panel26.Name = "panel26";
            panel26.Size = new Size(1645, 117);
            panel26.TabIndex = 14;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.ForeColor = SystemColors.ControlDarkDark;
            label14.Location = new Point(1436, 68);
            label14.Name = "label14";
            label14.Size = new Size(91, 20);
            label14.TabIndex = 12;
            label14.Text = "Receptionist";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label15.Location = new Point(1415, 40);
            label15.Name = "label15";
            label15.Size = new Size(131, 28);
            label15.TabIndex = 11;
            label15.Text = "Carl Christian";
            // 
            // pictureBox16
            // 
            pictureBox16.Image = (Image)resources.GetObject("pictureBox16.Image");
            pictureBox16.Location = new Point(1548, 27);
            pictureBox16.Name = "pictureBox16";
            pictureBox16.Size = new Size(64, 62);
            pictureBox16.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox16.TabIndex = 9;
            pictureBox16.TabStop = false;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label16.ForeColor = SystemColors.ControlDarkDark;
            label16.Location = new Point(132, 66);
            label16.Name = "label16";
            label16.Size = new Size(76, 23);
            label16.TabIndex = 8;
            label16.Text = "Payment";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label17.ForeColor = SystemColors.ControlDarkDark;
            label17.Location = new Point(27, 66);
            label17.Name = "label17";
            label17.Size = new Size(108, 23);
            label17.TabIndex = 7;
            label17.Text = " Front Desk -";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Font = new Font("Cambria", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label18.Location = new Point(20, 27);
            label18.Name = "label18";
            label18.Size = new Size(553, 36);
            label18.TabIndex = 6;
            label18.Text = "Hotel Reservation Management System";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label19.ForeColor = Color.Black;
            label19.Location = new Point(969, 47);
            label19.Name = "label19";
            label19.Size = new Size(59, 28);
            label19.TabIndex = 5;
            label19.Text = "Time";
            label19.Click += label19_Click;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1;
            timer1.Tick += label19_Click;
            // 
            // Payment
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel26);
            Name = "Payment";
            Size = new Size(1645, 1271);
            panel26.ResumeLayout(false);
            panel26.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox16).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel26;
        private Label label14;
        private Label label15;
        private PictureBox pictureBox16;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private System.Windows.Forms.Timer timer1;
    }
}
