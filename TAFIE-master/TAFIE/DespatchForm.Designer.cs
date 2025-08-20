namespace TAFIE
{
    partial class DespatchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DespatchForm));
            btnExit = new Button();
            txbBcode = new TextBox();
            dgContents = new DataGridView();
            lblTitle = new Label();
            label1 = new Label();
            lblLastScan = new Label();
            ((System.ComponentModel.ISupportInitialize)dgContents).BeginInit();
            SuspendLayout();
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.FromArgb(11, 159, 161);
            btnExit.Cursor = Cursors.Hand;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnExit.ForeColor = Color.Black;
            btnExit.Location = new Point(12, 455);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(96, 46);
            btnExit.TabIndex = 24;
            btnExit.Text = "Close";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            btnExit.MouseEnter += btnExit_MouseEnter;
            btnExit.MouseLeave += btnExit_MouseLeave;
            // 
            // txbBcode
            // 
            txbBcode.BackColor = Color.White;
            txbBcode.BorderStyle = BorderStyle.FixedSingle;
            txbBcode.CharacterCasing = CharacterCasing.Upper;
            txbBcode.Cursor = Cursors.IBeam;
            txbBcode.Font = new Font("Impact", 18F, FontStyle.Regular, GraphicsUnit.Point);
            txbBcode.ForeColor = Color.Black;
            txbBcode.Location = new Point(12, 136);
            txbBcode.MaxLength = 24;
            txbBcode.Name = "txbBcode";
            txbBcode.Size = new Size(458, 37);
            txbBcode.TabIndex = 25;
            txbBcode.Enter += txbBcode_Enter;
            txbBcode.Leave += txbBcode_Leave;
            // 
            // dgContents
            // 
            dgContents.AllowUserToAddRows = false;
            dgContents.AllowUserToDeleteRows = false;
            dgContents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgContents.BackgroundColor = Color.FromArgb(11, 159, 161);
            dgContents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.Font = new Font("Impact", 14F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = Color.White;
            dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgContents.DefaultCellStyle = dataGridViewCellStyle1;
            dgContents.GridColor = Color.Black;
            dgContents.Location = new Point(12, 179);
            dgContents.Name = "dgContents";
            dgContents.ReadOnly = true;
            dgContents.RowHeadersVisible = false;
            dgContents.RowTemplate.Height = 25;
            dgContents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgContents.Size = new Size(712, 270);
            dgContents.TabIndex = 26;
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Impact", 40F, FontStyle.Regular, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.FromArgb(11, 159, 161);
            lblTitle.Location = new Point(12, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(712, 76);
            lblTitle.TabIndex = 27;
            lblTitle.Text = "Barcode Scanning";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Impact", 20F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.FromArgb(11, 159, 161);
            label1.Location = new Point(12, 85);
            label1.Name = "label1";
            label1.Size = new Size(712, 34);
            label1.TabIndex = 28;
            label1.Text = "Before Despatching, please verify load note contents";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblLastScan
            // 
            lblLastScan.BackColor = Color.Transparent;
            lblLastScan.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblLastScan.ForeColor = Color.FromArgb(11, 159, 161);
            lblLastScan.Location = new Point(476, 136);
            lblLastScan.Name = "lblLastScan";
            lblLastScan.Size = new Size(248, 37);
            lblLastScan.TabIndex = 29;
            lblLastScan.Text = "Scanned";
            lblLastScan.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // DespatchForm
            // 
            AutoScaleDimensions = new SizeF(6F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(736, 510);
            ControlBox = false;
            Controls.Add(lblLastScan);
            Controls.Add(label1);
            Controls.Add(lblTitle);
            Controls.Add(dgContents);
            Controls.Add(txbBcode);
            Controls.Add(btnExit);
            Font = new Font("Impact", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "DespatchForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DespatchForm";
            Load += DespatchForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgContents).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnExit;
        private TextBox txbBcode;
        private DataGridView dgContents;
        private Label lblTitle;
        private Label label1;
        private Label lblLastScan;
    }
}