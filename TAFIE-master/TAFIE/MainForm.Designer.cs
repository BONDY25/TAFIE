namespace TAFIE
{
    partial class MainForm
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            panel1 = new Panel();
            panel2 = new Panel();
            btnExit = new Button();
            btnSearch = new Button();
            pbLogo = new PictureBox();
            cbClient = new ComboBox();
            txbLoadNote = new TextBox();
            label1 = new Label();
            label2 = new Label();
            lblTitle = new Label();
            rtbxError = new RichTextBox();
            rtbxFix = new RichTextBox();
            label3 = new Label();
            label4 = new Label();
            rtbxCall = new RichTextBox();
            label5 = new Label();
            rtbxResp = new RichTextBox();
            label6 = new Label();
            btnExpand = new Button();
            btnClear = new Button();
            label7 = new Label();
            lblUsername = new Label();
            ttAPI = new ToolTip(components);
            btnFix = new Button();
            lblFixAtmp = new Label();
            btnGetLbl = new Button();
            ((System.ComponentModel.ISupportInitialize)pbLogo).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.DarkGray;
            panel1.Location = new Point(0, 197);
            panel1.Name = "panel1";
            panel1.Size = new Size(702, 11);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BackColor = Color.DarkGray;
            panel2.Location = new Point(0, 463);
            panel2.Name = "panel2";
            panel2.Size = new Size(702, 11);
            panel2.TabIndex = 0;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.FromArgb(11, 159, 161);
            btnExit.Cursor = Cursors.Hand;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnExit.ForeColor = Color.Black;
            btnExit.Location = new Point(12, 772);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(75, 46);
            btnExit.TabIndex = 1;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            btnExit.MouseEnter += btnExit_MouseEnter;
            btnExit.MouseLeave += btnExit_MouseLeave;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(11, 159, 161);
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Impact", 12F, FontStyle.Italic, GraphicsUnit.Point);
            btnSearch.ForeColor = Color.Black;
            btnSearch.Location = new Point(444, 163);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(71, 28);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            btnSearch.MouseEnter += btnSearch_MouseEnter;
            btnSearch.MouseLeave += btnSearch_MouseLeave;
            // 
            // pbLogo
            // 
            pbLogo.Image = Properties.Resources.TAFIE_Logo;
            pbLogo.Location = new Point(563, 12);
            pbLogo.Name = "pbLogo";
            pbLogo.Size = new Size(127, 118);
            pbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            pbLogo.TabIndex = 3;
            pbLogo.TabStop = false;
            // 
            // cbClient
            // 
            cbClient.BackColor = Color.DarkGray;
            cbClient.DropDownStyle = ComboBoxStyle.DropDownList;
            cbClient.FlatStyle = FlatStyle.System;
            cbClient.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            cbClient.FormattingEnabled = true;
            cbClient.Location = new Point(12, 163);
            cbClient.Name = "cbClient";
            cbClient.Size = new Size(206, 28);
            cbClient.TabIndex = 4;
            cbClient.TextChanged += cbClient_TextChanged;
            cbClient.Enter += cbClient_Enter;
            cbClient.Leave += cbClient_Leave;
            // 
            // txbLoadNote
            // 
            txbLoadNote.BackColor = Color.White;
            txbLoadNote.BorderStyle = BorderStyle.FixedSingle;
            txbLoadNote.CharacterCasing = CharacterCasing.Upper;
            txbLoadNote.Cursor = Cursors.IBeam;
            txbLoadNote.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txbLoadNote.ForeColor = Color.Black;
            txbLoadNote.Location = new Point(232, 164);
            txbLoadNote.MaxLength = 24;
            txbLoadNote.Name = "txbLoadNote";
            txbLoadNote.Size = new Size(206, 27);
            txbLoadNote.TabIndex = 5;
            txbLoadNote.Enter += txbLoadNote_Enter;
            txbLoadNote.Leave += txbLoadNote_Leave;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.FromArgb(11, 159, 161);
            label1.Location = new Point(12, 144);
            label1.Name = "label1";
            label1.Size = new Size(36, 16);
            label1.TabIndex = 6;
            label1.Text = "Client";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.FromArgb(11, 159, 161);
            label2.Location = new Point(232, 145);
            label2.Name = "label2";
            label2.Size = new Size(55, 16);
            label2.TabIndex = 7;
            label2.Text = "Load Note";
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Impact", 60F, FontStyle.Regular, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.FromArgb(11, 159, 161);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(232, 90);
            lblTitle.TabIndex = 8;
            lblTitle.Text = "Home";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // rtbxError
            // 
            rtbxError.BackColor = Color.FromArgb(224, 224, 224);
            rtbxError.BorderStyle = BorderStyle.FixedSingle;
            rtbxError.Font = new Font("Arial", 14F, FontStyle.Bold, GraphicsUnit.Point);
            rtbxError.Location = new Point(12, 228);
            rtbxError.Name = "rtbxError";
            rtbxError.ReadOnly = true;
            rtbxError.Size = new Size(678, 96);
            rtbxError.TabIndex = 9;
            rtbxError.Text = "";
            // 
            // rtbxFix
            // 
            rtbxFix.BackColor = Color.FromArgb(224, 224, 224);
            rtbxFix.BorderStyle = BorderStyle.FixedSingle;
            rtbxFix.Font = new Font("Arial", 14F, FontStyle.Bold, GraphicsUnit.Point);
            rtbxFix.Location = new Point(12, 351);
            rtbxFix.Name = "rtbxFix";
            rtbxFix.ReadOnly = true;
            rtbxFix.Size = new Size(580, 96);
            rtbxFix.TabIndex = 9;
            rtbxFix.Text = "";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.FromArgb(11, 159, 161);
            label3.Location = new Point(12, 211);
            label3.Name = "label3";
            label3.Size = new Size(77, 16);
            label3.TabIndex = 10;
            label3.Text = "Error Message";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.FromArgb(11, 159, 161);
            label4.Location = new Point(12, 332);
            label4.Name = "label4";
            label4.Size = new Size(75, 16);
            label4.TabIndex = 10;
            label4.Text = "Suggested Fix";
            // 
            // rtbxCall
            // 
            rtbxCall.BackColor = Color.FromArgb(224, 224, 224);
            rtbxCall.BorderStyle = BorderStyle.FixedSingle;
            rtbxCall.Cursor = Cursors.Cross;
            rtbxCall.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            rtbxCall.Location = new Point(12, 496);
            rtbxCall.Name = "rtbxCall";
            rtbxCall.ReadOnly = true;
            rtbxCall.Size = new Size(333, 270);
            rtbxCall.TabIndex = 11;
            rtbxCall.Text = "";
            rtbxCall.DoubleClick += rtbxCall_DoubleClick;
            rtbxCall.MouseEnter += rtbxCall_MouseEnter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.FromArgb(11, 159, 161);
            label5.Location = new Point(12, 477);
            label5.Name = "label5";
            label5.Size = new Size(43, 16);
            label5.TabIndex = 10;
            label5.Text = "API Call";
            // 
            // rtbxResp
            // 
            rtbxResp.BackColor = Color.FromArgb(224, 224, 224);
            rtbxResp.BorderStyle = BorderStyle.FixedSingle;
            rtbxResp.Cursor = Cursors.Cross;
            rtbxResp.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            rtbxResp.Location = new Point(357, 496);
            rtbxResp.Name = "rtbxResp";
            rtbxResp.ReadOnly = true;
            rtbxResp.Size = new Size(333, 270);
            rtbxResp.TabIndex = 12;
            rtbxResp.Text = "";
            rtbxResp.DoubleClick += rtbxResp_DoubleClick;
            rtbxResp.MouseEnter += rtbxResp_MouseEnter;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = Color.FromArgb(11, 159, 161);
            label6.Location = new Point(357, 477);
            label6.Name = "label6";
            label6.Size = new Size(72, 16);
            label6.TabIndex = 13;
            label6.Text = "API Response";
            // 
            // btnExpand
            // 
            btnExpand.BackColor = Color.FromArgb(11, 159, 161);
            btnExpand.Cursor = Cursors.Hand;
            btnExpand.FlatStyle = FlatStyle.Flat;
            btnExpand.Font = new Font("Impact", 12F, FontStyle.Italic, GraphicsUnit.Point);
            btnExpand.ForeColor = Color.Black;
            btnExpand.Location = new Point(661, 162);
            btnExpand.Name = "btnExpand";
            btnExpand.Size = new Size(29, 28);
            btnExpand.TabIndex = 14;
            btnExpand.Text = "+";
            btnExpand.UseVisualStyleBackColor = false;
            btnExpand.Click += btnExpand_Click;
            btnExpand.MouseEnter += btnExpand_MouseEnter;
            btnExpand.MouseLeave += btnExpand_MouseLeave;
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.FromArgb(11, 159, 161);
            btnClear.Cursor = Cursors.Hand;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Impact", 12F, FontStyle.Italic, GraphicsUnit.Point);
            btnClear.ForeColor = Color.Black;
            btnClear.Location = new Point(521, 163);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(71, 28);
            btnClear.TabIndex = 15;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;
            btnClear.MouseEnter += btnClear_MouseEnter;
            btnClear.MouseLeave += btnClear_MouseLeave;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Impact", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label7.ForeColor = Color.FromArgb(11, 159, 161);
            label7.Location = new Point(12, 90);
            label7.Name = "label7";
            label7.Size = new Size(224, 17);
            label7.TabIndex = 16;
            label7.Text = "Toolbox for APIs to Fix Integration Errors";
            // 
            // lblUsername
            // 
            lblUsername.ForeColor = Color.FromArgb(11, 159, 161);
            lblUsername.Location = new Point(238, 12);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(319, 16);
            lblUsername.TabIndex = 17;
            lblUsername.Text = "Username";
            lblUsername.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ttAPI
            // 
            ttAPI.BackColor = Color.Black;
            ttAPI.ForeColor = Color.White;
            // 
            // btnFix
            // 
            btnFix.BackColor = Color.FromArgb(11, 159, 161);
            btnFix.Cursor = Cursors.Hand;
            btnFix.FlatStyle = FlatStyle.Flat;
            btnFix.Font = new Font("Impact", 12F, FontStyle.Italic, GraphicsUnit.Point);
            btnFix.ForeColor = Color.Black;
            btnFix.Location = new Point(598, 351);
            btnFix.Name = "btnFix";
            btnFix.Size = new Size(92, 96);
            btnFix.TabIndex = 18;
            btnFix.Text = "Fix";
            btnFix.UseVisualStyleBackColor = false;
            btnFix.Visible = false;
            btnFix.VisibleChanged += btnFix_VisibleChanged;
            btnFix.Click += btnFix_Click;
            btnFix.MouseEnter += btnFix_MouseEnter;
            btnFix.MouseLeave += btnFix_MouseLeave;
            // 
            // lblFixAtmp
            // 
            lblFixAtmp.BackColor = Color.Transparent;
            lblFixAtmp.Font = new Font("Impact", 13F, FontStyle.Regular, GraphicsUnit.Point);
            lblFixAtmp.ForeColor = Color.FromArgb(11, 159, 161);
            lblFixAtmp.Location = new Point(238, 103);
            lblFixAtmp.Name = "lblFixAtmp";
            lblFixAtmp.Size = new Size(319, 27);
            lblFixAtmp.TabIndex = 19;
            lblFixAtmp.Text = "Fix Last Attempted: XXXX/XX/XX XX:XX:XXX";
            lblFixAtmp.TextAlign = ContentAlignment.BottomRight;
            lblFixAtmp.Visible = false;
            // 
            // btnGetLbl
            // 
            btnGetLbl.BackColor = Color.FromArgb(11, 159, 161);
            btnGetLbl.Cursor = Cursors.Hand;
            btnGetLbl.FlatStyle = FlatStyle.Flat;
            btnGetLbl.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnGetLbl.ForeColor = Color.Black;
            btnGetLbl.Location = new Point(501, 772);
            btnGetLbl.Name = "btnGetLbl";
            btnGetLbl.Size = new Size(189, 46);
            btnGetLbl.TabIndex = 20;
            btnGetLbl.Text = "Get Label";
            btnGetLbl.UseVisualStyleBackColor = false;
            btnGetLbl.Click += btnGetLbl_Click;
            btnGetLbl.MouseEnter += btnGetLbl_MouseEnter;
            btnGetLbl.MouseLeave += btnGetLbl_MouseLeave;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(6F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(702, 827);
            Controls.Add(btnGetLbl);
            Controls.Add(lblFixAtmp);
            Controls.Add(btnFix);
            Controls.Add(lblUsername);
            Controls.Add(label7);
            Controls.Add(btnClear);
            Controls.Add(btnExpand);
            Controls.Add(label6);
            Controls.Add(rtbxResp);
            Controls.Add(rtbxCall);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(rtbxFix);
            Controls.Add(rtbxError);
            Controls.Add(lblTitle);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txbLoadNote);
            Controls.Add(cbClient);
            Controls.Add(pbLogo);
            Controls.Add(btnSearch);
            Controls.Add(btnExit);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Font = new Font("Impact", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MainForm";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            KeyDown += MainForm_KeyDown;
            ((System.ComponentModel.ISupportInitialize)pbLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Button btnExit;
        private Button btnSearch;
        private PictureBox pbLogo;
        private ComboBox cbClient;
        private TextBox txbLoadNote;
        private Label label1;
        private Label label2;
        private Label lblTitle;
        private RichTextBox rtbxError;
        private RichTextBox rtbxFix;
        private Label label3;
        private Label label4;
        private RichTextBox rtbxCall;
        private Label label5;
        private RichTextBox rtbxResp;
        private Label label6;
        private Button btnExpand;
        private Button btnClear;
        private Label label7;
        private Label lblUsername;
        private ToolTip ttAPI;
        private Button btnFix;
        private Label lblFixAtmp;
        private Button btnGetLbl;
    }
}