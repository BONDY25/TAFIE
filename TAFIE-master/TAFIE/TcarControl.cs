using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace TAFIE
{
    internal class TcarControl
    {
        public static string? tcarRef { get; set; }

        private const string connectionString = SessionMaintenance.connectionString;

        private static bool executeOrder66 = false; // true = Load Completion is ON. false = Load Completion is OFF

        // Array of barcoding clients
        public static string[] barc = {
            //"Museum Selection - MAIN",
            // "Culture Vulture - MAIN",
            // "Pia - MAIN",
             "Museum Selection - TEST"
        };

        //=============================================================================================================================================================================================
        //-- SQL Data Tasks --//
        //=============================================================================================================================================================================================

        // Get Header ------------------------------------------------------------------------------------------------------------------
        public static void GetHeaderDetails(string loadNote, string client)
        {
            string query = "EXECUTE [TCAR_Get_Headers] @TCAR_Ref, @Session_Id, @Client, @Load_Note, @User";

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
                        cmd.Parameters.AddWithValue("@Session_Id", SessionMaintenance.sessionId);
                        cmd.Parameters.AddWithValue("@Client", client);
                        cmd.Parameters.AddWithValue("@Load_Note", loadNote);
                        cmd.Parameters.AddWithValue("@User", SessionMaintenance.userName);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close(); // Close SQL Connection
                }
            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured getting shipment headers \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[TcarControl]", "[GetHeaderDetails]", $"FAILED: Code 226 ( {ex.Message} )");
            }
        }

        // Get Delivery Details ------------------------------------------------------------------------------------------------------------------
        public static void GetDelDetails()
        {
            string query = "EXECUTE [TCAR_Get_Del] @TCAR_Ref";

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);

                        cmd.ExecuteNonQuery();
                    }
                    conn.Close(); // Close SQL Connection
                }
            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured getting delivery details \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[TcarControl]", "[GetDelDetails]", $"FAILED: Code 226 ( {ex.Message} )");
            }
        }

        // Get Componants ------------------------------------------------------------------------------------------------------------------
        public static void GetComp()
        {
            string query = "EXECUTE [TCAR_Get_Comp] @TCAR_Ref";

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);

                        cmd.ExecuteNonQuery();
                    }
                    conn.Close(); // Close SQL Connection
                }
            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured getting Component details \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[TcarControl]", "[GetComp]", $"FAILED: Code 226 ( {ex.Message} )");
            }
        }

        // Recalculate Boxes ------------------------------------------------------------------------------------------------------------------
        public static void RecalculateBoxes()
        {
            SessionMaintenance.LogBook("", "[TcarControl]", "[RecalculateBoxes]", $"Box Calculations Started");

            string query = "EXECUTE [TCAR_RECAL] @TCAR_Ref";

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);

                        cmd.ExecuteNonQuery();
                    }
                    conn.Close(); // Close SQL Connection
                }
            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured getting Component details \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[TcarControl]", "[RecalculateBoxes]", $"FAILED: Code 226 ( {ex.Message} )");
            }
        }

        // Complete TCAR ------------------------------------------------------------------------------------------------------------------
        public static void CompleteTCAR(string tracking, int BoxRef)
        {
            string query = "EXECUTE [TCAR_Complete_V2] @TCAR_Ref, @Tracking, @Box_Ref";

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
                        cmd.Parameters.AddWithValue("@Tracking", tracking);
                        cmd.Parameters.AddWithValue("@Box_Ref", BoxRef);


                        cmd.ExecuteNonQuery();
                    }
                    conn.Close(); // Close SQL Connection
                }
            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured Completing TCAR \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[TcarControl]", "[CompleteTCAR]", $"FAILED: ( {ex.Message} )");
            }
        }

        // Complete Load Note ---------------------------------------------------------------------------------------------------
        public static void CompleteLoadNote(string client)
        {
            if (barc.Contains(client) && executeOrder66)
            {
                SessionMaintenance.LogBook("", "[TcarControl]", "[CompleteLoadNote]", $"Load Note Completion started");

                string[] query =
                {
                        "EXECUTE [TCAR_Complete_Load_P1] @TCAR_Ref",
                        "EXECUTE [TCAR_Complete_Load_P2] @TCAR_Ref",
                        "EXECUTE [TCAR_Complete_Load_P3] @TCAR_Ref",
                        "EXECUTE [TCAR_Complete_Load_P4] @TCAR_Ref"
                };

                try
                {
                    // Execute SQL Query for each part of the completion
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        foreach (var q in query)
                        {
                            SessionMaintenance.LogBook("", "[TcarControl]", "[CompleteLoadNote]", $"Query: {q} started");
                            using (SqlCommand cmd = new SqlCommand(q, conn))
                            {
                                cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
                                cmd.ExecuteNonQuery();
                            }
                            SessionMaintenance.LogBook("", "[TcarControl]", "[CompleteLoadNote]", $"Query: {q} finished");
                        }
                    }

                    UpdateSordHchg(client);

                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowInfo("Load Note Completed Successfully");


                    SessionMaintenance.LogBook("", "[TcarControl]", "[CompleteLoadNote]", $"Load Note Completion finished");
                }
                catch (Exception ex)  // Catch any errors
                {
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowError($"An error occured completing load note \n{ex.Message}");
                    SessionMaintenance.LogBook("ERROR", "[TcarControl]", "[CompleteLoadNote]", $"FAILED: ( {ex.Message} )");
                }
            }
        }

        // Update SORD_HCHG ---------------------------------------------------------------------------------------------------
        public static void UpdateSordHchg(string client)
        {
            string getDatabaseQuery = "SELECT RTRIM([Server]) as [Server], RTRIM([Database]) as [Database] FROM TAFIE_Clients WHERE [Description] = @Client";
            string getQuery = "SELECT String_1 FROM TAFIE_Parameters WHERE ID = '004'";
            string getRef = "SELECT TOP 1 Ref_No FROM TCAR WHERE TCAR_Ref = @TCAR_Ref";
            string refNo = "";
            string updateConnectionString = null;
            string queryUpdate = null;            
            string server = null;
            string database = null;

            SessionMaintenance.LogBook("", "[TcarControl]", "[UpdateSordHchg]", $"Method Started");

            try
            {
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Get the database connection details
                    using (SqlCommand cmd = new SqlCommand(getDatabaseQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Client", client);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                server = reader["Server"].ToString();
                                database = reader["Database"].ToString();
                            }
                        }
                    }

                    SessionMaintenance.LogBook("", "[TcarControl]", "[UpdateSordHchg]", $"Database Retrieved: {server},{database}");

                    // Get the reference number for the TCAR_Ref
                    using (SqlCommand cmd = new SqlCommand(getRef, conn))
                    {
                        cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                refNo = reader["Ref_No"].ToString();

                            }
                        }
                    }

                    SessionMaintenance.LogBook("", "[TcarControl]", "[UpdateSordHchg]", $"Reference number Retrieved: {refNo}");

                    // Get UPDATE query from TAFIE_Parameters
                    using (SqlCommand cmd = new SqlCommand(getQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                queryUpdate = reader["String_1"].ToString();

                            }
                        }
                    }

                    SessionMaintenance.LogBook("", "[TcarControl]", "[UpdateSordHchg]", $"UPDATE query Retrieved: {queryUpdate}");

                    conn.Close();
                }              

                // Build the connection string
                updateConnectionString = $"Server={server};Database={database};Integrated Security=True;Encrypt=False;";

                SessionMaintenance.LogBook("", "[TcarControl]", "[UpdateSordHchg]", $"Connection string constructed {updateConnectionString}");

                // Execute the update query
                using (SqlConnection conn = new SqlConnection(updateConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(queryUpdate, conn))
                    {
                        cmd.Parameters.AddWithValue("@Ref_No", refNo);
                        cmd.Parameters.AddWithValue("@User", SessionMaintenance.userName);
                        cmd.ExecuteNonQuery();

                        SessionMaintenance.LogBook("", "[TcarControl]", "[UpdateSordHchg]", $"Update Query Executed");
                    }
                    conn.Close();
                }

                SessionMaintenance.LogBook("", "[TcarControl]", "[UpdateSordHchg]", $"Method Finished");

            }
            catch (Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occurred updating Order Details: {ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[TcarControl]", "[UpdateSordHchg]", $"FAILED: Code 226 ( {ex.Message} )");
            }
        }

        // Check Load Note Exists-----------------------------------------------------------------------------------------------------------------------
        public static int CheckLoadNote(string loadNote, string client)
        {
            int check = 0;
            string query = "EXECUTE TAFIE_Check_Load @Session_Id, @Client, @Load_Note";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Session_Id", SessionMaintenance.sessionId);
                        cmd.Parameters.AddWithValue("@Client", client);
                        cmd.Parameters.AddWithValue("@Load_Note", loadNote);

                        int result = (int)cmd.ExecuteScalar();

                        check = result;
                    }
                    conn.Close();
                }
            }
            // Catch Errors
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("109", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[TcarControl]", "[CheckLoadNote]", $"FAILED: Code 109 ( {ex.Message} )");
            }

            return check;
        }

        // Get Boxes ------------------------------------------------------------------------------------------------------------------       
        public static List<int> GetBoxes()
        {
            string query = "SELECT Box_Ref FROM TCAR_Boxes WHERE TCAR_Ref = @TCAR_Ref";
            List<int> boxRefs = new List<int>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);

                        // Execute Data Reader
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Populate list from reader
                            while (reader.Read())
                            {
                                boxRefs.Add((int)reader["Box_Ref"]);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            // Catch Errors
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured getting box references.\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[TcarControl]", "[GetBoxes]", $"FAILED: ( {ex.Message} )");
            }

            return boxRefs;
        }

        // Insert Data from DataTable to SQL Server -------------------------------------------------------------------
        public static void InsertDataGrid(DataTable dt, string user)
        {
            string queryDel = "DELETE TCAR_BOXES WHERE TCAR_Ref = @TCAR_Ref; DELETE TCAR_BCON WHERE TCAR_Ref = @TCAR_Ref";
            string queryIns = "EXECUTE [TCAR_Insert_New_BCON] @TCAR_Ref, @Box_Ref, @Part, @Qty, @User";
            string querySplit = "EXECUTE [TCAR_Split_Post] @TCAR_Ref";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection

                    // Clear existing data for the TCAR_Ref
                    using (SqlCommand cmdDel = new SqlCommand(queryDel, conn))
                    {
                        // Set Parameters
                        cmdDel.Parameters.AddWithValue("@TCAR_Ref", tcarRef);

                        // Execute Query
                        cmdDel.ExecuteNonQuery();
                    }

                    // Insert new data from DataTable to SQL Server
                    foreach (DataRow row in dt.Rows)
                    {
                        // Safely parse BoxNo
                        if (!int.TryParse(row["BoxNo"]?.ToString(), out int boxNo))
                        {
                            // Log or skip the row if invalid
                            SessionMaintenance.LogBook("ERROR", "[TcarControl]", "[InsertDataGrid]",
                                $"Skipping row due to invalid BoxNo: '{row["BoxNo"]}'");
                            continue;
                        }

                        using (SqlCommand cmd = new SqlCommand(queryIns, conn))
                        {
                            // Set Parameters
                            cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
                            cmd.Parameters.AddWithValue("@Box_Ref", Convert.ToInt32(row["BoxNo"]));
                            cmd.Parameters.AddWithValue("@Part", row["Part"]);
                            cmd.Parameters.AddWithValue("@Qty", row["Qty"]);
                            cmd.Parameters.AddWithValue("@User", user);

                            // Execute Query
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Calculate postage split
                    using (SqlCommand cmdSplit = new SqlCommand(querySplit, conn))
                    {
                        // Set Parameters
                        cmdSplit.Parameters.AddWithValue("@TCAR_Ref", tcarRef);

                        // Execute Query
                        cmdSplit.ExecuteNonQuery();
                    }

                    conn.Close(); // Close SQL Connection
                }
            }
            catch (Exception ex) // Catch Errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"Error Inserting Data \n{ex}");
                SessionMaintenance.LogBook("ERROR", "[TcarControl]", "[InsertDataGrid]", $"FAILED: Code 118 ( {ex.Message} )");
            }
        }

        // Insert Payload String ------------------------------------------------------------------------------------------------------------------
        public static void InsertPayload(string payLoad, string tKey, int mode)
        {
            string query = "EXECUTE [TCAR_INSERT_XML] @tkey, @TCAR_Ref, @User, @Payload_String, @Mode";

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tkey", tKey);
                        cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
                        cmd.Parameters.AddWithValue("@User", SessionMaintenance.userName);
                        cmd.Parameters.AddWithValue("@Payload_String", payLoad);
                        cmd.Parameters.AddWithValue("@Mode", mode);

                        cmd.ExecuteNonQuery();
                    }
                    conn.Close(); // Close SQL Connection
                }

                SessionMaintenance.LogBook("", "[TcarControl]", "[InsertPayload]", $"Payload String Inserted: {mode} - {tKey}");
            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured saving API Payload \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[TcarControl]", "[InsertPayload]", $"FAILED: Code 226 ( {ex.Message} )");
            }
        }

        // Run Reports  -----------------------------------------------------------------------------------------------------------------------
        public static void RunReport(string ID)
        {
            string url = "";
            string query = $"SELECT String_1 FROM [TAFIE_Parameters] WHERE ID = '{ID}'";

            // Get URL
            try
            {
                // Execute SQL
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open(); // Open SQL Connection

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Execute Data Reader
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Populate variables from reader
                            while (reader.Read())
                            {
                                url = reader["String_1"].ToString();
                            }
                        }
                    }

                    conn.Close(); // Close SQL Connection

                }
            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("119", $"\n{ex.Message}");
                SessionMaintenance.LogBook($"ERROR", "[TcarControl]", "[RunReport]", $"FAILED: Code 119 (  {ex.Message}  )");
            }

            // Open URL
            try
            {
                // Open the URL in the default web browser
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };

                Process.Start(processStartInfo);

                SessionMaintenance.LogBook($"", "[TcarControl]", "[RunReport]", $"Process Executed With Parameter: {ID}");

            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("121", $"\n{ex.Message}");
                SessionMaintenance.LogBook($"ERROR", "[MaiTcarControlnForm]", "[RunReport]", $"FAILED: Code 121 (  {ex.Message}  )");

            }

        }

        //=============================================================================================================================================================================================
        //-- API Helpers --//
        //=============================================================================================================================================================================================

        // Create tKey -------------------------------------------------------------------------------------------------------------
        public static string CreateTKey(string name)
        {
            name = name.Replace("'", "X");  // Replace single quotes with 'X' to avoid SQL errors
            string tKeySufix = name.Length >= 2 ? name.Substring(0, 2).ToUpper() : name.ToUpper();
            string tKeyTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string tKey = tKeyTimeStamp + tKeySufix;
            return tKey;
        }

        // Open URL ------------------------------------------------------------------------------------------------------------------
        public static void OpenURL()
        {
            string url = "https://www.royalmail.com/find-a-postcode";

            try
            {
                // Open the URL in the default web browser
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };

                Process.Start(processStartInfo);

                SessionMaintenance.LogBook($"", "[TcarControl]", "[OpenURL]", $"Process Executed");

            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("121", $"\n{ex.Message}");
                SessionMaintenance.LogBook($"ERROR", "[TcarControl]", "[OpenURL]", $"FAILED: Code 121 (  {ex.Message}  )");

            }
        }

        // Print Labels for all Boxes ---------------------------------------------------------------------------------------------------
        public static async Task PrintLabels(List<int> boxRefs, Func<int, int, Task> printMethod)
        {
            int boxCount = boxRefs.Count;
            foreach (var boxRef in boxRefs)
            {
                await printMethod(boxRef, boxCount);
            }
        }

        // Print PDF ------------------------------------------------------------------------------------------------------------------
        public static void PrintPdf(string filePath)
        {
            try
            {
                string adobeReaderPath = @"C:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader\AcroRd32.exe";

                // Create a list of possible Adobe Reader paths
                List<string> adoobeReaderPaths = new List<string>
                {
                    @"C:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader\AcroRd32.exe",
                    @"C:\Program Files\Adobe\Acrobat Reader DC\Reader\AcroRd32.exe",
                    @"C:\Program Files\Adobe\Acrobat Reader DC\Reader\AcroRd.exe",
                    @"C:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader\AcroRd.exe",
                };

                // Find the first existing path
                foreach (var path in adoobeReaderPaths)
                {
                    if (File.Exists(path))
                    {
                        adobeReaderPath = path;
                        break;
                    }
                }

                // Check if Adobe Reader is installed
                if (!File.Exists(adobeReaderPath))
                {
                    throw new Exception("Adobe Reader not found. Please install a PDF viewer that supports command-line printing.");
                }

                // Start the print process
                Process printProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = adobeReaderPath,
                        Arguments = $"/t \"{filePath}\"",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    }
                };

                printProcess.Start();
                SessionMaintenance.LogBook("", "[TcarControl]", "[PrintPdf]", $"Printing {filePath}");

                // Wait up to 15 seconds for the printing process to finish
                bool exited = printProcess.WaitForExit(15000);
                if (!exited)
                {
                    SessionMaintenance.LogBook("", "[TcarControl]", "[PrintPdf]", "Printing process is taking too long.");
                }

                // Try closing Acrobat gracefully first
                foreach (var process in Process.GetProcessesByName("AcroRd32"))
                {
                    if (!process.CloseMainWindow()) // Graceful close attempt
                    {
                        process.Kill(); // Force close if necessary
                        SessionMaintenance.LogBook("", "[TcarControl]", "[PrintPdf]", "Process forcibly terminated: AcroRd32");
                    }
                    else
                    {
                        SessionMaintenance.LogBook("", "[TcarControl]", "[PrintPdf]", "Process closed normally: AcroRd32");
                    }
                }

                SessionMaintenance.LogBook("", "[TcarControl]", "[PrintPdf]", "Print Process Complete");
            }
            catch (Exception ex)
            {
                SessionMaintenance.LogBook("", "[TcarControl]", "[PrintPdf]", $"Error printing: {ex.Message}");
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"Error printing: {ex.Message}");
            }
        }
    }
}
