namespace HRMS.UCForms
{
    partial class UCRooms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCRooms));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            panel1 = new Panel();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtSearchBar = new TextBox();
            button1 = new Button();
            BtnAddNewRoom = new Button();
            dataGridView1 = new DataGridView();
            RoomNumber = new DataGridViewTextBoxColumn();
            RoomType = new DataGridViewTextBoxColumn();
            BedConfiguration = new DataGridViewTextBoxColumn();
            MaximumOccupancy = new DataGridViewTextBoxColumn();
            RoomFloor = new DataGridViewTextBoxColumn();
            RoomStatus = new DataGridViewTextBoxColumn();
            ViewType = new DataGridViewTextBoxColumn();
            colAmenities = new DataGridViewTextBoxColumn();
            RoomRate = new DataGridViewTextBoxColumn();
            BtnDelete = new Button();
            BtnEdit = new Button();
            panel2 = new Panel();
            panel3 = new Panel();
            panel4 = new Panel();
            panel5 = new Panel();
            label4 = new Label();
            label5 = new Label();
            panel6 = new Panel();
            panel7 = new Panel();
            label6 = new Label();
            label7 = new Label();
            panel8 = new Panel();
            panel9 = new Panel();
            label8 = new Label();
            label9 = new Label();
            roomServiceBindingSource = new BindingSource(components);
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            panel6.SuspendLayout();
            panel7.SuspendLayout();
            panel8.SuspendLayout();
            panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)roomServiceBindingSource).BeginInit();
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
            panel1.Size = new Size(1674, 104);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(25, 30);
            label1.Name = "label1";
            label1.Size = new Size(270, 38);
            label1.TabIndex = 0;
            label1.Text = "Room Management";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(149, 0);
            label2.Name = "label2";
            label2.Size = new Size(123, 25);
            label2.TabIndex = 1;
            label2.Text = "Total Rooms: ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(203, 35);
            label3.Name = "label3";
            label3.Size = new Size(22, 25);
            label3.TabIndex = 2;
            label3.Text = "0";
            // 
            // txtSearchBar
            // 
            txtSearchBar.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearchBar.Location = new Point(25, 203);
            txtSearchBar.Name = "txtSearchBar";
            txtSearchBar.Size = new Size(302, 30);
            txtSearchBar.TabIndex = 3;
            txtSearchBar.Text = "Search";
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(42, 93, 159);
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Image = (Image)resources.GetObject("button1.Image");
            button1.Location = new Point(324, 203);
            button1.Name = "button1";
            button1.Size = new Size(49, 30);
            button1.TabIndex = 4;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // BtnAddNewRoom
            // 
            BtnAddNewRoom.BackColor = Color.FromArgb(42, 93, 159);
            BtnAddNewRoom.FlatAppearance.BorderSize = 0;
            BtnAddNewRoom.FlatStyle = FlatStyle.Flat;
            BtnAddNewRoom.ForeColor = Color.White;
            BtnAddNewRoom.Image = (Image)resources.GetObject("BtnAddNewRoom.Image");
            BtnAddNewRoom.ImageAlign = ContentAlignment.MiddleLeft;
            BtnAddNewRoom.Location = new Point(390, 203);
            BtnAddNewRoom.Name = "BtnAddNewRoom";
            BtnAddNewRoom.Size = new Size(151, 29);
            BtnAddNewRoom.TabIndex = 5;
            BtnAddNewRoom.Text = "Add New Room";
            BtnAddNewRoom.TextAlign = ContentAlignment.MiddleRight;
            BtnAddNewRoom.UseVisualStyleBackColor = false;
            BtnAddNewRoom.Click += BtnAddNewRoom_Click;
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
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { RoomNumber, RoomType, BedConfiguration, MaximumOccupancy, RoomFloor, RoomStatus, ViewType, colAmenities, RoomRate });
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.Location = new Point(25, 237);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1625, 628);
            dataGridView1.TabIndex = 6;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // RoomNumber
            // 
            RoomNumber.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            RoomNumber.HeaderText = "Room Number";
            RoomNumber.MinimumWidth = 6;
            RoomNumber.Name = "RoomNumber";
            // 
            // RoomType
            // 
            RoomType.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            RoomType.HeaderText = "Room Type";
            RoomType.MinimumWidth = 6;
            RoomType.Name = "RoomType";
            // 
            // BedConfiguration
            // 
            BedConfiguration.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            BedConfiguration.HeaderText = "Bed Configuration";
            BedConfiguration.MinimumWidth = 6;
            BedConfiguration.Name = "BedConfiguration";
            // 
            // MaximumOccupancy
            // 
            MaximumOccupancy.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            MaximumOccupancy.HeaderText = "Maximum Occupancy";
            MaximumOccupancy.MinimumWidth = 6;
            MaximumOccupancy.Name = "MaximumOccupancy";
            // 
            // RoomFloor
            // 
            RoomFloor.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            RoomFloor.HeaderText = "Room Floor";
            RoomFloor.MinimumWidth = 6;
            RoomFloor.Name = "RoomFloor";
            // 
            // RoomStatus
            // 
            RoomStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            RoomStatus.HeaderText = "Room Status";
            RoomStatus.MinimumWidth = 6;
            RoomStatus.Name = "RoomStatus";
            // 
            // ViewType
            // 
            ViewType.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ViewType.HeaderText = "View Type";
            ViewType.MinimumWidth = 6;
            ViewType.Name = "ViewType";
            // 
            // colAmenities
            // 
            colAmenities.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colAmenities.HeaderText = "Amenities";
            colAmenities.MinimumWidth = 6;
            colAmenities.Name = "colAmenities";
            // 
            // RoomRate
            // 
            RoomRate.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            RoomRate.HeaderText = "Room Rate";
            RoomRate.MinimumWidth = 6;
            RoomRate.Name = "RoomRate";
            // 
            // BtnDelete
            // 
            BtnDelete.BackColor = Color.FromArgb(192, 0, 0);
            BtnDelete.FlatAppearance.BorderSize = 0;
            BtnDelete.FlatStyle = FlatStyle.Flat;
            BtnDelete.ForeColor = Color.White;
            BtnDelete.Image = (Image)resources.GetObject("BtnDelete.Image");
            BtnDelete.ImageAlign = ContentAlignment.MiddleLeft;
            BtnDelete.Location = new Point(1561, 204);
            BtnDelete.Name = "BtnDelete";
            BtnDelete.Size = new Size(88, 29);
            BtnDelete.TabIndex = 7;
            BtnDelete.Text = "Delete";
            BtnDelete.TextAlign = ContentAlignment.MiddleRight;
            BtnDelete.UseVisualStyleBackColor = false;
            BtnDelete.Click += BtnDelete_Click;
            // 
            // BtnEdit
            // 
            BtnEdit.BackColor = Color.White;
            BtnEdit.FlatStyle = FlatStyle.Flat;
            BtnEdit.ForeColor = Color.FromArgb(42, 93, 159);
            BtnEdit.Image = (Image)resources.GetObject("BtnEdit.Image");
            BtnEdit.ImageAlign = ContentAlignment.MiddleLeft;
            BtnEdit.Location = new Point(1467, 204);
            BtnEdit.Name = "BtnEdit";
            BtnEdit.Padding = new Padding(0, 0, 10, 0);
            BtnEdit.Size = new Size(88, 29);
            BtnEdit.TabIndex = 8;
            BtnEdit.Text = "Edit";
            BtnEdit.TextAlign = ContentAlignment.MiddleRight;
            BtnEdit.UseVisualStyleBackColor = false;
            BtnEdit.Click += BtnEdit_Click;
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(label3);
            panel2.Location = new Point(89, 113);
            panel2.Name = "panel2";
            panel2.Size = new Size(505, 65);
            panel2.TabIndex = 9;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(42, 93, 159);
            panel3.Controls.Add(label2);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(503, 32);
            panel3.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Controls.Add(panel5);
            panel4.Controls.Add(label5);
            panel4.Location = new Point(562, 113);
            panel4.Name = "panel4";
            panel4.Size = new Size(310, 65);
            panel4.TabIndex = 10;
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
            label4.Size = new Size(145, 25);
            label4.TabIndex = 1;
            label4.Text = "Room Available:";
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
            // panel6
            // 
            panel6.BorderStyle = BorderStyle.FixedSingle;
            panel6.Controls.Add(panel7);
            panel6.Controls.Add(label7);
            panel6.Location = new Point(1258, 113);
            panel6.Name = "panel6";
            panel6.Size = new Size(392, 65);
            panel6.TabIndex = 11;
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
            label6.Location = new Point(69, 1);
            label6.Name = "label6";
            label6.Size = new Size(235, 25);
            label6.TabIndex = 1;
            label6.Text = "Room Under Maintenance:";
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
            // panel8
            // 
            panel8.BorderStyle = BorderStyle.FixedSingle;
            panel8.Controls.Add(panel9);
            panel8.Controls.Add(label9);
            panel8.Location = new Point(871, 113);
            panel8.Name = "panel8";
            panel8.Size = new Size(391, 65);
            panel8.TabIndex = 11;
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
            label8.Location = new Point(108, -1);
            label8.Name = "label8";
            label8.Size = new Size(156, 25);
            label8.TabIndex = 1;
            label8.Text = "Occupied Rooms:";
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
            // roomServiceBindingSource
            // 
            roomServiceBindingSource.DataSource = typeof(Services.RoomService);
            // 
            // UCRooms
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(panel8);
            Controls.Add(panel6);
            Controls.Add(panel4);
            Controls.Add(panel2);
            Controls.Add(BtnEdit);
            Controls.Add(BtnDelete);
            Controls.Add(dataGridView1);
            Controls.Add(BtnAddNewRoom);
            Controls.Add(button1);
            Controls.Add(txtSearchBar);
            Controls.Add(panel1);
            Name = "UCRooms";
            Size = new Size(1674, 1055);
            Load += UCRooms_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            panel8.ResumeLayout(false);
            panel8.PerformLayout();
            panel9.ResumeLayout(false);
            panel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)roomServiceBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtSearchBar;
        private Button button1;
        private Button BtnAddNewRoom;
        private DataGridView dataGridView1;
        private Button BtnDelete;
        private Button BtnEdit;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private Label label4;
        private Label label5;
        private Panel panel6;
        private Panel panel7;
        private Label label6;
        private Label label7;
        private Panel panel8;
        private Panel panel9;
        private Label label8;
        private Label label9;
        private BindingSource roomServiceBindingSource;
        private DataGridViewTextBoxColumn RoomNumber;
        private DataGridViewTextBoxColumn RoomType;
        private DataGridViewTextBoxColumn BedConfiguration;
        private DataGridViewTextBoxColumn MaximumOccupancy;
        private DataGridViewTextBoxColumn RoomFloor;
        private DataGridViewTextBoxColumn RoomStatus;
        private DataGridViewTextBoxColumn ViewType;
        private DataGridViewTextBoxColumn colAmenities;
        private DataGridViewTextBoxColumn RoomRate;

    }
}
