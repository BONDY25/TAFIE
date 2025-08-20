using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TAFIE
{
    public partial class CarrierCombos : Form
    {
        //=============================================================================================================================================================================================
        //-- Initialization --//
        //=============================================================================================================================================================================================

        public string passedClient { get; set; }
        private const string connectionString = SessionMaintenance.connectionString;

        public CarrierCombos()
        {
            InitializeComponent();
        }

        private void CarrierCombos_Load(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierCombos]", "[FormLoad]", $"Form Started");
            Text = $"{Environment.UserName.ToUpper()} - Carrier Combos";
            lblTitle.Text = $"Carrier Combos - {passedClient}";
            GetCombos();
        }

        //=============================================================================================================================================================================================
        //-- Operational Methods --//
        //=============================================================================================================================================================================================

        // Get Carrier Combos ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void GetCombos()
        {
            string query = "EXECUTE [TCAR_Get_Combos] @Client";

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
                        cmd.Parameters.AddWithValue("@Client", passedClient);

                        // Execute Query
                        SqlDataReader reader = cmd.ExecuteReader();

                        // Populate DataTable From Reader
                        dataTable.Load(reader);
                    }

                    conn.Close(); // Close SQL Connection

                    // Populate Data Grid
                    dgCarrier.DataSource = dataTable;
                    dgCarrier.Refresh();
                }
            }
            catch (Exception ex) // Catch Errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("117", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[GetCombos]", "[PopulateDataGrid]", $"FAILED: Code 117 ( {ex.Message} )");
            }
        }

        //=============================================================================================================================================================================================
        //-- Enviroment Box Events --//
        //=============================================================================================================================================================================================

        // Close Button ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnClose);
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnClose);
        }

        //=============================================================================================================================================================================================
        //-- Button Click Events --//
        //=============================================================================================================================================================================================

        // Close Button ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void btnClose_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierCombos]", "[FormClose]", $"Form Closed");
            this.Close();
        }


    }
}
