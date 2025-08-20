using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TAFIE
{
    public partial class CarrierForm : Form
    {
        //=============================================================================================================================================================================================
        //-- Initialization --//
        //=============================================================================================================================================================================================

        public string passedClient { get; set; }
        public string passedLoadNote { get; set; }

        public static string? tcarRef = null;
        public string? apiAccCode = null;
        public string? intgCode = null;
        public string? deltaCheck = null;
        public string? lastLoadNote = null;
        public string? lastClient = null;

        private const string connectionString = SessionMaintenance.connectionString;

        string? pbCarrier = null;
        string? pbService = null;
        string? pbServiceDescr = null;
        string? pbInco = null;
        string? pbEori = null;
        string? pbIoss = null;
        string? pbName = null;
        string? pbCompany = null;
        string? pbPostcode = null;
        string? pbCity = null;
        string? pbAddr1 = null;
        string? pbAddr2 = null;
        string? pbCounty = null;
        string? pbCountry = null;
        string? pbEmail = null;
        string? pbPhone = null;
        string? pbRef1 = null;
        string? pbRef2 = null;
        string? pbTotWeight = null;
        string? pbTotValue = null;
        string? pbVolume = null;
        string? pbBoxes = null;

        int delta = 0;
        int ddp = 0; // Flag for duties paid
        int domestic = 1; // flag for domestic shipments
        public static int scanned = 0;

        public CarrierForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        // Form Load ------------------------------------------------------------------------------------------------------------------
        private void CarrierForm_Load(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[FormLoad]", $"Form Started", lblLogBook);
            Text = $"{Environment.UserName.ToUpper()} - {SessionMaintenance.appName} Label Creation";
            PopulateComboBoxes(cbClient, "CLIENT");
            PopulateComboBoxes(cbCountry, "COUNTRY");
            lblUsername.Text = SessionMaintenance.userName;
            cbClient.Text = passedClient;
            txbLoadNote.Text = passedLoadNote;
            tcarRef = "";

            TcarControl.tcarRef = tcarRef; // Set the static tcarRef in TcarControl

            lblTotWeight.Text = $"";
            lblTotValue.Text = $"";
            lblVol.Text = $"";
            lblBoxes.Text = $"";
            lblServiceDescr.Text = "";
            lblDdp.Text = "";
            lblDomestic.Text = "";
            lblCountry.BackColor = Color.FromArgb(11, 159, 161);
            btnPrint.Visible = false;
            cntCarr.Visible = false;
            cntDel.Visible = false;
            cntShip.Visible = false;
            cntCont.Visible = false;
            cntContact.Visible = false;
            pbLogoBack.Visible = true;
            lblInstruct.Visible = true;
            lblInstruct.Text = "This form can be used to create carrier labels for orders where the carrier integration provided by Elucid has failed. " +
                "Select a client, enter a load note and click search to bring up the details" +
                "\nEdit any details as necessary, if there are errors you should be able to fix them here." +
                "\nClick print label and watch the magic of an Application programming interface happen right before your eyes!";

            if (!string.IsNullOrEmpty(passedLoadNote) && !string.IsNullOrEmpty(passedClient))
            {
                GetLoadNote(passedLoadNote, passedClient);
                btnPrint.Visible = true;
            }


            CustomMessageBox messageBox = new CustomMessageBox();
            messageBox.ShowWarning("This module is still under development.\nSome features or functions may not work as expected and could result in errors, bugs, or application crashes.");
        }

        //=============================================================================================================================================================================================
        //-- Operational Methods --//
        //=============================================================================================================================================================================================

        // Populate Combo Boxes -----------------------------------------------------------------------------------------------------------------------
        public void PopulateComboBoxes(ComboBox comboBox, string field)
        {
            // Declare Variables
            string query = "";

            if (field == "CLIENT")
            {
                query = "SELECT [Description] FROM TAFIE_Clients WHERE [Active] = '1' ORDER BY [Description]";
            }
            else if (field == "COUNTRY")
            {
                query = "SELECT [Description] FROM TAFIE_Ctry WHERE [Active] = '1' ORDER BY [Description]";
            }

            try
            {
                // Execute SQL Command 
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open(); // Open SQL Connection

                    // Combo Box //
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Execute Data Reader
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            comboBox.Items.Clear(); // Clear Combo box ready for new data

                            // Populate ComboBox from reader
                            while (reader.Read())
                            {
                                comboBox.Items.Add(reader["Description"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex) // Catch any errors
            {
                Cursor.Current = Cursors.Default;
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("112", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[PopulateComboBoxes]", $"FAILED: Code 112 ( {ex.Message} )", lblLogBook);
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[PopulateComboBoxes]", "Application Closed", lblLogBook);
                Application.Exit();
            }
        }

        // Populate Combo Boxes -----------------------------------------------------------------------------------------------------------------------
        private void PopulateCarrierComboBoxes(ComboBox comboBox, string client)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[PopulateCarrierComboBoxes]", "Method Started", lblLogBook);
            string query = "";
            string carrier = cbCarrier.Text ?? "";

            if (comboBox == cbCarrier)
            {
                query = "EXECUTE [TCAR_Get_Combos] @Client, 3";

            }
            else if (comboBox == cbService)
            {
                query = "EXECUTE [TCAR_Get_Combos] @Client, 2, @Carrier";
            }

            try
            {
                // Execute SQL Command 
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open(); // Open SQL Connection

                    // Combo Box //
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Client", client);
                        cmd.Parameters.AddWithValue("@Carrier", carrier);
                        // Execute Data Reader
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            comboBox.Items.Clear(); // Clear Combo box ready for new data

                            if (comboBox == cbCarrier)
                            {
                                // Populate ComboBox from reader
                                while (reader.Read())
                                {
                                    comboBox.Items.Add(reader["Carrier"].ToString());
                                }
                            }
                            else if (comboBox == cbService)
                            {
                                // Populate ComboBox from reader
                                while (reader.Read())
                                {
                                    comboBox.Items.Add(reader["Delivery Method"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) // Catch any errors
            {
                Cursor.Current = Cursors.Default;
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("112", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[PopulateComboBoxes]", $"FAILED: Code 112 ( {ex.Message} )", lblLogBook);
            }
        }

        // Populate Combo Boxes -----------------------------------------------------------------------------------------------------------------------
        private void UpdateCarrDescr(string delMethod)
        {
            string query = "SELECT [Description], [Intg_Code] FROM TCAR_CMAP WHERE E9_Del_Method = @Del_Method";

            try
            {
                // Execute SQL Command 
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open(); // Open SQL Connection

                    // Combo Box //
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Del_Method", delMethod);
                        // Execute Data Reader
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Populate ComboBox from reader
                            if (reader.Read())
                            {
                                lblServiceDescr.Text = reader["Description"].ToString() ?? "";
                                intgCode = reader["Intg_Code"].ToString() ?? "";
                            }
                        }
                    }
                }

                SaveData();
                TcarControl.RecalculateBoxes();
                PopulateUI(2);
                UpdateApiLabel();
            }
            catch (Exception ex) // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("An error occured getting delivery description");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[UpdateCarrDescr]", $"FAILED: Code 112 ( {ex.Message} )", lblLogBook);
            }
        }

        // Update API Integration Label --------------------------------------------------------------------------------
        private void UpdateApiLabel()
        {
            if (intgCode == "PHUB_API")
            {
                lblIntgCode.Visible = true;
                lblIntgCode.Text = "API Integration: Parcel Hub";
            }
            else if (intgCode == "PROCAR")
            {
                lblIntgCode.Visible = true;
                lblIntgCode.Text = "API Integration: Pro Carrier";
            }
            else
            {
                lblIntgCode.Visible = false;
                lblIntgCode.Text = "";
            }

            string client = cbClient.Text ?? "";
            if (TcarControl.barc.Contains(client) && !string.IsNullOrEmpty(intgCode))
            {
                lblIntgCode.Text += " - Barcode Scanning Enabled";
            }
            else
            {
                lblIntgCode.Text += " - Barcode Scanning Disabled";
            }
        }

        // Save Data ------------------------------------------------------------------------------------------------------------------
        private void SaveData()
        {
            string? carrier = cbCarrier.Text;
            string? service = cbService.Text;

            string? eori = txbEORI.Text;
            string? ioss = txbIOSS.Text;
            string? name = txbName.Text;
            string? company = txbCompany.Text;
            string? postcode = txbPostcode.Text;
            string? city = txbCity.Text;
            string? addr1 = txbAddr1.Text;
            string? addr2 = txbAddr2.Text;
            string? county = txbCounty.Text;
            string? country = "";
            string? email = txbEmail.Text;
            string? phone = txbPhone.Text;
            string? ref1 = txbRef1.Text;
            string? ref2 = txbRef2.Text;

            if (cbCountry.SelectedItem != null)
            {
                country = cbCountry.SelectedItem.ToString();
            }

            string query = "EXECUTE [TCAR_Save]" +
                "@TCAR_Ref " +
                ",@Carrier " +
                ",@Service " +
                ",@Inco " +
                ",@Eori " +
                ",@IOSS " +
                ",@Name " +
                ",@Company " +
                ",@Postcode " +
                ",@City " +
                ",@Addr1 " +
                ",@Addr2 " +
                ",@County " +
                ",@Country " +
                ",@Email " +
                ",@Phone " +
                ",@Ref1 " +
                ",@Ref2 " +
                ",@User";

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
                        cmd.Parameters.AddWithValue("@Carrier", carrier);
                        cmd.Parameters.AddWithValue("@Service", service);
                        cmd.Parameters.AddWithValue("@Inco", "");
                        cmd.Parameters.AddWithValue("@Eori", eori);
                        cmd.Parameters.AddWithValue("@IOSS", ioss);
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Company", company);
                        cmd.Parameters.AddWithValue("@Postcode", postcode);
                        cmd.Parameters.AddWithValue("@City", city);
                        cmd.Parameters.AddWithValue("@Addr1", addr1);
                        cmd.Parameters.AddWithValue("@Addr2", addr2);
                        cmd.Parameters.AddWithValue("@County", county);
                        cmd.Parameters.AddWithValue("@Country", country);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@Ref1", ref1);
                        cmd.Parameters.AddWithValue("@Ref2", ref2);
                        cmd.Parameters.AddWithValue("@User", SessionMaintenance.userName);

                        cmd.ExecuteNonQuery();
                    }
                    conn.Close(); // Close SQL Connection
                }
            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured saving shipment headers \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[SaveData]", $"FAILED: ( {ex.Message} )", lblLogBook);
                return;
            }
        }

        // Get Load Note ------------------------------------------------------------------------------------------------------------------
        private void GetLoadNote(string loadNote, string client)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                PopulateCarrierComboBoxes(cbCarrier, client);
                PopulateCarrierComboBoxes(cbService, client);
                GetTcarRef(client);
                TcarControl.GetHeaderDetails(loadNote, client);
                TcarControl.GetDelDetails();
                TcarControl.GetComp();
                PopulateUI(1);
                PopulateUI(2);
                PopulateDataGrid();

                //btnBoxes.Visible = true;

                cntCarr.Visible = true;
                cntDel.Visible = true;
                cntShip.Visible = true;
                cntCont.Visible = true;
                cntContact.Visible = true;
                pbLogoBack.Visible = false;
                lblInstruct.Visible = false;

                if (TcarControl.barc.Contains(client))
                {
                    scanned = 0;
                    DespatchForm despatchForm = new DespatchForm(this);
                    despatchForm.tcarRef = tcarRef;
                    despatchForm.Show();
                }
                else
                {
                    scanned = 1;
                }
            }
            finally
            {
                Cursor = Cursors.Default;
                lblStatus.Visible = false;
            }
        }

        // Get TCAR_Ref ------------------------------------------------------------------------------------------------------------------
        private void GetTcarRef(string client)
        {
            string query = "EXECUTE [TCAR_Get_Ref] @Client";

            tcarRef = null;

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {

                        cmd.Parameters.AddWithValue("@Client", client);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tcarRef = reader["TCAR_Ref"].ToString();
                                TcarControl.tcarRef = tcarRef; // Set the static tcarRef in TcarControl
                            }
                        }
                    }
                    conn.Close(); // Close SQL Connection
                }
            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured getting TCAR Reference \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetTcarRef]", $"FAILED: Code 226 ( {ex.Message} )", lblLogBook);
            }
        }

        // Populate UI ------------------------------------------------------------------------------------------------------------------
        private void PopulateUI(int mode)
        {
            string query = "";

            switch (mode)
            {
                case 1: query = "EXECUTE [TCAR_Get_Results] @TCAR_Ref, 1"; break;
                case 2: query = "EXECUTE [TCAR_Get_Results] @TCAR_Ref, 3"; break;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (mode == 1)
                                {
                                    pbCarrier = reader["Carrier"].ToString();
                                    pbService = reader["Del_Method"].ToString();
                                    pbServiceDescr = reader["Service_Descr"].ToString();
                                    pbInco = reader["Inco"].ToString();
                                    pbEori = reader["Eori"].ToString();
                                    pbIoss = reader["IOSS"].ToString();
                                    pbName = reader["Name"].ToString();
                                    pbCompany = reader["Company"].ToString();
                                    pbPostcode = reader["Postcode"].ToString();
                                    pbCity = reader["City"].ToString();
                                    pbAddr1 = reader["Addr1"].ToString();
                                    pbAddr2 = reader["Addr2"].ToString();
                                    pbCounty = reader["County"].ToString();
                                    pbCountry = reader["Country"].ToString();
                                    pbEmail = reader["Email"].ToString();
                                    pbPhone = reader["Phone"].ToString();
                                    pbRef1 = reader["Ref1"].ToString();
                                    pbRef2 = reader["Ref2"].ToString();
                                    apiAccCode = reader["Acc_Code"].ToString();
                                    intgCode = reader["Intg_Code"].ToString();
                                }
                                else if (mode == 2)
                                {
                                    pbTotWeight = reader["Total_Weight"].ToString();
                                    pbTotValue = reader["Total_Value"].ToString();
                                    pbVolume = reader["Volume"].ToString();
                                    pbBoxes = reader["Boxes"].ToString();
                                    ddp = (int)reader["DDP"];
                                    domestic = (int)reader["Domestic"];
                                }
                            }
                        }
                    }
                }

                if (mode == 1)
                {
                    cbCarrier.Text = pbCarrier;
                    cbService.Text = pbService;

                    txbEORI.Text = pbEori;
                    txbIOSS.Text = pbIoss;
                    txbName.Text = pbName;
                    txbCompany.Text = pbCompany;
                    txbPostcode.Text = pbPostcode;
                    txbCity.Text = pbCity;
                    txbAddr1.Text = pbAddr1;
                    txbAddr2.Text = pbAddr2;
                    txbCounty.Text = pbCounty;
                    cbCountry.Text = pbCountry;
                    txbEmail.Text = pbEmail;
                    txbPhone.Text = pbPhone;
                    txbRef1.Text = pbRef1;
                    txbRef2.Text = pbRef2;
                    lblServiceDescr.Text = pbServiceDescr;
                    UpdateApiLabel();

                    if (string.IsNullOrEmpty(pbName))
                    {
                        pbName = pbCompany;
                        txbName.Text = pbName;
                    }

                }
                else if (mode == 2)
                {
                    lblTotWeight.Text = $"Total Weight: {pbTotWeight} KG";
                    lblTotValue.Text = $"Total Value: {pbTotValue} GBP";
                    lblVol.Text = $"Volume: {pbVolume}";
                    lblBoxes.Text = $"{pbBoxes}";
                    lblDdp.Text = ddp == 1 ? "Duties Paid" : "Duties Unpaid";
                    btnReload.Visible = false;
                }


            }
            catch (Exception ex) // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("227", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[PopulateUI]", $"FAILED: Code 227 ( {ex.Message} )", lblLogBook);
            }
        }

        // Populate DataGrid ------------------------------------------------------------------------------------------------------------------
        private void PopulateDataGrid()
        {
            string query = "EXECUTE [TCAR_Get_Results] @TCAR_Ref, 2";

            DataTable dataTable = new DataTable();

            try
            {
                // Start SQL
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Set Parameters
                        cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);

                        // Execute Query
                        SqlDataReader reader = cmd.ExecuteReader();

                        // Populate DataTable From Reader
                        dataTable.Load(reader);
                    }

                    conn.Close(); // Close SQL Connection

                    // Populate Data Grid
                    dgContents.DataSource = dataTable;
                    dgContents.Refresh();

                    // Hide the last column
                    if (dgContents.Columns.Count > 0)
                    {
                        int errorColumnIndex = dgContents.Columns.Count - 1; // Last column index
                        dgContents.Columns[errorColumnIndex].Visible = false;
                    }

                    // Apply conditional formatting
                    FormatDataGrid();
                }
            }
            catch (Exception ex) // Catch Errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("117", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[PopulateDataGrid]", $"FAILED: Code 117 ( {ex.Message} )", lblLogBook);
            }
        }

        // Apply Conditional Formatting Based on Error Flag ----------------------------------------------------------------------------
        private void FormatDataGrid()
        {
            foreach (DataGridViewRow row in dgContents.Rows)
            {
                if (row.Cells.Count > 0)
                {
                    int errorColumnIndex = dgContents.Columns.Count - 1; // Last column index

                    if (int.TryParse(row.Cells[errorColumnIndex].Value?.ToString(), out int errorFlag) && errorFlag == 2)
                    {
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else if (int.TryParse(row.Cells[errorColumnIndex].Value?.ToString(), out int errorFlag2) && errorFlag2 == 1)
                    {
                        row.DefaultCellStyle.BackColor = Color.Red;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }

        // Check Load Note Exists-----------------------------------------------------------------------------------------------------------------------
        private void ClearFields()
        {
            // Clear OG Parameters
            pbCarrier = null;
            pbService = null;
            pbInco = null;
            pbEori = null;
            pbIoss = null;
            pbName = null;
            pbCompany = null;
            pbPostcode = null;
            pbCity = null;
            pbAddr1 = null;
            pbAddr2 = null;
            pbCounty = null;
            pbCountry = null;
            pbEmail = null;
            pbPhone = null;
            pbRef1 = null;
            pbRef2 = null;
            pbTotWeight = null;
            pbTotValue = null;
            pbVolume = null;
            pbBoxes = null;

            // Clear fields
            tcarRef = null;
            string? clear = null;
            lblStatus.Visible = false;
            txbLoadNote.Text = clear;
            cbCarrier.Items.Clear();
            cbService.Items.Clear();
            cbCarrier.Text = clear;
            cbService.Text = clear;

            txbEORI.Text = clear;
            txbIOSS.Text = clear;
            txbName.Text = clear;
            txbCompany.Text = clear;
            txbPostcode.Text = clear;
            txbCity.Text = clear;
            txbAddr1.Text = clear;
            txbAddr2.Text = clear;
            txbCounty.Text = clear;
            cbCountry.Text = clear;
            txbEmail.Text = clear;
            txbPhone.Text = clear;
            txbRef1.Text = clear;
            txbRef2.Text = clear;
            lblTotWeight.Text = clear;
            lblTotValue.Text = clear;
            lblVol.Text = clear;
            lblBoxes.Text = clear;
            lblServiceDescr.Text = clear;
            lblCountry.BackColor = Color.FromArgb(11, 159, 161);
            lblDdp.Text = clear;
            lblDomestic.Text = clear;

            // Reset field back colours
            txbLoadNote.BackColor = Color.White;
            cbCarrier.BackColor = Color.White;
            cbService.BackColor = Color.White;

            txbEORI.BackColor = Color.White;
            txbIOSS.BackColor = Color.White;
            txbName.BackColor = Color.White;
            txbCompany.BackColor = Color.White;
            txbPostcode.BackColor = Color.White;
            txbCity.BackColor = Color.White;
            txbAddr1.BackColor = Color.White;
            txbAddr2.BackColor = Color.White;
            txbCounty.BackColor = Color.White;
            cbCountry.BackColor = Color.White;
            txbEmail.BackColor = Color.White;
            txbPhone.BackColor = Color.White;
            txbRef1.BackColor = Color.White;
            txbRef2.BackColor = Color.White;
            lblTotWeight.BackColor = Color.White;
            lblTotValue.BackColor = Color.White;
            lblVol.BackColor = Color.White;
            lblBoxes.BackColor = Color.White;
            lblDdp.BackColor = Color.White;
            lblDomestic.BackColor = Color.White;

            // Hide Buttons            
            btnPrint.Visible = false;

            // Clear Box Contents
            dgContents.DataSource = null;
            dgContents.Refresh();

            txbLoadNote.Focus();

            // Reset UI Elements
            btnPrint.Visible = false;
            cntCarr.Visible = false;
            cntDel.Visible = false;
            cntShip.Visible = false;
            cntCont.Visible = false;
            cntContact.Visible = false;
            pbLogoBack.Visible = true;
            lblInstruct.Visible = true;
            lblIntgCode.Visible = false;
            btnReload.Visible = true;

            delta = 0;
            ddp = 0;
            domestic = 1;
        }

        // Check Field ------------------------------------------------------------------------------------------------------------------       
        private int CheckField(int field, Control control)
        {
            string value = control.Text.Trim();
            CustomMessageBox messageBox = new CustomMessageBox();
            int c = 0;

            switch (field)
            {
                case 0: // Carrier
                    if (string.IsNullOrEmpty(value))
                    {
                        messageBox.ShowError("Carrier cannot be empty.");
                        control.Focus();
                        c = 1;
                    }
                    break;

                case 1: // Del Method
                    if (string.IsNullOrEmpty(value))
                    {
                        messageBox.ShowError("Delivery Method cannot be empty.");
                        control.Focus();
                        c = 1;
                    }
                    break;

                case 2: // IncoTerms

                    break;

                case 3: // EORI
                    if (!Regex.IsMatch(value, @"^[A-Z0-9]{10,17}$", RegexOptions.IgnoreCase) && !string.IsNullOrEmpty(value))
                    {
                        messageBox.ShowError("Invalid EORI number format.");
                        control.Focus();
                        c = 1;
                    }
                    break;

                case 4: // IOSS
                    if (!Regex.IsMatch(value, @"^IM\d{10}$") && !string.IsNullOrEmpty(value))
                    {
                        messageBox.ShowError("Invalid IOSS number format (IM followed by 10 digits).");
                        control.Focus();
                        c = 1;
                    }
                    break;

                case 5: // Name
                    if (value.Length < 1 || value.Length > 35)
                    {
                        messageBox.ShowError("Name must be between 1 and 35 characters.");
                        control.Focus();
                        c = 1;
                    }
                    break;

                case 6: // Company
                    if (value.Length > 35)
                    {
                        messageBox.ShowError("Company name must be less than 35 characters.");
                        control.Focus();
                        c = 1;
                    }
                    break;

                case 7: // Postcode
                    if (cbCountry.SelectedItem != null && cbCountry.SelectedItem.ToString() == "Great Britain")
                    {
                        if (!Regex.IsMatch(value, @"^[A-Z]{1,2}\d[A-Z\d]? ?\d[A-Z]{2}$", RegexOptions.IgnoreCase))
                        {
                            messageBox.ShowError("Invalid UK postcode format.");
                            control.Focus();
                            c = 1;
                        }
                    }
                    break;

                case 8: // City
                    if (value.Length < 1 || value.Length > 35)
                    {
                        messageBox.ShowError("City must be between 1 and 35 characters.");
                        control.Focus();
                        c = 1;
                    }
                    break;

                case 9: // Addr1
                    if (value.Length < 1 || value.Length > 35)
                    {
                        messageBox.ShowError("Address Line 1 must be between 1 and 35 characters.");
                        control.Focus();
                        c = 1;
                    }
                    break;

                case 10: // Addr2
                    if (value.Length > 35 && !string.IsNullOrEmpty(value))
                    {
                        messageBox.ShowError("Address line 2 must be less than 35 characters.");
                        control.Focus();
                        c = 1;
                    }
                    break;
                case 11: // County
                    if (value.Length > 35 && !string.IsNullOrEmpty(value))
                    {
                        messageBox.ShowError("County must be less than 35 characters.");
                        control.Focus();
                        c = 1;
                    }
                    break;

                case 12: // Email
                    {
                        if (cbCountry.SelectedItem != null && cbCountry.SelectedItem.ToString() != "Great Britain" && string.IsNullOrEmpty(value))
                        {
                            messageBox.ShowError("Email is required for international shipments");
                            control.Focus();
                            c = 1;
                        }
                        if (!Regex.IsMatch(value, @"^[\w\.-]+@[\w\.-]+\.\w+$") && !string.IsNullOrEmpty(value))
                        {
                            messageBox.ShowError("Invalid email format.");
                            control.Focus();
                            c = 1;
                        }
                    }
                    break;

                case 13: // Phone
                    {
                        if (cbCountry.SelectedItem != null && cbCountry.SelectedItem.ToString() != "Great Britain" && string.IsNullOrEmpty(value))
                        {
                            messageBox.ShowError("Phone is required for international shipments");
                            control.Focus();
                            c = 1;
                        }
                        if (!Regex.IsMatch(value, @"^\+?[0-9]{1,14}$") && !string.IsNullOrEmpty(value))
                        {
                            messageBox.ShowError("Invalid phone number format.");
                            control.Focus();
                            c = 1;
                        }
                    }
                    break;
            }

            return c;
        }

        // Check all Fields ------------------------------------------------------------------------------------------------------------------       
        private bool CheckAllFields()
        {
            int c = 0;
            c += CheckField(0, cbCarrier);
            c += CheckField(1, cbService);

            c += CheckField(3, txbEORI);
            c += CheckField(4, txbIOSS);
            c += CheckField(5, txbName);
            c += CheckField(6, txbCompany);
            c += CheckField(7, txbPostcode);
            c += CheckField(8, txbCity);
            c += CheckField(9, txbAddr1);
            c += CheckField(10, txbAddr2);
            c += CheckField(11, txbCounty);
            c += CheckField(12, txbEmail);
            c += CheckField(13, txbPhone);

            if (c == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //=============================================================================================================================================================================================
        //-- API Methods --//
        //=============================================================================================================================================================================================

        // Get Label Process Pro Carrier ------------------------------------------------------------------------------------------------------------------       
        public async Task GetLabelProCarr(int boxRef, int boxCount)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[GetLabelProCarr]", "Method Started", lblLogBook);
            if (string.IsNullOrEmpty(tcarRef))
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("No Order data loaded");
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                string tkey = TcarControl.CreateTKey(txbName.Text);
                string CallJson = ProCarrApi.CreateJsonString(ProCarrApi.GetJsonData(tcarRef, boxRef));

                // Insert Call XML for reference (logging)
                TcarControl.InsertPayload(CallJson, tkey, 1);

                // Make API Call to get label data
                string respJson = await ProCarrApi.MakeLabelApiCall(CallJson, tkey);

                if (string.IsNullOrEmpty(respJson))
                {
                    SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetLabelProCarr]", "Received empty response JSON.", lblLogBook);
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowError("Failed to retrieve label data. Empty response from API.");
                    return;
                }

                // Extract tracking number and label files
                string trackingNumber = ProCarrApi.ExtractTrackingNumberJson(respJson);
                List<string> labelFiles = ProCarrApi.ExtractLabelDataJson(respJson);

                // Log details
                if (string.IsNullOrEmpty(trackingNumber))
                {
                    SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetLabelProCarr]", "Failed to extract tracking number.", lblLogBook);
                }
                else
                {
                    SessionMaintenance.LogBook("", "[CarrierForm]", "[GetLabelProCarr]", $"Tracking Number: {trackingNumber}, {boxRef}", lblLogBook);
                }

                if (labelFiles == null || labelFiles.Count == 0)
                {
                    SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetLabelProCarr]", "Failed to extract any labels.", lblLogBook);
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowError("No label data found in API response.");
                    return;
                }

                // Print all labels
                foreach (var labelFile in labelFiles)
                {
                    TcarControl.PrintPdf(labelFile);
                }

                TcarControl.CompleteTCAR(trackingNumber, boxRef);
                TcarControl.CompleteLoadNote(cbClient.Text ?? "");
            }
            catch (Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occurred during label processing: {ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetLabelProCarr]", $"Exception: {ex.Message}", lblLogBook);
                return;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            SessionMaintenance.LogBook("", "[CarrierForm]", "[GetLabelProCarr]", "Method Finished", lblLogBook);

            CustomMessageBox messageBoxfin = new CustomMessageBox();
            messageBoxfin.ShowInfo($"Label Printed: {boxRef} of {boxCount}");
        }

        // Get Label Process PHUB ------------------------------------------------------------------------------------------------------------------       
        private async Task GetLabelPhub(int boxRef, int boxCount)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[GetLabelPhub]", "Method Started", lblLogBook);

            if (string.IsNullOrEmpty(tcarRef))
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("No Order data loaded");
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                string tkey = TcarControl.CreateTKey(txbName.Text);
                string callXml = WcmsApi.CreateXmlString(WcmsApi.GetXmlData(tcarRef, boxRef), await WcmsApi.GetCustomerUid(boxRef, domestic, ddp), domestic, ddp);

                // Insert Call XML for reference (logging)
                TcarControl.InsertPayload(callXml, tkey, 1);

                // Make API Call to get label data
                string respXml = await WcmsApi.MakeCarrierApiCall(callXml, tkey);

                if (string.IsNullOrEmpty(respXml))
                {
                    SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetLabelPhub]", "Received empty response XML.", lblLogBook);
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowError("Failed to retrieve label data. Empty response from API.");
                    return;
                }

                // Extract tracking number and label files
                string trackingNumber = WcmsApi.ExtractTrackingNumber(respXml);
                List<string> labelFiles = WcmsApi.ExtractLabelData(respXml);

                // Log details
                if (string.IsNullOrEmpty(trackingNumber))
                {
                    SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetLabelPhub]", "Failed to extract tracking number.", lblLogBook);
                }
                else
                {
                    SessionMaintenance.LogBook("", "[CarrierForm]", "[GetLabelPhub]", $"Tracking Number: {trackingNumber}, {boxRef}", lblLogBook);
                }

                if (labelFiles == null || labelFiles.Count == 0)
                {
                    SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetLabelPhub]", "Failed to extract any labels.", lblLogBook);
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowError("No label data found in API response.");
                    return;
                }

                // Print all labels
                foreach (var labelFile in labelFiles)
                {
                    TcarControl.PrintPdf(labelFile);
                }

                TcarControl.CompleteTCAR(trackingNumber, boxRef);
                TcarControl.CompleteLoadNote(cbClient.Text ?? "");

            }
            catch (Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occurred during label processing: {ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetLabelPhub]", $"Exception: {ex.Message}", lblLogBook);
                return;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            SessionMaintenance.LogBook("", "[CarrierForm]", "[GetLabelPhub]", "Method Finished", lblLogBook);

            CustomMessageBox messageBoxfin = new CustomMessageBox();
            messageBoxfin.ShowInfo($"Label Printed: {boxRef} of {boxCount}");

        }

        //=============================================================================================================================================================================================
        //-- Print Process Methods --//
        //=============================================================================================================================================================================================

        // Set API Session Details ---------------------------------------------------------------------------------------------------
        private void SetApiSessionDetails()
        {
            WcmsApi.tcarRef = tcarRef;
            ProCarrApi.tcarRef = tcarRef;
        }

        // Handle single Box Print ---------------------------------------------------------------------------------------------------
        private async Task HandleSingleBoxPrint(string intgCode)
        {
            var dataTable = CreateDataTableFromGrid();
            TcarControl.InsertDataGrid(dataTable, SessionMaintenance.userName);

            var boxRefs = TcarControl.GetBoxes();

            // Use PROCARRIER Integration
            if (intgCode == "PROCAR")
            {
                await TcarControl.PrintLabels(boxRefs, GetLabelProCarr);
            }

            // Use PHUB Integration
            else if (intgCode == "PHUB_API")
            {
                await TcarControl.PrintLabels(boxRefs, GetLabelPhub);
            }

            // Error
            else
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("Invalid Integration Code for printing labels.");
                return;
            }

            ClearFields();
        }

        // Show Box Manager with Callback ---------------------------------------------------------------------------------------------------
        private void ShowBoxManagerWithCallback(string intgCode)
        {
            // Create a new BoxManager instance and set its properties
            var boxManager = new BoxManager
            {
                tcarRef = tcarRef
            };

            // Set up call back for when box data is saved
            boxManager.OnBoxDataSaved = async (DataTable boxData) =>
            {
                var boxRefs = boxData.AsEnumerable()
                                     .Select(r => r["BoxNo"]?.ToString())
                                     .Where(b => !string.IsNullOrWhiteSpace(b))
                                     .Select(b => int.TryParse(b, out int result) ? result : -1)
                                     .Where(b => b > 0)
                                     .Distinct()
                                     .ToList();

                // Use PROCARRIER Integration
                if (intgCode == "PROCAR")
                {
                    await TcarControl.PrintLabels(boxRefs, GetLabelProCarr);
                }

                // Use PHUB Integration
                else if (intgCode == "PHUB_API")
                {
                    await TcarControl.PrintLabels(boxRefs, GetLabelPhub);
                }

                // Error
                else
                {
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowError("Invalid Integration Code for printing labels.");
                    return;
                }

                ClearFields();
            };

            boxManager.Show();
        }

        // Create DataTable from DataGridView for Box Contents -------------------------------------------------------------
        private DataTable CreateDataTableFromGrid()
        {
            // Create a new DataTable to hold the box contents
            var dataTable = new DataTable();
            dataTable.Columns.Add("BoxNo", typeof(string));
            dataTable.Columns.Add("Part", typeof(string));
            dataTable.Columns.Add("Qty", typeof(string));

            // Loop through each row in the DataGridView and add it to the DataTable
            foreach (DataGridViewRow row in dgContents.Rows)
            {
                if (row.IsNewRow) continue;

                var dr = dataTable.NewRow();
                dr["BoxNo"] = lblBoxes.Text ?? "1";
                dr["Part"] = row.Cells["Part"].Value?.ToString();
                dr["Qty"] = row.Cells["Qty"].Value?.ToString();
                dataTable.Rows.Add(dr);
            }

            return dataTable;
        }

        // Print Label Method (Async) ---------------------------------------------------------------------------------------------------
        public async void PrintLabel(int source = 1)
        {
            WcmsApi.error = false;

            // Check if all fields are valid before proceeding with printing
            if (CheckAllFields()) return;

            if (source == 1)
            {
                var messageBox = new CustomMessageBox();
                bool confirmed = messageBox.ShowQuestion("Print Label(s)?", "Are you sure you want to save current data and print all carrier labels?");
                if (!confirmed) return;
            }

            // Save the current data before printing
            SaveData();
            string? accCode = apiAccCode;
            int attempts = 0;

            SetApiSessionDetails();

            // Get Access Token if required
            if (intgCode == "PHUB_API")
            {
                while (!WcmsApi.CheckAccessTkn(accCode) && attempts <= 1)
                {
                    await WcmsApi.GetAccessToken(accCode, txbName.Name);
                    attempts++;
                }
            }

            // Count Number of lines
            int rowCount = dgContents.Rows.Count;

            // Evaluate the number of lines
            if (rowCount == 1)
            {
                // Handle single box print
                await HandleSingleBoxPrint(intgCode);
            }
            else if (rowCount > 1)
            {
                // Handle multiple boxes print
                var messageBox = new CustomMessageBox();
                bool multiBox = messageBox.ShowQuestion("Boxes", "Do you require more than 1 box?");
                if (multiBox)
                {
                    ShowBoxManagerWithCallback(intgCode);
                }
                else
                {
                    await HandleSingleBoxPrint(intgCode);
                }
            }
        }

        //=============================================================================================================================================================================================
        //-- Enviroment Box Events --//
        //=============================================================================================================================================================================================

        // Exit Button ------------------------------------------------------------------------------------------------------------------
        private void btnExit_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnExit);
        }

        private void btnExit_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnExit);
        }

        // Search Button ------------------------------------------------------------------------------------------------------------------
        private void btnSearch_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnSearch);
        }

        private void btnSearch_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnSearch);
        }

        // Reload Button ------------------------------------------------------------------------------------------------------------------
        private void btnReload_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnReload);
        }

        private void btnReload_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnReload);
        }

        // Print Button ------------------------------------------------------------------------------------------------------------------
        private void btnPrint_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnPrint);
        }

        private void btnPrint_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnPrint);
        }

        // Load Note Field ------------------------------------------------------------------------------------------------------------------
        private void txbLoadNote_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbLoadNote);
            SessionMaintenance.LogBook("", "[CarrierForm]", "[txbLoadNote_Enter]", $"Field Entered", lblLogBook);
        }

        private void txbLoadNote_Leave(object sender, EventArgs e)
        {
            // If Load Note field has text, search for it
            if (!string.IsNullOrEmpty(txbLoadNote.Text))
            {
                btnSearch_Click(sender, e);
            }

            SessionMaintenance.ControlLeave(txbLoadNote);
            SessionMaintenance.LogBook("", "[CarrierForm]", "[txbLoadNote_Enter]", $"Field Left", lblLogBook);
        }

        // Client Field ------------------------------------------------------------------------------------------------------------------
        private void cbClient_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(cbClient);
        }

        private void cbClient_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(cbClient);
        }

        // Carrier Field ------------------------------------------------------------------------------------------------------------------
        private void cbCarrier_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(cbCarrier);
        }

        private void cbCarrier_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(cbCarrier);
        }
        private void cbCarrier_TextChanged(object sender, EventArgs e)
        {
            lblServiceDescr.Text = "";
            PopulateCarrierComboBoxes(cbService, cbClient.Text ?? "");
            SessionMaintenance.LogBook("", "[CarrierForm]", "[cbService_TextChanged]", $"Carrier Changed {cbCarrier.Text ?? ""}", lblLogBook);
        }

        // Service Field ------------------------------------------------------------------------------------------------------------------
        private void cbService_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(cbService);
        }

        private void cbService_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(cbService);
        }

        private void cbService_TextChanged(object sender, EventArgs e)
        {
            string Service = cbService.Text ?? "";
            SessionMaintenance.LogBook("", "[CarrierForm]", "[cbService_TextChanged]", $"Service Changed {Service}", lblLogBook);
            UpdateCarrDescr(Service);
        }


        // EORI Field ------------------------------------------------------------------------------------------------------------------
        private void txbEORI_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbEORI);
        }

        private void txbEORI_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbEORI);

            CheckField(3, txbEORI);
        }
        // IOSS Field ------------------------------------------------------------------------------------------------------------------
        private void txbIOSS_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbIOSS);
        }

        private void txbIOSS_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbIOSS);

            CheckField(4, txbIOSS);
        }
        // Name Field ------------------------------------------------------------------------------------------------------------------
        private void txbName_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbName);
        }

        private void txbName_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbName);

            CheckField(5, txbName);
        }
        // Company Field ------------------------------------------------------------------------------------------------------------------
        private void txbCompany_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbCompany);
        }

        private void txbCompany_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbCompany);

            CheckField(6, txbCompany);
        }
        // Postcode Field ------------------------------------------------------------------------------------------------------------------
        private void txbPostcode_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbPostcode);
        }

        private void txbPostcode_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbPostcode);

            CheckField(7, txbPostcode);
        }
        // City Field ------------------------------------------------------------------------------------------------------------------
        private void txbCity_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbCity);
        }

        private void txbCity_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbCity);

            CheckField(8, txbCity);
        }
        // Addr1 Field ------------------------------------------------------------------------------------------------------------------
        private void txbAddr1_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbAddr1);
        }

        private void txbAddr1_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbAddr1);

            CheckField(9, txbAddr1);
        }
        // Addr2 Field ------------------------------------------------------------------------------------------------------------------
        private void txbAddr2_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbAddr2);
        }

        private void txbAddr2_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbAddr2);

            CheckField(10, txbAddr2);

        }
        // County Field ------------------------------------------------------------------------------------------------------------------
        private void txbCounty_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbCounty);
        }

        private void txbCounty_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbCounty);

            CheckField(11, txbCounty);
        }
        // Country Field ------------------------------------------------------------------------------------------------------------------
        private void cbCountry_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(cbCountry);
        }

        private void cbCountry_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(cbCountry);

        }
        private void cbCountry_TextChanged(object sender, EventArgs e)
        {
            string country = cbCountry.Text ?? "";
            if (country == "Great Britain")
            {
                domestic = 1;
                lblDomestic.Text = "Domestic";
            }
            else
            {
                domestic = 0;
                lblDomestic.Text = "International";
            }
        }

        // Email Field ------------------------------------------------------------------------------------------------------------------
        private void txbEmail_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbEmail);
        }

        private void txbEmail_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbEmail);

            CheckField(12, txbEmail);
        }
        // Phone Field ------------------------------------------------------------------------------------------------------------------
        private void txbPhone_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbPhone);
        }

        private void txbPhone_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbPhone);

            CheckField(13, txbPhone);
        }
        // Ref 1 Field ------------------------------------------------------------------------------------------------------------------
        private void txbRef1_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbRef1);
        }

        private void txbRef1_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbRef1);

        }
        // Ref 2 Field ------------------------------------------------------------------------------------------------------------------
        private void txbRef2_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbRef2);
        }

        private void txbRef2_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbRef2);

        }

        // Client Field ------------------------------------------------------------------------------------------------------------------
        private void cbClient_TextChanged(object sender, EventArgs e)
        {
            ClearFields();
        }

        // LogBook Label ------------------------------------------------------------------------------------------------------------------
        private void lblLogBook_MouseEnter(object sender, EventArgs e)
        {
            lblLogBook.BackColor = Color.FromArgb(70, 70, 70);
            lblLogBook.ForeColor = Color.FromArgb(10, 10, 10);
        }

        private void lblLogBook_MouseLeave(object sender, EventArgs e)
        {
            lblLogBook.BackColor = Color.FromArgb(10, 10, 10);
            lblLogBook.ForeColor = Color.FromArgb(64, 64, 64);
        }


        //=============================================================================================================================================================================================
        //-- Button Click Events --//
        //=============================================================================================================================================================================================

        // Exit Button ------------------------------------------------------------------------------------------------------------------
        private void btnExit_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[btnExit_Click]", $"Exit Button Clicked", lblLogBook);
            SessionMaintenance.LogBook("", "[CarrierForm]", "[FormClosing]", $"Form Closed", lblLogBook);
            this.Close();
        }

        // Search Button ------------------------------------------------------------------------------------------------------------------
        private void btnSearch_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[btnSearch_Click]", $"Search Button Clicked", lblLogBook);
            string loadNote = txbLoadNote.Text;
            string client = "";

            CustomMessageBox messageBox = new CustomMessageBox();

            if (cbClient.SelectedItem != null)
            {
                client = cbClient.SelectedItem.ToString();
            }

            lastClient = null;
            lastLoadNote = null;

            lastClient = client;
            lastLoadNote = loadNote;

            if (string.IsNullOrEmpty(client))
            {
                messageBox.ShowDefError("130", $"");
                return;
            }
            else if (string.IsNullOrEmpty(loadNote))
            {
                messageBox.ShowDefError("228", $"");
                return;
            }
            else
            {
                lblStatus.Visible = true;
                int status = TcarControl.CheckLoadNote(loadNote, client);

                switch (status)
                {
                    case 0:
                        messageBox.ShowError($"load note not found: {loadNote}");
                        btnPrint.Visible = false;
                        break;
                    case 2:
                        messageBox.ShowError($"Load note: {loadNote} is no longer open.");
                        btnPrint.Visible = false;
                        break;
                    case 4:
                        messageBox.ShowWarning($"A Carrier label has been created for Load note: {loadNote} using the TAFIE application");
                        GetLoadNote(loadNote, client);
                        btnPrint.Visible = true;
                        break;
                    default:
                        GetLoadNote(loadNote, client);
                        btnPrint.Visible = true;
                        break;
                }
                lblStatus.Visible = false;
            }
        }

        // Get Service Button ------------------------------------------------------------------------------------------------------------------
        private void btnService_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[btnService_Click]", $"Get Service Button Clicked", lblLogBook);
            CustomMessageBox messageBox = new CustomMessageBox();
            messageBox.ShowError($"Sorry, This feature is not currently available.");
            return;
        }


        // Print Label Button (Async) ------------------------------------------------------------------------------------------
        private async void btnPrint_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[btnPrint_Click]", $"Print Button Clicked", lblLogBook);

            if (scanned == 1)
            {
                PrintLabel();
            }
            else
            {
                DespatchForm despatchForm = new DespatchForm(this);
                despatchForm.tcarRef = tcarRef;
                despatchForm.Show();
            }

        }

        // Menu Strip Click //
        // Close ------------------------------------------------------------------------------------------
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[closeToolStripMenuItem_Click]", $"MenuStipItem - Close Clicked", lblLogBook);
            btnExit_Click(sender, e);
        }
        // Print Label ------------------------------------------------------------------------------------------
        private void printLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[printLabelToolStripMenuItem_Click]", $"MenuStipItem - Print Label Clicked", lblLogBook);
            if (btnPrint.Visible)
            {
                btnPrint_Click(sender, e);
            }
            else
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"No Label data to print");
                return;
            }
        }
        // Clear ------------------------------------------------------------------------------------------
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[clearToolStripMenuItem_Click]", $"MenuStipItem - Clear Clicked", lblLogBook);
            cbClient.Text = "";
            ClearFields();
        }
        // Reprint ------------------------------------------------------------------------------------------
        private void reprintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[reprintToolStripMenuItem_Click]", $"MenuStipItem - RePrint Clicked", lblLogBook);
            RePrint rePrint = new RePrint();
            rePrint.Show();
        }
        // Carrier Combos ------------------------------------------------------------------------------------------
        private void combinationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[combinationsToolStripMenuItem_Click]", $"MenuStipItem - Carrier Combos Clicked", lblLogBook);
            string client = "";
            if (cbClient.SelectedItem != null)
            {
                client = cbClient.SelectedItem.ToString();

                CarrierCombos carrierCombos = new CarrierCombos();
                carrierCombos.passedClient = client;
                carrierCombos.Show();
            }
            else
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("130", $"");
                return;
            }
        }
        // Address Search ------------------------------------------------------------------------------------------
        private void addressSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[addressSearchToolStripMenuItem_Click]", $"MenuStipItem - Address Search Clicked", lblLogBook);
            TcarControl.OpenURL();
        }

        // MenuStip client manager ------------------------------------------------------------------------------------------
        private void clientManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[clientManagerToolStripMenuItem_Click]", $"MenuStipItem - Client Manager Clicked", lblLogBook);
            if (
                SessionMaintenance.userName == "AIDENB"
                || SessionMaintenance.userName == "KYALC"
                || SessionMaintenance.userName == "JACOBDR"
                || SessionMaintenance.userName == "JOSEPH"
                || SessionMaintenance.userName == "STEVE"
                || SessionMaintenance.userName == "ANDYC"
                || SessionMaintenance.userName == "JAMEST"
                || SessionMaintenance.userName == "SARAHS"
                || SessionMaintenance.userName == "REBECCACO"
                || SessionMaintenance.userName == "MARTINY"
                )
            {
                ClientManager clientManager = new ClientManager(this);
                clientManager.Show();
            }
            else
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("Sorry, you do not have permission to use this feature.");
                return;
            }

        }

        // Reload Button ------------------------------------------------------------------------------------------
        private void btnReload_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[btnReload_Click]", $"Reload Button Clicked", lblLogBook);
            if (string.IsNullOrEmpty(lastLoadNote))
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("No Data to load");
                return;
            }
            else
            {
                cbClient.Text = lastClient;
                txbLoadNote.Text = lastLoadNote;
                btnSearch_Click(sender, e); // Re-run the search with the last used values
            }
        }

        // LogBook Label ------------------------------------------------------------------------------------------
        private void lblLogBook_Click(object sender, EventArgs e)
        {
            CustomMessageBox messageBox = new CustomMessageBox();
            messageBox.ShowLogBook(lblLogBook.Text);
        }

        // Open printer settings ------------------------------------------------------------------------------------------
        private void printersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[printersToolStripMenuItem_Click]", $"MenuStipItem - Printer Settings Clicked", lblLogBook);
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "ms-settings:printers",
                    UseShellExecute = true
                });
            }
            catch (Exception ex) // Catch Errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured attempting to open printer settings\n{ex}");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[PopulateDataGrid]", $"FAILED:( {ex.Message} )", lblLogBook);
            }
        }

        // Run Labels Generated Report ------------------------------------------------------------------------------------------
        private void labelsGeneratedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[labelsGeneratedToolStripMenuItem_Click]", $"MenuStipItem - Reports - Labels Generated Clicked", lblLogBook);
            TcarControl.RunReport("001");
        }

        // Run Client Breakdown Report ------------------------------------------------------------------------------------------
        private void clientBreakdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[clientBreakdownToolStripMenuItem_Click]", $"MenuStipItem - Reports - Client Breakdown Clicked", lblLogBook);
            TcarControl.RunReport("002");
        }

        // Run Carrier Breakdown Report ------------------------------------------------------------------------------------------
        private void carrierBreakdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[carrierBreakdownToolStripMenuItem_Click]", $"MenuStipItem - Reports - Carrier Breakdown Clicked", lblLogBook);
            TcarControl.RunReport("003");
        }

        //=============================================================================================================================================================================================
        //-- Key Down Events --//
        //=============================================================================================================================================================================================

        private void CarrierForm_KeyDown(object sender, KeyEventArgs e)
        {
            // ctrl + G
            if (e.Control && e.KeyCode == Keys.G)
            {
                cbClient.Text = "";
                ClearFields();
            }

            // ctrl + R
            if (e.Control && e.KeyCode == Keys.R)
            {
                btnSearch_Click(sender, e);
            }

            // ctrl + P
            if (e.Control && e.KeyCode == Keys.P)
            {
                printLabelToolStripMenuItem_Click(sender, e);
            }

            // Esc
            if (e.KeyCode == Keys.Escape)
            {
                btnExit_Click(sender, e);
            }
        }
    }
}
