namespace TAFIE
{
    partial class RePrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RePrint));
            btnExit = new Button();
            btnReprint = new Button();
            txbSearch = new TextBox();
            btnSearch = new Button();
            dgReprint = new DataGridView();
            lblTitle = new Label();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgReprint).BeginInit();
            SuspendLayout();
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.FromArgb(11, 159, 161);
            btnExit.Cursor = Cursors.Hand;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnExit.ForeColor = Color.Black;
            btnExit.Location = new Point(12, 481);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(94, 46);
            btnExit.TabIndex = 24;
            btnExit.Text = "Close";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            btnExit.MouseEnter += btnExit_MouseEnter;
            btnExit.MouseLeave += btnExit_MouseLeave;
            // 
            // btnReprint
            // 
            btnReprint.BackColor = Color.FromArgb(11, 159, 161);
            btnReprint.Cursor = Cursors.Hand;
            btnReprint.FlatStyle = FlatStyle.Flat;
            btnReprint.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnReprint.ForeColor = Color.Black;
            btnReprint.Location = new Point(411, 481);
            btnReprint.Name = "btnReprint";
            btnReprint.Size = new Size(129, 46);
            btnReprint.TabIndex = 24;
            btnReprint.Text = "Re-Print";
            btnReprint.UseVisualStyleBackColor = false;
            btnReprint.Click += btnReprint_Click;
            btnReprint.MouseEnter += btnReprint_MouseEnter;
            btnReprint.MouseLeave += btnReprint_MouseLeave;
            // 
            // txbSearch
            // 
            txbSearch.BackColor = Color.White;
            txbSearch.BorderStyle = BorderStyle.FixedSingle;
            txbSearch.CharacterCasing = CharacterCasing.Upper;
            txbSearch.Cursor = Cursors.IBeam;
            txbSearch.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txbSearch.ForeColor = Color.Black;
            txbSearch.Location = new Point(12, 102);
            txbSearch.MaxLength = 24;
            txbSearch.Name = "txbSearch";
            txbSearch.Size = new Size(257, 27);
            txbSearch.TabIndex = 25;
            txbSearch.Enter += txbSearch_Enter;
            txbSearch.Leave += txbSearch_Leave;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(11, 159, 161);
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Impact", 10F, FontStyle.Italic, GraphicsUnit.Point);
            btnSearch.ForeColor = Color.Black;
            btnSearch.Location = new Point(275, 102);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(74, 26);
            btnSearch.TabIndex = 24;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            btnSearch.MouseEnter += btnSearch_MouseEnter;
            btnSearch.MouseLeave += btnSearch_MouseLeave;
            // 
            // dgReprint
            // 
            dgReprint.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgReprint.BackgroundColor = Color.White;
            dgReprint.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Impact", 11F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = Color.Black;
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(11, 159, 161);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgReprint.DefaultCellStyle = dataGridViewCellStyle1;
            dgReprint.GridColor = Color.Black;
            dgReprint.Location = new Point(12, 160);
            dgReprint.Name = "dgReprint";
            dgReprint.RowHeadersVisible = false;
            dgReprint.RowTemplate.Height = 25;
            dgReprint.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgReprint.Size = new Size(528, 315);
            dgReprint.TabIndex = 26;
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Impact", 40F, FontStyle.Regular, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.FromArgb(11, 159, 161);
            lblTitle.Location = new Point(12, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(528, 90);
            lblTitle.TabIndex = 27;
            lblTitle.Text = "Re-Print Carrier Label";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.Font = new Font("Impact", 11F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 132);
            label1.Name = "label1";
            label1.Size = new Size(528, 25);
            label1.TabIndex = 28;
            label1.Text = "Only labels created in the last 24 hours can be re-printed";
            // 
            // RePrint
            // 
            AutoScaleDimensions = new SizeF(6F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(557, 539);
            ControlBox = false;
            Controls.Add(label1);
            Controls.Add(lblTitle);
            Controls.Add(dgReprint);
            Controls.Add(txbSearch);
            Controls.Add(btnReprint);
            Controls.Add(btnSearch);
            Controls.Add(btnExit);
            Font = new Font("Impact", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "RePrint";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RePrint";
            Load += RePrint_Load;
            ((System.ComponentModel.ISupportInitialize)dgReprint).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnExit;
        private Button btnReprint;
        private TextBox txbSearch;
        private Button btnSearch;
        private DataGridView dgReprint;
        private Label lblTitle;
        private Label label1;
    }
}