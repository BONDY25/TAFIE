using Microsoft.Data.SqlClient;
using System.Data;

namespace TAFIE
{
    public partial class RePrint : Form
    {
        //=============================================================================================================================================================================================
        //-- Initialization --//
        //=============================================================================================================================================================================================

        private const string connectionString = SessionMaintenance.connectionString;

        public RePrint()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        // Form Load ------------------------------------------------------------------------------------------------------------------
        private void RePrint_Load(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[RePrint]", "[FormLoad]", $"Form Started");
            Text = $"{Environment.UserName.ToUpper()} - {SessionMaintenance.appName} Reprint Label";
        }


        //=============================================================================================================================================================================================
        //-- Operational Methods --//
        //=============================================================================================================================================================================================

        // Populate Datagrid ------------------------------------------------------------------------------------------------------------------
        private void PopulateDataGrid(string search)
        {
            string query = "EXECUTE [TCAR_Rpnt_Srch] @Session_Id, @Search";

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
                        cmd.Parameters.AddWithValue("@Session_Id", SessionMaintenance.sessionId);
                        cmd.Parameters.AddWithValue("@Search", search);

                        // Execute Query
                        cmd.ExecuteNonQuery();

                        // Execute Data Reader
                        SqlDataReader reader = cmd.ExecuteReader();

                        // Populate DataTable From Reader
                        dataTable.Load(reader);
                    }

                    conn.Close(); // Close SQL Connection

                    // Populate Data Grid
                    dgReprint.DataSource = dataTable;
                    dgReprint.Refresh();
                }
            }
            catch (Exception ex) // Catch Errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("117", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[RePrint]", "[PopulateDataGrid]", $"FAILED: Code 117 ( {ex.Message} )");
            }
        }

        // Get XML ------------------------------------------------------------------------------------------------------------------
        private string GetXML(string tracking)
        {
            string xml = "PLACEHOLDER";
            string query = "SELECT ISNULL([XML_MESSAGE], 'NO DATA') FROM [TCAR_Srch] WHERE Session_Id = @Session_Id AND Tracking = @Tracking";

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {

                        cmd.Parameters.AddWithValue("@Session_Id", SessionMaintenance.sessionId);
                        cmd.Parameters.AddWithValue("@Tracking", tracking);

                        SessionMaintenance.LogBook("", "[RePrint]", "[GetXML]", $"Parameters set: [{SessionMaintenance.sessionId}], [{tracking}]");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                xml = reader[0].ToString(); // Access by column index
                                SessionMaintenance.LogBook("", "[RePrint]", "[GetXML]", $"Data found in reader");
                            }
                        }
                    }
                    conn.Close(); // Close SQL Connection
                }
            }
            catch (Exception ex) // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("230", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[RePrint]", "[GetXML]", $"FAILED Code: 230 ( {ex.Message} )");
            }

            SessionMaintenance.LogBook("", "[RePrint]", "[GetXML]", $"XML Retrieved:\n{xml}");
            return xml;
        }

        // Update TCAR ------------------------------------------------------------------------------------------------------------------
        private void UpdateTCAR(string tracking)
        {
            string query = "EXECUTE [TCAR_Rpnt] @Tracking, @User";

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Tracking", tracking);
                        cmd.Parameters.AddWithValue("@User", SessionMaintenance.userName);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close(); // Close SQL Connection
                }
            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured updating shipment records \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[RePrint]", "[UpdateTCAR]", $"FAILED: Code 226 ( {ex.Message} )");
            }
        }


        // Reprint Label ------------------------------------------------------------------------------------------------------------------
        private void RePrintLabel(string xml)
        {
            List<string> labelFiles = WcmsApi.ExtractLabelData(xml);

            if (labelFiles == null || labelFiles.Count == 0)
            {
                labelFiles = ProCarrApi.ExtractLabelDataJson(xml);
                if (labelFiles == null || labelFiles.Count == 0)
                {
                    SessionMaintenance.LogBook("ERROR", "[RePrint]", "[RePrintLabel]", "Failed to extract any labels.");
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowError("No label data found in API response.");
                    return;
                }
            }

            // Print all labels
            foreach (var labelFile in labelFiles)
            {
                TcarControl.PrintPdf(labelFile);
            }
        }

        // Reprint Function ------------------------------------------------------------------------------------------------------------------
        private void ReprintFunction()
        {
            SessionMaintenance.LogBook("", "[RePrint]", "[ReprintFunction]", $"Reprint attempted");

            if (dgReprint.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgReprint.SelectedRows[0];

                string tracking = selectedRow.Cells[2].Value.ToString();

                RePrintLabel(GetXML(tracking));

                SessionMaintenance.LogBook("", "[RePrint]", "[ReprintFunction]", $"Carrier Label Re-Printed: {tracking}, {SessionMaintenance.userName}, {SessionMaintenance.sessionId}");

                UpdateTCAR(tracking);
            }
            else
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("205", $"");
                SessionMaintenance.LogBook("", "[RePrint]", "[ReprintFunction]", "Error Triggered: 205");
                return;
            }
        }

        //=============================================================================================================================================================================================
        //-- Enviroment Box Events --//
        //=============================================================================================================================================================================================

        // Search Field ------------------------------------------------------------------------------------------------------------------
        private void txbSearch_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbSearch);
        }

        private void txbSearch_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbSearch);
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

        // Exit Button ------------------------------------------------------------------------------------------------------------------
        private void btnExit_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnExit);
        }

        private void btnExit_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnExit);
        }

        // Reprint Button ------------------------------------------------------------------------------------------------------------------
        private void btnReprint_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnReprint);
        }

        private void btnReprint_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnReprint);
        }

        //=============================================================================================================================================================================================
        //-- Button Click Events --//
        //=============================================================================================================================================================================================

        // Exit Button ------------------------------------------------------------------------------------------------------------------
        private void btnExit_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[RePrint]", "[FormClosing]", $"Form Closed");
            this.Close();
        }

        // Search Button ------------------------------------------------------------------------------------------------------------------
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txbSearch.Text;

            dgReprint.DataSource = null;
            dgReprint.Refresh();

            PopulateDataGrid(search);

            txbSearch.Text = "";
        }

        // Reprint Button ------------------------------------------------------------------------------------------------------------------
        private void btnReprint_Click(object sender, EventArgs e)
        {
            ReprintFunction();
        }

        //=============================================================================================================================================================================================
        //-- Key Down Events --//
        //=============================================================================================================================================================================================

    }
}
