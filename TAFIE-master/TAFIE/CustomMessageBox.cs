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
    public partial class CustomMessageBox : Form
    {
        //====================================================================================================================================//
        //-- Initialization --//
        //====================================================================================================================================//

        private const string connectionString = SessionMaintenance.connectionString;

        public CustomMessageBox()
        {
            InitializeComponent();
        }

        //====================================================================================================================================//
        //-- Operation Methods --//
        //====================================================================================================================================//

        private string GetError(string code)
        {
            string query = "SELECT RTRIM(Error) as [Error] FROM Appz_Errors WHERE code = @Code";
            string error = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Code", code);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                error = reader["Error"].ToString(); // Populate variable
                            }
                        }
                    }

                    conn.Close(); // Close SQL Connection
                }
            }
            catch
            {
                Application.Exit();
            }
            return error ?? "Unknown Error!";
        }

        // Show An Error
        public void ShowDefError(string code, string additional)
        {
            string error = GetError(code);

            //lblDescription.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblSummary.Text = "Error!";
            Text = "Error!";
            lblDescription.Text = $"Error {code}: \n{error} {additional}";
            btnNo.Visible = false;
            btnYesOk.Text = "Ok";

            this.ShowDialog();
        }

        // Show An Error
        public void ShowError(string error)
        {
            //lblDescription.Font = new Font("Arial", 20F, FontStyle.Bold, GraphicsUnit.Point);
            lblSummary.Text = "Error!";
            Text = "Error!";
            lblDescription.Text = error;
            btnNo.Visible = false;
            btnYesOk.Text = "Ok";

            this.ShowDialog();
        }

        // Show Info
        public void ShowInfo(string info)
        {
            //lblDescription.Font = new Font("Arial", 20F, FontStyle.Bold, GraphicsUnit.Point);
            lblSummary.Text = "Info!";
            Text = "Info!";
            lblDescription.Text = info;
            btnNo.Visible = false;
            btnYesOk.Text = "Ok";

            this.ShowDialog();
        }

        // Show A Warning
        public void ShowWarning(string Warning)
        {
            //lblDescription.Font = new Font("Arial", 20F, FontStyle.Bold, GraphicsUnit.Point);
            lblSummary.Text = "Warning!";
            Text = "Warning!";
            lblDescription.Text = Warning;
            btnNo.Visible = false;
            btnYesOk.Text = "Ok";

            this.ShowDialog();
        }

        // Show Exit
        public bool ShowExitDialog()
        {
            //lblDescription.Font = new Font("Arial", 20F, FontStyle.Bold, GraphicsUnit.Point);
            lblSummary.Text = "Exit?";
            Text = "Exit?";
            lblDescription.Text = "Are you sure you want to Exit the application?";
            btnNo.Visible = true;
            btnYesOk.Text = "Yes";
            btnYesOk.Click += btnYesOk_Click;
            btnNo.Click += btnNo_Click;

            this.ShowDialog();

            return DialogResult == DialogResult.Yes;
        }

        // Show Misc Question
        public bool ShowQuestion(string summary, string message)
        {
            //lblDescription.Font = new Font("Arial", 20F, FontStyle.Bold, GraphicsUnit.Point);
            lblSummary.Text = summary;
            Text = summary;
            lblDescription.Text = message;
            btnNo.Visible = true;
            btnYesOk.Text = "Yes";
            btnYesOk.Click += btnYesOk_Click;
            btnNo.Click += btnNo_Click;

            this.ShowDialog();

            return DialogResult == DialogResult.Yes;
        }

        // Show LogBook
        public void ShowLogBook(string message)
        {
            //lblDescription.Font = new Font("Arial", 20F, FontStyle.Bold, GraphicsUnit.Point);
            lblSummary.Text = "LogBook";
            Text = "LogBook";
            lblDescription.Text = $"Last LogBook Activity Recorded: \n{message}";
            btnNo.Visible = false;
            btnYesOk.Text = "Ok";

            this.ShowDialog();
        }

        // Show are you sure?
        public bool ShowRUSure()
        {
            //lblDescription.Font = new Font("Arial", 20F, FontStyle.Bold, GraphicsUnit.Point);
            lblSummary.Text = "Are you sure?";
            Text = "Are you sure?";
            lblDescription.Text = "Are you sure you want to delete this record(s)? \nThis can not be undone!";
            btnNo.Visible = true;
            btnYesOk.Text = "Yes";
            btnYesOk.Click += btnYesOk_Click;
            btnNo.Click += btnNo_Click;

            this.ShowDialog();

            return DialogResult == DialogResult.Yes;
        }

        // Show Message
        public void ShowMessage(string message, string summary)
        {
            lblDescription.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblSummary.Text = $"{summary}";
            Text = $"{summary}";
            lblDescription.Text = $"{message}";
            btnNo.Visible = false;
            btnYesOk.Text = "Ok";
            this.ShowDialog();
        }

        //====================================================================================================================================//
        //-- Enviroment Events --//
        //====================================================================================================================================//
        private void btnYesOk_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnYesOk);
        }

        private void btnYesOk_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnYesOk);
        }

        private void btnNo_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnNo);
        }

        private void btnNo_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnNo);
        }

        //====================================================================================================================================//
        //-- Button Click Events --//
        //====================================================================================================================================//


        // Yes/Ok Button Click Event
        private void btnYesOk_Click(object sender, EventArgs e)
        {
            if (btnYesOk.Text == "Ok")
            {
                this.Close();
            }
            else if (btnYesOk.Text == "Yes")
            {
                DialogResult = DialogResult.Yes;
                this.Close();
            }
        }

        //No Button Click Event
        private void btnNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
