using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;
using System.Text.Json;

namespace TAFIE
{
    internal class ProCarrApi
    {
        private static readonly string apiUrl = "https://dgapi.app/API/";
        private const string connectionString = SessionMaintenance.connectionString;

        public static string? tcarRef = null;

        //=============================================================================================================================================================================================
        //-- API Methods --//
        //=============================================================================================================================================================================================

        // Get JSON Data from SQL Server for a specific TCAR and Box Reference -------------------------------------------------------------
        public static DataTable GetJsonData(string tcarRef, int boxRef)
        {
            string query = "EXECUTE [TCAR_Get_JSON] @TCAR_Ref, @Box_Ref";

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
                        cmd.Parameters.AddWithValue("@Box_Ref", boxRef);

                        // Execute Query
                        cmd.ExecuteNonQuery();

                        // Execute Data Reader
                        SqlDataReader reader = cmd.ExecuteReader();

                        // Populate DataTable From Reader
                        dataTable.Load(reader);
                    }

                    conn.Close(); // Close SQL Connection
                }

                SessionMaintenance.LogBook("", "[ProCarrApi]", "[GetJSONData]", $"JSON Data retrived for {tcarRef}, {boxRef}");
            }
            catch (Exception ex) // Catch Errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("117", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[ProCarrApi]", "[GetJSONData]", $"FAILED: Code 117 ( {ex.Message} )");
            }

            return dataTable;
        }

        //=============================================================================================================================================================================================
        //-- Make API Calls --//
        //=============================================================================================================================================================================================

        // Make API Call to get label ------------------------------------------------------------------------------------------------------------------        
        public static async Task<string> MakeLabelApiCall(string jsonPayload, string tKey)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.Timeout = TimeSpan.FromSeconds(30);

                    SessionMaintenance.LogBook("", "[ProCarrApi]", "[MakeLabelApiCall]", "API Call Attempted");

                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    string responseData = await response.Content.ReadAsStringAsync();
                    SessionMaintenance.LogBook("", "[ProCarrApi]", "[MakeLabelApiCall]", $"Response Data:\n{responseData}");

                    // Check for transport-level error first
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = $"Status: {response.StatusCode}, Content: {responseData}";
                        ShowError(errorMessage);
                        SessionMaintenance.LogBook("ERROR", "[ProCarrApi]", "[MakeLabelApiCall]", $"FAILED: {errorMessage}");
                        TcarControl.InsertPayload(responseData, tKey, 3);
                        return responseData;
                    }

                    // Check for application-level error in JSON
                    var apiError = ParseInternalApiError(responseData);
                    if (apiError != null)
                    {
                        ShowError(apiError);
                        SessionMaintenance.LogBook("ERROR", "[ProCarrApi]", "[MakeLabelApiCall]", $"FAILED: {apiError}");
                        TcarControl.InsertPayload(responseData, tKey, 3);
                        return responseData;
                    }

                    TcarControl.InsertPayload(responseData, tKey, 2);
                    SessionMaintenance.LogBook("", "[ProCarrApi]", "[MakeLabelApiCall]", "API Call Successful");
                    return responseData;
                }
                catch (HttpRequestException ex)
                {
                    string error = $"Request error: {ex.Message}";
                    ShowError(error);
                    SessionMaintenance.LogBook("ERROR", "[ProCarrApi]", "[MakeLabelApiCall]", $"FAILED: ( {error} )");
                    throw;
                }
            }

            // Helper method to extract error if ErrorLevel > 0
            static string? ParseInternalApiError(string json)
            {
                try
                {
                    using var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("ErrorLevel", out JsonElement errorLevelElement) &&
                        errorLevelElement.GetInt32() > 0)
                    {
                        if (doc.RootElement.TryGetProperty("Error", out JsonElement errorElement))
                        {
                            return errorElement.GetString() ?? "Unknown API error.";
                        }
                        return "API returned error with no message.";
                    }
                    return null;
                }
                catch
                {
                    // If parsing fails, assume no internal error structure
                    return null;
                }
            }

            // Helper method to show errors via UI
            static void ShowError(string errorMessage)
            {
                var messageBox = new CustomMessageBox();
                // Show Error
                messageBox.lblDescription.Font = new Font("Consolas", 24F, FontStyle.Bold, GraphicsUnit.Point);
                messageBox.lblDescription.BackColor = Color.Red;
                messageBox.ClientSize = new Size(800, 500);
                messageBox.lblDescription.Size = new Size(770, 372);
                messageBox.ShowError($"{errorMessage}");

                // Reset message box
                messageBox.lblDescription.Font = new Font("Consolas", 12F, FontStyle.Bold, GraphicsUnit.Point);
                messageBox.lblDescription.BackColor = Color.FromArgb(11, 159, 161);
                messageBox.ClientSize = new Size(525, 342);
                messageBox.lblDescription.Size = new Size(501, 214);
            }
        }

        //=============================================================================================================================================================================================
        //-- Create JSON --//
        //=============================================================================================================================================================================================

        // Create JSON string from DataTable ---------------------------------------------------------------------------------------------------------------------
        public static string CreateJsonString(DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return string.Empty;

            DataRow firstRow = dt.Rows[0];

            var result = new
            {
                Apikey = firstRow["Apikey"].ToString().ToLower(),
                Command = firstRow["Command"],
                Shipment = new
                {
                    RequireCarrierTrackingNumber = Convert.ToBoolean(firstRow["RequireCarrierTrackingNumber"]),
                    LabelOption = firstRow["LabelOption"],
                    LabelFormat = firstRow["LabelFormat"],
                    ShipperReference = firstRow["ShipperReference"],
                    DisplayId = firstRow["DisplayId"],
                    InvoiceNumber = firstRow["InvoiceNumber"],
                    Service = firstRow["Service"],

                    SenderAddress = new
                    {
                        Name = firstRow["Sender_Name"],
                        Company = firstRow["Sender_Company"],
                        AddressLine1 = firstRow["Sender_AddressLine1"],
                        AddressLine2 = firstRow["Sender_AddressLine2"],
                        City = firstRow["Sender_City"],
                        State = firstRow["Sender_State"],
                        Zip = firstRow["Sender_Zip"],
                        Country = firstRow["Sender_Country"],
                        Phone = firstRow["Sender_Phone"],
                        Email = firstRow["Sender_Email"],
                        Vat = firstRow["Sender_Vat"],
                        Eori = firstRow["Sender_Eori"],
                        Ioss = firstRow["Sender_Ioss"]
                    },

                    ConsigneeAddress = new
                    {
                        Name = firstRow["Consignee_Name"],
                        Company = firstRow["Consignee_Company"],
                        AddressLine1 = firstRow["Consignee_AddressLine1"],
                        AddressLine2 = firstRow["Consignee_AddressLine2"],
                        City = firstRow["Consignee_City"],
                        State = firstRow["Consignee_State"],
                        Zip = firstRow["Consignee_Zip"],
                        Country = firstRow["Consignee_Country"],
                        Phone = firstRow["Consignee_Phone"],
                        Email = firstRow["Consignee_Email"],
                        Vat = firstRow["Consignee_Vat"]
                    },

                    Weight = firstRow["Box_Weight"],
                    WeightUnit = firstRow["Box_WeightUnit"],
                    Length = firstRow["Box_Length"],
                    Width = firstRow["Box_Width"],
                    Height = firstRow["Box_Height"],
                    DimUnit = firstRow["Box_DimUnit"],
                    Value = firstRow["Box_Value"],
                    Currency = firstRow["Currency"],
                    CustomsDuty = firstRow["CustomsDuty"],
                    Description = firstRow["Description"],
                    DeclarationType = firstRow["DeclarationType"],
                    DeliveryInstructions = firstRow["DeliveryInstructions"],

                    Products = dt.AsEnumerable().Select(row => new
                    {
                        Weight = row["Product_Weight"],
                        WeightUnit = row["Product_WeightUnit"],
                        Description = row["Product_Description"],
                        Sku = row["Product_Sku"],
                        HsCode = row["Product_HsCode"],
                        OriginCountry = row["Product_OriginCountry"],
                        Quantity = row["Product_Quantity"],
                        Value = row["Product_Value"]
                    }).ToList()
                }
            };

            return JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
        }

        //=============================================================================================================================================================================================
        //-- Extract Data --//
        //=============================================================================================================================================================================================

        // Get Label from JSON ------------------------------------------------------------------------------------------------------------------
        public static List<string> ExtractLabelDataJson(string jsonResponse)
        {
            List<string> savedPdfFiles = new List<string>();

            try
            {
                // Parse JSON
                JObject responseObj = JObject.Parse(jsonResponse);

                // Check for Shipment and LabelImage
                var shipment = responseObj["Shipment"];
                if (shipment == null || shipment["LabelImage"] == null)
                    throw new Exception("LabelImage not found in response.");

                string base64Label = shipment["LabelImage"].ToString();
                byte[] pdfBytes = Convert.FromBase64String(base64Label);

                // Create temp folder
                string tempFolder = Path.Combine(Path.GetTempPath(), "CarrierLabels");
                if (!Directory.Exists(tempFolder))
                    Directory.CreateDirectory(tempFolder);

                // Save PDF
                string filePath = Path.Combine(tempFolder, $"Label_1_{tcarRef}.pdf");
                File.WriteAllBytes(filePath, pdfBytes);
                savedPdfFiles.Add(filePath);
            }
            catch (Exception ex)
            {
                SessionMaintenance.LogBook("ERROR", "[WcmsApi]", "[ExtractLabelDataFromJson]", $"Error extracting label from JSON: {ex.Message}");
            }

            return savedPdfFiles;
        }

        // Get Tracking from JSON ------------------------------------------------------------------------------------------------------------------
        public static string ExtractTrackingNumberJson(string jsonResponse)
        {
            try
            {
                JObject responseObj = JObject.Parse(jsonResponse);

                var shipment = responseObj["Shipment"];
                if (shipment == null || shipment["TrackingNumber"] == null)
                    throw new Exception("TrackingNumber not found in response.");

                return shipment["TrackingNumber"].ToString();
            }
            catch (Exception ex)
            {
                SessionMaintenance.LogBook("ERROR", "[WcmsApi]", "[ExtractTrackingNumberFromJson]", $"Error extracting tracking number: {ex.Message}");
                return string.Empty;
            }
        }


    }
}
