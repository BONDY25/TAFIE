using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace TAFIE
{
    public partial class ClientManager : Form
    {
        //=============================================================================================================================================================================================
        //-- Initialization --//
        //=============================================================================================================================================================================================

        private const string connectionString = SessionMaintenance.connectionString;

        private CarrierForm carrierForm;

        public ClientManager(CarrierForm carrierForm)
        {
            InitializeComponent();
            this.carrierForm = carrierForm;
        }

        // Form Load -----------------------------------------------------------------------------------------------------------------------
        private void ClientManager_Load(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[ClientManager]", "[FormLoad]", $"Form Started");
            Text = $"{Environment.UserName.ToUpper()} - Client Manager";
            PopulateComboBoxes(cbClient, "CLIENT");
            PopulateComboBoxes(cbAccCode, "AccCode");

            lblAccCode.Visible = false;
            lblEORI.Visible = false;
            lblIOSS.Visible = false;

            txbEORI.Visible = false;
            txbIOSS.Visible = false;
            cbAccCode.Visible = false;

            ClientSize = new Size(374, 282);
            lblUpdate.Visible = false;

            btnSave.Visible = false;
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
                query = "SELECT [Description] FROM TAFIE_Clients ORDER BY [Description]";
            }
            else if (field == "AccCode")
            {
                query = "SELECT DISTINCT Acc_Code as [Description] FROM TCAR_Acct ORDER BY [Description]";
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
                            if (field == "AccCode")
                            {
                                comboBox.Items.Add("None");
                            }
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
                SessionMaintenance.LogBook("ERROR", "[ClientManager]", "[PopulateComboBoxes]", $"FAILED: Code 112 ( {ex.Message} )");
                SessionMaintenance.LogBook("ERROR", "[ClientManager]", "[PopulateComboBoxes]", "Application Closed");
                Application.Exit();
            }
        }

        // Get Client -----------------------------------------------------------------------------------------------------------------------
        private void GetClient()
        {
            string query = "EXECUTE [TCAR_Get_Client] @Client";
            string client = "";

            if (cbClient.SelectedItem != null)
            {
                client = cbClient.SelectedItem.ToString();
            }

            try
            {
                // Execute SQL Command
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Client", client);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txbIOSS.Text = reader["IOSS"].ToString();
                                txbEORI.Text = reader["EORI"].ToString();
                                cbAccCode.SelectedItem = reader["AccCode"].ToString();
                                chkActive.Checked = Convert.ToBoolean(reader["Active"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured getting Client Details \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[ClientManager]", "[GetClient]", $"FAILED: Code 226 ( {ex.Message} )");
            }
        }

        // Load Client -----------------------------------------------------------------------------------------------------------------------
        private void LoadClient()
        {
            // Check if Active show fields
            if (chkActive.Checked)
            {
                lblAccCode.Visible = true;
                lblEORI.Visible = true;
                lblIOSS.Visible = true;

                txbEORI.Visible = true;
                txbIOSS.Visible = true;
                cbAccCode.Visible = true;

                ClientSize = new Size(374, 408);
                lblUpdate.Visible = false;                
            }

            // Else hide fields
            else
            {
                lblAccCode.Visible = false;
                lblEORI.Visible = false;
                lblIOSS.Visible = false;

                txbEORI.Visible = false;
                txbIOSS.Visible = false;
                cbAccCode.Visible = false;

                ClientSize = new Size(374, 282);
                lblUpdate.Visible = false;
            }
        }

        // Save Client -----------------------------------------------------------------------------------------------------------------------
        private void SaveClient()
        {
            string query = "EXECUTE [TCAR_Save_Client] @Client, @IOSS, @EORI, @AccCode, @Active, @User";
            string client = !string.IsNullOrEmpty(cbClient.Text) ? cbClient.Text : "";
            string ioss = txbIOSS.Text ?? "";
            string eori = txbEORI.Text ?? "";
            string accCode = !string.IsNullOrEmpty(cbAccCode.Text) || cbAccCode.Text == "None" ? cbAccCode.Text : "";
            bool active = chkActive.Checked;

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Client", client);
                        cmd.Parameters.AddWithValue("@IOSS", ioss);
                        cmd.Parameters.AddWithValue("@EORI", eori);
                        cmd.Parameters.AddWithValue("@AccCode", accCode);
                        cmd.Parameters.AddWithValue("@Active", active);
                        cmd.Parameters.AddWithValue("@User", SessionMaintenance.userName);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close(); // Close SQL Connection
                }

                SessionMaintenance.LogBook("", "[ClientManager]", "[SaveClient]", $"Client Details Updated");

                // Update UI
                lblUpdate.Visible = true;
                lblUpdate.Text = $"Client Updated @ {DateTime.Now}";

                // Show Message Box
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowInfo($"Client Details Updated");

                carrierForm.PopulateComboBoxes(carrierForm.cbClient, "CLIENT");

            }
            catch (Exception ex) // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured saving Client Details \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[ClientManager]", "[SaveClient]", $"FAILED: Code 226 ( {ex.Message} )");
            }
        }

        //=============================================================================================================================================================================================
        //-- Enviroment Box Events --//
        //=============================================================================================================================================================================================

        // Close Button -----------------------------------------------------------------------------------------------------------------------
        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnClose);
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnClose);
        }

        // Save Button -----------------------------------------------------------------------------------------------------------------------
        private void btnSave_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnSave);
        }

        private void btnSave_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnSave);
        }

        // Client Field -----------------------------------------------------------------------------------------------------------------------
        private void cbClient_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(cbClient);
        }

        private void cbClient_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(cbClient);

        }

        private void cbClient_TextChanged(object sender, EventArgs e)
        {
            GetClient();
            LoadClient();
        }

        // Account Code Field -----------------------------------------------------------------------------------------------------------------------
        private void cbAccCode_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(cbAccCode);
        }

        private void cbAccCode_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(cbAccCode);
        }

        // IOSS Field -----------------------------------------------------------------------------------------------------------------------
        private void txbIOSS_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbIOSS);
        }

        private void txbIOSS_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbIOSS);
        }

        // EORI Field -----------------------------------------------------------------------------------------------------------------------
        private void txbEORI_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbEORI);
        }

        private void txbEORI_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbEORI);
        }

        // Active Field -----------------------------------------------------------------------------------------------------------------------
        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbClient.Text))
            {
                LoadClient();
                btnSave.Visible = true;
            }
        }

        //=============================================================================================================================================================================================
        //-- Button Click Events --//
        //=============================================================================================================================================================================================

        // Save Button -----------------------------------------------------------------------------------------------------------------------
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbClient.Text))
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowInfo($"Client Details not Selected");
                return;
            }
            else
            {
                SaveClient();
            }
        }

        // Close Button -----------------------------------------------------------------------------------------------------------------------
        private void btnClose_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[ClientManager]", "[FormClosing]", $"Form Closed");
            this.Close();
        }


    }
}
