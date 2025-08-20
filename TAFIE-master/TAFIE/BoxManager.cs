using Microsoft.Data.SqlClient;
using System.Data;

namespace TAFIE
{
    public partial class BoxManager : Form
    {
        //=============================================================================================================================================================================================
        //-- Initialization --//
        //=============================================================================================================================================================================================

        public string tcarRef { get; set; }

        public int boxQty = 1;

        public Action<DataTable> OnBoxDataSaved; // This will be set by the parent form

        private const string connectionString = SessionMaintenance.connectionString;
        public BoxManager()
        {
            InitializeComponent();
        }

        private void BoxManager_Load(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[BoxManager]", "[FormLoad]", $"Form Started");
            Text = $"{Environment.UserName.ToUpper()} - {SessionMaintenance.appName} Box Manager";
            dgContents.Visible = false;
            txbBoxQty.Text = boxQty.ToString();
        }

        //=============================================================================================================================================================================================
        //-- Operational Methods --//
        //=============================================================================================================================================================================================

        // Populate DataGridView with Results from SQL Server -------------------------------------------------------------------
        private void PopulateDataGrid()
        {
            string query = "EXECUTE [TCAR_Get_Results] @TCAR_Ref, 5";

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
                }
            }
            catch (Exception ex) // Catch Errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("117", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[PopulateDataGrid]", $"FAILED: Code 117 ( {ex.Message} )");
            }
        }

        // Populate DataGridView ComboBox Column with Box Numbers -------------------------------------------------------------
        private void populateDgComboBox()
        {
            if (boxQty <= 0)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("Please enter a valid positive number of boxes.");
                return;
            }

            // Generate list of box numbers as strings
            List<string> boxNumbers = Enumerable.Range(1, boxQty).Select(i => i.ToString()).ToList();

            // Check that the BoxNo column exists and is a combo box column
            if (dgContents.Columns["BoxNo"] is DataGridViewComboBoxColumn comboBoxColumn)
            {
                // Clear any existing items in the column template
                comboBoxColumn.Items.Clear();
                comboBoxColumn.Items.AddRange(boxNumbers.ToArray());

                // Populate each row's cell with a combo box if not already
                foreach (DataGridViewRow row in dgContents.Rows)
                {
                    if (row.IsNewRow) continue;

                    // Set the cell value to null or default
                    var cell = row.Cells["BoxNo"] as DataGridViewComboBoxCell;
                    if (cell == null)
                    {
                        cell = new DataGridViewComboBoxCell();
                        row.Cells["BoxNo"] = cell;
                    }

                    cell.DataSource = new List<string>(boxNumbers); // fresh list to prevent reference issues
                }
            }
            else
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("BoxNo column is not a ComboBox column.");
                return;
            }
        }

        // Save Data from DataGridView to DataTable -------------------------------------------------------------------
        private DataTable SaveDataGrid()
        {
            DataTable dt = new DataTable();

            try
            {
                // Define columns
                dt.Columns.Add("BoxNo", typeof(string));
                dt.Columns.Add("Part", typeof(string));
                dt.Columns.Add("Qty", typeof(string));

                foreach (DataGridViewRow row in dgContents.Rows)
                {
                    if (row.IsNewRow) continue;

                    DataRow dr = dt.NewRow();

                    // Assuming correct cell names/indices
                    dr["BoxNo"] = row.Cells["BoxNo"].Value?.ToString();
                    dr["Part"] = row.Cells["Part"].Value?.ToString();
                    dr["Qty"] = row.Cells["Qty"].Value?.ToString();

                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex) // Catch Errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("Error Saving Data");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[SaveDataGrid]", $"FAILED: Code 119 ( {ex.Message} )");
            }

            return dt;
        }        

        //=============================================================================================================================================================================================
        //-- Enviroment Box Events --//
        //=============================================================================================================================================================================================

        // Box Quantity TextBox ------------------------------------------------------------------------------------------------
        private void txbBoxQty_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbBoxQty);
        }

        private void txbBoxQty_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbBoxQty);
            if (int.TryParse(txbBoxQty.Text, out int qty) && qty > 0)
            {
                boxQty = qty;
                dgContents.Visible = true; // Show DataGridView if boxQty is valid
                PopulateDataGrid(); // Populate the DataGridView with data
                populateDgComboBox();
                txbBoxQty.ReadOnly = true;// Make the TextBox read-only after entering a valid value
                txbBoxQty.Enabled = false;
            }
            else
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("Please enter a valid positive number of boxes.");
                txbBoxQty.Text = boxQty.ToString(); // Reset to previous valid value
                txbBoxQty.ReadOnly = false;
                txbBoxQty.Enabled = true;
                return;
            }
        }

        // Exit Button ------------------------------------------------------------------------------------------------
        private void btnExit_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnExit);
        }

        private void btnExit_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnExit);
        }

        // Save Button ------------------------------------------------------------------------------------------------
        private void btnSave_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnSave);
        }

        private void btnSave_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnSave);
        }

        //=============================================================================================================================================================================================
        //-- Button Click Events --//
        //=============================================================================================================================================================================================

        // Exit Button ------------------------------------------------------------------------------------------
        private void btnExit_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[BoxManager]", "[btnExit_Click]", $"Exiting Box Manager");
            this.Close();
        }

        // Save Button ------------------------------------------------------------------------------------------
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check all comboboxes have a value
            foreach (DataGridViewRow row in dgContents.Rows)
            {
                if (row.IsNewRow) continue;

                var cell = row.Cells["BoxNo"] as DataGridViewComboBoxCell;
                if (cell == null || cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowError("Please ensure all BoxNo fields are filled.");
                    return;
                }
            }

            // Save Data from DataGridView to DataTable
            DataTable dt = SaveDataGrid();
            if (dt.Rows.Count == 0)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("No data to save. Please ensure the DataGridView is populated.");
                return;
            }
            // Insert Data from DataTable to SQL Server
            TcarControl.InsertDataGrid(dt, SessionMaintenance.userName);

            SessionMaintenance.LogBook("", "[BoxManager]", "[btnSave_Click]", $"Data saved successfully for TCAR Ref: {tcarRef}");

            OnBoxDataSaved?.Invoke(dt);

            this.Close();
            // carry on printing label// 
        }
    }
}
