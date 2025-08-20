[Back](ClassIndex.md)

# TAFIE Carrier Module - ProCarrApi - Methods

## 8.1 - Methods - Initialization

### 8.1.1 - Initialization and declaration

This section of the `ProCarrApi` class declares the essential global variables required for making API calls to request carrier labels, as well as for interacting with the database and tracking user/session information.

The following variables are declared:
* `apiUrl`: The URL used to make the API call for requesting a carrier label
* `connectionString`: The SQL Server connection string that allows the application to interact with the database. It connects to the `Appz` database on the server SQL-SSRS using Windows authentication.
* `userName`: A property that holds the username of the user interacting with the application. This value is passed from outside the class.
* `tcarRef`: A nullable string used to store the `TCAR` reference for the shipment label request. This unique reference is essential for tracking the shipment request throughout the application and is updated dynamically during the flow of the application (e.g., after executing the stored procedure for creating a new `TCAR` record).

This section of code is critical in ensuring that the class has all the necessary data for the smooth operation of the carrier label request process.

```cs
internal class ProCarrApi
    {
        private static readonly string apiUrl = "https://dgapi.app/API/";
        private const string connectionString = "Server=SQL-SSRS;Database=Appz;Integrated Security=True;Encrypt=False;";

        public static string userName { get; set; }

        public static string? tcarRef = null;
```

---

## 8.2 - API Methods

### 8.2.1 - GetJsonData

The `GetJsonData` method retrieves structured JSON-related data from a SQL Server database using a stored procedure. It accepts a specific `TCAR` reference and Box reference as parameters, executes the stored procedure `TCAR_Get_JSON`, and returns the resulting data in a `DataTable`. This method is used to fetch pre-formatted content tied to specific load note records.

#### Step-by-Step Breakdown

* Define SQL Query
    * The method specifies a query string that executes a stored procedure:

```sql
EXECUTE [TCAR_Get_JSON] @TCAR_Ref, @Box_Ref
```

* Initialize a `DataTable`
    * An empty `DataTable` object is created to hold the results retrieved from the SQL Server.

* Establish SQL Connection
    * A `SqlConnection` object is instantiated using the application's connection string, and the connection is opened.

* Prepare SQL Command
    * A `SqlCommand` is constructed with the query and the open connection. Two parameters (`@TCAR_Ref` and `@Box_Ref`) are added to the command using values provided by the method's input arguments.

* Execute the Stored Procedure
    * `cmd.ExecuteNonQuery()` is called.
    * A `SqlDataReader` is then used to fetch the result set from the procedure execution.

* Populate the DataTable
    * The data retrieved by the `SqlDataReader` is loaded into the previously created `DataTable`.

* Close the Connection
    * The SQL connection is closed explicitly after data retrieval is complete.

* Log Success
    * A log entry is recorded to indicate that JSON data was successfully retrieved for the given TCAR and Box references.

* Handle Exceptions
    * If any errors occur during the process:
        * A custom error message box is displayed with a predefined error code and the exception message.
        * An error log is written to track the failure and its reason.

* Return the Result
    * The populated (or empty, if failed) `DataTable` is returned to the calling code.

#### Summary

The `GetJsonData` method retrieves JSON-formatted data for a specific `TCAR` and Box reference by executing a stored procedure on the SQL Server. It handles parameterized SQL execution, populates a `DataTable` with results, includes full exception handling, and logs both successes and failures. This method plays a key role in accessing backend JSON structures required for downstream processes like label generation & external API interfacing.

```cs
// Get JSON Data from SQL Server for a specific TCAR and Box Reference --------------------=---------------------------------------------
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
```

---

### 8.2.2 - MakeLabelApiCall

The `MakeLabelApiCall` method is a static asynchronous function designed to interact with the external Pro Carrier API to request shipping label information. It takes a JSON payload containing shipment details, and a transaction key, sending the payload to a predefined API endpoint. The method is robust, incorporating comprehensive error handling for both network-level and application-level issues, and it logs various stages of the API interaction. It aims to be the primary point of contact for requesting labels from the Pro Carrier API.

#### Step-by-Step Breakdown

* Method Signature and `HttpClient` Initialization:
    * The method is `async` because it performs asynchronous operations (like network calls), and it returns a `Task<string>`, indicating it will eventually return a string (the API response) when the asynchronous operation completes.
    * It takes two arguments: `jsonPayload` (a `string` containing the JSON request body) and `tKey` (a `string` used as a unique transaction key to identify the request.).
    * Creates an instance of `HttpClient` within a using statement. This ensures the `HttpClient` is properly disposed of after use, preventing resource leaks.

```cs
public static async Task<string> MakeLabelApiCall(string jsonPayload, string tKey)
        {
            using (HttpClient client = new HttpClient())
            {
```

* `HttpClient` Configuration:
    * Sets the `Accept` header to `”application/json”` inside the `try` block. This tells the API that the client prefers to receive responses in JSON format.
    * Configures a 30-second timeout for the API call. If the API doesn't respond within this period, a `TaskCanceledException` (or `HttpRequestException` in some .NET versions) will be thrown.

```cs
try
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.Timeout = TimeSpan.FromSeconds(30);
```

* API Call Logging:
    * Logs a message indicating that an API call attempt is being made. This is useful for monitoring the application's interaction with external services.

```cs
SessionMaintenance.LogBook("", "[ProCarrApi]", "[MakeLabelApiCall]", "API Call Attempted");
```

* Preparing Request Content:
    * Creates the HTTP request body. It wraps the `jsonPayload` string, specifies UTF-8 encoding, and sets the content type to `"application/json"`.

```cs
var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
```

* Sending the POST Request:
    * Sends an asynchronous HTTP POST request.
        * `apiUrl` is a global variable representing the target API endpoint.
        * `content` is the `StringContent` created in the previous step.
        * The `await` keyword pauses execution of this method until the API call completes and the `HttpResponseMessage` is received.

```cs
 HttpResponseMessage response = await client.PostAsync(apiUrl, content);
```

* Reading and Logging Response Data:
    * Asynchronously reads the entire content of the HTTP response into a string.
    * Logs the full response data received from the API, which is crucial for debugging and auditing.

```cs
 string responseData = await response.Content.ReadAsStringAsync();
 SessionMaintenance.LogBook("", "[ProCarrApi]", "[MakeLabelApiCall]", $"Response Data:\n{responseData}");
```

* Transport-Level Error Checking:
    * Checks if the HTTP status code of the response indicates success (i.e., status codes in the 2xx range). This catches network-level errors, server-side errors, or issues with the request format that prevent a successful HTTP transaction.
    * If the status code is not successful:
        * Creates an error message including the HTTP status code and the response content.
        * Calls a helper method (`ShowError`) to display this error to the user interface.
        * Logs the failure.
        * Calls the method `TcarControl.InsertPayload` to log the response data, `tKey`, and a mode into a database.
        * Returns the raw `responseData` even in case of failure, allowing the calling code to potentially parse it for more details.

```cs
if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = $"Status: {response.StatusCode}, Content: {responseData}";
                        ShowError(errorMessage);
                        SessionMaintenance.LogBook("ERROR", "[ProCarrApi]", "[MakeLabelApiCall]", $"FAILED: {errorMessage}");
                        TcarControl.InsertPayload(responseData, tKey, 3);
                        return responseData;
                    }
```

* Application-Level Error Checking (Internal API Error):
    * Calls a nested helper method `ParseInternalApiError` to inspect the JSON `responseData` for application-specific error indicators.
    * Checks if the `ParseInternalApiError` method found an application-level error.
    * If an application error is found:
        * Displays the application-specific error message.
        * Logs this failure.
        * Again logs the payload.
        * Returns the raw response data.

```cs
 // Check for application-level error in JSON
 var apiError = ParseInternalApiError(responseData);
 if (apiError != null)
 {
    ShowError(apiError);
    SessionMaintenance.LogBook("ERROR", "[ProCarrApi]", "[MakeLabelApiCall]", $"FAILED: {apiError}");
    TcarControl.InsertPayload(responseData, tKey, 3);
    return responseData;
 }
```

* Successful API Call Handling:
    * If both transport-level and application-level checks pass, the API call is considered successful.
    * Logs the successful response payload.
    * Logs a success message.
    * Returns the successful API response data.

```cs
 TcarControl.InsertPayload(responseData, tKey, 2);
 SessionMaintenance.LogBook("", "[ProCarrApi]", "[MakeLabelApiCall]", "API Call Successful");
 return responseData;
```

* Catching `HttpRequestException`:
    * Specifically catches exceptions related to HTTP requests (e.g., network issues, DNS resolution failures, connection refused, or the 30-second timeout).
    * Creates an error message from the exception.
    * Displays the error.
    * Logs the specific request error.
    * Re-throws the exception. This means that while the error is logged and displayed, the calling code will still receive an exception, allowing for further upstream error handling if needed.

```cs
 catch (HttpRequestException ex)
      {
         string error = $"Request error: {ex.Message}";
         ShowError(error);
         SessionMaintenance.LogBook("ERROR", "[ProCarrApi]", "[MakeLabelApiCall]", $"FAILED: ( {error} )");
         throw;
      }
```

* Helper Method: `ParseInternalApiError`:
    * This nested `static` method attempts to parse the JSON string to check for an application-specific error structure.
    * It specifically looks for an `"ErrorLevel"` property. If it exists and its integer value is greater than 0, it then tries to extract the message from an `"Error"` property.
    * It returns the error message if found, "API returned error with no message." if `ErrorLevel` > 0 but no `Error` property, or `null` if no internal error is detected or if parsing fails.

```cs
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
```

* Helper Method: `ShowError`:
    * This nested `static` method is responsible for displaying error messages to the user interface.
    * It instigates a `CustomMessageBox`, sets its background color to red (for error visual cue), calls `ShowError` on it and then resets the background color.

```cs
// Helper method to show errors via UI
static void ShowError(string errorMessage)
{
   var messageBox = new CustomMessageBox();
   messageBox.lblDescription.BackColor = Color.Red;
   messageBox.ShowError(errorMessage);
   messageBox.lblDescription.BackColor = Color.FromArgb(11, 159, 161);
}
```

#### Summary

The `MakeLabelApiCall` method orchestrates the complete lifecycle of an API request for shipping label data. It meticulously handles the setup of the HTTP client, sends the JSON payload, and then performs multi-layered error checking: first for HTTP transport-level success, and then for application-specific errors embedded within the JSON response. Comprehensive logging is integrated throughout to provide visibility into the API interaction. In case of any failures, relevant error messages are displayed to the user and logged, and the raw response data (or an exception for network issues) is returned, enabling robust error management in the calling application.

```cs
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
                messageBox.lblDescription.BackColor = Color.Red;
                messageBox.ShowError(errorMessage);
                messageBox.lblDescription.BackColor = Color.FromArgb(11, 159, 161);
            }
        }
```

---

## 8.3 - API Methods

### 8.3.1 - CreateJsonString

The `CreateJsonString` method constructs a structured JSON string from a `DataTable` containing shipment and product data. It extracts shipment-level details from the first row and compiles a list of product details from all rows. The resulting JSON object follows the schema required for external carrier label APIs and is serialized using `JsonConvert`.

#### Step-by-Step Breakdown

* Check for Empty Data
    * The method first checks if the input `DataTable` has any rows. If not, it returns an empty string, as no JSON can be created from an empty table.

* Extract the First Row
    * The first row is treated as the source of general shipment and address-level data, assuming that this metadata is consistent across all rows in the table.

* Construct the JSON Object
    * A nested anonymous object is built to match the expected JSON structure:
        * Top-level Properties:
            * `Apikey`: Converted to lowercase.
            * `Command`: Passed through directly.
        * Shipment Object:
            * Includes properties such as `RequireCarrierTrackingNumber`, `LabelOption`, `LabelFormat`, and other shipment metadata.
            * `SenderAddress` and `ConsigneeAddress` sub-objects contain full contact and location information for both parties.
            * Box dimensions and metadata (`Weight`, `Value`, `Currency`, etc.) are extracted from the same row.

* Create Product List
    * A LINQ query (`dt.AsEnumerable().Select`) loops through each row in the `DataTable` to construct a `Products` list, where each product includes:
        * `Weight`, `WeightUnit`
        * `Description`, `Sku`, `HsCode`
        * `OriginCountry`, `Quantity`, `Value`

* Serialize to JSON
    * The entire object is serialized into a JSON-formatted string using `JsonConvert.SerializeObject`, with indentation enabled for readability.

#### Summary

The `CreateJsonString` method transforms a `DataTable` into a formatted JSON string that encapsulates both shipment-level data and a list of products. It assumes the first row contains all overarching shipment information and that all rows represent individual product line items. This method prepares data for API submission in a format compatible with external shipping or logistics services, enabling structured, machine-readable communication.

```cs   
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
```
        
---

## 8.4 - Extract Data

### 8.4.1 - ExtractLabelDataJson

The `ExtractLabelDataJson` method is a static method, specifically crafted to process JSON responses that contain a Base64 encoded shipping label. Its primary function is to parse this JSON, decode the Base64 string into a binary PDF, and then save this PDF to a temporary directory on the system. The method is designed to provide a robust mechanism for handling shipping label data received from an API, incorporating error handling for various failure scenarios.

#### Step-by-Step Breakdown

* Method Signature and Initialization:
    * The method accepts a single argument, `jsonResponse`, which is a `string` expected to contain a JSON payload with label data.
    * The method is designed to return a `List<string>`, which will contain the file paths of the saved PDF labels.
    * It initializes an empty list to store the paths of any successfully saved PDF files. This list will be returned at the end of the method.

```cs
public static List<string> ExtractLabelDataJson(string jsonResponse)
        {
            List<string> savedPdfFiles = new List<string>();
```

* Error Handling (Try-Catch Block):
    * The entire label extraction and saving process is enclosed within a `try-catch` block. This ensures that any exceptions that occur during JSON parsing, Base64 decoding, or file operations are caught and handled gracefully, preventing application termination.

* JSON Parsing:
    * Inside the `try` block, `JObject responseObj = JObject.Parse(jsonResponse);` uses the Json.NET library to parse the input `jsonResponse` string into a `JObject`. This object representation allows for easy navigation and extraction of data from the JSON structure.

* Checking for "Shipment" and "LabelImage" Nodes:
    * The method attempts to retrieve the JSON element corresponding to the "Shipment" key.
    * Performs crucial validation:
        * Checks if the "Shipment" node exists at all.
        * If "Shipment" exists, this checks for the presence of a "LabelImage" property within it.
    * If either of these conditions is false, and neither exists, it indicates that the expected label data is not present in the JSON, and an `Exception` is thrown with the message "LabelImage not found in response."

```cs
var shipment = responseObj["Shipment"];
                if (shipment == null || shipment["LabelImage"] == null)
                    throw new Exception("LabelImage not found in response.");
```

* Base64 Decoding of Label Data:
    * If the "LabelImage" is found:
        * The method then extracts the value of the "LabelImage" property as a `string`. (This string is expected to be a Base64 encoded representation of a PDF)
        * Decodes the `base64Label` string back into its original binary format (an array of bytes), which represents the PDF content.

```cs
string base64Label = shipment["LabelImage"].ToString();
                byte[] pdfBytes = Convert.FromBase64String(base64Label);
```

* Temporary Folder Creation:
    * Constructs the path for a temporary directory where the PDF label will be saved. It uses `Path.GetTempPath()` to get the system's temporary directory and appends "CarrierLabels" to create a dedicated subfolder.
    * Checks if this temporary folder already exists.
    * Creates the folder if it does not exist.

```cs
string tempFolder = Path.Combine(Path.GetTempPath(), "CarrierLabels");
                if (!Directory.Exists(tempFolder))
                    Directory.CreateDirectory(tempFolder);
```

* Saving the PDF File:
    * Generates a unique file path for the PDF. It combines the `tempFolder` path with a filename constructed using "Label_1_", a placeholder variable `tcarRef`  and the ".pdf" extension.
    * Writes the decoded PDF bytes (`pdfBytes`) to the specified `filePath`, effectively saving the PDF label to the temporary directory.
    * Adds the path of the newly saved PDF file to the `savedPdfFiles` list.

```cs
string filePath = Path.Combine(tempFolder, $"Label_1_{tcarRef}.pdf");
                File.WriteAllBytes(filePath, pdfBytes);
                savedPdfFiles.Add(filePath);
```

* Exception Handling (Catch Block):
    * If any exception occurs during the execution of the `try` block:
        * Logs the error details using the `SessionMaintenance.LogBook` method. The log includes the error level, source components, and the specific error message from the caught exception.

```cs
 catch (Exception ex)
            {
                SessionMaintenance.LogBook("ERROR", "[WcmsApi]", "[ExtractLabelDataFromJson]", $"Error extracting label from JSON: {ex.Message}");
            }
```

* Return Value:
    * Finally, `return savedPdfFiles;` returns the list of saved PDF file paths. In a successful execution, this list will contain the path to the newly created label PDF. If an error occurred, the list would be empty.

### Summary

The `ExtractLabelDataJson` method provides a comprehensive solution for handling shipping label data embedded within JSON responses. It efficiently parses the JSON, decodes Base64 encoded PDF data, and securely saves the resulting PDF files to a temporary location on the system. The method is designed with robust error handling, logging any issues that arise during the extraction, decoding, or saving processes. This functionality is crucial for applications that need to dynamically generate and manage shipping labels based on API responses, ensuring reliability and proper resource management.

```cs
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
```

---

### 8.4.2 - ExtractTrackingNumberJson

The `ExtractTrackingNumberJson` method serves as a utility function within a C# application, specifically purposed for parsing JSON responses to retrieve a shipment tracking number. It is a static method, indicating it can be called directly on the class without needing an instance of the class. The method aims to provide a robust way to access specific data nested within a JSON structure, incorporating error handling for scenarios where the expected data is absent or the JSON is malformed.

#### Step-by-Step Breakdown

* Method Signature and Input:
    * The method accepts a single argument, `jsonResponse`, which is a string expected to contain a JSON payload.
    * The method is designed to return a `string`, which will be the extracted tracking number, or an empty string if an error occurs.

```cs
 public static string ExtractTrackingNumberJson(string jsonResponse)
```

* Error Handling (Try-Catch Block):
    * The core logic of the method is encapsulated within a `try-catch` block. This ensures that any exceptions arising during the JSON parsing or data extraction process are gracefully handled, preventing application crashes.

* JSON Parsing:
    * Inside the `try` block, the first operation is `JObject responseObj = JObject.Parse(jsonResponse);`. This line utilizes the Json.NET library (indicated by `JObject`) to parse the input `jsonResponse` string into a `JObject` instance. A JObject represents a JSON object, allowing for easy navigation and querying of its constituent elements.

* Accessing "Shipment" Node:
    * Attempts to retrieve the JSON element associated with the key "Shipment" from the parsed `responseObj`. The result is stored in a dynamic variable `shipment`.

```cs
var shipment = responseObj["Shipment"];
```

* Null and Tracking Number Existence Check:
    * Performs a critical validation step.
    * Checks if a "Shipment" node even exists within the parsed JSON.
        * If the "Shipment" node exists, this further checks if a "TrackingNumber" property is present within the "Shipment" node.
        * If either of these conditions fail, indicating the tracking number cannot be found at the expected path, an `Exception` is thrown with the message "TrackingNumber not found in response."

```cs
if (shipment == null || shipment["TrackingNumber"] == null)
                    throw new Exception("TrackingNumber not found in response.");
```

* Extracting and Returning Tracking Number:
    * If the checks in the previous step pass, it means both "Shipment" and "TrackingNumber" nodes are present.
    * The method then extracts the value associated with the "TrackingNumber" key from the `shipment` `JToken` and converts it to a `string` before returning it as the method's result.

```cs
return shipment["TrackingNumber"].ToString();
```

* Exception Handling (Catch Block):
    * If any exception occurs within the `try` block (e.g., malformed JSON, `NullReferenceException` if not caught by the explicit checks, etc.), control is transferred to the `catch` block.
    * The method logs the error using a the `SessionMaintenance.LogBook` method. This log entry includes the error level (`"ERROR"`), source components (`"[WcmsApi]"`, `"[ExtractTrackingNumberFromJson]"`), and a descriptive message including the exception's message.
    * Finally returns an empty string, signaling that the tracking number could not be successfully extracted due to an error.

```cs
catch (Exception ex)
            {
                SessionMaintenance.LogBook("ERROR", "[WcmsApi]", "[ExtractTrackingNumberFromJson]", $"Error extracting tracking number: {ex.Message}");
                return string.Empty;
            }
```

#### Summary

The ExtractTrackingNumberJson method provides a structured and error-resilient mechanism for retrieving a shipment tracking number from a JSON response. It leverages the Json.NET library for efficient JSON parsing and incorporates explicit checks for the presence of expected JSON elements ("Shipment" and "TrackingNumber"). In the event of missing data or parsing errors, the method gracefully handles exceptions by logging the issue and returning an empty string, ensuring the calling application can appropriately manage failed tracking number retrievals. This design promotes robust data extraction and error management within systems that interact with JSON-based APIs.

```cs
 // Get Tracking from JSON ---------------------------------------------------------------------------------------------
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
```
