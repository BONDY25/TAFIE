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
    public partial class ExpandForm : Form
    {
        //=============================================================================================================================================================================================
        //-- Initialization --//
        //=============================================================================================================================================================================================

        private const string connectionString = SessionMaintenance.connectionString;

        public ExpandForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        // Form Load ------------------------------------------------------------------------------------------------------------------
        private void ExpandForm_Load(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[ExpandForm]", "[FormLoad]", $"Form Started");
            Text = $"{Environment.UserName.ToUpper()} - {SessionMaintenance.appName} Load Note Details";
            PopulateUI();
            PopulateTable();
        }

        // Form Closing ------------------------------------------------------------------------------------------------------------------
        private void ExpandForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SessionMaintenance.LogBook("", "[ExpandForm]", "[FormClosing]", $"Form Closed");
        }

        //=============================================================================================================================================================================================
        //-- Operational Methods --//
        //=============================================================================================================================================================================================

        //Populate Table -----------------------------------------------------------------------------------------------------------------------
        private void PopulateTable()
        {
            string query = "SELECT " +
                "[Line]" +
                ",[Part]" +
                ",[Description]" +
                ",[qty],[Price]" +
                ",[Line_Total]" +
                ",[Status]" +
                ",RTRIM([Item_Weight]) + 'g' as [Item_Weight]"+
                ",[HS_Code]" +
                ",[Coo]" +
                "FROM TAFIE_Lnot_Line_Results " +
                "WHERE Session_Id = @Session_Id";

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

                        // Execute Query
                        cmd.ExecuteNonQuery();

                        // Execute Data Reader
                        SqlDataReader reader = cmd.ExecuteReader();

                        // Populate DataTable From Reader
                        dataTable.Load(reader);
                    }

                    conn.Close(); // Close SQL Connection

                    // Populate Data Grid
                    dgLines.DataSource = dataTable;
                    dgLines.Refresh();
                }
            }
            catch (Exception ex) // Catch Errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("117", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[ExpandForm]", "[PopulateUI]", $"FAILED: Code 117 ( {ex.Message} )");
            }
        }

        //Populate UI Elements -----------------------------------------------------------------------------------------------------------------------
        private void PopulateUI()
        {
            string queryLnot = "SELECT * FROM TAFIE_Lnot_Results WHERE Session_Id = @Session_Id";

            string loadNote = "";
            string refNo = "";
            DateTime? orderDate = null;
            DateTime? printDate = null;
            string lines = "";
            string units = "";
            string weight = "";
            string totalVal = "";
            string carrier = "";
            string DelMeth = "";
            string callAtps = "";
            string status = "";
            string apiAccoutNo = "";
            string apiUsername = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // TAFIE_Lnot_Results
                    using (SqlCommand cmd = new SqlCommand(queryLnot, conn))
                    {
                        cmd.Parameters.AddWithValue("@Session_Id", SessionMaintenance.sessionId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                loadNote = reader["Load_Note"].ToString();
                                refNo = reader["Ref_No"].ToString();
                                if (!reader.IsDBNull(reader.GetOrdinal("Order_Date")))
                                    orderDate = reader.GetDateTime(reader.GetOrdinal("Order_Date"));
                                if (!reader.IsDBNull(reader.GetOrdinal("Print_Date")))
                                    printDate = reader.GetDateTime(reader.GetOrdinal("Print_Date"));
                                lines = reader["Lines"].ToString();
                                units = reader["Units"].ToString();
                                weight = reader["Weight"].ToString();
                                totalVal = $"{reader["Total_Value"]:F2}";
                                carrier = reader["Carrier"].ToString();
                                DelMeth = reader["Del_Method"].ToString();
                                callAtps = reader["Call_Attempts"].ToString();
                                status = reader["Load_Status"].ToString();
                                apiAccoutNo = reader["API_Account_No"].ToString();
                                apiUsername = reader["API_Username"].ToString();
                            }
                        }
                    }
                }

                txbLoad.Text = loadNote;
                txbRefNo.Text = refNo;
                txbOrderDate.Text = orderDate.ToString();
                txbPrintDate.Text = printDate.ToString();
                txbLines.Text = lines;
                txbUnits.Text = units;
                txbWeight.Text = weight;
                txbTotVal.Text = $"{totalVal}";
                txbCarrier.Text = carrier;
                txbDelMeth.Text = DelMeth;
                txbCallAtps.Text = callAtps;
                txbStatus.Text = status;
                txbApiAccount.Text = apiAccoutNo;
                txbApiUsername.Text = apiUsername;

            }
            catch (Exception ex) // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("227", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[ExpandForm]", "[PopulateUI]", $"FAILED: Code 227 ( {ex.Message} )");
            }
        }
        //=============================================================================================================================================================================================
        //-- Enviroment Box Events --//
        //=============================================================================================================================================================================================

        // Close Button ------------------------------------------------------------------------------------------------------------------
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

        // Close Button ------------------------------------------------------------------------------------------------------------------
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //=============================================================================================================================================================================================
        //-- Key Down Events --//
        //=============================================================================================================================================================================================

        // Keyboard Shortcuts ------------------------------------------------------------------------------------------------------------------
        private void ExpandForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Esc
            if (e.KeyCode == Keys.Escape)
            {
                btnClose_Click(sender, e);
            }
        }
    }
}
