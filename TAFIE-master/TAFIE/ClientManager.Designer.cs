namespace TAFIE
{
    partial class ClientManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientManager));
            cbClient = new ComboBox();
            lblTitle = new Label();
            label1 = new Label();
            chkActive = new CheckBox();
            cbAccCode = new ComboBox();
            lblAccCode = new Label();
            txbIOSS = new TextBox();
            lblIOSS = new Label();
            lblEORI = new Label();
            txbEORI = new TextBox();
            btnClose = new Button();
            btnSave = new Button();
            lblUpdate = new Label();
            SuspendLayout();
            // 
            // cbClient
            // 
            cbClient.BackColor = Color.DarkGray;
            cbClient.DropDownStyle = ComboBoxStyle.DropDownList;
            cbClient.FlatStyle = FlatStyle.System;
            cbClient.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            cbClient.FormattingEnabled = true;
            cbClient.Location = new Point(12, 118);
            cbClient.Name = "cbClient";
            cbClient.Size = new Size(350, 28);
            cbClient.TabIndex = 0;
            cbClient.TextChanged += cbClient_TextChanged;
            cbClient.Enter += cbClient_Enter;
            cbClient.Leave += cbClient_Leave;
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Impact", 30F, FontStyle.Regular, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.FromArgb(11, 159, 161);
            lblTitle.Location = new Point(12, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(350, 58);
            lblTitle.TabIndex = 9;
            lblTitle.Text = "Client Manager";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.FromArgb(11, 159, 161);
            label1.Location = new Point(12, 99);
            label1.Name = "label1";
            label1.Size = new Size(36, 16);
            label1.TabIndex = 10;
            label1.Text = "Client";
            // 
            // chkActive
            // 
            chkActive.AutoSize = true;
            chkActive.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            chkActive.ForeColor = Color.FromArgb(11, 159, 161);
            chkActive.Location = new Point(12, 152);
            chkActive.Name = "chkActive";
            chkActive.Size = new Size(68, 24);
            chkActive.TabIndex = 1;
            chkActive.Text = "Active";
            chkActive.UseVisualStyleBackColor = true;
            chkActive.CheckedChanged += chkActive_CheckedChanged;
            // 
            // cbAccCode
            // 
            cbAccCode.BackColor = Color.DarkGray;
            cbAccCode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAccCode.FlatStyle = FlatStyle.System;
            cbAccCode.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            cbAccCode.FormattingEnabled = true;
            cbAccCode.Location = new Point(12, 219);
            cbAccCode.Name = "cbAccCode";
            cbAccCode.Size = new Size(349, 28);
            cbAccCode.TabIndex = 2;
            cbAccCode.Enter += cbAccCode_Enter;
            cbAccCode.Leave += cbAccCode_Leave;
            // 
            // lblAccCode
            // 
            lblAccCode.AutoSize = true;
            lblAccCode.ForeColor = Color.FromArgb(11, 159, 161);
            lblAccCode.Location = new Point(11, 200);
            lblAccCode.Name = "lblAccCode";
            lblAccCode.Size = new Size(74, 16);
            lblAccCode.TabIndex = 10;
            lblAccCode.Text = "Account Code";
            // 
            // txbIOSS
            // 
            txbIOSS.BackColor = Color.White;
            txbIOSS.BorderStyle = BorderStyle.FixedSingle;
            txbIOSS.CharacterCasing = CharacterCasing.Upper;
            txbIOSS.Cursor = Cursors.IBeam;
            txbIOSS.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txbIOSS.ForeColor = Color.Black;
            txbIOSS.Location = new Point(11, 269);
            txbIOSS.MaxLength = 24;
            txbIOSS.Name = "txbIOSS";
            txbIOSS.Size = new Size(350, 27);
            txbIOSS.TabIndex = 3;
            txbIOSS.Enter += txbIOSS_Enter;
            txbIOSS.Leave += txbIOSS_Leave;
            // 
            // lblIOSS
            // 
            lblIOSS.AutoSize = true;
            lblIOSS.ForeColor = Color.FromArgb(11, 159, 161);
            lblIOSS.Location = new Point(11, 250);
            lblIOSS.Name = "lblIOSS";
            lblIOSS.Size = new Size(69, 16);
            lblIOSS.TabIndex = 10;
            lblIOSS.Text = "IOSS Number";
            // 
            // lblEORI
            // 
            lblEORI.AutoSize = true;
            lblEORI.ForeColor = Color.FromArgb(11, 159, 161);
            lblEORI.Location = new Point(12, 299);
            lblEORI.Name = "lblEORI";
            lblEORI.Size = new Size(90, 16);
            lblEORI.TabIndex = 10;
            lblEORI.Text = "EORI/Tax Number";
            // 
            // txbEORI
            // 
            txbEORI.BackColor = Color.White;
            txbEORI.BorderStyle = BorderStyle.FixedSingle;
            txbEORI.CharacterCasing = CharacterCasing.Upper;
            txbEORI.Cursor = Cursors.IBeam;
            txbEORI.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txbEORI.ForeColor = Color.Black;
            txbEORI.Location = new Point(11, 318);
            txbEORI.MaxLength = 24;
            txbEORI.Name = "txbEORI";
            txbEORI.Size = new Size(350, 27);
            txbEORI.TabIndex = 4;
            txbEORI.Enter += txbEORI_Enter;
            txbEORI.Leave += txbEORI_Leave;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.FromArgb(11, 159, 161);
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Impact", 12F, FontStyle.Italic, GraphicsUnit.Point);
            btnClose.ForeColor = Color.Black;
            btnClose.Location = new Point(10, 356);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 35);
            btnClose.TabIndex = 6;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            btnClose.MouseEnter += btnClose_MouseEnter;
            btnClose.MouseLeave += btnClose_MouseLeave;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.BackColor = Color.FromArgb(11, 159, 161);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Impact", 12F, FontStyle.Italic, GraphicsUnit.Point);
            btnSave.ForeColor = Color.Black;
            btnSave.Location = new Point(286, 356);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 35);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            btnSave.MouseEnter += btnSave_MouseEnter;
            btnSave.MouseLeave += btnSave_MouseLeave;
            // 
            // lblUpdate
            // 
            lblUpdate.BackColor = Color.Transparent;
            lblUpdate.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblUpdate.ForeColor = Color.White;
            lblUpdate.Location = new Point(10, 67);
            lblUpdate.Name = "lblUpdate";
            lblUpdate.Size = new Size(350, 32);
            lblUpdate.TabIndex = 9;
            lblUpdate.Text = "Client Updated";
            lblUpdate.TextAlign = ContentAlignment.MiddleLeft;
            lblUpdate.Visible = false;
            // 
            // ClientManager
            // 
            AutoScaleDimensions = new SizeF(6F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(374, 406);
            ControlBox = false;
            Controls.Add(txbEORI);
            Controls.Add(txbIOSS);
            Controls.Add(cbAccCode);
            Controls.Add(lblEORI);
            Controls.Add(chkActive);
            Controls.Add(lblIOSS);
            Controls.Add(lblAccCode);
            Controls.Add(label1);
            Controls.Add(lblUpdate);
            Controls.Add(lblTitle);
            Controls.Add(cbClient);
            Controls.Add(btnSave);
            Controls.Add(btnClose);
            Font = new Font("Impact", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ClientManager";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ClientManager";
            Load += ClientManager_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cbClient;
        private Label lblTitle;
        private Label label1;
        private CheckBox chkActive;
        private ComboBox cbAccCode;
        private Label lblAccCode;
        private TextBox txbIOSS;
        private Label lblIOSS;
        private Label lblEORI;
        private TextBox txbEORI;
        private Button btnClose;
        private Button btnSave;
        private Label lblUpdate;
    }
}