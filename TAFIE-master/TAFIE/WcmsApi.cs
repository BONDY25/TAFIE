using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;

namespace TAFIE
{
    public class WcmsApi
    {
        //*** API DOCS: https://api.parcelhub.net/docs/ ***//

        private static readonly string apiUrl = "https://api.parcelhub.net/1.0/Shipment?RequestedLabelFormat=PDF&RequestedLabelSize=6";
        private static readonly string tknApiurl = "https://api.parcelhub.net/1.0/TokenV2";
        public static string accessToken = "";
        public static string apiAccCode = "";
        public static bool error = false;

        private const string connectionString = SessionMaintenance.connectionString;

        public static string? tcarRef = null;

        //=============================================================================================================================================================================================
        //-- API Methods --//
        //=============================================================================================================================================================================================

        // Get Access Token (Async) ---------------------------------------------------------------------------------------------------
        public static async Task GetAccessToken(string accCode, string name)
        {
            string query = "SELECT TOP 1 RTRIM([API_UserName]) AS API_UserName, RTRIM([API_Password]) AS API_Password FROM TCAR_Acct WHERE Acc_Code = @Acc_Code";
            string apiUsername = string.Empty;
            string apiPassword = string.Empty;
            string tkey = TcarControl.CreateTKey(name);

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync().ConfigureAwait(false); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Acc_Code", accCode);
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                        {
                            if (await reader.ReadAsync().ConfigureAwait(false))
                            {
                                apiUsername = reader.GetString(0);
                                apiPassword = reader.GetString(1);
                            }
                        }
                    }
                }

                // Ensure credentials are valid
                if (string.IsNullOrEmpty(apiUsername) || string.IsNullOrEmpty(apiPassword))
                {
                    throw new Exception("API credentials not found for the given account code.");
                }

                // Generate XML & Call API
                string callXml = WcmsApi.GetTknXml(apiUsername, apiPassword);
                string respXml = await WcmsApi.MakeTKNApiCall(callXml).ConfigureAwait(false);

                // Ensure response is valid
                if (!string.IsNullOrEmpty(respXml))
                {
                    WcmsApi.accessToken = WcmsApi.ExtractAccessToken(respXml);
                    string refreshTkn = WcmsApi.ExtractRefreshToken(respXml);
                    WcmsApi.InsertAccessTkn(tkey, accCode, refreshTkn);
                }
                else
                {
                    throw new Exception("Invalid response received from API.");
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occurred while getting the Access Token: \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[WcmsApi]", "[GetAccessToken]", $"FAILED: {ex.Message}");
                return;
            }
        }

        // Check Access Token ------------------------------------------------------------------------------------------------------------------
        public static bool CheckAccessTkn(string accCode)
        {
            bool check = false;
            string query = "EXECUTE [TCAR_Check_TOKN] @Acc_Code";

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {

                        cmd.Parameters.AddWithValue("@Acc_Code", accCode);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                accessToken = reader["Acc_Code"].ToString();
                            }
                        }
                    }
                    conn.Close(); // Close SQL Connection
                }

                if (accessToken != "NO TOKEN")
                {
                    check = true;
                }
            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured Checking Access Token \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[WcmsApi]", "[CheckAccessTkn]", $"FAILED: ( {ex.Message} )");
            }

            return check;
        }

        // Get XML Data ------------------------------------------------------------------------------------------------------------------
        public static DataTable GetXmlData(string tcarRef, int boxRef)
        {
            string query = "EXECUTE [TCAR_Get_XML_V2] @TCAR_Ref, @Box_Ref";

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

                SessionMaintenance.LogBook("", "[WcmsApi]", "[GetXmlData]", $"XML Data retrived for {tcarRef}, {boxRef}");
            }
            catch (Exception ex) // Catch Errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("117", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[WcmsApi]", "[GetXmlData]", $"FAILED: Code 117 ( {ex.Message} )");
            }

            return dataTable;
        }

        // Insert Access Token ------------------------------------------------------------------------------------------------------------------
        public static void InsertAccessTkn(string tKey, string accCode, string refreshTkn)
        {
            string query = "EXECUTE [TCAR_INSERT_TOKN] @tkey, @Acc_Code, @Access_Tkn, @Refresh_Tkn";

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tkey", tKey);
                        cmd.Parameters.AddWithValue("@Acc_Code", accCode);
                        cmd.Parameters.AddWithValue("@Access_Tkn", accessToken);
                        cmd.Parameters.AddWithValue("@Refresh_Tkn", refreshTkn);

                        cmd.ExecuteNonQuery();
                    }
                    conn.Close(); // Close SQL Connection
                }

            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured saving Access Token \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[WcmsApi]", "[InsertAccessTkn]", $"FAILED: Code 226 ( {ex.Message} )");
            }
        }

        // Get Customer UID ------------------------------------------------------------------------------------------------------------------       
        public static async Task<string> GetCustomerUid(int boxRef, int domestic, int ddp)
        {
            string callXml = CreateXmlString(GetXmlData(tcarRef, boxRef), null, domestic, ddp);

            // Make API Call to get label data
            string respXml = await MakeServiceApiCall(callXml);

            string ServiceCustomerUID = ExtractCustomerUID(respXml);

            SessionMaintenance.LogBook("", "[WcmsApi]", "[GetCustomerUid]", $"Customer UID Retrived: {ServiceCustomerUID}");

            return ServiceCustomerUID;
        }

        // Insert Customer UID ------------------------------------------------------------------------------------------------------------------
        private static void InsertCustomerUID(string customerUID = "ER")
        {
            string query = "UPDATE TCAR SET Customer_UID = @Customer_UID WHERE TCAR_Ref = @TCAR_Ref";

            try
            {
                // Start SQL
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open(); // Open SQL Connection

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Set Parameters
                        cmd.Parameters.AddWithValue("@Customer_UID", customerUID);
                        cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);

                        // Execute Query
                        cmd.ExecuteNonQuery();
                    }

                    conn.Close(); // Close SQL Connection
                }
            }
            catch (Exception ex) // Catch Errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("117", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[WcmsApi]", "[InsertCustomerUID]", $"FAILED: Code 117 ( {ex.Message} )");
            }
        }

        //=============================================================================================================================================================================================
        //-- Make API Calls --//
        //=============================================================================================================================================================================================

        // API Call for Get Services -----------------------------------------------------------------------------------------------------------------------
        public static async Task<string> MakeServiceApiCall(string callXml)
        {
            string responseData = "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    if (!error)
                    {
                        SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeServiceApiCall]", $"Get Services Requested");
                        string requestUrl = "https://api.parcelhub.net/1.0/Service?AccountId=";  // Use the API URL directly

                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        client.DefaultRequestHeaders.Add("User-Agent", "WFSW CMS");

                        //Log the request headers
                        var headersLog = new StringBuilder();
                        headersLog.AppendLine("API Request Headers:");
                        foreach (var header in client.DefaultRequestHeaders)
                        {
                            headersLog.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                        }
                        SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeTKNApiCall]", headersLog.ToString());

                        // Log the request
                        SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeServiceApiCall]", $"API Call Attempted\n{callXml}");

                        // Prepare the request content
                        HttpContent content = new StringContent(callXml, Encoding.UTF8, "application/xml");

                        // Send the request as POST
                        HttpResponseMessage response = await client.PostAsync(requestUrl, content);
                        responseData = await response.Content.ReadAsStringAsync();
                        SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeServiceApiCall]", $"API Response Headers:\n{response}");
                        SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeServiceApiCall]", $"API Response Body:\n{responseData}");
                        // Log the response status
                        if (!response.IsSuccessStatusCode)
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            CustomMessageBox messageBox = new CustomMessageBox();
                            messageBox.ShowError($"An error occured when retrieving services: \n{errorContent}");
                            SessionMaintenance.LogBook("ERROR", "[WcmsApi]", "[MakeServiceApiCall]", $"FAILED: API Call Failed {errorContent}");
                            error = true;  // Set error flag
                            return errorContent;  // Return error details
                        }
                        // If successful, return the response data                    
                        SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeServiceApiCall]", $"API Call Successful");
                    }

                    return responseData;
                }
                catch (HttpRequestException ex)
                {
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowError($"Request error: {ex.Message}");
                    SessionMaintenance.LogBook("ERROR", "[WcmsApi]", "[MakeServiceApiCall]", $"FAILED: ( {ex.Message} )");
                    throw;
                }
            }
        }

        // API Call for Token -----------------------------------------------------------------------------------------------------------------------
        public static async Task<string> MakeTKNApiCall(string CallXml)
        {
            string responseData = "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    if (!error)
                    {
                        SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeTKNApiCall]", $"TOKEN REQUEST XML:\n{CallXml}");
                        string requestUrl = tknApiurl;  // Use the API URL directly

                        client.DefaultRequestHeaders.Add("User-Agent", "WFSW CMS");

                        //Log the request headers
                        var headersLog = new StringBuilder();
                        headersLog.AppendLine("API Request Headers:");
                        foreach (var header in client.DefaultRequestHeaders)
                        {
                            headersLog.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                        }
                        SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeTKNApiCall]", headersLog.ToString());

                        // Log the request
                        SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeTKNApiCall]", $"API Call Attempted\n{CallXml}");

                        // Prepare the request content
                        HttpContent content = new StringContent(CallXml, Encoding.UTF8, "application/xml");

                        // Send the request as POST
                        HttpResponseMessage response = await client.PostAsync(requestUrl, content);
                        responseData = await response.Content.ReadAsStringAsync();
                        SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeTKNApiCall]", $"API Response Headers:\n{response}");
                        SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeTKNApiCall]", $"API Response Body:\n{responseData}");

                        // Log the response status
                        if (!response.IsSuccessStatusCode)
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            CustomMessageBox messageBox = new CustomMessageBox();
                            messageBox.ShowError($"An error occured when retrieving token: \n{errorContent}");
                            SessionMaintenance.LogBook("ERROR", "[WcmsApi]", "[MakeTKNApiCall]", $"FAILED: API Call Failed {errorContent}");
                            error = true;  // Set error flag
                            return errorContent;  // Return error details
                        }
                        // If successful, return the response data                    
                        SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeTKNApiCall]", $"API Call Successful");
                    }

                    return responseData;
                }
                catch (HttpRequestException ex)
                {
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowError($"Request error: {ex.Message}");
                    SessionMaintenance.LogBook("ERROR", "[WcmsApi]", "[MakeTKNApiCall]", $"FAILED: ( {ex.Message} )");
                    throw;
                }

            }
        }

        // Make API Call to get label ------------------------------------------------------------------------------------------------------------------        
        public static async Task<string> MakeCarrierApiCall(string CallXml, string tKey)
        {
            string responseData = "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    if (!error)
                    {
                        // Set the API endpoint URL
                        string requestUrl = apiUrl;

                        // Set the Authorization header
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        client.DefaultRequestHeaders.Add("User-Agent", "WFSW CMS");

                        //Log the request headers
                        var headersLog = new StringBuilder();
                        headersLog.AppendLine("API Request Headers:");
                        foreach (var header in client.DefaultRequestHeaders)
                        {
                            headersLog.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                        }
                        SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeTKNApiCall]", headersLog.ToString());

                        // Log the request attempt
                        SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeCarrierApiCall]", $"API Call Attempted With token: {accessToken}");

                        // Prepare the request body with XML content
                        HttpContent content = new StringContent(CallXml, Encoding.UTF8, "application/xml");

                        // Make the API call using POST
                        HttpResponseMessage response = await client.PostAsync(requestUrl, content);

                        // Read and return the response XML
                        responseData = await response.Content.ReadAsStringAsync();

                        SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeCarrierApiCall]", $"Response:\n{response}");

                        // Log the response status
                        if (!response.IsSuccessStatusCode)
                        {
                            // Extract Error
                            string errorMessage = ExtractErrorMessage(responseData);

                            // Show Error
                            CustomMessageBox messageBox = new CustomMessageBox();
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

                            // Insert XML
                            TcarControl.InsertPayload(responseData, tKey, 3);

                            // Return Error response
                            string errorContent = await response.Content.ReadAsStringAsync();
                            return errorContent;
                        }

                        SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeCarrierApiCall]", $"API Call Successful");

                        // Insert response XML into database
                        TcarControl.InsertPayload(responseData, tKey, 2);
                    }
                    return responseData;
                }
                catch (HttpRequestException ex)
                {
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowError($"Request error: {ex.Message}");
                    SessionMaintenance.LogBook("ERROR", "[WcmsApi]", "[MakeCarrierApiCall]", $"FAILED: ( {ex.Message} )");
                    throw;
                }
            }
        }

        //=============================================================================================================================================================================================
        //-- Create XMLS --//
        //=============================================================================================================================================================================================

        // Create XML For token request -----------------------------------------------------------------------------------------------------------------------
        public static string GetTknXml(string apiUsername, string apiPassword)
        {

            var token = new XElement("RequestToken",
                         new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                        new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                        new XElement("grant_type", "bearer"),
                        new XElement("username", apiUsername),
                        new XElement("password", apiPassword)

                );

            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                token
            );

            string xmlString = doc.ToString(); // Convert XML to string

            return $"<?xml version=\"1.0\" encoding=\"utf-8\"?> {xmlString}";
        }

        // Create XML String ------------------------------------------------------------------------------------------------------------------
        public static string CreateXmlString(DataTable dt, string? ServiceCustomerUID = null, int domestic = 1, int ddp = 0)
        {
            if (dt.Rows.Count == 0) return string.Empty;  // Ensure there's data

            DataRow firstRow = dt.Rows[0];  // Use first row for shipment-level data

            string serviceProviderId = firstRow["ServiceProviderId"].ToString();

            XNamespace ns = "http://api.parcelhub.net/schemas/api/parcelhub-api-v0.4.xsd";

            // Namespace //
            var shipment = new XElement(ns + "Shipment",
                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),

                new XElement(ns + "Account", firstRow["Account"]),

                new XElement(ns + "CollectionDetails",
                    new XElement(ns + "CollectionDate", DateTime.Now.ToString("yyyy-MM-dd")),
                    new XElement(ns + "CollectionReadyTime", DateTime.Now.ToString("HH:mm:ss")),
                    new XElement(ns + "LocationCloseTime", firstRow["LocationCloseTime"])
                ),

                // CollectionAddress //
                new XElement(ns + "CollectionAddress",
                    new XElement(ns + "ContactName", new XCData(firstRow["Sender_Name"].ToString())),
                    new XElement(ns + "CompanyName", new XCData(firstRow["Sender_Company"].ToString())),
                    new XElement(ns + "Email", new XCData(firstRow["Sender_Email"].ToString())),
                    new XElement(ns + "Phone", new XCData(firstRow["Sender_Phone"].ToString())),
                    new XElement(ns + "Address1", new XCData(firstRow["Sender_AddressLine1"].ToString())),
                    new XElement(ns + "Address2", new XCData(firstRow["Sender_AddressLine2"].ToString())),
                    new XElement(ns + "City", new XCData(firstRow["Sender_City"].ToString())),
                    new XElement(ns + "Area", new XCData(firstRow["Sender_State"].ToString())),
                    new XElement(ns + "Postcode", new XCData(firstRow["Sender_Zip"].ToString())),
                    new XElement(ns + "Country", firstRow["Sender_Country"])
                ),
                // DeliveryAddress //
                new XElement(ns + "DeliveryAddress",
                    new XElement(ns + "ContactName", new XCData(firstRow["ContactName"].ToString())),
                    new XElement(ns + "CompanyName", new XCData(firstRow["CompanyName"].ToString())),
                    new XElement(ns + "Email", new XCData(firstRow["Email"].ToString())),
                    new XElement(ns + "Phone", new XCData(firstRow["Phone"].ToString())),
                    new XElement(ns + "Address1", new XCData(firstRow["Address1"].ToString())),
                    new XElement(ns + "Address2", new XCData(firstRow["Address2"].ToString())),
                    new XElement(ns + "City", new XCData(firstRow["City"].ToString())),
                    new XElement(ns + "Area", new XCData(firstRow["Area"].ToString())),
                    new XElement(ns + "Postcode", new XCData(firstRow["Postcode"].ToString())),
                    new XElement(ns + "Country", firstRow["Country"]),
                    new XElement(ns + "AddressType", firstRow["AddressType"])
                ),

                // Headers //
                new XElement(ns + "Reference1", firstRow["Reference1"]),
                new XElement(ns + "Reference2", firstRow["Reference2"]),
                new XElement(ns + "SpecialInstructions", new XCData(firstRow["SpecialInstructions"].ToString())),
                new XElement(ns + "ContentsDescription", firstRow["ContentsDescription"]),
                new XElement(ns + "CurrencyCode", firstRow["CurrencyCode"]),
                new XElement(ns + "HasBeenManifested", firstRow["HasBeenManifested"]),
                new XElement(ns + "Department"),

                // CustomsDeclarationInfo //
                new XElement(ns + "CustomsDeclarationInfo",
                    new XElement(ns + "TermsOfTrade", firstRow["TermsOfTrade"]),
                    new XElement(ns + "CategoryOfItem", firstRow["CategoryOfItem"]),
                    domestic == 0 && ddp == 1 ? new XElement(ns + "RecipientTaxIDCountry", firstRow["RecipientTaxIDCountry"]) : null,
                    domestic == 0 && ddp == 1 ? new XElement(ns + "IOSSNumber", firstRow["IOSSNumber"]) : null,
                    domestic == 0 && ddp == 1 ? new XElement(ns + "PostalCharges", firstRow["PostalCharges"]) : null,
                    domestic == 0 && ddp == 1 ? new XElement(ns + "IOSSNumberCountry", firstRow["IOSSNumberCountry"]) : null,
                    domestic == 0 && ddp == 1 ? new XElement(ns + "RecipientVATNumber", firstRow["RecipientVATNumber"]) : null,
                    domestic == 0 && ddp == 1 ? new XElement(ns + "RecipientVATNumberCountry", firstRow["RecipientVATNumberCountry"]) : null,
                    domestic == 0 && ddp == 1 ? new XElement(ns + "DutyBillingTerm", firstRow["DutyBillingTerm"]) : null,
                    serviceProviderId == "59" ? new XElement(ns + "CarriageValue", firstRow["CarriageValue"]) : null,
                    serviceProviderId == "59" ? new XElement(ns + "InsuranceValue", firstRow["InsuranceValue"]) : null
                ),

                // ServiceInfo //
                new XElement(ns + "ServiceInfo",
                    new XElement(ns + "ServiceId", firstRow["ServiceId"]),
                    new XElement(ns + "ServiceCustomerUID", string.IsNullOrEmpty(ServiceCustomerUID) ? firstRow["ServiceCustomerUID"] : ServiceCustomerUID),
                    new XElement(ns + "ServiceProviderId", firstRow["ServiceProviderId"])
                ),

                // Packages //
                new XElement(ns + "Packages",
                    new XElement(ns + "Package",
                        new XElement(ns + "PackageType", firstRow["PackageType"]),

                        // Dimensions //
                        new XElement(ns + "Dimensions",
                            new XElement(ns + "Length", firstRow["Length"]),
                            new XElement(ns + "Width", firstRow["Width"]),
                            new XElement(ns + "Height", firstRow["Height"])
                        ),

                        new XElement(ns + "Weight", firstRow["Weight"]),
                        new XElement(ns + "Value", new XAttribute("Currency", firstRow["CurrencyCode"]), firstRow["Value"]),
                        new XElement(ns + "Contents", firstRow["Contents"]),

                        new XElement(ns + "PackageCustomsDeclaration",
                            new XElement(ns + "Quantity", firstRow["Qty"]),
                            new XElement(ns + "Value", new XAttribute("Currency", firstRow["CurrencyCode"]), firstRow["Value"])
                        ),

                        // ItemLevelDeclarations //
                        // Create multiple ItemLevelDeclaration elements for each SKU
                        new XElement(ns + "ItemLevelDeclarations",
                            dt.AsEnumerable().Select(row =>
                                new XElement(ns + "ItemLevelDeclaration",
                                    new XElement(ns + "ProductSKU", row["ProductSKU"]),
                                    new XElement(ns + "ProductDescription", new XCData(row["ProductDescription"].ToString())),
                                    new XElement(ns + "ProductType", new XCData(row["ProductType"].ToString())),
                                    new XElement(ns + "ProductValue", row["ProductValue"]),
                                    new XElement(ns + "ProductQuantity", row["ProductQuantity"]),
                                    new XElement(ns + "ProductWeight", row["ProductWeight"]),
                                    new XElement(ns + "ProductCountryOfOrigin", row["ProductCountryOfOrigin"]),
                                    new XElement(ns + "ProductHarmonisedCode", row["ProductHarmonisedCode"])
                                )
                            )
                        )
                    )
                )
            );

            // Create XML Doc
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), shipment);
            SessionMaintenance.LogBook("", "[WcmsApi]", "[CreateXmlString]", $"XML String Created");

            string xmlString = doc.ToString(); // Convert XML to string

            return $"<?xml version=\"1.0\" encoding=\"utf-8\"?> {xmlString}"; // Return XML string with XML declaration
        }

        //=============================================================================================================================================================================================
        //-- Extract Data --//
        //=============================================================================================================================================================================================

        // Get Tracking from XML ------------------------------------------------------------------------------------------------------------------
        public static string ExtractTrackingNumber(string xmlResponse)
        {
            try
            {
                XDocument doc = XDocument.Parse(xmlResponse);
                XNamespace ns = "http://api.parcelhub.net/schemas/api/parcelhub-api-v0.4.xsd"; // Extract namespace

                XElement trackingElement = doc.Descendants(ns + "CourierTrackingNumber").FirstOrDefault();

                return trackingElement != null ? trackingElement.Value : "Tracking number not found";
            }
            catch (Exception ex)
            {
                SessionMaintenance.LogBook("", "[WcmsApi]", "[ExtractTrackingNumber]", $"Error: {ex.Message}");
                return "Error extracting tracking number";
            }
        }

        // Get Label from XML ------------------------------------------------------------------------------------------------------------------
        public static List<string> ExtractLabelData(string xmlResponse)
        {
            List<string> savedPdfFiles = new List<string>();

            try
            {
                XDocument doc = XDocument.Parse(xmlResponse);
                XNamespace ns = "http://api.parcelhub.net/schemas/api/parcelhub-api-v0.4.xsd"; // Extract namespace

                var labelElements = doc.Descendants(ns + "LabelData"); // Use namespace

                int labelIndex = 1;
                string tempFolder = Path.Combine(Path.GetTempPath(), "CarrierLabels");

                if (!Directory.Exists(tempFolder))
                    Directory.CreateDirectory(tempFolder);

                foreach (var label in labelElements)
                {
                    try
                    {
                        string base64Label = label.Value;
                        byte[] pdfBytes = Convert.FromBase64String(base64Label);

                        string filePath = Path.Combine(tempFolder, $"Label_{labelIndex}_{tcarRef}.pdf");
                        File.WriteAllBytes(filePath, pdfBytes);
                        savedPdfFiles.Add(filePath);

                        labelIndex++;
                    }
                    catch (Exception ex)
                    {
                        SessionMaintenance.LogBook("", "[WcmsApi]", "[ExtractLabelData]", $"Error decoding label: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                SessionMaintenance.LogBook("", "[WcmsApi]", "[ExtractLabelData]", $"Error parsing XML: {ex.Message}");
            }

            return savedPdfFiles;
        }

        // Get Access from XML ------------------------------------------------------------------------------------------------------------------
        public static string ExtractAccessToken(string xmlResponse)
        {
            try
            {
                XDocument doc = XDocument.Parse(xmlResponse);
                XElement accessTkn = doc.Descendants("access_token").FirstOrDefault();
                return accessTkn != null ? accessTkn.Value : "Access Token Not found";
            }
            catch (Exception ex)
            {
                SessionMaintenance.LogBook("", "[WcmsApi]", "[ExtractAccessToken]", $"Error: {ex.Message}");
                return "Error extracting Access Token";
            }
        }

        // Get Refresh from XML ------------------------------------------------------------------------------------------------------------------
        public static string ExtractRefreshToken(string xmlResponse)
        {
            try
            {
                XDocument doc = XDocument.Parse(xmlResponse);
                XElement refreshTkn = doc.Descendants("refreshToken").FirstOrDefault();
                return refreshTkn != null ? refreshTkn.Value : "Refresh Token Not found";
            }
            catch (Exception ex)
            {
                SessionMaintenance.LogBook("", "[WcmsApi]", "[ExtractRefreshToken]", $"Error: {ex.Message}");
                return "Error extracting Refresh Token";
            }
        }

        // Get Error Message from XML ------------------------------------------------------------------------------------------------------------------
        public static string ExtractErrorMessage(string xmlResponse)
        {
            try
            {
                XDocument doc = XDocument.Parse(xmlResponse);
                XElement accessTkn = doc.Descendants("Message").FirstOrDefault();
                return accessTkn != null ? accessTkn.Value : "Error Not found";
            }
            catch (Exception ex)
            {
                SessionMaintenance.LogBook("", "[WcmsApi]", "[ExtractErrorMessage]", $"Error: {ex.Message}");
                return "Error extracting Error";
            }
        }

        // Get CustomerUID from XML ------------------------------------------------------------------------------------------------------------------
        public static string ExtractCustomerUID(string xmlResponse)
        {
            string customerUID = "00000"; // Default value
            try
            {
                XDocument doc = XDocument.Parse(xmlResponse);

                // Get the namespace from the root element
                XNamespace ns = doc.Root.GetDefaultNamespace();

                // Search for ServiceCustomerUID using the namespace
                XElement ServiceCustomerUID = doc.Descendants(ns + "ServiceCustomerUID").FirstOrDefault();

                customerUID = ServiceCustomerUID != null ? ServiceCustomerUID.Value : "00000";

                InsertCustomerUID(customerUID);

                return customerUID;
            }
            catch (Exception ex)
            {
                SessionMaintenance.LogBook("", "[WcmsApi]", "[ExtractCustomerUID]", $"Error: {ex.Message}");
                return "Error extracting ServiceCustomerUID";
            }
        }
    }
}
