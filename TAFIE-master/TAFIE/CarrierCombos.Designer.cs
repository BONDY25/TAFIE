namespace TAFIE
{
    partial class CarrierCombos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CarrierCombos));
            lblTitle = new Label();
            btnClose = new Button();
            dgCarrier = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgCarrier).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Impact", 20F, FontStyle.Regular, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.FromArgb(11, 159, 161);
            lblTitle.Location = new Point(12, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(437, 58);
            lblTitle.TabIndex = 10;
            lblTitle.Text = "Carrier Combos";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.FromArgb(11, 159, 161);
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Impact", 12F, FontStyle.Italic, GraphicsUnit.Point);
            btnClose.ForeColor = Color.Black;
            btnClose.Location = new Point(12, 433);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 35);
            btnClose.TabIndex = 11;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            btnClose.MouseEnter += btnClose_MouseEnter;
            btnClose.MouseLeave += btnClose_MouseLeave;
            // 
            // dgCarrier
            // 
            dgCarrier.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgCarrier.BackgroundColor = Color.FromArgb(11, 159, 161);
            dgCarrier.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.Font = new Font("Impact", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(11, 159, 161);
            dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgCarrier.DefaultCellStyle = dataGridViewCellStyle1;
            dgCarrier.GridColor = Color.Black;
            dgCarrier.Location = new Point(12, 70);
            dgCarrier.Name = "dgCarrier";
            dgCarrier.RowHeadersVisible = false;
            dgCarrier.RowTemplate.Height = 25;
            dgCarrier.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgCarrier.Size = new Size(437, 357);
            dgCarrier.TabIndex = 26;
            // 
            // CarrierCombos
            // 
            AutoScaleDimensions = new SizeF(6F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(467, 480);
            ControlBox = false;
            Controls.Add(dgCarrier);
            Controls.Add(btnClose);
            Controls.Add(lblTitle);
            Font = new Font("Impact", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "CarrierCombos";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CarrierCombos";
            Load += CarrierCombos_Load;
            ((System.ComponentModel.ISupportInitialize)dgCarrier).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitle;
        private Button btnClose;
        private DataGridView dgCarrier;
    }
}