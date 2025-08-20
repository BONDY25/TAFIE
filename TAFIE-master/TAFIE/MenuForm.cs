namespace TAFIE
{
    public partial class MenuForm : Form
    {
        //=============================================================================================================================================================================================
        //-- Initialization --//
        //=============================================================================================================================================================================================

        //public string sessionId { get; set; }
        //public string userName { get; set; }

        public MenuForm()
        {
            InitializeComponent();
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[MenuForm]", "[FormLoad]", $"Form Started");
            Text = $"{Environment.UserName.ToUpper()} - {SessionMaintenance.appName} Home";
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

        // Carrier Label Button ------------------------------------------------------------------------------------------------------------------
        private void btnCarrLabels_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnCarrLabels);
        }

        private void btnCarrLabels_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnCarrLabels);
        }

        // ToolBox Button ------------------------------------------------------------------------------------------------------------------
        private void btnToolBox_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnToolBox);
        }

        private void btnToolBox_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnToolBox);
        }

        //=============================================================================================================================================================================================
        //-- Button Click Events --//
        //=============================================================================================================================================================================================

        // Exit Button ------------------------------------------------------------------------------------------------------------------
        private void btnExit_Click(object sender, EventArgs e)
        {
            CustomMessageBox messageBox = new CustomMessageBox();
            bool result = messageBox.ShowExitDialog(); // Ask user if they want to exit
            if (result == true)
            {
                SessionMaintenance.LogBook("", "[MenuForm]", "[FormClosing]", $"Termination");
                SessionMaintenance.ClearSessionID(SessionMaintenance.sessionId);
                Application.Exit();
            }
        }

        // ToolBox Button ------------------------------------------------------------------------------------------------------------------
        private void btnTooBox_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }

        // Carrier Label Button ------------------------------------------------------------------------------------------------------------------
        private void btnCarrLabels_Click(object sender, EventArgs e)
        {
            CarrierForm carrForm = new CarrierForm();
            carrForm.Show();

            //if (
            //    userName == "AIDENB"
            //    || userName == "KYALC"
            //    || userName == "JACOBDR"
            //    || userName == "JOSEPH"
            //    || userName == "STEVE"
            //    || userName == "ANDYC"
            //    || userName == "JAMEST"
            //    || userName == "SARAHS"
            //    || userName == "REBECCACO"
            //    )
            //{
            //    CarrierForm carrForm = new CarrierForm();
            //    carrForm.userName = userName;
            //    carrForm.sessionId = sessionId;
            //    SessionMaintenance.userName = userName;
            //    carrForm.Show();
            //}
            //else
            //{
            //    CustomMessageBox messageBox = new CustomMessageBox();
            //    messageBox.ShowError("Sorry, you do not have permission to use this feature.");
            //    return;
            //}
        }

    }
}
