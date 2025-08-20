namespace TAFIE
{
    partial class BoxManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoxManager));
            lblTitle = new Label();
            txbBoxQty = new TextBox();
            label1 = new Label();
            btnExit = new Button();
            btnSave = new Button();
            dgContents = new DataGridView();
            BoxNo = new DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgContents).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Impact", 30F, FontStyle.Regular, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.FromArgb(11, 159, 161);
            lblTitle.Location = new Point(10, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(649, 96);
            lblTitle.TabIndex = 25;
            lblTitle.Text = "Box Manager";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txbBoxQty
            // 
            txbBoxQty.Font = new Font("Impact", 14F, FontStyle.Regular, GraphicsUnit.Point);
            txbBoxQty.Location = new Point(96, 126);
            txbBoxQty.Name = "txbBoxQty";
            txbBoxQty.Size = new Size(77, 30);
            txbBoxQty.TabIndex = 26;
            txbBoxQty.Enter += txbBoxQty_Enter;
            txbBoxQty.Leave += txbBoxQty_Leave;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Impact", 14F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.FromArgb(11, 159, 161);
            label1.Location = new Point(12, 126);
            label1.Name = "label1";
            label1.Size = new Size(78, 29);
            label1.TabIndex = 28;
            label1.Text = "Boxes";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.FromArgb(11, 159, 161);
            btnExit.Cursor = Cursors.Hand;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnExit.ForeColor = Color.Black;
            btnExit.Location = new Point(12, 474);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(136, 46);
            btnExit.TabIndex = 29;
            btnExit.Text = "Cancel";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            btnExit.MouseEnter += btnExit_MouseEnter;
            btnExit.MouseLeave += btnExit_MouseLeave;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(11, 159, 161);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnSave.ForeColor = Color.Black;
            btnSave.Location = new Point(534, 474);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(125, 46);
            btnSave.TabIndex = 30;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            btnSave.MouseEnter += btnSave_MouseEnter;
            btnSave.MouseLeave += btnSave_MouseLeave;
            // 
            // dgContents
            // 
            dgContents.AllowUserToAddRows = false;
            dgContents.AllowUserToDeleteRows = false;
            dgContents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgContents.BackgroundColor = Color.White;
            dgContents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgContents.Columns.AddRange(new DataGridViewColumn[] { BoxNo });
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = Color.White;
            dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgContents.DefaultCellStyle = dataGridViewCellStyle1;
            dgContents.GridColor = Color.Black;
            dgContents.Location = new Point(12, 162);
            dgContents.Name = "dgContents";
            dgContents.RowHeadersVisible = false;
            dgContents.RowTemplate.Height = 25;
            dgContents.Size = new Size(647, 306);
            dgContents.TabIndex = 31;
            // 
            // BoxNo
            // 
            BoxNo.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            BoxNo.HeaderText = "Box Num";
            BoxNo.Name = "BoxNo";
            // 
            // BoxManager
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.Black;
            ClientSize = new Size(671, 531);
            ControlBox = false;
            Controls.Add(dgContents);
            Controls.Add(btnSave);
            Controls.Add(btnExit);
            Controls.Add(label1);
            Controls.Add(txbBoxQty);
            Controls.Add(lblTitle);
            Font = new Font("Impact", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "BoxManager";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "BoxManager";
            Load += BoxManager_Load;
            ((System.ComponentModel.ISupportInitialize)dgContents).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private TextBox txbBoxQty;
        private Label label1;
        private Button btnExit;
        private Button btnSave;
        private DataGridView dgContents;
        private DataGridViewComboBoxColumn BoxNo;
    }
}