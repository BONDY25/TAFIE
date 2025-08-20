namespace TAFIE
{
    partial class FixForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FixForm));
            btnClose = new Button();
            btnUpdate = new Button();
            lblTitle = new Label();
            panel1 = new Panel();
            lblSuggFix = new Label();
            lblField = new Label();
            cntAddr = new Panel();
            cbCountry = new ComboBox();
            rtbAddress = new RichTextBox();
            txbCompany = new TextBox();
            txbSurname = new TextBox();
            lblCtry = new Label();
            lblPc = new Label();
            lblCnty = new Label();
            lblCity = new Label();
            lblAddr = new Label();
            lblCom = new Label();
            lblLn = new Label();
            lblFn = new Label();
            label1 = new Label();
            txbPostcode = new TextBox();
            txbCounty = new TextBox();
            txbCity = new TextBox();
            txbInitials = new TextBox();
            cntContact = new Panel();
            label15 = new Label();
            lblEmail = new Label();
            lblPhone4 = new Label();
            lblPhone3 = new Label();
            lblPhone2 = new Label();
            lblPhone1 = new Label();
            txbEmail = new TextBox();
            txbMobile = new TextBox();
            txbPhoneExt = new TextBox();
            txbPhoneEve = new TextBox();
            txbPhoneDay = new TextBox();
            ttPc = new ToolTip(components);
            panel1.SuspendLayout();
            cntAddr.SuspendLayout();
            cntContact.SuspendLayout();
            SuspendLayout();
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(11, 159, 161);
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnClose.ForeColor = Color.Black;
            btnClose.Location = new Point(12, 543);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(95, 46);
            btnClose.TabIndex = 14;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            btnClose.MouseEnter += btnClose_MouseEnter;
            btnClose.MouseLeave += btnClose_MouseLeave;
            // 
            // btnUpdate
            // 
            btnUpdate.BackColor = Color.FromArgb(11, 159, 161);
            btnUpdate.Cursor = Cursors.Hand;
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnUpdate.ForeColor = Color.Black;
            btnUpdate.Location = new Point(421, 543);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(95, 46);
            btnUpdate.TabIndex = 13;
            btnUpdate.Text = "Update";
            btnUpdate.UseVisualStyleBackColor = false;
            btnUpdate.Click += btnUpdate_Click;
            btnUpdate.MouseEnter += btnUpdate_MouseEnter;
            btnUpdate.MouseLeave += btnUpdate_MouseLeave;
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Impact", 55F, FontStyle.Regular, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.FromArgb(11, 159, 161);
            lblTitle.Location = new Point(12, -4);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(504, 90);
            lblTitle.TabIndex = 10;
            lblTitle.Text = "Fix";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(11, 159, 161);
            panel1.Controls.Add(lblSuggFix);
            panel1.Controls.Add(lblField);
            panel1.Location = new Point(12, 89);
            panel1.Name = "panel1";
            panel1.Size = new Size(504, 103);
            panel1.TabIndex = 11;
            // 
            // lblSuggFix
            // 
            lblSuggFix.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblSuggFix.Location = new Point(3, 30);
            lblSuggFix.Name = "lblSuggFix";
            lblSuggFix.Size = new Size(489, 62);
            lblSuggFix.TabIndex = 2;
            lblSuggFix.Text = "SuggFix";
            // 
            // lblField
            // 
            lblField.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblField.Location = new Point(3, 10);
            lblField.Name = "lblField";
            lblField.Size = new Size(489, 20);
            lblField.TabIndex = 1;
            lblField.Text = "Field";
            // 
            // cntAddr
            // 
            cntAddr.BackColor = Color.FromArgb(11, 159, 161);
            cntAddr.Controls.Add(cbCountry);
            cntAddr.Controls.Add(rtbAddress);
            cntAddr.Controls.Add(txbCompany);
            cntAddr.Controls.Add(txbSurname);
            cntAddr.Controls.Add(lblCtry);
            cntAddr.Controls.Add(lblPc);
            cntAddr.Controls.Add(lblCnty);
            cntAddr.Controls.Add(lblCity);
            cntAddr.Controls.Add(lblAddr);
            cntAddr.Controls.Add(lblCom);
            cntAddr.Controls.Add(lblLn);
            cntAddr.Controls.Add(lblFn);
            cntAddr.Controls.Add(label1);
            cntAddr.Controls.Add(txbPostcode);
            cntAddr.Controls.Add(txbCounty);
            cntAddr.Controls.Add(txbCity);
            cntAddr.Controls.Add(txbInitials);
            cntAddr.Location = new Point(12, 198);
            cntAddr.Name = "cntAddr";
            cntAddr.Size = new Size(504, 339);
            cntAddr.TabIndex = 12;
            // 
            // cbCountry
            // 
            cbCountry.BackColor = Color.White;
            cbCountry.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCountry.FlatStyle = FlatStyle.System;
            cbCountry.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            cbCountry.ForeColor = Color.Black;
            cbCountry.FormattingEnabled = true;
            cbCountry.Location = new Point(4, 297);
            cbCountry.Name = "cbCountry";
            cbCountry.Size = new Size(140, 26);
            cbCountry.TabIndex = 12;
            cbCountry.Enter += cbCountry_Enter;
            cbCountry.Leave += cbCountry_Leave;
            // 
            // rtbAddress
            // 
            rtbAddress.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            rtbAddress.Location = new Point(4, 135);
            rtbAddress.MaxLength = 255;
            rtbAddress.Name = "rtbAddress";
            rtbAddress.Size = new Size(493, 96);
            rtbAddress.TabIndex = 8;
            rtbAddress.Text = "";
            rtbAddress.TextChanged += rtbAddress_TextChanged;
            rtbAddress.Enter += rtbAddress_Enter;
            rtbAddress.Leave += rtbAddress_Leave;
            // 
            // txbCompany
            // 
            txbCompany.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            txbCompany.Location = new Point(3, 91);
            txbCompany.MaxLength = 100;
            txbCompany.Name = "txbCompany";
            txbCompany.Size = new Size(494, 25);
            txbCompany.TabIndex = 7;
            txbCompany.TextChanged += txbCompany_TextChanged;
            txbCompany.Enter += txbCompany_Enter;
            txbCompany.Leave += txbCompany_Leave;
            // 
            // txbSurname
            // 
            txbSurname.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            txbSurname.Location = new Point(155, 47);
            txbSurname.MaxLength = 100;
            txbSurname.Name = "txbSurname";
            txbSurname.Size = new Size(342, 25);
            txbSurname.TabIndex = 6;
            txbSurname.TextChanged += txbInitials_TextChanged;
            txbSurname.Enter += txbSurname_Enter;
            txbSurname.Leave += txbSurname_Leave;
            // 
            // lblCtry
            // 
            lblCtry.Location = new Point(4, 278);
            lblCtry.Name = "lblCtry";
            lblCtry.Size = new Size(140, 16);
            lblCtry.TabIndex = 3;
            lblCtry.Text = "Country";
            // 
            // lblPc
            // 
            lblPc.Cursor = Cursors.Hand;
            lblPc.Location = new Point(358, 234);
            lblPc.Name = "lblPc";
            lblPc.Size = new Size(139, 16);
            lblPc.TabIndex = 3;
            lblPc.Text = "Post Code";
            lblPc.DoubleClick += lblPc_DoubleClick;
            lblPc.MouseEnter += lblPc_MouseEnter;
            // 
            // lblCnty
            // 
            lblCnty.Location = new Point(205, 234);
            lblCnty.Name = "lblCnty";
            lblCnty.Size = new Size(146, 16);
            lblCnty.TabIndex = 3;
            lblCnty.Text = "County";
            // 
            // lblCity
            // 
            lblCity.Location = new Point(4, 234);
            lblCity.Name = "lblCity";
            lblCity.Size = new Size(192, 16);
            lblCity.TabIndex = 3;
            lblCity.Text = "City";
            // 
            // lblAddr
            // 
            lblAddr.Location = new Point(4, 116);
            lblAddr.Name = "lblAddr";
            lblAddr.Size = new Size(493, 16);
            lblAddr.TabIndex = 3;
            lblAddr.Text = "Address";
            // 
            // lblCom
            // 
            lblCom.Location = new Point(4, 72);
            lblCom.Name = "lblCom";
            lblCom.Size = new Size(493, 16);
            lblCom.TabIndex = 3;
            lblCom.Text = "Company";
            // 
            // lblLn
            // 
            lblLn.Location = new Point(155, 28);
            lblLn.Name = "lblLn";
            lblLn.Size = new Size(342, 16);
            lblLn.TabIndex = 3;
            lblLn.Text = "Last Name";
            // 
            // lblFn
            // 
            lblFn.Location = new Point(4, 28);
            lblFn.Name = "lblFn";
            lblFn.Size = new Size(145, 16);
            lblFn.TabIndex = 3;
            lblFn.Text = "First Name";
            // 
            // label1
            // 
            label1.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(3, 6);
            label1.Name = "label1";
            label1.Size = new Size(489, 22);
            label1.TabIndex = 2;
            label1.Text = "Address Details";
            // 
            // txbPostcode
            // 
            txbPostcode.CharacterCasing = CharacterCasing.Upper;
            txbPostcode.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            txbPostcode.Location = new Point(358, 253);
            txbPostcode.MaxLength = 100;
            txbPostcode.Name = "txbPostcode";
            txbPostcode.Size = new Size(139, 25);
            txbPostcode.TabIndex = 11;
            txbPostcode.TextChanged += txbPostcode_TextChanged;
            txbPostcode.Enter += txbPostcode_Enter;
            txbPostcode.Leave += txbPostcode_Leave;
            // 
            // txbCounty
            // 
            txbCounty.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            txbCounty.Location = new Point(205, 253);
            txbCounty.MaxLength = 100;
            txbCounty.Name = "txbCounty";
            txbCounty.Size = new Size(146, 25);
            txbCounty.TabIndex = 10;
            txbCounty.TextChanged += txbCounty_TextChanged;
            txbCounty.Enter += txbCounty_Enter;
            txbCounty.Leave += txbCounty_Leave;
            // 
            // txbCity
            // 
            txbCity.CharacterCasing = CharacterCasing.Upper;
            txbCity.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            txbCity.Location = new Point(4, 253);
            txbCity.MaxLength = 100;
            txbCity.Name = "txbCity";
            txbCity.Size = new Size(192, 25);
            txbCity.TabIndex = 9;
            txbCity.TextChanged += txbCity_TextChanged;
            txbCity.Enter += txbCity_Enter;
            txbCity.Leave += txbCity_Leave;
            // 
            // txbInitials
            // 
            txbInitials.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            txbInitials.Location = new Point(3, 47);
            txbInitials.MaxLength = 100;
            txbInitials.Name = "txbInitials";
            txbInitials.Size = new Size(146, 25);
            txbInitials.TabIndex = 5;
            txbInitials.TextChanged += txbInitials_TextChanged;
            txbInitials.Enter += txbInitials_Enter;
            txbInitials.Leave += txbInitials_Leave;
            // 
            // cntContact
            // 
            cntContact.BackColor = Color.FromArgb(11, 159, 161);
            cntContact.Controls.Add(label15);
            cntContact.Controls.Add(lblEmail);
            cntContact.Controls.Add(lblPhone4);
            cntContact.Controls.Add(lblPhone3);
            cntContact.Controls.Add(lblPhone2);
            cntContact.Controls.Add(lblPhone1);
            cntContact.Controls.Add(txbEmail);
            cntContact.Controls.Add(txbMobile);
            cntContact.Controls.Add(txbPhoneExt);
            cntContact.Controls.Add(txbPhoneEve);
            cntContact.Controls.Add(txbPhoneDay);
            cntContact.Location = new Point(12, 198);
            cntContact.Name = "cntContact";
            cntContact.Size = new Size(504, 339);
            cntContact.TabIndex = 7;
            // 
            // label15
            // 
            label15.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label15.Location = new Point(3, 6);
            label15.Name = "label15";
            label15.Size = new Size(263, 22);
            label15.TabIndex = 7;
            label15.Text = "Contact Details";
            // 
            // lblEmail
            // 
            lblEmail.Location = new Point(4, 210);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(423, 16);
            lblEmail.TabIndex = 4;
            lblEmail.Text = "Email";
            // 
            // lblPhone4
            // 
            lblPhone4.Location = new Point(4, 163);
            lblPhone4.Name = "lblPhone4";
            lblPhone4.Size = new Size(227, 16);
            lblPhone4.TabIndex = 4;
            lblPhone4.Text = "Mobile";
            // 
            // lblPhone3
            // 
            lblPhone3.Location = new Point(4, 119);
            lblPhone3.Name = "lblPhone3";
            lblPhone3.Size = new Size(227, 16);
            lblPhone3.TabIndex = 4;
            lblPhone3.Text = "Phone ext";
            // 
            // lblPhone2
            // 
            lblPhone2.Location = new Point(3, 75);
            lblPhone2.Name = "lblPhone2";
            lblPhone2.Size = new Size(228, 16);
            lblPhone2.TabIndex = 4;
            lblPhone2.Text = "Phone Eve";
            // 
            // lblPhone1
            // 
            lblPhone1.Location = new Point(4, 28);
            lblPhone1.Name = "lblPhone1";
            lblPhone1.Size = new Size(227, 16);
            lblPhone1.TabIndex = 4;
            lblPhone1.Text = "Phone Day";
            // 
            // txbEmail
            // 
            txbEmail.CharacterCasing = CharacterCasing.Lower;
            txbEmail.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            txbEmail.Location = new Point(4, 225);
            txbEmail.Name = "txbEmail";
            txbEmail.Size = new Size(423, 25);
            txbEmail.TabIndex = 4;
            txbEmail.TextChanged += txbEmail_TextChanged;
            txbEmail.Enter += txbEmail_Enter;
            txbEmail.Leave += txbEmail_Leave;
            // 
            // txbMobile
            // 
            txbMobile.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            txbMobile.Location = new Point(4, 182);
            txbMobile.Name = "txbMobile";
            txbMobile.Size = new Size(228, 25);
            txbMobile.TabIndex = 3;
            txbMobile.TextChanged += txbMobile_TextChanged;
            txbMobile.Enter += txbMobile_Enter;
            txbMobile.Leave += txbMobile_Leave;
            // 
            // txbPhoneExt
            // 
            txbPhoneExt.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            txbPhoneExt.Location = new Point(4, 135);
            txbPhoneExt.Name = "txbPhoneExt";
            txbPhoneExt.Size = new Size(228, 25);
            txbPhoneExt.TabIndex = 2;
            txbPhoneExt.TextChanged += txbPhoneExt_TextChanged;
            txbPhoneExt.Enter += txbPhoneExt_Enter;
            txbPhoneExt.Leave += txbPhoneExt_Leave;
            // 
            // txbPhoneEve
            // 
            txbPhoneEve.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            txbPhoneEve.Location = new Point(4, 91);
            txbPhoneEve.Name = "txbPhoneEve";
            txbPhoneEve.Size = new Size(228, 25);
            txbPhoneEve.TabIndex = 1;
            txbPhoneEve.TextChanged += txbPhoneEve_TextChanged;
            txbPhoneEve.Enter += txbPhoneEve_Enter;
            txbPhoneEve.Leave += txbPhoneEve_Leave;
            // 
            // txbPhoneDay
            // 
            txbPhoneDay.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            txbPhoneDay.Location = new Point(3, 47);
            txbPhoneDay.Name = "txbPhoneDay";
            txbPhoneDay.Size = new Size(228, 25);
            txbPhoneDay.TabIndex = 0;
            txbPhoneDay.TextChanged += txbPhoneDay_TextChanged;
            txbPhoneDay.Enter += txbPhoneDay_Enter;
            txbPhoneDay.Leave += txbPhoneDay_Leave;
            // 
            // FixForm
            // 
            AutoScaleDimensions = new SizeF(6F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(528, 602);
            ControlBox = false;
            Controls.Add(cntContact);
            Controls.Add(cntAddr);
            Controls.Add(panel1);
            Controls.Add(lblTitle);
            Controls.Add(btnUpdate);
            Controls.Add(btnClose);
            Font = new Font("Impact", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FixForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FixForm";
            Load += FixForm_Load;
            KeyDown += FixForm_KeyDown;
            panel1.ResumeLayout(false);
            cntAddr.ResumeLayout(false);
            cntAddr.PerformLayout();
            cntContact.ResumeLayout(false);
            cntContact.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button btnClose;
        private Button btnUpdate;
        private Label lblTitle;
        private Panel panel1;
        private Label lblSuggFix;
        private Label lblField;
        private Panel cntAddr;
        private Label label1;
        private Label lblFn;
        private ComboBox cbCountry;
        private RichTextBox rtbAddress;
        private TextBox txbCompany;
        private TextBox txbSurname;
        private Label lblCtry;
        private Label lblCnty;
        private Label lblCity;
        private Label lblAddr;
        private Label lblCom;
        private Label lblLn;
        private TextBox txbCounty;
        private TextBox txbCity;
        private TextBox txbInitials;
        private Label lblPc;
        private TextBox txbPostcode;
        private Panel cntContact;
        private Label lblEmail;
        private Label lblPhone4;
        private Label lblPhone3;
        private Label lblPhone2;
        private Label lblPhone1;
        private TextBox txbEmail;
        private TextBox txbMobile;
        private TextBox txbPhoneExt;
        private TextBox txbPhoneEve;
        private TextBox txbPhoneDay;
        private Label label15;
        private ToolTip ttPc;
    }
}