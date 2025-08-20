using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using System.Windows.Forms;

namespace TAFIE
{
    //=============================================================================================================================================================================================
    //-- Initialization --//
    //=============================================================================================================================================================================================


    public partial class FixForm : Form
    {
        private const string connectionString = SessionMaintenance.connectionString;
        public string updateField { get; set; }
        public string suggFix { get; set; }
        public string client { get; set; }

        public string value = "";

        public FixForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        // Form Load ------------------------------------------------------------------------------------------------------------------
        private void FixForm_Load(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[FixForm]", "[FormLoad]", $"Form Started");
            Text = $"{Environment.UserName.ToUpper()} - {SessionMaintenance.appName} Fix";
            lblField.Text = $"Error Field: {updateField}";
            lblSuggFix.Text = suggFix;
            PopulateComboBox();
            GetFieldValue();
            HighlightField();
        }

        //=============================================================================================================================================================================================
        //-- Operational Methods --//
        //=============================================================================================================================================================================================

        // Get Error Field Value ------------------------------------------------------------------------------------------------------------------
        private void GetFieldValue()
        {
            string executeQuery = "EXECUTE [TAFIE_Get_Error_Field_V2] @Session_Id, @Client";

            string initials = "";
            string fullName = "";
            string organisation = "";
            string address = "";
            string city = "";
            string county = "";
            string country = "";
            string postcode = "";
            string phoneDay = "";
            string phoneEve = "";
            string phoneExt = "";
            string mobile = "";
            string email = "";
            int mode = GetMode(SessionMaintenance.sessionId);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // EXECUTE STORED PROC
                    using (SqlCommand cmd = new SqlCommand(executeQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Session_Id", SessionMaintenance.sessionId);
                        cmd.Parameters.AddWithValue("@Client", client);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                initials = reader["initials"].ToString();
                                fullName = reader["full_name"].ToString();
                                organisation = reader["organisation"].ToString();
                                address = reader["address"].ToString();
                                city = reader["city"].ToString();
                                county = reader["county"].ToString();
                                country = reader["country"].ToString();
                                postcode = reader["postcode"].ToString();
                                phoneDay = reader["phone_day"].ToString();
                                phoneEve = reader["phone_eve"].ToString();
                                phoneExt = reader["phone_ext"].ToString();
                                mobile = reader["mobile"].ToString();
                                email = reader["email"].ToString();
                            }
                        }
                        SessionMaintenance.LogBook("", "[FixForm]", "[GetFieldValue]", $"SQL Stored Proc Executed");
                    }
                    conn.Close();
                }

                SessionMaintenance.LogBook("", "[FixForm]", "[GetFieldValue]", $"Evaluating Type: {mode}");

                switch (mode)
                {
                    case 1:
                        txbInitials.Text = initials;
                        txbSurname.Text = fullName;
                        txbCompany.Text = organisation;
                        rtbAddress.Text = address;
                        txbCity.Text = city;
                        txbCounty.Text = county;
                        txbPostcode.Text = postcode;
                        cbCountry.Text = country;
                        cntAddr.Visible = true;
                        cntContact.Visible = false;
                        break;
                    case 2:
                        txbPhoneDay.Text = phoneDay;
                        txbPhoneEve.Text = phoneEve;
                        txbPhoneExt.Text = phoneExt;
                        txbMobile.Text = mobile;
                        txbEmail.Text = email;
                        cntAddr.Visible = false;
                        cntContact.Visible = true;
                        break;
                    default:
                        cntAddr.Visible = false;
                        cntContact.Visible = false;
                        break;
                }
            }
            catch (Exception ex) // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError($"118", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[FixForm]", "[GetFieldValue]", $"FAILED: Code 118 ( {ex.Message} )");
            }
        }

        // Get Mode ------------------------------------------------------------------------------------------------------------------
        public static int GetMode(string sessionId)
        {
            int mode = 0;
            int type = 0;
            string query = "SELECT [TYPE] as [1] FROM intg_errs WHERE ID = (SELECT [Error_Id] FROM TAFIE_Error_Results WHERE Session_Id = @Session_Id)";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Session_Id", sessionId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                type = (int)reader["1"];
                            }
                        }

                        switch (type)
                        {
                            case 101: mode = 1; break;
                            case 102: mode = 2; break;
                        }
                    }
                    conn.Close();
                }
            }
            // Catch Errors
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("118", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[FixForm]", "[GetMode]", $"FAILED: Code 118 ( {ex.Message} )");
            }

            return mode;
        }

        // Update Error Field Value ------------------------------------------------------------------------------------------------------------------
        private void UpdateFieldValue(int mode)
        {
            string query = "EXECUTE [TAFIE_Fix_Error_V2] @Session_Id,@Client,@User,@Field_A,@Field_B,@Field_C,@Field_D,@Field_E,@Field_F,@Field_G,@Field_H";
            string fieldA = "";
            string fieldB = "";
            string fieldC = "";
            string fieldD = "";
            string fieldE = "";
            string fieldF = "";
            string fieldG = "";
            string fieldH = "";

            switch (mode)
            {
                case 1:
                    fieldA = txbInitials.Text;
                    fieldB = txbSurname.Text;
                    fieldC = txbCompany.Text;
                    fieldD = rtbAddress.Text;
                    fieldE = txbCity.Text;
                    if (cbCountry.SelectedItem != null)
                    {
                        fieldF = cbCountry.SelectedItem.ToString();
                    }
                    fieldG = txbCounty.Text;
                    fieldH = txbPostcode.Text;
                    break;
                case 2:
                    fieldA = txbPhoneDay.Text;
                    fieldB = txbPhoneEve.Text;
                    fieldC = txbPhoneExt.Text;
                    fieldD = txbMobile.Text;
                    fieldF = txbEmail.Text;
                    break;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Session_Id", SessionMaintenance.sessionId);
                        cmd.Parameters.AddWithValue("@Client", client);
                        cmd.Parameters.AddWithValue("@User", SessionMaintenance.userName);
                        cmd.Parameters.AddWithValue("@Field_A", fieldA);
                        cmd.Parameters.AddWithValue("@Field_B", fieldB);
                        cmd.Parameters.AddWithValue("@Field_C", fieldC);
                        cmd.Parameters.AddWithValue("@Field_D", fieldD);
                        cmd.Parameters.AddWithValue("@Field_E", fieldE);
                        cmd.Parameters.AddWithValue("@Field_F", fieldF);
                        cmd.Parameters.AddWithValue("@Field_G", fieldG);
                        cmd.Parameters.AddWithValue("@Field_H", fieldH);
                        cmd.ExecuteNonQuery();
                        SessionMaintenance.LogBook("", "[FixForm]", "[UpdateFieldValue]", $"SQL Stored Proc Executed: {mode}");

                        CustomMessageBox messageBox = new CustomMessageBox();
                        messageBox.ShowMessage($"Field has been updated, please try to despatch the load note again.", "Updated!");
                    }
                    conn.Close();
                }
            }
            catch (Exception ex) // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError($"229", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[FixForm]", "[UpdateFieldValue]", $"FAILED: Code 229 ( {ex.Message} )");
            }
        }

        // Populate Combo Box ------------------------------------------------------------------------------------------------------------------
        private void PopulateComboBox()
        {
            string query = "SELECT RTRIM(Descr) as [1] FROM [SQL-BISCUIT].[eluciddb_ms].dbo.ctry ORDER BY Descr";

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
                            cbCountry.Items.Clear(); // Clear Combo box ready for new data

                            // Populate ComboBox from reader
                            while (reader.Read())
                            {
                                cbCountry.Items.Add(reader["1"].ToString());
                            }
                        }
                    }

                    SessionMaintenance.LogBook("", "[FixForm]", "[PopulateComboBox]", $"ComboBox Populated");
                }
            }
            catch (Exception ex) // Catch any errors
            {
                Cursor.Current = Cursors.Default;
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("112", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[MainForm]", "[PopulateComboBoxes]", $"FAILED: Code 112 ( {ex.Message} )");
            }
        }

        // Populate Combo Box ------------------------------------------------------------------------------------------------------------------
        private void HighlightField()
        {
            switch (updateField)
            {
                case "initials": lblFn.BackColor = Color.Red; lblLn.BackColor = Color.Red; break;
                case "full_name": lblFn.BackColor = Color.Red; lblLn.BackColor = Color.Red; break;
                case "company_name": lblCom.BackColor = Color.Red; break;
                case "Address": lblAddr.BackColor = Color.Red; break;
                case "city": lblCity.BackColor = Color.Red; break;
                case "county": lblCnty.BackColor = Color.Red; break;
                case "country": lblCtry.BackColor = Color.Red; break;
                case "postcode": lblPc.BackColor = Color.Red; break;
                case "phone_day":
                    lblPhone1.BackColor = Color.Red;
                    lblPhone2.BackColor = Color.Red;
                    lblPhone3.BackColor = Color.Red;
                    lblPhone4.BackColor = Color.Red;
                    break;
                case "email": lblEmail.BackColor = Color.Red; break;
            }

            SessionMaintenance.LogBook("", "[FixForm]", "[HighlightField]", $"Field Highlighted {updateField}");
        }

        // Delta Display ------------------------------------------------------------------------------------------------------------------
        private void DeltaDisplay()
        {
            switch (updateField)
            {
                case "initials": txbInitials.BackColor = Color.LimeGreen; txbSurname.BackColor = Color.LimeGreen; break;
                case "full_name": txbInitials.BackColor = Color.LimeGreen; txbSurname.BackColor = Color.LimeGreen; break;
                case "company_name": txbCompany.BackColor = Color.LimeGreen; break;
                case "Address": rtbAddress.BackColor = Color.LimeGreen; break;
                case "city": txbCity.BackColor = Color.LimeGreen; break;
                case "county": txbCounty.BackColor = Color.LimeGreen; break;
                case "country": cbCountry.BackColor = Color.LimeGreen; break;
                case "postcode": txbPostcode.BackColor = Color.LimeGreen; break;
                case "phone_day":
                    txbPhoneDay.BackColor = Color.LimeGreen;
                    txbPhoneEve.BackColor = Color.LimeGreen;
                    txbPhoneExt.BackColor = Color.LimeGreen;
                    txbMobile.BackColor = Color.LimeGreen;
                    break;
                case "email": txbEmail.BackColor = Color.LimeGreen; break;
            }

            SessionMaintenance.LogBook("", "[FixForm]", "[DeltaDisplay]", $"Delta Field Highlighted {updateField}");
        }

        // Check Field ------------------------------------------------------------------------------------------------------------------
        private string CheckField()
        {
            // Helper method for string length validation
            bool IsTooLong(string input, int maxLength) => input.Length > maxLength;

            // Helper method for null or empty check
            bool IsNullOrEmpty(string input) => string.IsNullOrEmpty(input);

            // Helper method to validate phone numbers
            bool IsValidPhoneNumber(string input) => System.Text.RegularExpressions.Regex.IsMatch(input, @"^[0-9+]*$");

            // Helper method to validate email
            bool IsValidEmail(string input) => System.Text.RegularExpressions.Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            // Helper method to allow only specific special characters
            bool HasValidSpecialCharacters(string input, string pattern) => System.Text.RegularExpressions.Regex.IsMatch(input, pattern);

            string error = "No Error";
            int mode = GetMode(SessionMaintenance.sessionId);

            string country = cbCountry.SelectedItem?.ToString() ?? "";

            if (mode == 1)
            {
                if (IsNullOrEmpty(txbInitials.Text) || IsNullOrEmpty(txbSurname.Text))
                    return "Customer Name is required";
                if (IsNullOrEmpty(txbCity.Text))
                    return "City is required";
                if (IsNullOrEmpty(rtbAddress.Text))
                    return "Address is required";
                if (IsNullOrEmpty(txbPostcode.Text))
                    return "Postcode/Zipcode is required";
                if (IsNullOrEmpty(txbCounty.Text))
                    return "County/State is required";
                if (IsNullOrEmpty(country))
                    return "Country is required";

                if (!HasValidSpecialCharacters(txbInitials.Text + txbSurname.Text, @"^[a-zA-Z\-']+$"))
                    return "Customer Name contains invalid characters";
                if (!HasValidSpecialCharacters(rtbAddress.Text, @"^[a-zA-Z0-9\s\-']+$"))
                    return "Address contains invalid characters";
                if (!IsNullOrEmpty(txbCompany.Text) && !HasValidSpecialCharacters(txbCompany.Text, @"^[a-zA-Z0-9\s\-\(\)']+$"))
                    return "Company contains invalid characters";

                if (txbInitials.Text.Length + txbSurname.Text.Length > 35)
                    return "Customer Name is too long, Maximum length: 35 (Combined)";
                if (IsTooLong(txbCompany.Text, 35))
                    return "Company is too long, Maximum length: 35";
                if (IsTooLong(txbPostcode.Text, 10))
                    return "Postcode is too long, Maximum length: 10";
                if (IsTooLong(rtbAddress.Text, 35))
                    return "Address is too long, Maximum length: 35";
                if (IsTooLong(txbCity.Text, 30))
                    return "City is too long, Maximum length: 30";
                if (IsTooLong(txbCounty.Text, 100))
                    return "County/State is too long, Maximum length: 100";
            }
            else if (mode == 2)
            {
                if (!IsNullOrEmpty(txbPhoneDay.Text) && !IsValidPhoneNumber(txbPhoneDay.Text))
                    return "Phone Day contains invalid characters";
                if (!IsNullOrEmpty(txbPhoneEve.Text) && !IsValidPhoneNumber(txbPhoneEve.Text))
                    return "Phone Evening contains invalid characters";
                if (!IsNullOrEmpty(txbPhoneExt.Text) && !IsValidPhoneNumber(txbPhoneExt.Text))
                    return "Phone Extension contains invalid characters";
                if (!IsNullOrEmpty(txbMobile.Text) && !IsValidPhoneNumber(txbMobile.Text))
                    return "Mobile contains invalid characters";

                if (IsTooLong(txbPhoneDay.Text, 16) || IsTooLong(txbPhoneEve.Text, 16) ||
                    IsTooLong(txbPhoneExt.Text, 16) || IsTooLong(txbMobile.Text, 16))
                    return "Phone is too long, Maximum length: 16";

                if (!IsNullOrEmpty(txbEmail.Text) && !IsValidEmail(txbEmail.Text))
                    return "Invalid email format";

                if (IsTooLong(txbEmail.Text, 40))
                    return "Email is too long, Maximum length: 40";
            }

            return error;
        }

        // Populate Character Count ------------------------------------------------------------------------------------------------------------------
        private string CharacterCount(string field, Control control, int max)
        {
            string labelText = "";
            int length = control.Text.Length;

            labelText = $"{field} - {length}/{max}";

            return labelText;
        }

        // Delta Check ------------------------------------------------------------------------------------------------------------------
        private void DeltaCheckIn(Control control)
        {
            SessionMaintenance.ControlEnter(control);
            if (!string.IsNullOrEmpty(control.Text))
            {
                value = control.Text;
            }
            else
            {
                value = "";
            }
        }

        private void DeltaCheckOut(Control control)
        {
            string delta = control.Text;

            if (delta != value)
            {
                control.BackColor = Color.Aqua;
            }
            else
            {
                SessionMaintenance.ControlLeave(control);
            }
        }

        // Open URL ------------------------------------------------------------------------------------------------------------------
        private void OpenURL()
        {
            string url = "https://www.royalmail.com/find-a-postcode";

            try
            {
                // Open the URL in the default web browser
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };

                Process.Start(processStartInfo);

                SessionMaintenance.LogBook($"", "[FixForm]", "[OpenURL]", $"Process Executed");

            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("121", $"\n{ex.Message}");
                SessionMaintenance.LogBook($"ERROR", "[FixForm]", "[OpenURL]", $"FAILED: Code 121 (  {ex.Message}  )");

            }
        }

        //=============================================================================================================================================================================================
        //-- Enviroment Box Events --//
        //=============================================================================================================================================================================================

        // Change Button Colours //
        // Close Button ------------------------------------------------------------------------------------------------------------------
        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnClose);
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnClose);
        }

        // Update Button ------------------------------------------------------------------------------------------------------------------
        private void btnUpdate_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnUpdate);
        }

        private void btnUpdate_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnUpdate);
        }

        // Change Control Colours //
        // Phone Day ------------------------------------------------------------------------------------------------------------------
        private void txbPhoneDay_Enter(object sender, EventArgs e)
        {
            DeltaCheckIn(txbPhoneDay);
        }

        private void txbPhoneDay_Leave(object sender, EventArgs e)
        {
            DeltaCheckOut(txbPhoneDay);
        }

        // Phone Eve ------------------------------------------------------------------------------------------------------------------
        private void txbPhoneEve_Enter(object sender, EventArgs e)
        {
            DeltaCheckIn(txbPhoneEve);
        }

        private void txbPhoneEve_Leave(object sender, EventArgs e)
        {
            DeltaCheckOut(txbPhoneEve);
        }

        //Phone Ext ------------------------------------------------------------------------------------------------------------------
        private void txbPhoneExt_Enter(object sender, EventArgs e)
        {
            DeltaCheckIn(txbPhoneExt);
        }

        private void txbPhoneExt_Leave(object sender, EventArgs e)
        {
            DeltaCheckOut(txbPhoneExt);
        }

        // Mobile ------------------------------------------------------------------------------------------------------------------
        private void txbMobile_Enter(object sender, EventArgs e)
        {
            DeltaCheckIn(txbMobile);
        }

        private void txbMobile_Leave(object sender, EventArgs e)
        {
            DeltaCheckOut(txbMobile);
        }

        // Email ------------------------------------------------------------------------------------------------------------------
        private void txbEmail_Enter(object sender, EventArgs e)
        {
            DeltaCheckIn(txbEmail);
        }

        private void txbEmail_Leave(object sender, EventArgs e)
        {
            DeltaCheckOut(txbEmail);
        }

        // Initials ------------------------------------------------------------------------------------------------------------------
        private void txbInitials_Enter(object sender, EventArgs e)
        {
            DeltaCheckIn(txbInitials);
        }

        private void txbInitials_Leave(object sender, EventArgs e)
        {
            DeltaCheckOut(txbInitials);
        }

        // Surname------------------------------------------------------------------------------------------------------------------
        private void txbSurname_Enter(object sender, EventArgs e)
        {
            DeltaCheckIn(txbSurname);
        }

        private void txbSurname_Leave(object sender, EventArgs e)
        {
            DeltaCheckOut(txbSurname);
        }

        // Company ------------------------------------------------------------------------------------------------------------------
        private void txbCompany_Enter(object sender, EventArgs e)
        {
            DeltaCheckIn(txbCompany);
        }

        private void txbCompany_Leave(object sender, EventArgs e)
        {
            DeltaCheckOut(txbCompany);
        }

        // Address ------------------------------------------------------------------------------------------------------------------
        private void rtbAddress_Enter(object sender, EventArgs e)
        {
            DeltaCheckIn(rtbAddress);
        }

        private void rtbAddress_Leave(object sender, EventArgs e)
        {
            DeltaCheckOut(rtbAddress);
        }

        // City ------------------------------------------------------------------------------------------------------------------
        private void txbCity_Enter(object sender, EventArgs e)
        {
            DeltaCheckIn(txbCity);
        }

        private void txbCity_Leave(object sender, EventArgs e)
        {
            DeltaCheckOut(txbCity);
        }

        // County ------------------------------------------------------------------------------------------------------------------
        private void txbCounty_Enter(object sender, EventArgs e)
        {
            DeltaCheckIn(txbCounty);
        }

        private void txbCounty_Leave(object sender, EventArgs e)
        {
            DeltaCheckOut(txbCounty);
        }

        // Postcode ------------------------------------------------------------------------------------------------------------------
        private void txbPostcode_Enter(object sender, EventArgs e)
        {
            DeltaCheckIn(txbPostcode);
        }

        private void txbPostcode_Leave(object sender, EventArgs e)
        {
            DeltaCheckOut(txbPostcode);
        }

        // Country ------------------------------------------------------------------------------------------------------------------
        private void cbCountry_Enter(object sender, EventArgs e)
        {
            DeltaCheckIn(cbCountry);
        }

        private void cbCountry_Leave(object sender, EventArgs e)
        {
            DeltaCheckOut(cbCountry);
        }

        // Text Changed ------------------------------------------------------------------------------------------------------------------
        private void txbPhoneDay_TextChanged(object sender, EventArgs e)
        {
            lblPhone1.Text = CharacterCount("Phone Day", txbPhoneDay, 16);
        }

        private void txbPhoneEve_TextChanged(object sender, EventArgs e)
        {
            lblPhone2.Text = CharacterCount("Phone Eve", txbPhoneEve, 16);
        }

        private void txbPhoneExt_TextChanged(object sender, EventArgs e)
        {
            lblPhone3.Text = CharacterCount("Phone Ext", txbPhoneExt, 16);
        }

        private void txbMobile_TextChanged(object sender, EventArgs e)
        {
            lblPhone4.Text = CharacterCount("Mobile", txbMobile, 16);
        }

        private void txbEmail_TextChanged(object sender, EventArgs e)
        {
            lblEmail.Text = CharacterCount("Email", txbEmail, 40);
        }

        private void txbCompany_TextChanged(object sender, EventArgs e)
        {
            lblCom.Text = CharacterCount("Company", txbCompany, 35);
        }

        private void rtbAddress_TextChanged(object sender, EventArgs e)
        {
            lblAddr.Text = CharacterCount("Address", rtbAddress, 35);
        }

        private void txbCity_TextChanged(object sender, EventArgs e)
        {
            lblCity.Text = CharacterCount("City", txbCity, 30);
        }

        private void txbCounty_TextChanged(object sender, EventArgs e)
        {
            lblCnty.Text = CharacterCount("County", txbCounty, 100);
        }

        private void txbPostcode_TextChanged(object sender, EventArgs e)
        {
            lblPc.Text = CharacterCount("Postcode", txbPostcode, 10);
        }

        private void txbInitials_TextChanged(object sender, EventArgs e)
        {
            string value = txbInitials.Text + " " + txbSurname.Text;
            int length = value.Length;

            lblLn.Text = $"Last Name - {length}/35";
        }

        private void lblPc_MouseEnter(object sender, EventArgs e)
        {
            ttPc.SetToolTip(lblPc, "Double click for postcode finder.");
        }

        //=============================================================================================================================================================================================
        //-- Button Click Events --//
        //=============================================================================================================================================================================================

        // Close Button ------------------------------------------------------------------------------------------------------------------
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Update Button ------------------------------------------------------------------------------------------------------------------
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            CustomMessageBox messageBox = new CustomMessageBox();
            string error = CheckField();

            if (error == "No Error")
            {
                bool result = messageBox.ShowQuestion("Update?", "Are you sure you want to update this field?"); // Ask user if they want to exit
                if (result == true)
                {
                    UpdateFieldValue(GetMode(SessionMaintenance.sessionId));
                    DeltaDisplay();
                }
                else
                {
                    return;
                }
            }
            else
            {
                messageBox.ShowError(error);
                return;
            }
        }

        // Double Click Post code lable ------------------------------------------------------------------------------------------------------------------
        private void lblPc_DoubleClick(object sender, EventArgs e)
        {
            OpenURL();
        }

        //=============================================================================================================================================================================================
        //-- Key Down Events --//
        //=============================================================================================================================================================================================

        // Keyboard Shortcuts ------------------------------------------------------------------------------------------------------------------
        private void FixForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Esc
            if (e.KeyCode == Keys.Escape)
            {
                btnClose_Click(sender, e);
            }
        }


    }
}
