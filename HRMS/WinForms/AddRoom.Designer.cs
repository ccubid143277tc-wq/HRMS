namespace HRMS.WinForms
{
    partial class AddRoom
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
            groupBox1 = new GroupBox();
            textBox1 = new TextBox();
            cmbViewType = new ComboBox();
            label9 = new Label();
            cmbRoomStatus = new ComboBox();
            label8 = new Label();
            label7 = new Label();
            txtRoomFloor = new TextBox();
            label6 = new Label();
            txtMaximumOccupancy = new TextBox();
            label5 = new Label();
            cmbBedConfiguration = new ComboBox();
            label4 = new Label();
            cmbRoomType = new ComboBox();
            label3 = new Label();
            txtRoomNumber = new TextBox();
            label2 = new Label();
            groupBox2 = new GroupBox();
            chckCoffeeMaker = new CheckBox();
            chckSafe = new CheckBox();
            chckHairDryer = new CheckBox();
            chckRoomService = new CheckBox();
            chckMinibar = new CheckBox();
            chckAirConditioning = new CheckBox();
            chckTV = new CheckBox();
            chckWifi = new CheckBox();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            BtnAdd = new Button();
            BtnClear = new Button();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(42, 93, 159);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(778, 77);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 24);
            label1.Name = "label1";
            label1.Size = new Size(155, 28);
            label1.TabIndex = 0;
            label1.Text = "Add New Room";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(cmbViewType);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(cmbRoomStatus);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(txtRoomFloor);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(txtMaximumOccupancy);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(cmbBedConfiguration);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(cmbRoomType);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtRoomNumber);
            groupBox1.Controls.Add(label2);
            groupBox1.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(12, 83);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(749, 380);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Room Information";
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(429, 214);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(281, 30);
            textBox1.TabIndex = 16;
            // 
            // cmbViewType
            // 
            cmbViewType.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbViewType.FormattingEnabled = true;
            cmbViewType.Items.AddRange(new object[] { "City view", "Garden View", "Sea View" });
            cmbViewType.Location = new Point(429, 289);
            cmbViewType.Name = "cmbViewType";
            cmbViewType.Size = new Size(281, 31);
            cmbViewType.TabIndex = 15;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.Location = new Point(429, 263);
            label9.Name = "label9";
            label9.Size = new Size(86, 23);
            label9.TabIndex = 14;
            label9.Text = "View Type";
            // 
            // cmbRoomStatus
            // 
            cmbRoomStatus.DisplayMember = "RoomStatusName";
            cmbRoomStatus.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbRoomStatus.FormattingEnabled = true;
            cmbRoomStatus.Items.AddRange(new object[] { "Available ", "Occupied", "Maintenance", "Reserved", "Out of Service" });
            cmbRoomStatus.Location = new Point(23, 289);
            cmbRoomStatus.Name = "cmbRoomStatus";
            cmbRoomStatus.Size = new Size(281, 31);
            cmbRoomStatus.TabIndex = 13;
            cmbRoomStatus.ValueMember = "RoomStatus";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(23, 263);
            label8.Name = "label8";
            label8.Size = new Size(106, 23);
            label8.TabIndex = 12;
            label8.Text = "Room Status";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(429, 188);
            label7.Name = "label7";
            label7.Size = new Size(94, 23);
            label7.TabIndex = 10;
            label7.Text = "Room Rate";
            // 
            // txtRoomFloor
            // 
            txtRoomFloor.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtRoomFloor.Location = new Point(23, 214);
            txtRoomFloor.Name = "txtRoomFloor";
            txtRoomFloor.Size = new Size(281, 30);
            txtRoomFloor.TabIndex = 9;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(23, 188);
            label6.Name = "label6";
            label6.Size = new Size(98, 23);
            label6.TabIndex = 8;
            label6.Text = "Room Floor";
            // 
            // txtMaximumOccupancy
            // 
            txtMaximumOccupancy.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMaximumOccupancy.Location = new Point(429, 145);
            txtMaximumOccupancy.Name = "txtMaximumOccupancy";
            txtMaximumOccupancy.Size = new Size(281, 30);
            txtMaximumOccupancy.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(429, 118);
            label5.Name = "label5";
            label5.Size = new Size(175, 23);
            label5.TabIndex = 6;
            label5.Text = "Maximum Occupancy";
            // 
            // cmbBedConfiguration
            // 
            cmbBedConfiguration.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbBedConfiguration.FormattingEnabled = true;
            cmbBedConfiguration.Items.AddRange(new object[] { "king", "Queen", "Twin Beds" });
            cmbBedConfiguration.Location = new Point(429, 73);
            cmbBedConfiguration.Name = "cmbBedConfiguration";
            cmbBedConfiguration.Size = new Size(281, 31);
            cmbBedConfiguration.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(429, 47);
            label4.Name = "label4";
            label4.Size = new Size(149, 23);
            label4.TabIndex = 4;
            label4.Text = "Bed Configuration";
            // 
            // cmbRoomType
            // 
            cmbRoomType.DisplayMember = "RoomTypeName";
            cmbRoomType.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbRoomType.FormattingEnabled = true;
            cmbRoomType.Items.AddRange(new object[] { "Single", "Double", "Twin", "Suite", "Deluxe", "Family", "Executive" });
            cmbRoomType.Location = new Point(23, 144);
            cmbRoomType.Name = "cmbRoomType";
            cmbRoomType.Size = new Size(281, 31);
            cmbRoomType.TabIndex = 3;
            cmbRoomType.ValueMember = "RoomType";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(23, 118);
            label3.Name = "label3";
            label3.Size = new Size(95, 23);
            label3.TabIndex = 2;
            label3.Text = "Room Type";
            // 
            // txtRoomNumber
            // 
            txtRoomNumber.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtRoomNumber.Location = new Point(23, 73);
            txtRoomNumber.Name = "txtRoomNumber";
            txtRoomNumber.Size = new Size(281, 30);
            txtRoomNumber.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(23, 47);
            label2.Name = "label2";
            label2.Size = new Size(123, 23);
            label2.TabIndex = 0;
            label2.Text = "Room Number";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(chckCoffeeMaker);
            groupBox2.Controls.Add(chckSafe);
            groupBox2.Controls.Add(chckHairDryer);
            groupBox2.Controls.Add(chckRoomService);
            groupBox2.Controls.Add(chckMinibar);
            groupBox2.Controls.Add(chckAirConditioning);
            groupBox2.Controls.Add(chckTV);
            groupBox2.Controls.Add(chckWifi);
            groupBox2.Controls.Add(comboBox1);
            groupBox2.Controls.Add(comboBox2);
            groupBox2.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(12, 475);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(749, 174);
            groupBox2.TabIndex = 16;
            groupBox2.TabStop = false;
            groupBox2.Text = "Amenities";
            // 
            // chckCoffeeMaker
            // 
            chckCoffeeMaker.AutoSize = true;
            chckCoffeeMaker.Location = new Point(580, 113);
            chckCoffeeMaker.Name = "chckCoffeeMaker";
            chckCoffeeMaker.Size = new Size(140, 29);
            chckCoffeeMaker.TabIndex = 23;
            chckCoffeeMaker.Text = "Coffee Maker";
            chckCoffeeMaker.UseVisualStyleBackColor = true;
            // 
            // chckSafe
            // 
            chckSafe.AutoSize = true;
            chckSafe.Location = new Point(580, 59);
            chckSafe.Name = "chckSafe";
            chckSafe.Size = new Size(68, 29);
            chckSafe.TabIndex = 22;
            chckSafe.Text = "Safe";
            chckSafe.UseVisualStyleBackColor = true;
            // 
            // chckHairDryer
            // 
            chckHairDryer.AutoSize = true;
            chckHairDryer.Location = new Point(381, 113);
            chckHairDryer.Name = "chckHairDryer";
            chckHairDryer.Size = new Size(114, 29);
            chckHairDryer.TabIndex = 21;
            chckHairDryer.Text = "Hair Dryer";
            chckHairDryer.UseVisualStyleBackColor = true;
            // 
            // chckRoomService
            // 
            chckRoomService.AutoSize = true;
            chckRoomService.Location = new Point(381, 59);
            chckRoomService.Name = "chckRoomService";
            chckRoomService.Size = new Size(142, 29);
            chckRoomService.TabIndex = 20;
            chckRoomService.Text = "Room Service";
            chckRoomService.UseVisualStyleBackColor = true;
            // 
            // chckMinibar
            // 
            chckMinibar.AutoSize = true;
            chckMinibar.Location = new Point(162, 113);
            chckMinibar.Name = "chckMinibar";
            chckMinibar.Size = new Size(99, 29);
            chckMinibar.TabIndex = 19;
            chckMinibar.Text = "Mini bar";
            chckMinibar.UseVisualStyleBackColor = true;
            // 
            // chckAirConditioning
            // 
            chckAirConditioning.AutoSize = true;
            chckAirConditioning.Location = new Point(162, 59);
            chckAirConditioning.Name = "chckAirConditioning";
            chckAirConditioning.Size = new Size(164, 29);
            chckAirConditioning.TabIndex = 18;
            chckAirConditioning.Text = "Air Conditioning";
            chckAirConditioning.UseVisualStyleBackColor = true;
            // 
            // chckTV
            // 
            chckTV.AutoSize = true;
            chckTV.Location = new Point(29, 113);
            chckTV.Name = "chckTV";
            chckTV.Size = new Size(54, 29);
            chckTV.TabIndex = 17;
            chckTV.Text = "TV";
            chckTV.UseVisualStyleBackColor = true;
            // 
            // chckWifi
            // 
            chckWifi.AutoSize = true;
            chckWifi.Location = new Point(29, 59);
            chckWifi.Name = "chckWifi";
            chckWifi.Size = new Size(72, 29);
            chckWifi.TabIndex = 16;
            chckWifi.Text = "Wi-fi";
            chckWifi.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            comboBox1.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(429, 289);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(281, 31);
            comboBox1.TabIndex = 15;
            // 
            // comboBox2
            // 
            comboBox2.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(23, 289);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(281, 31);
            comboBox2.TabIndex = 13;
            // 
            // BtnAdd
            // 
            BtnAdd.Location = new Point(41, 679);
            BtnAdd.Name = "BtnAdd";
            BtnAdd.Size = new Size(181, 45);
            BtnAdd.TabIndex = 17;
            BtnAdd.Text = "Add";
            BtnAdd.UseVisualStyleBackColor = true;
            BtnAdd.Click += BtnAdd_Click;
            // 
            // BtnClear
            // 
            BtnClear.Location = new Point(551, 679);
            BtnClear.Name = "BtnClear";
            BtnClear.Size = new Size(181, 45);
            BtnClear.TabIndex = 18;
            BtnClear.Text = "Clear";
            BtnClear.UseVisualStyleBackColor = true;
            // 
            // AddRoom
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(778, 764);
            Controls.Add(BtnClear);
            Controls.Add(BtnAdd);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "AddRoom";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AddRoom";
            Load += AddRoom_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private GroupBox groupBox1;
        private ComboBox cmbRoomType;
        private Label label3;
        private TextBox txtRoomNumber;
        private Label label2;
        private ComboBox cmbViewType;
        private Label label9;
        private ComboBox cmbRoomStatus;
        private Label label8;
        private Label label7;
        private TextBox txtRoomFloor;
        private Label label6;
        private TextBox txtMaximumOccupancy;
        private Label label5;
        private ComboBox cmbBedConfiguration;
        private Label label4;
        private GroupBox groupBox2;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private CheckBox chckCoffeeMaker;
        private CheckBox chckSafe;
        private CheckBox chckHairDryer;
        private CheckBox chckRoomService;
        private CheckBox chckMinibar;
        private CheckBox chckAirConditioning;
        private CheckBox chckTV;
        private CheckBox chckWifi;
        private Button BtnAdd;
        private Button BtnClear;
        private TextBox textBox1;
    }
}