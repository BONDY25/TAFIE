using Microsoft.Data.SqlClient;
using System.Xml;

namespace TAFIE
{
    public partial class MainForm : Form
    {
        //=============================================================================================================================================================================================
        //-- Initialization --//
        //=============================================================================================================================================================================================

        private const string connectionString = SessionMaintenance.connectionString;

        public MainForm()
        {
            InitializeComponent();
            this.MaximizeBox = false; // Diasble Maximize window option
            this.KeyPreview = true;
        }

        // Form Load ------------------------------------------------------------------------------------------------------------------
        private void MainForm_Load(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[MainForm]", "[FormLoad]", $"Form Started");
            Text = $"{Environment.UserName.ToUpper()} - {SessionMaintenance.appName} Home";
            lblUsername.Text = SessionMaintenance.userName;
            ClearFields();
            PopulateComboBoxes(cbClient, "CLIENT");
            cbClient.SelectedItem = null;
            txbLoadNote.Text = null;
            btnFix.Visible = ShowbtnFix();
            btnGetLbl.Visible = ShowbtnFix();
            rtbxFix.Size = new Size(678, 96);
        }

        // Exit Application Method -----------------------------------------------------------------------------------------------------------------------
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SessionMaintenance.LogBook("", "[MainForm]", "[FormClosing]", "Form Closed");
        }

        //=============================================================================================================================================================================================
        //-- Operational Methods --//
        //=============================================================================================================================================================================================

        // Populate Combo Boxes -----------------------------------------------------------------------------------------------------------------------
        private void PopulateComboBoxes(ComboBox comboBox, string field)
        {
            // Declare Variables
            string query = "";

            if (field == "CLIENT")
            {
                query = "SELECT [Description] FROM TAFIE_Clients WHERE [Active] = '1' ORDER BY [Description]";
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
                SessionMaintenance.LogBook("ERROR", "[MainForm]", "[PopulateComboBoxes]", $"FAILED: Code 112 ( {ex.Message} )");
                SessionMaintenance.LogBook("ERROR", "[MainForm]", "[PopulateComboBoxes]", "Application Closed");
                Application.Exit();
            }
        }

        // Clear Fields -----------------------------------------------------------------------------------------------------------------------
        private void ClearFields()
        {
            txbLoadNote.Text = "";
            rtbxError.Text = "";
            rtbxFix.Text = "";
            rtbxCall.Text = "";
            rtbxResp.Text = "";
            btnClear.Visible = false;
            btnExpand.Visible = false;
            btnSearch.Visible = true;
            lblFixAtmp.Visible = false;

            string query = "EXECUTE [TAFIE_Clear_Session] @Session_Id";

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Session_Id", SessionMaintenance.sessionId);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close(); // Close SQL Connection
                }
            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("226", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[MainForm]", "[ClearFields]", $"FAILED: Code 226 ( {ex.Message} )");
            }

            rtbxFix.Size = new Size(678, 96);
            btnFix.Visible = ShowbtnFix();
            btnGetLbl.Visible = ShowbtnFix();
            txbLoadNote.Focus();
        }

        // Check Load Note Exists-----------------------------------------------------------------------------------------------------------------------
        private int CheckLoadNote(string loadNote, string client)
        {
            int check = 0;
            string query = "EXECUTE TAFIE_Check_Load @Session_Id, @Client, @Load_Note";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Session_Id", SessionMaintenance.sessionId);
                        cmd.Parameters.AddWithValue("@Client", client);
                        cmd.Parameters.AddWithValue("@Load_Note", loadNote);

                        int result = (int)cmd.ExecuteScalar();

                        check = result;
                    }
                    conn.Close();
                }
            }
            // Catch Errors
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("109", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[MainForm]", "[CheckLoadNote]", $"FAILED: Code 109 ( {ex.Message} )");
            }

            return check;
        }

        // Get Load Note Details -----------------------------------------------------------------------------------------------------------------------
        private void GetLoadDetails(string loadNote, string client, int mode)
        {
            string query = "";

            switch (mode)
            {
                case 0: query = "EXECUTE [TAFIE_Get_Lnot_Results] @Session_Id, @Client, @Load_Note"; break;
                case 1: query = "EXECUTE [TAFIE_Get_Lnot_Line_Results] @Session_Id, @Client, @Load_Note"; break;
                case 2: query = "EXECUTE [TAFIE_Get_Error_Results] @Session_Id, @Client, @Load_Note"; break;
                case 3: query = "EXECUTE [TAFIE_Get_API_Results] @Session_Id, @Client, @Load_Note"; break;
            }

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Session_Id", SessionMaintenance.sessionId);
                        cmd.Parameters.AddWithValue("@Client", client);
                        cmd.Parameters.AddWithValue("@Load_Note", loadNote);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close(); // Close SQL Connection

                    SessionMaintenance.LogBook("", "[MainForm]", "[GetLoadDetails]", $"Get Details executed for: {loadNote} - {mode}");
                }
            }
            catch (Exception ex)  // Catch any errors
            {
                Cursor.Current = Cursors.Default;
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("115", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[MainForm]", "[GetLoadDetails]", $"FAILED: Code 115 ( {ex.Message} )");
            }
        }

        // Populate UI Elements -----------------------------------------------------------------------------------------------------------------------
        private void PopulateUI()
        {
            string querError = "SELECT * FROM TAFIE_Error_Results WHERE Session_Id = @Session_Id";
            string queryAPI = "SELECT * FROM TAFIE_API_Results WHERE Session_Id = @Session_Id";

            string error = "";
            string suggFix = "";
            string call = "";
            string resp = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // TAFIE_Error_Results
                    using (SqlCommand cmd = new SqlCommand(querError, conn))
                    {
                        cmd.Parameters.AddWithValue("@Session_Id", SessionMaintenance.sessionId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                error = reader["Error_Mess"].ToString();
                                suggFix = reader["Sugg_Fix"].ToString();
                            }
                        }
                    }

                    // TAFIE_API_Results
                    using (SqlCommand cmd = new SqlCommand(queryAPI, conn))
                    {
                        cmd.Parameters.AddWithValue("@Session_Id", SessionMaintenance.sessionId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                call = reader["API_Call"].ToString();
                                resp = reader["API_Resp"].ToString();
                            }
                        }
                    }
                }

                rtbxError.Text = error;
                rtbxFix.Text = suggFix;

                DisplayPrettyXml(call, rtbxCall);
                DisplayPrettyXml(resp, rtbxResp);
            }
            catch (Exception ex) // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("227", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[MainForm]", "[PopulateUI]", $"FAILED: Code 227 ( {ex.Message} )");
            }
        }

        // Do Search ------------------------------------------------------------------------------------------------------------------
        private void DoSearch(string loadNote, string client)
        {
            Cursor.Current = Cursors.WaitCursor;

            SessionMaintenance.LogBook("", "[MainForm]", "[DoSearch]", $"Search Started");

            btnSearch.Visible = true;
            btnExpand.Visible = true;
            btnClear.Visible = true;

            GetLoadDetails(loadNote, client, 0); // Executes TAFIE_Get_Lnot_Results
            GetLoadDetails(loadNote, client, 1); // Executes TAFIE_Get_Lnot_Line_Results
            GetLoadDetails(loadNote, client, 2); // Executes TAFIE_Get_Error_Results
            GetLoadDetails(loadNote, client, 3); // Executes TAFIE_Get_API_Results

            PopulateUI();

            btnFix.Visible = ShowbtnFix();
            btnGetLbl.Visible = ShowbtnFix();

            Cursor.Current = Cursors.Default;

            SessionMaintenance.LogBook("", "[MainForm]", "[DoSearch]", $"Search Finished");
        }

        // Format XML data ------------------------------------------------------------------------------------------------------------------
        public void DisplayPrettyXml(string xmlData, RichTextBox richTextBox)
        {
            try
            {
                // Load the XML data into an XmlDocument
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlData);

                // Create a StringWriter to capture the formatted output
                using (StringWriter stringWriter = new StringWriter())
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
                {
                    // Configure the XmlTextWriter to format the XML
                    xmlTextWriter.Formatting = Formatting.Indented;
                    xmlDoc.WriteTo(xmlTextWriter);

                    // Set the formatted XML to the RichTextBox
                    richTextBox.Text = stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                richTextBox.Text = $"Error formatting XML: {ex.Message}";
            }

            SessionMaintenance.LogBook("", "[MainForm]", "[DisplayPrettyXml]", $"Method Executed");
        }

        // Check is Fix button should show ------------------------------------------------------------------------------------------------------------------
        private bool ShowbtnFix()
        {
            bool show = false;
            int result = 0;
            string query = "SELECT ISNULL([TYPE], 0) as [1] " +
                "FROM intg_Errs " +
                "WHERE id = (SELECT ISNULL(Error_Id, 145) FROM TAFIE_Error_Results WHERE Session_Id = @Session_Id)";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Session_Id", SessionMaintenance.sessionId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                result = (int)reader["1"];
                            }
                        }
                    }
                    conn.Close();
                }

                switch (result)
                {
                    case 101: show = true; break;
                    case 102: show = true; break;
                    default: show = false; break;
                }

            }
            // Catch Errors
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("117", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[MainForm]", "[ShowbtnFix]", $"FAILED: Code 117 ( {ex.Message} )");
            }

            return show;
        }

        // Get Error field for fix form ------------------------------------------------------------------------------------------------------------------
        private string GetField()
        {
            string field = "";
            string query = "SELECT ISNULL([FIELD], '') as [1] " +
                "FROM intg_Errs " +
                "WHERE id = (SELECT ISNULL(Error_Id, 145) FROM TAFIE_Error_Results WHERE Session_Id = @Session_Id)";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Session_Id", SessionMaintenance.sessionId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                field = reader["1"].ToString();
                            }
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
                SessionMaintenance.LogBook("ERROR", "[MainForm]", "[GetField]", $"FAILED: Code 118 ( {ex.Message} )");
            }

            return field;
        }

        // Get Last Attempted Date ------------------------------------------------------------------------------------------------------------------
        private DateTime GetLastAtmpDateTime(string loadNote, string client)
        {
            DateTime dt = DateTime.Now;
            string query = "SELECT MAX(DT_Attempt) as [1] FROM TAFIE_Fix_Atmp WHERE Load_Note = @Load_Note AND Client = @Client";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Load_Note", loadNote);
                        cmd.Parameters.AddWithValue("@Client", client);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dt = (DateTime)reader["1"];
                            }
                        }
                    }
                    conn.Close();
                }

                lblFixAtmp.Visible = true;

            }
            // Catch Errors
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("118", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[MainForm]", "[GetLastAtmpDateTime]", $"FAILED: Code 118 ( {ex.Message} )");
            }

            return dt;

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

        // Expand Button ------------------------------------------------------------------------------------------------------------------
        private void btnExpand_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnExpand);
        }
        private void btnExpand_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnExpand);
        }

        // Clear Button ------------------------------------------------------------------------------------------------------------------
        private void btnClear_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnClear);
        }

        private void btnClear_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnClear);
        }

        // Fix Button ------------------------------------------------------------------------------------------------------------------
        private void btnFix_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnFix);
        }

        private void btnFix_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnFix);
        }

        // Get Label Button ------------------------------------------------------------------------------------------------------------------
        private void btnGetLbl_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnGetLbl);
        }

        private void btnGetLbl_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnGetLbl);
        }

        // Load Note Field ------------------------------------------------------------------------------------------------------------------
        private void txbLoadNote_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbLoadNote);
            SessionMaintenance.LogBook("", "[MainForm]", "[txbLoadNote_Enter]", $"Field Entered");
        }

        private void txbLoadNote_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbLoadNote);
            SessionMaintenance.LogBook("", "[MainForm]", "[txbLoadNote_Enter]", $"Field Left");
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

        // Client Field ------------------------------------------------------------------------------------------------------------------
        private void cbClient_TextChanged(object sender, EventArgs e)
        {
            ClearFields();
        }

        // API Response Field ------------------------------------------------------------------------------------------------------------------
        private void rtbxResp_MouseEnter(object sender, EventArgs e)
        {
            ttAPI.SetToolTip(rtbxResp, "Double click for full view.");
        }

        // API Call Field ------------------------------------------------------------------------------------------------------------------
        private void rtbxCall_MouseEnter(object sender, EventArgs e)
        {
            ttAPI.SetToolTip(rtbxCall, "Double click for full view.");
        }

        // Fix button Visability ------------------------------------------------------------------------------------------------------------------
        private void btnFix_VisibleChanged(object sender, EventArgs e)
        {
            if (btnFix.Visible)
            {
                rtbxFix.Size = new Size(580, 96);
            }
            else
            {
                rtbxFix.Size = new Size(678, 96);
            }
        }

        //=============================================================================================================================================================================================
        //-- Button Click Events --//
        //=============================================================================================================================================================================================

        // Exit Button ------------------------------------------------------------------------------------------------------------------
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Search Button ------------------------------------------------------------------------------------------------------------------
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string loadNote = txbLoadNote.Text;
            string client = "";

            CustomMessageBox messageBox = new CustomMessageBox();

            if (cbClient.SelectedItem != null)
            {
                client = cbClient.SelectedItem.ToString();
            }

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
                int status = CheckLoadNote(loadNote, client);

                switch (status)
                {
                    case 0:
                        messageBox.ShowError($"No Integration error found for load note: {loadNote} " +
                            $"\nThis means the API has not attempted to get a label" +
                            $"\n\nTroubleshoot:" +
                            $"\n-Check the configuration settings for the packing desk or move to another packing desk." +
                            $"\n-Check unsupported characters in the customer's email or phone number fields." +
                            $"\n-Escalate to a Supervisor.");
                        ClearFields();
                        break;
                    case 1:
                        DoSearch(loadNote, client);
                        break;
                    case 2:
                        messageBox.ShowError($"Load note: {loadNote} is no longer open.");
                        ClearFields();
                        break;
                    case 3:
                        messageBox.ShowWarning($"A fix has already been attempted for this load note, please double check the error is still occuring.");
                        lblFixAtmp.Text = $"Fix Last Attempted: {GetLastAtmpDateTime(loadNote, client)}";
                        DoSearch(loadNote, client);
                        break;
                    case 4:
                        messageBox.ShowWarning($"A Carrier label has been created for Load note: {loadNote} using the TAFIE application");
                        DoSearch(loadNote, client);
                        break;

                }
            }
        }

        // Expand Button ------------------------------------------------------------------------------------------------------------------
        private void btnExpand_Click(object sender, EventArgs e)
        {
            ExpandForm expandForm = new ExpandForm();
            //expandForm.sessionId = SessionMaintenance.sessionId;
            expandForm.Show();
        }

        // Clear Button ------------------------------------------------------------------------------------------------------------------
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        // Call XML DoubleClick ------------------------------------------------------------------------------------------------------------------
        private void rtbxCall_DoubleClick(object sender, EventArgs e)
        {
            xmlView view = new xmlView();
            view.xmlString = rtbxCall.Text;
            view.Show();
        }

        // Reponse XML DoubleClick ------------------------------------------------------------------------------------------------------------------
        private void rtbxResp_DoubleClick(object sender, EventArgs e)
        {
            xmlView view = new xmlView();
            view.xmlString = rtbxResp.Text;
            view.Show();
        }

        // Fix Button ------------------------------------------------------------------------------------------------------------------
        private void btnFix_Click(object sender, EventArgs e)
        {
            string client = "";

            if (cbClient.SelectedItem != null)
            {
                client = cbClient.SelectedItem.ToString();
            }

            FixForm fixForm = new FixForm();
            fixForm.updateField = GetField();
            fixForm.suggFix = rtbxFix.Text;
            fixForm.client = client;
            fixForm.Show();
        }

        // Get Label Button ------------------------------------------------------------------------------------------------------------------
        private void btnGetLbl_Click(object sender, EventArgs e)
        {
            string client = "";
            string loadNote = txbLoadNote.Text;

            if (cbClient.SelectedItem != null)
            {
                client = cbClient.SelectedItem.ToString();
            }

            CarrierForm carrierForm = new CarrierForm();
            carrierForm.passedClient = client;
            carrierForm.passedLoadNote = loadNote;
            carrierForm.Show();
        }

        //=============================================================================================================================================================================================
        //-- Key Down Events --//
        //=============================================================================================================================================================================================

        // Keybord Shortcuts ------------------------------------------------------------------------------------------------------------------
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            // ctrl + G
            if (e.Control && e.KeyCode == Keys.G)
            {
                cbClient.SelectedItem = null;
                ClearFields();
            }

            // Esc
            if (e.KeyCode == Keys.Escape)
            {
                btnExit_Click(sender, e);
            }
        }


    }
}
