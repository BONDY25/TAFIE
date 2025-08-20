namespace TAFIE
{
    partial class ExpandForm
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExpandForm));
            pbLogo = new PictureBox();
            lblTitle = new Label();
            panel1 = new Panel();
            txbCallAtps = new TextBox();
            label11 = new Label();
            txbTotVal = new TextBox();
            txbWeight = new TextBox();
            txbCarrier = new TextBox();
            txbDelMeth = new TextBox();
            txbStatus = new TextBox();
            txbUnits = new TextBox();
            txbLines = new TextBox();
            txbPrintDate = new TextBox();
            txbOrderDate = new TextBox();
            txbLoad = new TextBox();
            txbRefNo = new TextBox();
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            lblRefNo = new Label();
            dgLines = new DataGridView();
            btnClose = new Button();
            label12 = new Label();
            txbApiUsername = new TextBox();
            txbApiAccount = new TextBox();
            label13 = new Label();
            ((System.ComponentModel.ISupportInitialize)pbLogo).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgLines).BeginInit();
            SuspendLayout();
            // 
            // pbLogo
            // 
            pbLogo.Image = Properties.Resources.TAFIE_Logo;
            pbLogo.Location = new Point(1055, 12);
            pbLogo.Name = "pbLogo";
            pbLogo.Size = new Size(127, 118);
            pbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            pbLogo.TabIndex = 4;
            pbLogo.TabStop = false;
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Impact", 55F, FontStyle.Regular, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.FromArgb(11, 159, 161);
            lblTitle.Location = new Point(12, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(578, 90);
            lblTitle.TabIndex = 9;
            lblTitle.Text = "Load Note Details";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(11, 159, 161);
            panel1.Controls.Add(txbCallAtps);
            panel1.Controls.Add(label11);
            panel1.Controls.Add(txbTotVal);
            panel1.Controls.Add(txbWeight);
            panel1.Controls.Add(txbCarrier);
            panel1.Controls.Add(txbDelMeth);
            panel1.Controls.Add(txbStatus);
            panel1.Controls.Add(txbUnits);
            panel1.Controls.Add(txbLines);
            panel1.Controls.Add(txbPrintDate);
            panel1.Controls.Add(txbOrderDate);
            panel1.Controls.Add(txbLoad);
            panel1.Controls.Add(txbRefNo);
            panel1.Controls.Add(label10);
            panel1.Controls.Add(label9);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(lblRefNo);
            panel1.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            panel1.ForeColor = Color.Black;
            panel1.Location = new Point(12, 169);
            panel1.Name = "panel1";
            panel1.Size = new Size(308, 378);
            panel1.TabIndex = 10;
            // 
            // txbCallAtps
            // 
            txbCallAtps.BackColor = Color.White;
            txbCallAtps.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            txbCallAtps.Location = new Point(132, 317);
            txbCallAtps.Name = "txbCallAtps";
            txbCallAtps.ReadOnly = true;
            txbCallAtps.Size = new Size(173, 22);
            txbCallAtps.TabIndex = 23;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(6, 321);
            label11.Name = "label11";
            label11.Size = new Size(101, 20);
            label11.TabIndex = 22;
            label11.Text = "Call Attempts: ";
            // 
            // txbTotVal
            // 
            txbTotVal.BackColor = Color.White;
            txbTotVal.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            txbTotVal.Location = new Point(132, 227);
            txbTotVal.Name = "txbTotVal";
            txbTotVal.ReadOnly = true;
            txbTotVal.Size = new Size(173, 22);
            txbTotVal.TabIndex = 21;
            // 
            // txbWeight
            // 
            txbWeight.BackColor = Color.White;
            txbWeight.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            txbWeight.Location = new Point(132, 197);
            txbWeight.Name = "txbWeight";
            txbWeight.ReadOnly = true;
            txbWeight.Size = new Size(173, 22);
            txbWeight.TabIndex = 20;
            // 
            // txbCarrier
            // 
            txbCarrier.BackColor = Color.White;
            txbCarrier.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            txbCarrier.Location = new Point(132, 257);
            txbCarrier.Name = "txbCarrier";
            txbCarrier.ReadOnly = true;
            txbCarrier.Size = new Size(173, 22);
            txbCarrier.TabIndex = 19;
            // 
            // txbDelMeth
            // 
            txbDelMeth.BackColor = Color.White;
            txbDelMeth.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            txbDelMeth.Location = new Point(132, 287);
            txbDelMeth.Name = "txbDelMeth";
            txbDelMeth.ReadOnly = true;
            txbDelMeth.Size = new Size(173, 22);
            txbDelMeth.TabIndex = 18;
            // 
            // txbStatus
            // 
            txbStatus.BackColor = Color.White;
            txbStatus.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            txbStatus.Location = new Point(132, 347);
            txbStatus.Name = "txbStatus";
            txbStatus.ReadOnly = true;
            txbStatus.Size = new Size(173, 22);
            txbStatus.TabIndex = 17;
            // 
            // txbUnits
            // 
            txbUnits.BackColor = Color.White;
            txbUnits.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            txbUnits.Location = new Point(132, 168);
            txbUnits.Name = "txbUnits";
            txbUnits.ReadOnly = true;
            txbUnits.Size = new Size(173, 22);
            txbUnits.TabIndex = 16;
            // 
            // txbLines
            // 
            txbLines.BackColor = Color.White;
            txbLines.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            txbLines.Location = new Point(132, 138);
            txbLines.Name = "txbLines";
            txbLines.ReadOnly = true;
            txbLines.Size = new Size(173, 22);
            txbLines.TabIndex = 15;
            // 
            // txbPrintDate
            // 
            txbPrintDate.BackColor = Color.White;
            txbPrintDate.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            txbPrintDate.Location = new Point(132, 108);
            txbPrintDate.Name = "txbPrintDate";
            txbPrintDate.ReadOnly = true;
            txbPrintDate.Size = new Size(173, 22);
            txbPrintDate.TabIndex = 14;
            // 
            // txbOrderDate
            // 
            txbOrderDate.BackColor = Color.White;
            txbOrderDate.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            txbOrderDate.Location = new Point(132, 77);
            txbOrderDate.Name = "txbOrderDate";
            txbOrderDate.ReadOnly = true;
            txbOrderDate.Size = new Size(173, 22);
            txbOrderDate.TabIndex = 13;
            // 
            // txbLoad
            // 
            txbLoad.BackColor = Color.White;
            txbLoad.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            txbLoad.Location = new Point(132, 44);
            txbLoad.Name = "txbLoad";
            txbLoad.ReadOnly = true;
            txbLoad.Size = new Size(173, 22);
            txbLoad.TabIndex = 12;
            // 
            // txbRefNo
            // 
            txbRefNo.BackColor = Color.White;
            txbRefNo.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            txbRefNo.Location = new Point(132, 11);
            txbRefNo.Name = "txbRefNo";
            txbRefNo.ReadOnly = true;
            txbRefNo.Size = new Size(173, 22);
            txbRefNo.TabIndex = 11;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(6, 201);
            label10.Name = "label10";
            label10.Size = new Size(82, 20);
            label10.TabIndex = 10;
            label10.Text = "Weight (g): ";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(6, 231);
            label9.Name = "label9";
            label9.Size = new Size(85, 20);
            label9.TabIndex = 9;
            label9.Text = "Total Value: ";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(6, 261);
            label8.Name = "label8";
            label8.Size = new Size(62, 20);
            label8.TabIndex = 8;
            label8.Text = "Carrier: ";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(6, 291);
            label7.Name = "label7";
            label7.Size = new Size(120, 20);
            label7.TabIndex = 7;
            label7.Text = "Delivery Method: ";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 351);
            label6.Name = "label6";
            label6.Size = new Size(57, 20);
            label6.TabIndex = 6;
            label6.Text = "Status: ";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(4, 172);
            label5.Name = "label5";
            label5.Size = new Size(49, 20);
            label5.TabIndex = 5;
            label5.Text = "Units: ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 142);
            label4.Name = "label4";
            label4.Size = new Size(49, 20);
            label4.TabIndex = 4;
            label4.Text = "Lines: ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(4, 112);
            label3.Name = "label3";
            label3.Size = new Size(79, 20);
            label3.TabIndex = 3;
            label3.Text = "Print Date: ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(4, 81);
            label2.Name = "label2";
            label2.Size = new Size(85, 20);
            label2.TabIndex = 2;
            label2.Text = "Order Date: ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 48);
            label1.Name = "label1";
            label1.Size = new Size(78, 20);
            label1.TabIndex = 1;
            label1.Text = "Load Note: ";
            // 
            // lblRefNo
            // 
            lblRefNo.AutoSize = true;
            lblRefNo.Location = new Point(6, 15);
            lblRefNo.Name = "lblRefNo";
            lblRefNo.Size = new Size(106, 20);
            lblRefNo.TabIndex = 0;
            lblRefNo.Text = "Order Number: ";
            // 
            // dgLines
            // 
            dgLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgLines.BackgroundColor = Color.White;
            dgLines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = Color.White;
            dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgLines.DefaultCellStyle = dataGridViewCellStyle1;
            dgLines.GridColor = Color.Black;
            dgLines.Location = new Point(343, 169);
            dgLines.Name = "dgLines";
            dgLines.RowHeadersVisible = false;
            dgLines.RowTemplate.Height = 25;
            dgLines.Size = new Size(839, 378);
            dgLines.TabIndex = 11;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(11, 159, 161);
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnClose.ForeColor = Color.Black;
            btnClose.Location = new Point(12, 553);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(95, 46);
            btnClose.TabIndex = 2;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            btnClose.MouseEnter += btnClose_MouseEnter;
            btnClose.MouseLeave += btnClose_MouseLeave;
            // 
            // label12
            // 
            label12.BackColor = Color.White;
            label12.Location = new Point(12, 134);
            label12.Name = "label12";
            label12.Size = new Size(82, 22);
            label12.TabIndex = 24;
            label12.Text = "API Username";
            label12.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txbApiUsername
            // 
            txbApiUsername.BackColor = Color.White;
            txbApiUsername.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            txbApiUsername.Location = new Point(102, 134);
            txbApiUsername.Name = "txbApiUsername";
            txbApiUsername.ReadOnly = true;
            txbApiUsername.Size = new Size(218, 22);
            txbApiUsername.TabIndex = 24;
            // 
            // txbApiAccount
            // 
            txbApiAccount.BackColor = Color.White;
            txbApiAccount.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            txbApiAccount.Location = new Point(102, 105);
            txbApiAccount.Name = "txbApiAccount";
            txbApiAccount.ReadOnly = true;
            txbApiAccount.Size = new Size(218, 22);
            txbApiAccount.TabIndex = 25;
            // 
            // label13
            // 
            label13.BackColor = Color.White;
            label13.Location = new Point(12, 105);
            label13.Name = "label13";
            label13.Size = new Size(82, 22);
            label13.TabIndex = 26;
            label13.Text = "API Account No";
            label13.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ExpandForm
            // 
            AutoScaleDimensions = new SizeF(6F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1194, 605);
            ControlBox = false;
            Controls.Add(txbApiAccount);
            Controls.Add(label13);
            Controls.Add(txbApiUsername);
            Controls.Add(label12);
            Controls.Add(btnClose);
            Controls.Add(dgLines);
            Controls.Add(panel1);
            Controls.Add(lblTitle);
            Controls.Add(pbLogo);
            Font = new Font("Impact", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ExpandForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ExpandForm";
            FormClosing += ExpandForm_FormClosing;
            Load += ExpandForm_Load;
            KeyDown += ExpandForm_KeyDown;
            ((System.ComponentModel.ISupportInitialize)pbLogo).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgLines).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pbLogo;
        private Label lblTitle;
        private Panel panel1;
        private DataGridView dgLines;
        private Button btnClose;
        private Label label10;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox txbTotVal;
        private TextBox txbWeight;
        private TextBox txbCarrier;
        private TextBox txbDelMeth;
        private TextBox txbStatus;
        private TextBox txbUnits;
        private TextBox txbLines;
        private TextBox txbPrintDate;
        private TextBox txbOrderDate;
        private TextBox txbLoad;
        private TextBox txbRefNo;
        private TextBox txbCallAtps;
        private Label label11;
        private Label lblRefNo;
        private Label label12;
        private TextBox txbApiUsername;
        private TextBox txbApiAccount;
        private Label label13;
    }
}