using Microsoft.Data.SqlClient;

namespace TAFIE
{
    public partial class LoginForm : Form
    {
        //=============================================================================================================================================================================================
        //-- Initialization --//
        //=============================================================================================================================================================================================
          
        private const string connectionString = SessionMaintenance.connectionString;

        public string sessionId = SessionMaintenance.GetSessionID();

        public LoginForm()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.KeyPreview = true;
            this.KeyDown += LoginForm_KeyDown;
            txbUsername.KeyPress += txbUsername_KeyPress;
            txbUsername.KeyDown += txbUsername_KeyDown;
        }

        // Form Load ------------------------------------------------------------------------------------------------------------------------
        private void LoginForm_Load(object sender, EventArgs e)
        {
            SessionMaintenance.sessionId = sessionId;
            SessionMaintenance.userName = "UNDEFINED";                    
            Text = $"{Environment.UserName.ToUpper()} - {SessionMaintenance.appName} Login";

            if (SessionMaintenance.debugMode == 1)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowMessage($"This is not a live version of the application\nFeatures or function may still be in development which could cause bugs, crashes or unexpected behaviour", "Warning");
                lblVersion.Text = $"Build: DEBUG MODE";
                SessionMaintenance.LogBook("", "[LoginForm]", "[FormLoad]", $"Debug Mode Enabled - Initialization");
            }
            else
            {
                SessionMaintenance.LogBook("", "[LoginForm]", "[FormLoad]", $"Initialization");
                SessionMaintenance.CheckVersion();
                SessionMaintenance.CheckSessionID(sessionId);
                lblVersion.Text = $"Build: v{SessionMaintenance.currentVersion}";
            }
        }

        // Form Close ------------------------------------------------------------------------------------------------------------------------
        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                bool result = messageBox.ShowExitDialog(); // Ask user if they want to exit
                if (result == true)
                {
                    SessionMaintenance.LogBook("", "[LoginForm]", "[FormClosing]", $"Termination");
                    SessionMaintenance.ClearSessionID(sessionId);
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        // Prevent Special Characters (txbUsername) -----------------------------------------------------------------------------------------------------------
        private void txbUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("127", $"'{e.KeyChar}'");

                e.Handled = true;
            }
        }


        // Check Username ------------------------------------------------------------------------------------------------------------------------
        private bool CheckUser()
        {
            string username = txbUsername.Text;
            string query = "SELECT COUNT(*) FROM APP_USERS WHERE Username = @Username";
            bool checkUser = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        int count = (int)cmd.ExecuteScalar();

                        if (count != 1)
                        {
                            checkUser = false;
                        }
                        else
                        {
                            checkUser = true;
                        }
                    }
                    conn.Close();
                }
            }
            // Catch Errors
            catch (Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("123", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[LoginForm]", "[CheckUser]", $"FAILED: Code 123 ( {ex.Message} )");
            }

            return checkUser;
        }

        //=============================================================================================================================================================================================
        //-- Enviroment Box Events --//
        //=============================================================================================================================================================================================

        // Change Control Colours When Focused //
        // Username Field -----------------------------------------------------------------------------------------------------------
        private void txbUsername_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbUsername);
        }

        private void txbUsername_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbUsername);
        }

        // Login Button -----------------------------------------------------------------------------------------------------------
        private void btnLogin_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnLogin);
        }

        private void btnLogin_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnLogin);
        }

        // Exit Button -----------------------------------------------------------------------------------------------------------
        private void btnExit_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnExit);
        }

        private void btnExit_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnExit);
        }

        // Easter Egg -----------------------------------------------------------------------------------------------------------
        private void lblTitle_MouseEnter(object sender, EventArgs e)
        {
            ttEgg.SetToolTip(lblTitle, "???");
        }

        //=============================================================================================================================================================================================
        //-- Button Click Events --//
        //=============================================================================================================================================================================================

        // Exit button Event -----------------------------------------------------------------------------------------------------------
        private void btnExit_Click(object sender, EventArgs e)
        {
            CustomMessageBox messageBox = new CustomMessageBox();
            bool result = messageBox.ShowExitDialog(); // Ask user if they want to exit
            if (result == true)
            {
                SessionMaintenance.LogBook("", "[LoginForm]", "[FormClosing]", $"Termination");
                SessionMaintenance.ClearSessionID(sessionId);
                Application.Exit();
            }
        }

        // Login Button Event -----------------------------------------------------------------------------------------------------------
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txbUsername.Text.ToUpper();

            // Make sure the user has entered a username
            if (string.IsNullOrEmpty(userName))
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("158", $"");
                return;
            }
            else
            {
                if (CheckUser())
                {
                    MenuForm menuForm = new MenuForm();
                    SessionMaintenance.userName = userName;
                    SessionMaintenance.sessionId = sessionId;
                    menuForm.Show();
                    this.Hide();
                }
                else
                {
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowDefError("171", $"");
                    txbUsername.Focus();
                    txbUsername.SelectAll();
                    return;
                }
            }

        }

        // Easter Egg -----------------------------------------------------------------------------------------------------------
        private void lblTitle_Click(object sender, EventArgs e)
        {
            SessionMaintenance.ShowFact();
        }

        //=============================================================================================================================================================================================
        //-- Key Down Events --//
        //=============================================================================================================================================================================================

        // ctrl + V Event (txbBoxID) -----------------------------------------------------------------------------------------------------------
        private void txbUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                // Get the text from the clipboard
                string clipboardText = Clipboard.GetText();

                // Filter out invalid characters and set the filtered text to the TextBox
                txbUsername.Text = new string(clipboardText.Where(c => char.IsLetterOrDigit(c) || char.IsControl(c)).ToArray());

                // Cancel the paste operation
                e.SuppressKeyPress = true;
            }
        }

        // Keyboard Shortcuts -----------------------------------------------------------------------------------------------------------
        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            // ctrl + G
            if (e.Control && e.KeyCode == Keys.G)
            {
                txbUsername.Text = "";
            }

            // Esc
            if (e.KeyCode == Keys.Escape)
            {
                btnExit_Click(sender, e);
            }
        }
    }
}