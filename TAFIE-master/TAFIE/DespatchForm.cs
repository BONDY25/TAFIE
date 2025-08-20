using Microsoft.Data.SqlClient;
using System.Data;

namespace TAFIE
{
    public partial class DespatchForm : Form
    {
        //=============================================================================================================================================================================================
        //-- Initialization --//
        //=============================================================================================================================================================================================

        public string tcarRef { get; set; }
        public int qtyScanned = 0;
        public int qtyToScan = 0;
        public DataTable dataTable = new DataTable();
        private CarrierForm carrierForm;


        private const string connectionString = SessionMaintenance.connectionString;
        public DespatchForm(CarrierForm carrierForm)
        {
            InitializeComponent();
            this.carrierForm = carrierForm;
        }

        private void DespatchForm_Load(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[DespatchForm]", "[FormLoad]", $"Form Started");
            PopulateDataGrid(1);
            lblLastScan.Text = "";
        }

        //=============================================================================================================================================================================================
        //-- Operational Methods --//
        //=============================================================================================================================================================================================

        // Populate DataGridView with Results from SQL Server -------------------------------------------------------------------
        private void PopulateDataGrid(int mode = 0)
        {
            string query = "EXECUTE [TCAR_Get_Desp] @TCAR_Ref, @Mode";

            dgContents.DataSource = null; // Clear Data Grid
            dataTable.Clear(); // Clear Data Table

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
                        cmd.Parameters.AddWithValue("@Mode", mode);

                        // Execute Query
                        SqlDataReader reader = cmd.ExecuteReader();

                        // Populate DataTable From Reader
                        dataTable.Load(reader);

                        qtyToScan = dataTable.AsEnumerable()
                         .Where(row => !row.IsNull(8))
                         .Sum(row => row.Field<int>(8));

                        SessionMaintenance.LogBook("", "[DespatchForm]", "[PopulateDataGrid]", $"Qty Scanned:{qtyScanned}/{qtyToScan}");
                    }

                    conn.Close(); // Close SQL Connection

                    if (dataTable.Rows.Count == 0)
                    {
                        CustomMessageBox messageBox = new CustomMessageBox();
                        messageBox.ShowError("Load note not yet issued!");
                        SessionMaintenance.LogBook("", "[DespatchForm]", "[PopulateDataGrid]", $"Form Closing, Load note not yet issued! {tcarRef}");
                        this.Close();
                        return;
                    }

                    // Populate Data Grid
                    dgContents.DataSource = dataTable;

                    for (int i = 0; i < dgContents.Columns.Count; i++)
                    {
                        if (i < 6 || i > 9)
                        {
                            dgContents.Columns[i].Visible = false;
                        }
                    }

                    dgContents.Refresh();
                }
            }
            catch (Exception ex) // Catch Errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("117", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[DespatchForm]", "[PopulateDataGrid]", $"FAILED: Code 117 ( {ex.Message} )");
            }
        }

        // Check Barcode -------------------------------------------------------------------
        private void CheckBarcode(string bcode)
        {
            SessionMaintenance.LogBook("", "[DespatchForm]", "[CheckBarcode]", $"Checking barcode:{bcode}");
            string query = "EXECUTE [TCAR_Check_Bcode] @TCAR_Ref, @Bcode";
            int scanned = 0;

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
                        cmd.Parameters.AddWithValue("@Bcode", bcode);

                        // Execute Query
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                scanned = reader.GetInt32(0);
                                SessionMaintenance.LogBook("", "[DespatchForm]", "[CheckBarcode]", $"Check result: {scanned}");
                            }
                        }

                        if (scanned > 0) // If Barcode Found
                        {
                            qtyScanned += scanned; // Increment Scanned Quantity                                                        
                        }
                        else
                        {
                            CustomMessageBox messageBox = new CustomMessageBox();
                            messageBox.ShowError($"Barcode {bcode} not found or part no required");
                        }
                    }

                    conn.Close(); // Close SQL Connection

                    PopulateDataGrid(); // Refresh Data Grid
                }
            }
            catch (Exception ex) // Catch Errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("119", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[DespatchForm]", "[CheckBarcode]", $"FAILED: Code 119 ( {ex.Message} )");
            }
        }

        // Check scanned quantities -------------------------------------------------------------------
        private bool CheckScannedQty()
        {
            SessionMaintenance.LogBook("", "[DespatchForm]", "[CheckScannedQty]", $"Checking scanned qtys: {qtyScanned}/{qtyToScan}");
            bool isReadyForLabel = false;
            if (qtyScanned == qtyToScan)
            {
                CarrierForm.scanned = 1;

                CustomMessageBox messageBox = new CustomMessageBox();
                bool changes = messageBox.ShowQuestion("All Items Scanned", "All products have been scanned, are you ready for a label?");
                if (changes)
                {
                    SessionMaintenance.LogBook("", "[DespatchForm]", "[CheckScannedQty]", $"All items scanned, ready for label");
                    isReadyForLabel = true; // Ready for label
                }
                else
                {
                    SessionMaintenance.LogBook("", "[DespatchForm]", "[CheckScannedQty]", $"Not ready for label");
                    isReadyForLabel = false; // Not ready for label
                }
            }
            return isReadyForLabel; // Not ready for label
        }

        // verify barcode -------------------------------------------------------------------
        private void VerifyBarcode(string bcode)
        {
            SessionMaintenance.LogBook("", "[DespatchForm]", "[VerifyBarcode]", $"Verifying barcode {bcode}");
            CheckBarcode(bcode);
            if (CheckScannedQty())
            {
                SessionMaintenance.LogBook("", "[DespatchForm]", "[VerifyBarcode]", $"{qtyScanned} = {qtyToScan}, start print label.");
                carrierForm.PrintLabel(0);
                this.Close();
            }
        }

        //=============================================================================================================================================================================================
        //-- Enviroment Box Events --//
        //=============================================================================================================================================================================================

        // Barcode Field ----------------------------------------------------------------------
        private void txbBcode_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbBcode);
        }

        private void txbBcode_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbBcode);
            if (!string.IsNullOrEmpty(txbBcode.Text))
            {
                lblLastScan.Text = $"Last Scan: {txbBcode.Text.Trim()}";
                VerifyBarcode(txbBcode.Text.Trim());
                txbBcode.Clear(); // Clear the barcode field after processing
                txbBcode.Focus(); // Set focus back to the barcode field
            }
        }

        // Exit Button ----------------------------------------------------------------------   
        private void btnExit_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnExit);
        }

        private void btnExit_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnExit);
        }

        //=============================================================================================================================================================================================
        //-- Button Click Events --//
        //=============================================================================================================================================================================================

        // Exit Button -------------------------------------------------------------------------------------------------------------------------
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (qtyScanned != qtyToScan)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                bool exit = messageBox.ShowQuestion("Are you sure?", "You have not scanned all items. Are you sure you want to exit?");
                if (exit)
                {
                    SessionMaintenance.LogBook("", "[DespatchForm]", "[FormLoad]", $"Form Closed");
                    this.Close();
                }
                else
                {
                    return;
                }
            }
            else
            {
                SessionMaintenance.LogBook("", "[DespatchForm]", "[FormLoad]", $"Form Closed");
                this.Close();
            }
        }
    }
}
