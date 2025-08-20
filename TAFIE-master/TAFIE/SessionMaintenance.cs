using Microsoft.Data.SqlClient;

namespace TAFIE
{
    internal class SessionMaintenance
    {
        public static int debugMode = 0;
        public static string userName { get; set; }
        public static string sessionId { get; set; }

        public static Color accentColor = Color.FromArgb(11, 159, 161);
        public static Color backColor = Color.Black;
        public static Color foreColor = Color.DarkGray;

        public static string currentVersion = "1.4.0"; // Current Version of the Application
        public static string appName = "TAFIE"; //Name of the Application

        public const string connectionString = "Server=SQL-SSRS;Database=Appz;Integrated Security=True;Encrypt=False;";

        public static int factCount = 0;

        // LogBook Method --------------------------------------------------------------------------------------------------------------------------------
        public static void LogBook(string tKey, string Form, string Event, string notes, Label? label = null)
        {
            CheckSessionID(sessionId);

            string query = $"[Appz - Insert_LogBook] @tKey, @Session_Id, @Domain_User, @User_Created, @Form, @Event, @Notes, @Application";
            string user = $"{Environment.MachineName}.{Environment.UserName}";
            string tKeySufix = Environment.UserName.Length >= 2 ? Environment.UserName.Substring(0, 2).ToUpper() : Environment.UserName.ToUpper();


            if (string.IsNullOrEmpty(tKey))
            {
                tKey = DateTime.Now.ToString("yyyyMMddHHmmssfff") + tKeySufix;
            }
            else if (tKey == "ERROR")
            {
                tKey = DateTime.Now.ToString("yyyyMMddHHmmssfff") + tKeySufix + "-ER";
            }

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tKey", tKey);
                        cmd.Parameters.AddWithValue("@Session_Id", sessionId);
                        cmd.Parameters.AddWithValue("@Domain_User", user.ToUpper());
                        cmd.Parameters.AddWithValue("@User_Created", userName);
                        cmd.Parameters.AddWithValue("@Form", Form);
                        cmd.Parameters.AddWithValue("@Event", Event);
                        cmd.Parameters.AddWithValue("@Notes", notes);
                        cmd.Parameters.AddWithValue("@Application", appName);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close(); // Close SQL Connection

                    if (label != null)
                    {
                        UpdateStatusLabel(label);
                    }
                }
            }
            catch (Exception ex)  // Catch any errors
            {
                string logMessage = $"*** {DateTime.Now} - LOGBOOK FAILED VIA SQL ***\n*** {tKey},{sessionId},{user.ToUpper()},{userName},{Form},{Event} ***\n*** Error: {ex} ***";
                string filePath = $@"\\elucid9\elucid\Apps\Error_Logs\{appName}_{tKey}_{user.ToUpper()}.txt";
                try
                {
                    File.WriteAllText(filePath, logMessage);
                }
                catch (Exception exfp)
                {
                    CustomMessageBox messageBoxfp = new CustomMessageBox();
                    messageBoxfp.ShowDefError("106", $"{exfp}");
                }
                finally
                {
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowDefError("102", $"{ex.Message}");
                    Application.Exit();
                }
            }
        }

        // Get Session ID --------------------------------------------------------------------------------------------------------------
        public static string GetSessionID()
        {
            string domainUser = Environment.UserName.ToUpper();
            string machine = Environment.MachineName.ToUpper();
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string query = $"EXECUTE [Appz - Insert_Update_Session] @Session_Id, @Domain_User, @Application";

            Random random = new Random();

            // Generate a random number between 1000 and 9999
            int randomNumber = random.Next(1000, 10000);

            // Convert the random number to a string and use it
            string randomNumberString = randomNumber.ToString();

            string sessionId = $"{machine}{randomNumberString}{domainUser}{timeStamp}";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Session_Id", sessionId);
                        cmd.Parameters.AddWithValue("@Domain_User", domainUser);
                        cmd.Parameters.AddWithValue("@Application", appName);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex) // Catch any errors
            {
                Cursor.Current = Cursors.Default;
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("104", $"\n{ex.Message}");
                LogBook("ERROR", "[SessionMaintenance]", "[GetSessionID]", $"FAILED: Code 104 ( {ex.Message} )");
                Application.Exit();
            }

            return sessionId;

        }

        // Check Session --------------------------------------------------------------------------------------------------------------------------------
        public static void CheckSessionID(string sessionId)
        {
            string query = $"SELECT COUNT(*) FROM {appName}_Sessions WHERE Session_Id = @Session_Id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Session_Id", sessionId);
                        int count = (int)cmd.ExecuteScalar();

                        if (count != 1)
                        {
                            CustomMessageBox messageBox = new CustomMessageBox();
                            messageBox.ShowDefError("166", $"");
                            ClearSessionID(sessionId);
                            Application.Exit();
                        }
                    }
                    conn.Close();
                }
            }
            // Catch Errors
            catch (Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("122", $"\n{ex.Message}");
                LogBook($"ERROR", "[SessionMaintenance]", "[CheckSessionID]", $"FAILED (  {ex.Message}  )");
            }
        }

        // Clear Session --------------------------------------------------------------------------------------------------------------------------------
        public static void ClearSessionID(string sessionId)
        {
            string query = $"DELETE {appName}_Sessions WHERE Session_Id = @Session_Id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Session_Id", sessionId);
                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }
            }
            // Catch Errors
            catch (Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("111", $"\n{ex.Message}");
                LogBook($"ERROR", "[SessionMaintenance]", "[ClearSessionID]", $"FAILED (  {ex.Message}  )");
            }
        }

        // Check Version --------------------------------------------------------------------------------------------------------------
        public static void CheckVersion()
        {
            // Declare Variables
            string latestVersion = null;
            string query = $"SELECT TOP 1 [Version] FROM VERSION_CTRL WHERE [Application] = '{appName}' ORDER BY Last_Updated DESC";

            // Get Latest Version From SQL Database
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                latestVersion = reader["Version"].ToString(); // Populate variable
                            }
                        }
                    }

                    conn.Close(); // Close SQL Connection
                }
            }
            catch (Exception ex) // Catch any errors
            {
                Cursor.Current = Cursors.Default;
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("105", $"\n{ex.Message}");
                LogBook("ERROR", "[SessionMaintenance]", "[CheckVersion]", $"FAILED ( {ex.Message} )");
                Application.Exit();
            }

            // Compare latest version to current version
            if (latestVersion != currentVersion)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("172", $"\nCurrent Version:{currentVersion} Lastest Version: {latestVersion}");
                LogBook("ERROR", "[SessionMaintenance]", "[CheckVersion]", $"This is an outdated/unsupported version of the application, please use the latest version. This version: v{currentVersion} Latest Version: v{latestVersion}");
                Application.Exit();
            }
        }

        // update Status Label --------------------------------------------------------------------------------------------------------------------------------
        public static void UpdateStatusLabel(Label status)
        {
            string query = "SELECT TOP 1 " +
                "'Status: ' + RTRIM(FORMAT(GETDATE(), 'yyyy-MM-dd HH:mm:ss')) + ' - ' + RTRIM([Form]) + ' - ' + RTRIM([Event]) + ' - ' + RTRIM([Notes]) as [OUTPUT] " +
                $"FROM  {appName}_LogBook " +
                "WHERE Session_Id = @Session_Id " +
                "ORDER BY DT_Created DESC";
            string output = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Session_Id", sessionId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                output = (string)reader["OUTPUT"];
                            }
                        }

                    }
                    conn.Close();
                }

                status.Text = output;

            }
            // Catch Errors
            catch (Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("117", $"\n{ex.Message}");
                LogBook($"ERROR", "[SessionMaintenance]", "[UpdateStatusLabel]", $"FAILED: Code 117 (  {ex.Message}  )");
            }

        }

        // Get & Show Fact --------------------------------------------------------------------------------------------------------------------------------
        public static void ShowFact()
        {
            if (factCount < 3)
            {
                string fact = "";
                string query = "EXECUTE [Appz - GetFactz]";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    fact = (string)reader["OUTPUT"];
                                }
                            }
                        }
                        conn.Close();
                    }

                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ClientSize = new Size(525, 599);
                    messageBox.ShowMessage(fact, "Did you know?");
                    messageBox.ClientSize = new Size(525, 342);
                    factCount++;
                }
                // Catch Errors
                catch (Exception ex)
                {
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowDefError("117", $"\n{ex.Message}");
                    LogBook($"ERROR", "[SessionMaintenance]", "[ShowFact]", $"FAILED: Code 117 (  {ex.Message}  )");
                }
            }
            else
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowMessage("Right, I get that you wanna learn all these cool facts but save some for the rest, come back later if you're still in the mood for big facts!", "Calm Down");

            }
        }


        // Change Button Colours ------------------------------------------------------------------------------------------------------------
        public static void ButtonEnter(System.Windows.Forms.Button button)
        {
            button.BackColor = backColor;
            button.ForeColor = accentColor;
        }
        public static void ButtonLeave(System.Windows.Forms.Button button)
        {
            button.BackColor = accentColor;
            button.ForeColor = backColor;
        }

        // Change texbox colors Colours ------------------------------------------------------------------------------------------------------------
        public static void ControlEnter(Control control)
        {
            control.BackColor = accentColor;
            control.ForeColor = backColor;
        }
        public static void ControlLeave(Control control)
        {
            control.BackColor = Color.White;
            control.ForeColor = backColor;
        }
    }
}
