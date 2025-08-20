namespace TAFIE
{
    partial class CarrierForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CarrierForm));
            lblTitle = new Label();
            cbClient = new ComboBox();
            cntDel = new Panel();
            label25 = new Label();
            cbCountry = new ComboBox();
            label19 = new Label();
            label18 = new Label();
            label17 = new Label();
            label16 = new Label();
            label15 = new Label();
            label14 = new Label();
            label13 = new Label();
            lblCountry = new Label();
            txbCompany = new TextBox();
            txbPostcode = new TextBox();
            txbAddr1 = new TextBox();
            txbAddr2 = new TextBox();
            txbCity = new TextBox();
            txbCounty = new TextBox();
            txbName = new TextBox();
            txbLoadNote = new TextBox();
            label2 = new Label();
            label1 = new Label();
            pbLogo = new PictureBox();
            label3 = new Label();
            label4 = new Label();
            txbEORI = new TextBox();
            txbIOSS = new TextBox();
            label6 = new Label();
            label7 = new Label();
            cntShip = new Panel();
            label11 = new Label();
            txbRef2 = new TextBox();
            txbRef1 = new TextBox();
            label9 = new Label();
            label8 = new Label();
            btnSearch = new Button();
            cntContact = new Panel();
            label24 = new Label();
            txbPhone = new TextBox();
            txbEmail = new TextBox();
            label20 = new Label();
            label21 = new Label();
            cntCont = new Panel();
            label27 = new Label();
            dgContents = new DataGridView();
            btnExit = new Button();
            lblUsername = new Label();
            lblStatus = new Label();
            cntCarr = new Panel();
            lblBoxes = new Label();
            cbService = new ComboBox();
            cbCarrier = new ComboBox();
            lblServiceDescr = new Label();
            lblDdp = new Label();
            lblDomestic = new Label();
            lblBoxes1 = new Label();
            lblVol = new Label();
            lblTotValue = new Label();
            lblTotWeight = new Label();
            label26 = new Label();
            btnPrint = new Button();
            pbLogoBack = new PictureBox();
            lblInstruct = new Label();
            msCarrier = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            printLabelToolStripMenuItem = new ToolStripMenuItem();
            reportsToolStripMenuItem = new ToolStripMenuItem();
            labelsGeneratedToolStripMenuItem = new ToolStripMenuItem();
            clientBreakdownToolStripMenuItem = new ToolStripMenuItem();
            carrierBreakdownToolStripMenuItem = new ToolStripMenuItem();
            closeToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            clearToolStripMenuItem = new ToolStripMenuItem();
            clientManagerToolStripMenuItem = new ToolStripMenuItem();
            workbenchToolStripMenuItem = new ToolStripMenuItem();
            reprintToolStripMenuItem = new ToolStripMenuItem();
            carriersToolStripMenuItem = new ToolStripMenuItem();
            combinationsToolStripMenuItem = new ToolStripMenuItem();
            addressSearchToolStripMenuItem = new ToolStripMenuItem();
            printersToolStripMenuItem = new ToolStripMenuItem();
            lblIntgCode = new Label();
            btnReload = new Button();
            lblLogBook = new Label();
            cntDel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbLogo).BeginInit();
            cntShip.SuspendLayout();
            cntContact.SuspendLayout();
            cntCont.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgContents).BeginInit();
            cntCarr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbLogoBack).BeginInit();
            msCarrier.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Impact", 60F, FontStyle.Regular, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.FromArgb(11, 159, 161);
            lblTitle.Location = new Point(12, 32);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(1370, 90);
            lblTitle.TabIndex = 8;
            lblTitle.Text = "Label Creation";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cbClient
            // 
            cbClient.BackColor = Color.DarkGray;
            cbClient.DropDownStyle = ComboBoxStyle.DropDownList;
            cbClient.FlatStyle = FlatStyle.System;
            cbClient.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            cbClient.FormattingEnabled = true;
            cbClient.Location = new Point(12, 181);
            cbClient.Name = "cbClient";
            cbClient.Size = new Size(331, 28);
            cbClient.TabIndex = 1;
            cbClient.TextChanged += cbClient_TextChanged;
            cbClient.Enter += cbClient_Enter;
            cbClient.Leave += cbClient_Leave;
            // 
            // cntDel
            // 
            cntDel.BackColor = Color.FromArgb(11, 159, 161);
            cntDel.Controls.Add(label25);
            cntDel.Controls.Add(cbCountry);
            cntDel.Controls.Add(label19);
            cntDel.Controls.Add(label18);
            cntDel.Controls.Add(label17);
            cntDel.Controls.Add(label16);
            cntDel.Controls.Add(label15);
            cntDel.Controls.Add(label14);
            cntDel.Controls.Add(label13);
            cntDel.Controls.Add(lblCountry);
            cntDel.Controls.Add(txbCompany);
            cntDel.Controls.Add(txbPostcode);
            cntDel.Controls.Add(txbAddr1);
            cntDel.Controls.Add(txbAddr2);
            cntDel.Controls.Add(txbCity);
            cntDel.Controls.Add(txbCounty);
            cntDel.Controls.Add(txbName);
            cntDel.Location = new Point(12, 395);
            cntDel.Name = "cntDel";
            cntDel.Size = new Size(331, 336);
            cntDel.TabIndex = 9;
            // 
            // label25
            // 
            label25.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label25.ForeColor = Color.Black;
            label25.Location = new Point(12, 5);
            label25.Name = "label25";
            label25.Size = new Size(303, 24);
            label25.TabIndex = 14;
            label25.Text = "Delivery Details";
            label25.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cbCountry
            // 
            cbCountry.BackColor = Color.DarkGray;
            cbCountry.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCountry.FlatStyle = FlatStyle.System;
            cbCountry.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            cbCountry.FormattingEnabled = true;
            cbCountry.Location = new Point(153, 290);
            cbCountry.Name = "cbCountry";
            cbCountry.Size = new Size(161, 28);
            cbCountry.TabIndex = 17;
            cbCountry.TextChanged += cbCountry_TextChanged;
            cbCountry.Enter += cbCountry_Enter;
            cbCountry.Leave += cbCountry_Leave;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.ForeColor = Color.Black;
            label19.Location = new Point(13, 29);
            label19.Name = "label19";
            label19.Size = new Size(35, 16);
            label19.TabIndex = 29;
            label19.Text = "Name";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.ForeColor = Color.Black;
            label18.Location = new Point(12, 78);
            label18.Name = "label18";
            label18.Size = new Size(52, 16);
            label18.TabIndex = 28;
            label18.Text = "Company";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.ForeColor = Color.Black;
            label17.Location = new Point(12, 127);
            label17.Name = "label17";
            label17.Size = new Size(96, 16);
            label17.TabIndex = 27;
            label17.Text = "Postcode/Zipcode";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.ForeColor = Color.Black;
            label16.Location = new Point(12, 176);
            label16.Name = "label16";
            label16.Size = new Size(54, 16);
            label16.TabIndex = 26;
            label16.Text = "Address 1";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.ForeColor = Color.Black;
            label15.Location = new Point(12, 225);
            label15.Name = "label15";
            label15.Size = new Size(55, 16);
            label15.TabIndex = 25;
            label15.Text = "Address 2";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.ForeColor = Color.Black;
            label14.Location = new Point(153, 127);
            label14.Name = "label14";
            label14.Size = new Size(26, 16);
            label14.TabIndex = 24;
            label14.Text = "City";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.ForeColor = Color.Black;
            label13.Location = new Point(12, 272);
            label13.Name = "label13";
            label13.Size = new Size(72, 16);
            label13.TabIndex = 23;
            label13.Text = "County/State";
            // 
            // lblCountry
            // 
            lblCountry.ForeColor = Color.Black;
            lblCountry.Location = new Point(153, 272);
            lblCountry.Name = "lblCountry";
            lblCountry.Size = new Size(161, 16);
            lblCountry.TabIndex = 22;
            lblCountry.Text = "Country";
            // 
            // txbCompany
            // 
            txbCompany.BackColor = Color.White;
            txbCompany.BorderStyle = BorderStyle.FixedSingle;
            txbCompany.Cursor = Cursors.IBeam;
            txbCompany.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txbCompany.ForeColor = Color.Black;
            txbCompany.Location = new Point(12, 97);
            txbCompany.MaxLength = 35;
            txbCompany.Name = "txbCompany";
            txbCompany.Size = new Size(302, 27);
            txbCompany.TabIndex = 11;
            txbCompany.Enter += txbCompany_Enter;
            txbCompany.Leave += txbCompany_Leave;
            // 
            // txbPostcode
            // 
            txbPostcode.BackColor = Color.White;
            txbPostcode.BorderStyle = BorderStyle.FixedSingle;
            txbPostcode.CharacterCasing = CharacterCasing.Upper;
            txbPostcode.Cursor = Cursors.IBeam;
            txbPostcode.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txbPostcode.ForeColor = Color.Black;
            txbPostcode.Location = new Point(12, 146);
            txbPostcode.MaxLength = 10;
            txbPostcode.Name = "txbPostcode";
            txbPostcode.Size = new Size(133, 27);
            txbPostcode.TabIndex = 12;
            txbPostcode.Enter += txbPostcode_Enter;
            txbPostcode.Leave += txbPostcode_Leave;
            // 
            // txbAddr1
            // 
            txbAddr1.BackColor = Color.White;
            txbAddr1.BorderStyle = BorderStyle.FixedSingle;
            txbAddr1.Cursor = Cursors.IBeam;
            txbAddr1.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txbAddr1.ForeColor = Color.Black;
            txbAddr1.Location = new Point(12, 195);
            txbAddr1.MaxLength = 35;
            txbAddr1.Name = "txbAddr1";
            txbAddr1.Size = new Size(302, 27);
            txbAddr1.TabIndex = 14;
            txbAddr1.Enter += txbAddr1_Enter;
            txbAddr1.Leave += txbAddr1_Leave;
            // 
            // txbAddr2
            // 
            txbAddr2.BackColor = Color.White;
            txbAddr2.BorderStyle = BorderStyle.FixedSingle;
            txbAddr2.CharacterCasing = CharacterCasing.Upper;
            txbAddr2.Cursor = Cursors.IBeam;
            txbAddr2.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txbAddr2.ForeColor = Color.Black;
            txbAddr2.Location = new Point(12, 242);
            txbAddr2.MaxLength = 35;
            txbAddr2.Name = "txbAddr2";
            txbAddr2.Size = new Size(302, 27);
            txbAddr2.TabIndex = 15;
            txbAddr2.Enter += txbAddr2_Enter;
            txbAddr2.Leave += txbAddr2_Leave;
            // 
            // txbCity
            // 
            txbCity.BackColor = Color.White;
            txbCity.BorderStyle = BorderStyle.FixedSingle;
            txbCity.CharacterCasing = CharacterCasing.Upper;
            txbCity.Cursor = Cursors.IBeam;
            txbCity.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txbCity.ForeColor = Color.Black;
            txbCity.Location = new Point(153, 146);
            txbCity.MaxLength = 35;
            txbCity.Name = "txbCity";
            txbCity.Size = new Size(161, 27);
            txbCity.TabIndex = 13;
            txbCity.Enter += txbCity_Enter;
            txbCity.Leave += txbCity_Leave;
            // 
            // txbCounty
            // 
            txbCounty.BackColor = Color.White;
            txbCounty.BorderStyle = BorderStyle.FixedSingle;
            txbCounty.Cursor = Cursors.IBeam;
            txbCounty.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txbCounty.ForeColor = Color.Black;
            txbCounty.Location = new Point(12, 291);
            txbCounty.MaxLength = 35;
            txbCounty.Name = "txbCounty";
            txbCounty.Size = new Size(133, 27);
            txbCounty.TabIndex = 16;
            txbCounty.Enter += txbCounty_Enter;
            txbCounty.Leave += txbCounty_Leave;
            // 
            // txbName
            // 
            txbName.BackColor = Color.White;
            txbName.BorderStyle = BorderStyle.FixedSingle;
            txbName.Cursor = Cursors.IBeam;
            txbName.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txbName.ForeColor = Color.Black;
            txbName.Location = new Point(12, 48);
            txbName.MaxLength = 35;
            txbName.Name = "txbName";
            txbName.Size = new Size(302, 27);
            txbName.TabIndex = 10;
            txbName.Enter += txbName_Enter;
            txbName.Leave += txbName_Leave;
            // 
            // txbLoadNote
            // 
            txbLoadNote.BackColor = Color.White;
            txbLoadNote.BorderStyle = BorderStyle.FixedSingle;
            txbLoadNote.CharacterCasing = CharacterCasing.Upper;
            txbLoadNote.Cursor = Cursors.IBeam;
            txbLoadNote.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txbLoadNote.ForeColor = Color.Black;
            txbLoadNote.Location = new Point(349, 182);
            txbLoadNote.MaxLength = 24;
            txbLoadNote.Name = "txbLoadNote";
            txbLoadNote.Size = new Size(257, 27);
            txbLoadNote.TabIndex = 2;
            txbLoadNote.Enter += txbLoadNote_Enter;
            txbLoadNote.Leave += txbLoadNote_Leave;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.FromArgb(11, 159, 161);
            label2.Location = new Point(349, 163);
            label2.Name = "label2";
            label2.Size = new Size(55, 16);
            label2.TabIndex = 11;
            label2.Text = "Load Note";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.FromArgb(11, 159, 161);
            label1.Location = new Point(12, 160);
            label1.Name = "label1";
            label1.Size = new Size(36, 16);
            label1.TabIndex = 11;
            label1.Text = "Client";
            // 
            // pbLogo
            // 
            pbLogo.Image = Properties.Resources.TAFIE_Logo;
            pbLogo.Location = new Point(1247, 32);
            pbLogo.Name = "pbLogo";
            pbLogo.Size = new Size(127, 121);
            pbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            pbLogo.TabIndex = 12;
            pbLogo.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.Black;
            label3.Location = new Point(12, 31);
            label3.Name = "label3";
            label3.Size = new Size(41, 16);
            label3.TabIndex = 11;
            label3.Text = "Carrier";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.Black;
            label4.Location = new Point(219, 31);
            label4.Name = "label4";
            label4.Size = new Size(44, 16);
            label4.TabIndex = 11;
            label4.Text = "Service";
            // 
            // txbEORI
            // 
            txbEORI.BackColor = Color.White;
            txbEORI.BorderStyle = BorderStyle.FixedSingle;
            txbEORI.CharacterCasing = CharacterCasing.Upper;
            txbEORI.Cursor = Cursors.IBeam;
            txbEORI.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txbEORI.ForeColor = Color.Black;
            txbEORI.Location = new Point(13, 122);
            txbEORI.MaxLength = 255;
            txbEORI.Name = "txbEORI";
            txbEORI.Size = new Size(198, 27);
            txbEORI.TabIndex = 7;
            txbEORI.Enter += txbEORI_Enter;
            txbEORI.Leave += txbEORI_Leave;
            // 
            // txbIOSS
            // 
            txbIOSS.BackColor = Color.White;
            txbIOSS.BorderStyle = BorderStyle.FixedSingle;
            txbIOSS.CharacterCasing = CharacterCasing.Upper;
            txbIOSS.Cursor = Cursors.IBeam;
            txbIOSS.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txbIOSS.ForeColor = Color.Black;
            txbIOSS.Location = new Point(219, 122);
            txbIOSS.MaxLength = 255;
            txbIOSS.Name = "txbIOSS";
            txbIOSS.Size = new Size(271, 27);
            txbIOSS.TabIndex = 8;
            txbIOSS.Enter += txbIOSS_Enter;
            txbIOSS.Leave += txbIOSS_Leave;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = Color.Black;
            label6.Location = new Point(12, 103);
            label6.Name = "label6";
            label6.Size = new Size(49, 16);
            label6.TabIndex = 11;
            label6.Text = "VAT/EORI";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.ForeColor = Color.Black;
            label7.Location = new Point(219, 103);
            label7.Name = "label7";
            label7.Size = new Size(29, 16);
            label7.TabIndex = 11;
            label7.Text = "IOSS";
            // 
            // cntShip
            // 
            cntShip.BackColor = Color.FromArgb(11, 159, 161);
            cntShip.Controls.Add(label11);
            cntShip.Controls.Add(txbRef2);
            cntShip.Controls.Add(txbRef1);
            cntShip.Controls.Add(label9);
            cntShip.Controls.Add(label8);
            cntShip.Location = new Point(352, 395);
            cntShip.Name = "cntShip";
            cntShip.Size = new Size(331, 162);
            cntShip.TabIndex = 10;
            // 
            // label11
            // 
            label11.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label11.ForeColor = Color.Black;
            label11.Location = new Point(13, 5);
            label11.Name = "label11";
            label11.Size = new Size(303, 24);
            label11.TabIndex = 12;
            label11.Text = "Shipment References";
            label11.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txbRef2
            // 
            txbRef2.BackColor = Color.White;
            txbRef2.BorderStyle = BorderStyle.FixedSingle;
            txbRef2.CharacterCasing = CharacterCasing.Upper;
            txbRef2.Cursor = Cursors.IBeam;
            txbRef2.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txbRef2.ForeColor = Color.Black;
            txbRef2.Location = new Point(13, 97);
            txbRef2.MaxLength = 24;
            txbRef2.Name = "txbRef2";
            txbRef2.Size = new Size(303, 27);
            txbRef2.TabIndex = 19;
            txbRef2.Enter += txbRef2_Enter;
            txbRef2.Leave += txbRef2_Leave;
            // 
            // txbRef1
            // 
            txbRef1.BackColor = Color.White;
            txbRef1.BorderStyle = BorderStyle.FixedSingle;
            txbRef1.CharacterCasing = CharacterCasing.Upper;
            txbRef1.Cursor = Cursors.IBeam;
            txbRef1.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txbRef1.ForeColor = Color.Black;
            txbRef1.Location = new Point(13, 48);
            txbRef1.MaxLength = 24;
            txbRef1.Name = "txbRef1";
            txbRef1.Size = new Size(303, 27);
            txbRef1.TabIndex = 18;
            txbRef1.Enter += txbRef1_Enter;
            txbRef1.Leave += txbRef1_Leave;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.ForeColor = Color.Black;
            label9.Location = new Point(13, 78);
            label9.Name = "label9";
            label9.Size = new Size(64, 16);
            label9.TabIndex = 11;
            label9.Text = "Reference 2";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ForeColor = Color.Black;
            label8.Location = new Point(13, 29);
            label8.Name = "label8";
            label8.Size = new Size(63, 16);
            label8.TabIndex = 11;
            label8.Text = "Reference 1";
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(11, 159, 161);
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Impact", 12F, FontStyle.Italic, GraphicsUnit.Point);
            btnSearch.ForeColor = Color.Black;
            btnSearch.Location = new Point(612, 182);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(71, 28);
            btnSearch.TabIndex = 3;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            btnSearch.MouseEnter += btnSearch_MouseEnter;
            btnSearch.MouseLeave += btnSearch_MouseLeave;
            // 
            // cntContact
            // 
            cntContact.BackColor = Color.FromArgb(11, 159, 161);
            cntContact.Controls.Add(label24);
            cntContact.Controls.Add(txbPhone);
            cntContact.Controls.Add(txbEmail);
            cntContact.Controls.Add(label20);
            cntContact.Controls.Add(label21);
            cntContact.Location = new Point(352, 563);
            cntContact.Name = "cntContact";
            cntContact.Size = new Size(331, 168);
            cntContact.TabIndex = 12;
            // 
            // label24
            // 
            label24.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label24.ForeColor = Color.Black;
            label24.Location = new Point(11, 9);
            label24.Name = "label24";
            label24.Size = new Size(303, 24);
            label24.TabIndex = 13;
            label24.Text = "Contact Details";
            label24.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txbPhone
            // 
            txbPhone.BackColor = Color.White;
            txbPhone.BorderStyle = BorderStyle.FixedSingle;
            txbPhone.CharacterCasing = CharacterCasing.Upper;
            txbPhone.Cursor = Cursors.IBeam;
            txbPhone.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txbPhone.ForeColor = Color.Black;
            txbPhone.Location = new Point(13, 105);
            txbPhone.MaxLength = 12;
            txbPhone.Name = "txbPhone";
            txbPhone.Size = new Size(301, 27);
            txbPhone.TabIndex = 21;
            txbPhone.Enter += txbPhone_Enter;
            txbPhone.Leave += txbPhone_Leave;
            // 
            // txbEmail
            // 
            txbEmail.BackColor = Color.White;
            txbEmail.BorderStyle = BorderStyle.FixedSingle;
            txbEmail.CharacterCasing = CharacterCasing.Lower;
            txbEmail.Cursor = Cursors.IBeam;
            txbEmail.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txbEmail.ForeColor = Color.Black;
            txbEmail.Location = new Point(13, 54);
            txbEmail.MaxLength = 35;
            txbEmail.Name = "txbEmail";
            txbEmail.Size = new Size(301, 27);
            txbEmail.TabIndex = 20;
            txbEmail.Enter += txbEmail_Enter;
            txbEmail.Leave += txbEmail_Leave;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.ForeColor = Color.Black;
            label20.Location = new Point(13, 86);
            label20.Name = "label20";
            label20.Size = new Size(37, 16);
            label20.TabIndex = 11;
            label20.Text = "Phone";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.ForeColor = Color.Black;
            label21.Location = new Point(13, 33);
            label21.Name = "label21";
            label21.Size = new Size(33, 16);
            label21.TabIndex = 11;
            label21.Text = "Email";
            // 
            // cntCont
            // 
            cntCont.BackColor = Color.FromArgb(11, 159, 161);
            cntCont.Controls.Add(label27);
            cntCont.Controls.Add(dgContents);
            cntCont.Location = new Point(703, 227);
            cntCont.Name = "cntCont";
            cntCont.Size = new Size(671, 504);
            cntCont.TabIndex = 13;
            // 
            // label27
            // 
            label27.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label27.ForeColor = Color.Black;
            label27.Location = new Point(12, 13);
            label27.Name = "label27";
            label27.Size = new Size(644, 24);
            label27.TabIndex = 14;
            label27.Text = "Shipment Contents";
            label27.TextAlign = ContentAlignment.MiddleCenter;
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
            dataGridViewCellStyle1.Font = new Font("Impact", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = Color.LightGray;
            dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgContents.DefaultCellStyle = dataGridViewCellStyle1;
            dgContents.GridColor = Color.Black;
            dgContents.Location = new Point(12, 48);
            dgContents.Name = "dgContents";
            dgContents.RowHeadersVisible = false;
            dgContents.RowTemplate.Height = 25;
            dgContents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgContents.Size = new Size(644, 438);
            dgContents.TabIndex = 25;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.FromArgb(11, 159, 161);
            btnExit.Cursor = Cursors.Hand;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnExit.ForeColor = Color.Black;
            btnExit.Location = new Point(12, 737);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(96, 46);
            btnExit.TabIndex = 23;
            btnExit.Text = "Close";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            btnExit.MouseEnter += btnExit_MouseEnter;
            btnExit.MouseLeave += btnExit_MouseLeave;
            // 
            // lblUsername
            // 
            lblUsername.ForeColor = Color.FromArgb(11, 159, 161);
            lblUsername.Location = new Point(922, 32);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(319, 16);
            lblUsername.TabIndex = 17;
            lblUsername.Text = "Username";
            lblUsername.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblStatus
            // 
            lblStatus.BackColor = Color.FromArgb(11, 159, 161);
            lblStatus.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblStatus.ForeColor = Color.Black;
            lblStatus.Location = new Point(12, 122);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(1229, 31);
            lblStatus.TabIndex = 11;
            lblStatus.Text = "Getting Data...";
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            lblStatus.Visible = false;
            // 
            // cntCarr
            // 
            cntCarr.BackColor = Color.FromArgb(11, 159, 161);
            cntCarr.Controls.Add(lblBoxes);
            cntCarr.Controls.Add(cbService);
            cntCarr.Controls.Add(cbCarrier);
            cntCarr.Controls.Add(lblServiceDescr);
            cntCarr.Controls.Add(lblDdp);
            cntCarr.Controls.Add(lblDomestic);
            cntCarr.Controls.Add(lblBoxes1);
            cntCarr.Controls.Add(lblVol);
            cntCarr.Controls.Add(lblTotValue);
            cntCarr.Controls.Add(lblTotWeight);
            cntCarr.Controls.Add(label26);
            cntCarr.Controls.Add(txbEORI);
            cntCarr.Controls.Add(txbIOSS);
            cntCarr.Controls.Add(label3);
            cntCarr.Controls.Add(label4);
            cntCarr.Controls.Add(label7);
            cntCarr.Controls.Add(label6);
            cntCarr.Location = new Point(12, 227);
            cntCarr.Name = "cntCarr";
            cntCarr.Size = new Size(671, 162);
            cntCarr.TabIndex = 13;
            // 
            // lblBoxes
            // 
            lblBoxes.BackColor = Color.White;
            lblBoxes.Font = new Font("Impact", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblBoxes.ForeColor = Color.Black;
            lblBoxes.Location = new Point(537, 83);
            lblBoxes.Name = "lblBoxes";
            lblBoxes.Size = new Size(119, 18);
            lblBoxes.TabIndex = 28;
            lblBoxes.Text = "XX";
            // 
            // cbService
            // 
            cbService.BackColor = Color.DarkGray;
            cbService.DropDownStyle = ComboBoxStyle.DropDownList;
            cbService.FlatStyle = FlatStyle.System;
            cbService.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            cbService.FormattingEnabled = true;
            cbService.Location = new Point(219, 48);
            cbService.Name = "cbService";
            cbService.Size = new Size(271, 28);
            cbService.TabIndex = 27;
            cbService.TextChanged += cbService_TextChanged;
            cbService.Enter += cbService_Enter;
            cbService.Leave += cbService_Leave;
            // 
            // cbCarrier
            // 
            cbCarrier.BackColor = Color.DarkGray;
            cbCarrier.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCarrier.FlatStyle = FlatStyle.System;
            cbCarrier.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            cbCarrier.FormattingEnabled = true;
            cbCarrier.Location = new Point(12, 48);
            cbCarrier.Name = "cbCarrier";
            cbCarrier.Size = new Size(199, 28);
            cbCarrier.TabIndex = 27;
            cbCarrier.TextChanged += cbCarrier_TextChanged;
            cbCarrier.Enter += cbCarrier_Enter;
            cbCarrier.Leave += cbCarrier_Leave;
            // 
            // lblServiceDescr
            // 
            lblServiceDescr.BackColor = Color.White;
            lblServiceDescr.Font = new Font("Impact", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblServiceDescr.ForeColor = Color.Black;
            lblServiceDescr.Location = new Point(13, 80);
            lblServiceDescr.Name = "lblServiceDescr";
            lblServiceDescr.Size = new Size(477, 18);
            lblServiceDescr.TabIndex = 16;
            lblServiceDescr.Text = "Service Description";
            // 
            // lblDdp
            // 
            lblDdp.BackColor = Color.White;
            lblDdp.Font = new Font("Impact", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblDdp.ForeColor = Color.Black;
            lblDdp.Location = new Point(496, 127);
            lblDdp.Name = "lblDdp";
            lblDdp.Size = new Size(160, 18);
            lblDdp.TabIndex = 15;
            lblDdp.Text = "DDP: Yes";
            // 
            // lblDomestic
            // 
            lblDomestic.BackColor = Color.White;
            lblDomestic.Font = new Font("Impact", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblDomestic.ForeColor = Color.Black;
            lblDomestic.Location = new Point(496, 105);
            lblDomestic.Name = "lblDomestic";
            lblDomestic.Size = new Size(160, 18);
            lblDomestic.TabIndex = 15;
            lblDomestic.Text = "Domestic: Yes";
            // 
            // lblBoxes1
            // 
            lblBoxes1.BackColor = Color.White;
            lblBoxes1.Font = new Font("Impact", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblBoxes1.ForeColor = Color.Black;
            lblBoxes1.Location = new Point(496, 83);
            lblBoxes1.Name = "lblBoxes1";
            lblBoxes1.Size = new Size(160, 18);
            lblBoxes1.TabIndex = 15;
            lblBoxes1.Text = "Boxes: XX";
            // 
            // lblVol
            // 
            lblVol.BackColor = Color.White;
            lblVol.Font = new Font("Impact", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblVol.ForeColor = Color.Black;
            lblVol.Location = new Point(496, 61);
            lblVol.Name = "lblVol";
            lblVol.Size = new Size(160, 18);
            lblVol.TabIndex = 14;
            lblVol.Text = "Volume: XX";
            // 
            // lblTotValue
            // 
            lblTotValue.BackColor = Color.White;
            lblTotValue.Font = new Font("Impact", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblTotValue.ForeColor = Color.Black;
            lblTotValue.Location = new Point(496, 39);
            lblTotValue.Name = "lblTotValue";
            lblTotValue.Size = new Size(160, 18);
            lblTotValue.TabIndex = 14;
            lblTotValue.Text = "Total Value: XXX.XX GBP";
            // 
            // lblTotWeight
            // 
            lblTotWeight.BackColor = Color.White;
            lblTotWeight.Font = new Font("Impact", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblTotWeight.ForeColor = Color.Black;
            lblTotWeight.Location = new Point(496, 17);
            lblTotWeight.Name = "lblTotWeight";
            lblTotWeight.Size = new Size(160, 18);
            lblTotWeight.TabIndex = 14;
            lblTotWeight.Text = "Total Weight: XX.XX KG";
            // 
            // label26
            // 
            label26.Font = new Font("Impact", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label26.ForeColor = Color.Black;
            label26.Location = new Point(13, 5);
            label26.Name = "label26";
            label26.Size = new Size(640, 24);
            label26.TabIndex = 12;
            label26.Text = "Carrier Details";
            label26.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnPrint
            // 
            btnPrint.BackColor = Color.FromArgb(11, 159, 161);
            btnPrint.Cursor = Cursors.Hand;
            btnPrint.FlatStyle = FlatStyle.Flat;
            btnPrint.Font = new Font("Impact", 18F, FontStyle.Italic, GraphicsUnit.Point);
            btnPrint.ForeColor = Color.Black;
            btnPrint.Location = new Point(1247, 737);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(127, 46);
            btnPrint.TabIndex = 22;
            btnPrint.Text = "Print Label";
            btnPrint.UseVisualStyleBackColor = false;
            btnPrint.Click += btnPrint_Click;
            btnPrint.MouseEnter += btnPrint_MouseEnter;
            btnPrint.MouseLeave += btnPrint_MouseLeave;
            // 
            // pbLogoBack
            // 
            pbLogoBack.Image = Properties.Resources.TAFIE_Logo;
            pbLogoBack.Location = new Point(421, 216);
            pbLogoBack.Name = "pbLogoBack";
            pbLogoBack.Size = new Size(567, 567);
            pbLogoBack.SizeMode = PictureBoxSizeMode.StretchImage;
            pbLogoBack.TabIndex = 24;
            pbLogoBack.TabStop = false;
            // 
            // lblInstruct
            // 
            lblInstruct.Font = new Font("Impact", 16F, FontStyle.Regular, GraphicsUnit.Point);
            lblInstruct.ForeColor = Color.FromArgb(11, 159, 161);
            lblInstruct.Location = new Point(1012, 216);
            lblInstruct.Name = "lblInstruct";
            lblInstruct.Size = new Size(362, 497);
            lblInstruct.TabIndex = 25;
            lblInstruct.Text = "This form can be used to create carrier labels for orders where the carrier integration provided by Elucid has failed. Select a client, enter a load note and click search to bring up the details";
            // 
            // msCarrier
            // 
            msCarrier.BackColor = Color.Black;
            msCarrier.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            msCarrier.ForeColor = Color.FromArgb(11, 159, 161);
            msCarrier.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, workbenchToolStripMenuItem });
            msCarrier.Location = new Point(0, 0);
            msCarrier.Name = "msCarrier";
            msCarrier.Size = new Size(1394, 24);
            msCarrier.TabIndex = 26;
            msCarrier.Text = "msCarrier";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { printLabelToolStripMenuItem, reportsToolStripMenuItem, closeToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(47, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // printLabelToolStripMenuItem
            // 
            printLabelToolStripMenuItem.Name = "printLabelToolStripMenuItem";
            printLabelToolStripMenuItem.Size = new Size(151, 22);
            printLabelToolStripMenuItem.Text = "Print Label";
            printLabelToolStripMenuItem.Click += printLabelToolStripMenuItem_Click;
            // 
            // reportsToolStripMenuItem
            // 
            reportsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { labelsGeneratedToolStripMenuItem, clientBreakdownToolStripMenuItem, carrierBreakdownToolStripMenuItem });
            reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            reportsToolStripMenuItem.Size = new Size(151, 22);
            reportsToolStripMenuItem.Text = "Reports";
            // 
            // labelsGeneratedToolStripMenuItem
            // 
            labelsGeneratedToolStripMenuItem.Name = "labelsGeneratedToolStripMenuItem";
            labelsGeneratedToolStripMenuItem.Size = new Size(193, 22);
            labelsGeneratedToolStripMenuItem.Text = "Labels Generated";
            labelsGeneratedToolStripMenuItem.Click += labelsGeneratedToolStripMenuItem_Click;
            // 
            // clientBreakdownToolStripMenuItem
            // 
            clientBreakdownToolStripMenuItem.Name = "clientBreakdownToolStripMenuItem";
            clientBreakdownToolStripMenuItem.Size = new Size(193, 22);
            clientBreakdownToolStripMenuItem.Text = "Client Breakdown";
            clientBreakdownToolStripMenuItem.Click += clientBreakdownToolStripMenuItem_Click;
            // 
            // carrierBreakdownToolStripMenuItem
            // 
            carrierBreakdownToolStripMenuItem.Name = "carrierBreakdownToolStripMenuItem";
            carrierBreakdownToolStripMenuItem.Size = new Size(193, 22);
            carrierBreakdownToolStripMenuItem.Text = "Carrier Breakdown";
            carrierBreakdownToolStripMenuItem.Click += carrierBreakdownToolStripMenuItem_Click;
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.Size = new Size(151, 22);
            closeToolStripMenuItem.Text = "Close";
            closeToolStripMenuItem.Click += closeToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { clearToolStripMenuItem, clientManagerToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(47, 20);
            editToolStripMenuItem.Text = "Edit";
            // 
            // clearToolStripMenuItem
            // 
            clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            clearToolStripMenuItem.Size = new Size(172, 22);
            clearToolStripMenuItem.Text = "Clear";
            clearToolStripMenuItem.Click += clearToolStripMenuItem_Click;
            // 
            // clientManagerToolStripMenuItem
            // 
            clientManagerToolStripMenuItem.Name = "clientManagerToolStripMenuItem";
            clientManagerToolStripMenuItem.Size = new Size(172, 22);
            clientManagerToolStripMenuItem.Text = "Client Manager";
            clientManagerToolStripMenuItem.Click += clientManagerToolStripMenuItem_Click;
            // 
            // workbenchToolStripMenuItem
            // 
            workbenchToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { reprintToolStripMenuItem, carriersToolStripMenuItem, addressSearchToolStripMenuItem, printersToolStripMenuItem });
            workbenchToolStripMenuItem.Name = "workbenchToolStripMenuItem";
            workbenchToolStripMenuItem.Size = new Size(82, 20);
            workbenchToolStripMenuItem.Text = "Workbench";
            // 
            // reprintToolStripMenuItem
            // 
            reprintToolStripMenuItem.Name = "reprintToolStripMenuItem";
            reprintToolStripMenuItem.Size = new Size(172, 22);
            reprintToolStripMenuItem.Text = "Reprint";
            reprintToolStripMenuItem.Click += reprintToolStripMenuItem_Click;
            // 
            // carriersToolStripMenuItem
            // 
            carriersToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { combinationsToolStripMenuItem });
            carriersToolStripMenuItem.Name = "carriersToolStripMenuItem";
            carriersToolStripMenuItem.Size = new Size(172, 22);
            carriersToolStripMenuItem.Text = "Carriers";
            // 
            // combinationsToolStripMenuItem
            // 
            combinationsToolStripMenuItem.Name = "combinationsToolStripMenuItem";
            combinationsToolStripMenuItem.Size = new Size(158, 22);
            combinationsToolStripMenuItem.Text = "Combinations";
            combinationsToolStripMenuItem.Click += combinationsToolStripMenuItem_Click;
            // 
            // addressSearchToolStripMenuItem
            // 
            addressSearchToolStripMenuItem.Name = "addressSearchToolStripMenuItem";
            addressSearchToolStripMenuItem.Size = new Size(172, 22);
            addressSearchToolStripMenuItem.Text = "Address Search";
            addressSearchToolStripMenuItem.Click += addressSearchToolStripMenuItem_Click;
            // 
            // printersToolStripMenuItem
            // 
            printersToolStripMenuItem.Name = "printersToolStripMenuItem";
            printersToolStripMenuItem.Size = new Size(172, 22);
            printersToolStripMenuItem.Text = "Printers";
            printersToolStripMenuItem.Click += printersToolStripMenuItem_Click;
            // 
            // lblIntgCode
            // 
            lblIntgCode.Font = new Font("Impact", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lblIntgCode.ForeColor = Color.FromArgb(11, 159, 161);
            lblIntgCode.Location = new Point(114, 751);
            lblIntgCode.Name = "lblIntgCode";
            lblIntgCode.Size = new Size(1127, 23);
            lblIntgCode.TabIndex = 27;
            lblIntgCode.Text = "API Integration";
            lblIntgCode.TextAlign = ContentAlignment.TopRight;
            lblIntgCode.Visible = false;
            // 
            // btnReload
            // 
            btnReload.BackColor = Color.FromArgb(11, 159, 161);
            btnReload.Cursor = Cursors.Hand;
            btnReload.FlatStyle = FlatStyle.Flat;
            btnReload.Font = new Font("Impact", 12F, FontStyle.Italic, GraphicsUnit.Point);
            btnReload.ForeColor = Color.Black;
            btnReload.Location = new Point(689, 182);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(71, 28);
            btnReload.TabIndex = 3;
            btnReload.Text = "Reload";
            btnReload.UseVisualStyleBackColor = false;
            btnReload.Click += btnReload_Click;
            btnReload.MouseEnter += btnReload_MouseEnter;
            btnReload.MouseLeave += btnReload_MouseLeave;
            // 
            // lblLogBook
            // 
            lblLogBook.BackColor = Color.FromArgb(10, 10, 10);
            lblLogBook.Font = new Font("Consolas", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            lblLogBook.ForeColor = Color.FromArgb(64, 64, 64);
            lblLogBook.Location = new Point(12, 786);
            lblLogBook.Name = "lblLogBook";
            lblLogBook.Size = new Size(1362, 19);
            lblLogBook.TabIndex = 28;
            lblLogBook.Text = "LogBook";
            lblLogBook.TextAlign = ContentAlignment.MiddleLeft;
            lblLogBook.Click += lblLogBook_Click;
            lblLogBook.MouseEnter += lblLogBook_MouseEnter;
            lblLogBook.MouseLeave += lblLogBook_MouseLeave;
            // 
            // CarrierForm
            // 
            AutoScaleDimensions = new SizeF(6F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1394, 814);
            ControlBox = false;
            Controls.Add(lblLogBook);
            Controls.Add(lblIntgCode);
            Controls.Add(btnPrint);
            Controls.Add(cntCarr);
            Controls.Add(btnExit);
            Controls.Add(cntCont);
            Controls.Add(cntContact);
            Controls.Add(lblUsername);
            Controls.Add(btnReload);
            Controls.Add(btnSearch);
            Controls.Add(cntShip);
            Controls.Add(pbLogo);
            Controls.Add(lblStatus);
            Controls.Add(label1);
            Controls.Add(label2);
            Controls.Add(txbLoadNote);
            Controls.Add(cntDel);
            Controls.Add(lblTitle);
            Controls.Add(cbClient);
            Controls.Add(pbLogoBack);
            Controls.Add(lblInstruct);
            Controls.Add(msCarrier);
            Font = new Font("Impact", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = msCarrier;
            MaximizeBox = false;
            Name = "CarrierForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CarrierForm";
            Load += CarrierForm_Load;
            KeyDown += CarrierForm_KeyDown;
            cntDel.ResumeLayout(false);
            cntDel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbLogo).EndInit();
            cntShip.ResumeLayout(false);
            cntShip.PerformLayout();
            cntContact.ResumeLayout(false);
            cntContact.PerformLayout();
            cntCont.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgContents).EndInit();
            cntCarr.ResumeLayout(false);
            cntCarr.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbLogoBack).EndInit();
            msCarrier.ResumeLayout(false);
            msCarrier.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Panel cntDel;
        private TextBox txbLoadNote;
        private Label label2;
        private Label label1;
        private PictureBox pbLogo;
        private Label label3;
        private Label label4;
        private TextBox txbEORI;
        private TextBox txbIOSS;
        private Label label6;
        private Label label7;
        private Panel cntShip;
        private TextBox txbRef2;
        private TextBox txbRef1;
        private Label label9;
        private Label label8;
        private Button btnSearch;
        private Label label19;
        private Label label18;
        private Label label17;
        private Label label16;
        private Label label15;
        private Label label14;
        private Label label13;
        private Label lblCountry;
        private Label label10;
        private TextBox txbCompany;
        private TextBox txbPostcode;
        private TextBox txbAddr1;
        private TextBox txbAddr2;
        private TextBox txbCity;
        private TextBox txbCounty;
        private TextBox txbName;
        private Panel cntContact;
        private TextBox txbPhone;
        private TextBox txbEmail;
        private Label label20;
        private Label label21;
        private Panel cntCont;
        private DataGridView dgContents;
        private Button btnExit;
        private ComboBox cbCountry;
        private Label lblUsername;
        private Label lblStatus;
        private Label label11;
        private Label label24;
        private Label label25;
        private Panel cntCarr;
        private Label label26;
        private Label label27;
        private Label lblVol;
        private Label lblTotValue;
        private Label lblTotWeight;
        private Label lblBoxes1;
        private Button btnPrint;
        private Label lblServiceDescr;
        private PictureBox pbLogoBack;
        private Label lblInstruct;
        private MenuStrip msCarrier;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem printLabelToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem clearToolStripMenuItem;
        private ToolStripMenuItem workbenchToolStripMenuItem;
        private ToolStripMenuItem reprintToolStripMenuItem;
        private ToolStripMenuItem carriersToolStripMenuItem;
        private ToolStripMenuItem combinationsToolStripMenuItem;
        private ToolStripMenuItem addressSearchToolStripMenuItem;
        private ToolStripMenuItem clientManagerToolStripMenuItem;
        private ComboBox cbService;
        private ComboBox cbCarrier;
        private Label lblDdp;
        private Label lblDomestic;
        private Label lblIntgCode;
        private Button btnReload;
        private Label lblBoxes;
        private Label lblLogBook;
        private ToolStripMenuItem printersToolStripMenuItem;
        private ToolStripMenuItem reportsToolStripMenuItem;
        private ToolStripMenuItem labelsGeneratedToolStripMenuItem;
        private ToolStripMenuItem clientBreakdownToolStripMenuItem;
        private ToolStripMenuItem carrierBreakdownToolStripMenuItem;
        public ComboBox cbClient;
    }
}