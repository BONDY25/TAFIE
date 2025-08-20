[Back](ClassIndex.md)

# TAFIE Carrier Module - WcmsApi - Methods

## 1.1 - Methods - API Functions

### 1.1.1 - Initialization and declaration
This section of the `WcmsApi` class declares the essential global variables required for making API calls to request carrier labels, as well as for interacting with the database and tracking user/session information. A comment is included with a link to the API documentation for further reference.

The following variables are declared:
* `apiUrl`: The URL used to make the API call for requesting a carrier label, including query parameters for the label format (PDF) and size (6). This is the endpoint for initiating a shipment label request.
* `tknApiurl`: The URL for requesting an access token, which is needed for authorization in subsequent API calls. This is crucial for authenticating the request.
* `accessToken`: A static string that will hold the access token returned from the `tknApiurl` endpoint after the authentication request. This token is required for making further API calls to request shipment labels.
* `apiAccCode`: The API account code used as part of the authentication process when requesting the access token.
* `connectionString`: The SQL Server connection string that allows the application to interact with the database. It connects to the Appz database on the server SQL-SSRS using Windows authentication.
* `userName`: A property that holds the username of the user interacting with the application. This value is passed from outside the class.
* `tcarRef`: A nullable string used to store the `TCAR` reference for the shipment label request. This unique reference is essential for tracking the shipment request throughout the application and is updated dynamically during the flow of the application (e.g., after executing the stored procedure for creating a new `TCAR` record).

This section of code is critical in ensuring that the class has all the necessary data for the smooth operation of the carrier label request process.

```cs
namespace TAFIE
{
    public class WcmsApi
    {
        //*** API DOCS: https://api.parcelhub.net/docs/ ***//

        private static readonly string apiUrl = "https://api.parcelhub.net/1.0/Shipment?RequestedLabelFormat=PDF&RequestedLabelSize=6";
        private static readonly string tknApiurl = "https://api.parcelhub.net/1.0/TokenV2";
        public static string accessToken = "";
        public static string apiAccCode = "";

        private const string connectionString = "Server=SQL-SSRS;Database=Appz;Integrated Security=True;Encrypt=False;";

        public static string userName { get; set; }

        public static string? tcarRef = null;
```

---

### 1.1.2 - CheckAccessTkn

The `CheckAccessTkn` method is a public method that accepts a single string parameter, `accCode`, and returns a boolean value (`true` or `false`) to indicate the success or failure of the operation. The purpose of this method is to retrieve an API access token by executing an SQL stored procedure.

#### Input Parameter:

* `accCode`: This string parameter represents the API account code, which is passed to the SQL stored procedure as an argument.

#### SQL Query Execution:

* The method constructs and executes an SQL query to call the stored procedure `TCAR_Check_TOKN`. The stored procedure uses the provided `accCode` to attempt to retrieve an API access token.
* The stored procedure either returns a valid access token or a `“NO TOKEN”` message indicating failure.

#### Processing the SQL Data:

* The method uses an `SqlDataReader` to process the result of the stored procedure.
* If the stored procedure returns a valid result, the access token is assigned to the global variable `accessToken`. If the stored procedure returns `"NO TOKEN"`, the `accessToken` is not updated.

#### Error Handling:

* The method is wrapped in a try-catch block to gracefully handle any exceptions that may occur during the database operation.
* If an error occurs, a custom error message is displayed to the user via the `CustomMessageBox` class, and the error is logged in the session maintenance log for further troubleshooting.

#### Return Value:

* The method returns `true` if a valid access token is retrieved (i.e., the token is not equal to `"NO TOKEN"`), and `false` if the retrieval fails (i.e., `"NO TOKEN"` is returned).

```cs
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
```

---

### 1.1.3 - GetXmlData

The `GetXmlData` method is a public static method that accepts two parameters, `tcarRef` (a string) and `boxRef` (an integer), and returns a `DataTable` containing data retrieved from a SQL stored procedure that will be used to construct an XML Document. This method is responsible for querying the database, retrieving data, and loading it into a `DataTable` for further use.

#### Input Parameters:

* `tcarRef`: This string parameter represents the `TCAR` reference used to identify the shipment record.
* `boxRef`: This integer parameter represents the box reference used to identify the specific box within the shipment.

#### SQL Query Execution:

* The method constructs an SQL query that calls the stored procedure `[TCAR_Get_XML_V2]` with the `tcarRef` and `boxRef` as parameters.
* The query is executed against the SQL database using a `SqlCommand` object.

#### Data Retrieval:

* After executing the query, the method uses a `SqlDataReader` to read the results from the database.
* The data returned by the query is loaded into a `DataTable` using the `dataTable.Load(reader)` method. This allows the method to return the data in a structured format that can be used elsewhere in the application.

#### Error Handling:

* The method is enclosed in a try-catch block to gracefully handle any errors that might occur during the database connection or query execution.
* If an error occurs, a custom error message is shown to the user through the `CustomMessageBox` class. The error is also logged with the `SessionMaintenance.LogBook` method for debugging and tracking purposes.

#### Logging:

* If the data retrieval is successful, a log entry is created using `SessionMaintenance.LogBook` to record the successful retrieval of the XML data for the specified tcarRef and `boxRef`.

#### Return Value:

* The method returns a `DataTable` containing the XML data retrieved from the database. If there was an error during the process, an empty DataTable is returned.

```cs
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
```

---

### 1.1.4 - InsertXml 

The `InsertXml` method is a public static method that accepts three parameters: `xmlString` (a string), `tKey` (a string), and `mode` (an integer). This method is responsible for inserting the XML data into the database by executing a stored procedure that saves the provided XML string along with relevant information such as the `TCAR` reference, user details, and operation mode.

#### Input Parameters:

* `xmlString`: This string parameter represents the XML data that will be inserted into the database. It could be the XML response from the API or any other XML-related data used by the application.
* `tKey`: This string parameter is used as a key to identify the data being inserted. It's typically tied to a specific record in the system, such as a shipment or label.
* `mode`: This integer parameter determines the mode or operation type when inserting the XML data. It could represent different stages or versions of the XML data being inserted, depending on the system’s logic.

#### SQL Query Execution:

* The method constructs an SQL query that calls the stored procedure `[TCAR_INSERT_XML]` with the following parameters:
    * `@tkey`: The key that uniquely identifies the data to be inserted.
    * `@TCAR_Ref`: The `TCAR` reference, which is a global value that identifies the shipment label request in the application.
    * `@User`: The username of the user performing the operation, retrieved from the userName global property.
    * `@XML_String`: The XML string that will be stored in the database.
    * `@Mode`: The mode indicating the type of operation being performed.
* This query is executed using a `SqlCommand` object with the parameters passed into the query.

#### Database Operation:

* The method establishes a connection to the SQL Server using the connection string and opens the connection.
* Once the connection is open, the query is executed using `cmd.ExecuteNonQuery()`, which performs the insert operation.
* After the query execution, the connection is closed to ensure that resources are properly released.

#### Error Handling:

* The method is enclosed in a try-catch block to handle any exceptions that may occur during the database interaction. If an error occurs during the query execution, a custom error message is displayed to the user through the `CustomMessageBox` class.
* Additionally, the error is logged using `SessionMaintenance.LogBook` with the error message for debugging and tracking purposes.

#### Logging:

* If the insert operation is successful, a log entry is created using `SessionMaintenance.LogBook` to record the XML insertion along with the `mode` and `tKey` for reference. This helps in tracking the insertion process and identifying which data was inserted.

```cs
// Insert XML String -----------------------------------------------------------------------------------------------------
        public static void InsertXml(string xmlString, string tKey, int mode)
        {
            string query = "EXECUTE [TCAR_INSERT_XML] @tkey, @TCAR_Ref, @User, @XML_String, @Mode";

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
                        cmd.Parameters.AddWithValue("@User", userName);
                        cmd.Parameters.AddWithValue("@XML_String", xmlString);
                        cmd.Parameters.AddWithValue("@Mode", mode);

                        cmd.ExecuteNonQuery();
                    }
                    conn.Close(); // Close SQL Connection
                }

                SessionMaintenance.LogBook("", "[WcmsApi]", "[InsertXml]", $"XML String Inserted: {mode} - {tKey}");
            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured saving API XML \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[WcmsApi]", "[InsertXml]", $"FAILED: Code 226 ( {ex.Message} )");
            }
        }
```

---

### 1.1.5 - InsertAccessTkn 

The `InsertAccessTkn` method is a public static method responsible for storing an API access token in the database. It takes three parameters. This method executes a stored procedure `[TCAR_INSERT_TOKN]`, which inserts the API authentication credentials, including the access token and refresh token, into the database.

#### Input Parameters:

* `tKey`: A unique identifier used to track the token in the database. This may correspond to a session key or transaction ID.
* `accCode`: The API account code, which identifies the account requesting the access token.
* `refreshTkn`: The refresh token that allows the application to obtain a new access token without requiring user re-authentication.

#### SQL Execution Process:

* The method constructs an SQL query to call the `[TCAR_INSERT_TOKN]` stored procedure.
* A `SqlConnection` is established using the `connectionString` to connect to the database.
* The connection is opened using `conn.Open()`.
* A `SqlCommand` object (`cmd`) is created to execute the stored procedure with the following parameters:
    * `@tkey`: The unique identifier for the token.
    * `@Acc_Code`: The API account code.
    * `@Access_Tkn`: The global variable accessToken, which contains the current API access token.
    * `@Refresh_Tkn`: The refresh token, which is stored for future use in refreshing the session.
* The `ExecuteNonQuery()` method is called to insert the token data into the database.
* The connection is closed using `conn.Close()` to free up resources.
* Error Handling:
    * If an error occurs during execution, the catch block is triggered.
    * A `CustomMessageBox` displays an error message to the user.
    * The error is logged using `SessionMaintenance.LogBook`, storing the exception message for debugging.

```cs
// Insert Access Token ----------------------------------------------------------------------------------------------------
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
```

---

### 1.1.6 - MakeServiceApiCall 

The `MakeServiceApiCall` method is a public, asynchronous method that sends an API request to retrieve available carrier services. It takes a single string parameter (`callXml`) and returns a string response from the API.

#### Input Parameter:

* `callXml`: A string containing an XML request body, which will be sent to the API.

#### Step-by-Step Execution:

* Log API Request Initialization
    * The method begins by logging an entry to indicate that a service request is being made.
* Initialize HTTP Client
    * Creates an instance of `HttpClient`, which is used to send HTTP requests.
* Set Authorization Header
    * Adds a Bearer token to the request header, using the `accessToken` global variable.
    * This ensures authentication with the API.
* Log the XML Request
    * Logs the XML request body for debugging purposes.
* Prepare the HTTP Request
    * Creates an `HttpContent` object (`content`) that contains:
        * The XML request body (`callXml`).
        * Encoding set to UTF-8.
        * Content type set to `"application/xml"`.
* Send API Request
    * Sends a `POST` request to the Parcelhub API using `PostAsync()`.
    * The API URL is `"https://api.parcelhub.net/1.0/Service?AccountId="`.
* Process API Response
    * Reads the response body as a string.
    * Logs the response headers and response body.
* Handle Errors
    * If the API response status is unsuccessful (`IsSuccessStatusCode == false`):
        * Logs the error message.
        * Returns the error details as a string.
* Return API Response
    * If successful, logs the success and returns the response data
* Exception Handling
    * If a `HttpRequestException` occurs (e.g., network failure, invalid request):
        * Displays an error message to the user.
        * Logs the error details.
        * Throws the exception to be handled at a higher level.

The `MakeServiceApiCall` method is responsible for making an authenticated API request to fetch carrier services. It builds an XML request, sends a `POST` request, logs responses, and handles errors effectively.

```cs
// API Call for Get Services -----------------------------------------------------------------------------------------------------------------------
        public static async Task<string> MakeServiceApiCall(string callXml)
        {
            SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeServiceApiCall]", $"Get Services Requested");
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string requestUrl = "https://api.parcelhub.net/1.0/Service?AccountId=";  // Use the API URL directly

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Log the request
                    SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeServiceApiCall]", $"API Call Attempted\n{callXml}");

                    // Prepare the request content
                    HttpContent content = new StringContent(callXml, Encoding.UTF8, "application/xml");

                    // Send the request as POST
                    HttpResponseMessage response = await client.PostAsync(requestUrl, content);
                    string responseData = await response.Content.ReadAsStringAsync();
                    SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeServiceApiCall]", $"API Response Headers:\n{response}");
                    SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeServiceApiCall]", $"API Response Body:\n{responseData}");
                    // Log the response status
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        SessionMaintenance.LogBook("ERROR", "[WcmsApi]", "[MakeServiceApiCall]", $"FAILED: API Call Failed {errorContent}");
                        return errorContent;  // Return error details
                    }

                    // If successful, return the response data                    
                    SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeServiceApiCall]", $"API Call Successful");
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
```

---

### 1.1.7 - MakeTKNApiCall 

The `MakeTKNApiCall` method is a public, asynchronous method that sends an API request to obtain an authentication token. It takes a single string parameter (`CallXml`) and returns a string response containing either the access token or an error message.

#### Input Parameter:

* `CallXml`: A string containing an XML request body, which is sent to the API to request a token.

#### Step-by-Step Execution:

* Log API Request Initialization
    * The method logs that a token request is being made.
    * Logs the XML request (`CallXml`) that will be sent.
* Initialize HTTP Client
    * Creates an instance of `HttpClient`, which is used to send HTTP requests.
* Set API Request URL
    * Uses `tknApiurl` (a global variable storing the token request API URL) as the endpoint.
* Log the API Request
    * Logs the XML request body for debugging purposes.
* Prepare the HTTP Request
    * Creates an `HttpContent` object (`content`) containing:
        * The XML request body (`CallXml`).
        * Encoding set to `UTF-8`.
        * Content type set to `"application/xml"`.
* Send API Request
    * Sends a `POST` request to the token API using `PostAsync()`.
* Process API Response
    * Reads the response body as a string.
    * Logs the response headers and response body.
* Handle API Errors
    * If the API response status is unsuccessful (`IsSuccessStatusCode == false`):
        * Reads the error message from the response.
        * Logs the error message.
        * Returns the error message to indicate failure.
* Return API Response
    * If successful, logs the success and returns the response data.
* Exception Handling
    * If a `HttpRequestException` occurs (e.g., network failure, invalid request):
        * Displays an error message to the user.
        * Logs the error details.
        * Throws the exception to be handled at a higher level.

The `MakeTKNApiCall` method is responsible for making an authenticated API request to obtain a token. It builds an XML request, sends a `POST` request, logs responses, and handles errors effectively. The response data will later be processed to extract the access token for future API requests.

```cs
// API Call for Token -----------------------------------------------------------------------------------------------------------------------
        public static async Task<string> MakeTKNApiCall(string CallXml)
        {

            SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeTKNApiCall]", $"TOKEN REQUEST XML:\n{CallXml}");

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string requestUrl = tknApiurl;  // Use the API URL directly

                    // Log the request
                    SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeTKNApiCall]", $"API Call Attempted\n{CallXml}");

                    // Prepare the request content
                    HttpContent content = new StringContent(CallXml, Encoding.UTF8, "application/xml");

                    // Send the request as POST
                    HttpResponseMessage response = await client.PostAsync(requestUrl, content);
                    string responseData = await response.Content.ReadAsStringAsync();
                    SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeTKNApiCall]", $"API Response Headers:\n{response}");
                    SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeTKNApiCall]", $"API Response Body:\n{responseData}");

                    // Log the response status
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        SessionMaintenance.LogBook("ERROR", "[WcmsApi]", "[MakeTKNApiCall]", $"FAILED: API Call Failed {errorContent}");
                        return errorContent;  // Return error details
                    }

                    // If successful, return the response data                    
                    SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeTKNApiCall]", $"API Call Successful");
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
```

---

### 1.1.8 - MakeCarrierApiCall 

The `MakeCarrierApiCall` method is a public, asynchronous method that sends an API request to request a carrier label. It takes two input parameters and returns a string response containing either the carrier label XML or an error message.

#### Input Parameters:

* `CallXml` (string) – The XML request body containing shipment details, which is sent to the API to generate a carrier label.
* `tKey` (string) – A unique key used to associate the API response with a particular shipment record in the database.

#### Step-by-Step Execution:

* Initialize HTTP Client
    * Creates an instance of `HttpClient` to send HTTP requests.
* Set API Request URL
    * Uses `apiUrl` (a global variable storing the carrier label request API URL) as the endpoint.
* Set Authorization Header
    * Uses the stored `accessToken` to authenticate the request.
* Log the API Request Attempt
    * Logs the attempt to call the API, along with the access token being used.
* Prepare the HTTP Request Body
    * Creates an `HttpContent` object (`content`) containing:
        * The XML request body (`CallXml`).
        * Encoding set to `UTF-8`.
        * Content type set to `"application/xml"`.
* Send API Request
    * Sends a `POST` request to the carrier label API using `PostAsync()`.
* Read API Response
    * Reads the response body as a string.
    * Logs the response headers and response body.
* Handle API Errors
    * If the API response status is unsuccessful (`IsSuccessStatusCode == false`):
        * Extracts the error message using `ExtractErrorMessage(responseData)`.
        * Displays an error message in a `CustomMessageBox`
        * Inserts the error XML response into the database using `InsertXml(responseData, tKey, 3)`.
        * Returns the error message.
* Successful API Call Handling
    * Logs that the API call was successful.
    * Stores the response XML in the database using `InsertXml(responseData, tKey, 2)`.
    * Returns the API response data.
* Exception Handling
    * If a `HttpRequestException` occurs (e.g., network failure, invalid request):
        * Displays an error message to the user.
        * Logs the error details.
        * Throws the exception to be handled at a higher level.


The `MakeCarrierApiCall` method is responsible for making an authenticated API request to request a carrier label. It sends an XML request, logs responses, stores data in the database, and handles errors effectively.

```cs
// Make API Call to get label ------------------------------------------------------------------------------------------------------------------        
        public static async Task<string> MakeCarrierApiCall(string CallXml, string tKey)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the API endpoint URL
                    string requestUrl = apiUrl;

                    // Set the Authorization header
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Log the request attempt
                    SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeCarrierApiCall]", $"API Call Attempted With token: {accessToken}");

                    // Prepare the request body with XML content
                    HttpContent content = new StringContent(CallXml, Encoding.UTF8, "application/xml");

                    // Make the API call
                    HttpResponseMessage response = await client.PostAsync(requestUrl, content);

                    // Read and return the response XML
                    string responseData = await response.Content.ReadAsStringAsync();

                    SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeCarrierApiCall]", $"Response:\n{response}");

                    // Log the response status
                    if (!response.IsSuccessStatusCode)
                    {
                        // Extract Error
                        string errorMessage = ExtractErrorMessage(responseData);

                        // Show Error
                        CustomMessageBox messageBox = new CustomMessageBox();
                        messageBox.lblDescription.BackColor = Color.Red;
                        messageBox.ShowError($"{errorMessage}");
                        messageBox.lblDescription.BackColor = Color.FromArgb(11, 159, 161);

                        // Insert XML
                        InsertXml(responseData, tKey, 3);

                        // Return Error response
                        string errorContent = await response.Content.ReadAsStringAsync();
                        SessionMaintenance.LogBook("ERROR", "[WcmsApi]", "[MakeCarrierApiCall]", $"FAILED: API Call Failed {errorContent}");
                        return errorContent;
                    }

                    SessionMaintenance.LogBook("", "[WcmsApi]", "[MakeCarrierApiCall]", $"API Call Successful");

                    // Insert response XML into database
                    InsertXml(responseData, tKey, 2);

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
```

---

## 1.2 - Methods - XML Generation

### 1.2.1 - GetTknXml 
The `GetTknXml` method is a public static method that generates an XML request for an API token request. This XML is later sent to the API when requesting an authentication token.

#### Input Parameters:

* `apiUsername` (string) – The API account username, required for authentication.
* `apiPassword` (string) – The API account password, required for authentication.

#### Step-by-Step Execution:

* Create the Root XML Element (`RequestToken`)
    * The method uses LINQ to XML (`XElement`) to construct an XML document.
    * The root element is `<RequestToken>`, which contains:
        * XML Schema attributes (`xsi`, `xsd`) to define the XML structure.
        * Child elements:
            * `<grant_type>` – Specifies the token type (`"bearer"`).
            * `<username>` – Inserts the provided `apiUsername`.
            * `<password>` – Inserts the provided `apiPassword`.
* Wrap the XML Inside a `XDocument` Object
    * `XDocument` is created with:
        * XML declaration (`version="1.0"`, `encoding="utf-8"`, `standalone="yes"`).
        * The root `<RequestToken>` element.
* Convert the XML to a String
    * Uses `doc.ToString()` to convert the XML object into a string.
* Return the Final XML String
    * The method returns the XML string with an explicit XML declaration (`<?xml version="1.0" encoding="utf-8"?>`) at the beginning.

#### Example XML Output:

```xml
<?xml version="1.0" encoding="utf-8"?> 
<RequestToken xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <grant_type>bearer</grant_type>
    <username>myUsername</username>
    <password>myPassword</password>
</RequestToken>
```

The `GetTknXml` method dynamically generates an XML request body needed for API authentication. This XML is later sent to the API to obtain an access token that allows further API interactions.

```cs
// Create XML For token request ----------------------------------------------------------------------------
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
```

---

### 1.2.2 - CreateXmlString 

The `CreateXmlString` method dynamically generates an XML string that represents a shipment request using data from a `DataTable`. This XML is then sent to an external API for processing.

#### Input Parameters:

* `dt` (DataTable) – Contains shipment data. Each row represents a package/item in the shipment.
* `ServiceCustomerUID` (string? | optional) – A unique identifier for the service by customer. Defaults to null.
* `domestic` (int | optional) – Indicates if the shipment is domestic (1 = domestic, 0 = international). Defaults to `1`.
* `ddp` (int | optional) – Indicates if Delivered Duty Paid (DDP) applies (1 = DDP enabled, 0 = not enabled). Defaults to `0`.

#### Step-by-Step Execution:

* Ensure There is Data
    * If `dt` has no rows, return an empty string.
* Extract First Row as Shipment-Level Data
    * The first row (`firstRow = dt.Rows[0]`) contains general shipment details.
* Define XML Namespace (`XNamespace`)
    * The method defines the namespace (`http://api.parcelhub.net/schemas/api/parcelhub-api-v0.4.xsd`) used in the XML.
* Build the `<Shipment>` XML Root Element
    * Uses LINQ to XML (`XElement`) to create the XML structure.
    * Includes attributes for XML Schema Definition (XSD) compatibility.
* Construct XML Sections
    * Shipment Information (`<Account>`)
        * Fetches the account number from `dt`.
    * Collection Details (`<CollectionDetails>`)
        * Includes collection date, ready time, and location close time.
    * Delivery Address (`<DeliveryAddress>`)
        * Uses XCData to encode text values for elements like `ContactName`, `Email`, `Phone`, `Address1`, etc.
    * Shipment References (`Reference1`, `Reference2`, etc.)
    * Customs Declaration (`<CustomsDeclarationInfo>`)
        * Includes IOSS Number, VAT Number, and duty billing if the shipment is international and DDP is enabled.
    * Service Information (`<ServiceInfo>`)
    * Package Details (`<Packages>`)
        * Includes dimensions, weight, value, and customs declarations.
* Generate `<ItemLevelDeclaration>` Elements for Each Product
    * Uses LINQ’s Select method to create multiple `<ItemLevelDeclaration>` elements dynamically from `dt`.
    * Includes SKU, product description, type, value, quantity, weight, origin, and harmonized code.
* Create XML Document (`XDocument`)
    * Adds an XML declaration (`<?xml version="1.0" encoding="utf-8"?>`) at the start.
* Convert XML to String and Return
    * Calls `.ToString()` to convert the XDocument into a string.
    * Returns the final XML string.

If `dt` contains a shipment with one package and three items, the generated XML might look like this:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Shipment xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://api.parcelhub.net/schemas/api/parcelhub-api-v0.4.xsd">
	<Account>L309891</Account>
	<CollectionDetails>
		<CollectionDate>2025-04-03</CollectionDate>
		<CollectionReadyTime>15:34:08</CollectionReadyTime>
		<LocationCloseTime>17:00:00</LocationCloseTime>
	</CollectionDetails>
	<DeliveryAddress>
		<ContactName><![CDATA[Test Testerson]]></ContactName>
		<Email><![CDATA[Test@Test.co.uk]]></Email>
		<Phone><![CDATA[0123456789]]></Phone>
		<Address1><![CDATA[13 Test Street]]></Address1>
		<Address2><![CDATA[]]></Address2>
		<City><![CDATA[TEST TOWN]]></City>
		<Area><![CDATA[DEVON]]></Area>
		<Postcode><![CDATA[TQ4 7SR]]></Postcode>
		<Country>GB</Country>
		<AddressType>Residential</AddressType>
	</DeliveryAddress>
	<Reference1>TEST123456</Reference1>
	<Reference2>LN123456</Reference2>
	<SpecialInstructions><![CDATA[]]></SpecialInstructions>
	<ContentsDescription>Goods</ContentsDescription>
	<CurrencyCode>GBP</CurrencyCode>
	<HasBeenManifested>false</HasBeenManifested>
	<Department/>
	<CustomsDeclarationInfo>
		<TermsOfTrade>DutiesAndTaxesUnpaid</TermsOfTrade>
		<CategoryOfItem>Sold</CategoryOfItem>
	</CustomsDeclarationInfo>
	<ServiceInfo>
		<ServiceId>13000</ServiceId>
		<ServiceCustomerUID>52936</ServiceCustomerUID>
		<ServiceProviderId>18</ServiceProviderId>
	</ServiceInfo>
	<Packages>
		<Package>
			<PackageType>Parcel</PackageType>
			<Dimensions>
				<Length>1</Length>
				<Width>1</Width>
				<Height>1</Height>
			</Dimensions>
			<Weight>1.7600</Weight>
			<Value Currency="GBP">43.20</Value>
			<Contents>Goods</Contents>
			<PackageCustomsDeclaration>
				<Quantity>12</Quantity>
				<Value Currency="GBP">43.20</Value>
			</PackageCustomsDeclaration>
			<ItemLevelDeclarations>
				<ItemLevelDeclaration>
					<ProductSKU>DCM02</ProductSKU>
					<ProductDescription><![CDATA[Blues Mug]]></ProductDescription>
					<ProductType><![CDATA[Mugs]]></ProductType>
					<ProductValue>6.84</ProductValue>
					<ProductQuantity>3</ProductQuantity>
					<ProductWeight>0.19</ProductWeight>
					<ProductCountryOfOrigin>GB</ProductCountryOfOrigin>
					<ProductHarmonisedCode>69111000</ProductHarmonisedCode>
				</ItemLevelDeclaration>
				<ItemLevelDeclaration>
					<ProductSKU>BSM03</ProductSKU>
					<ProductDescription><![CDATA[Music Notes Mug]]></ProductDescription>
					<ProductType><![CDATA[Mugs]]></ProductType>
					<ProductValue>6.84</ProductValue>
					<ProductQuantity>3</ProductQuantity>
					<ProductWeight>0.21</ProductWeight>
					<ProductCountryOfOrigin>GB</ProductCountryOfOrigin>
					<ProductHarmonisedCode>69111000</ProductHarmonisedCode>
				</ItemLevelDeclaration>
				<ItemLevelDeclaration>
					<ProductSKU>MUGBOX</ProductSKU>
					<ProductDescription><![CDATA[Mug Box]]></ProductDescription>
					<ProductType><![CDATA[Mug Boxes]]></ProductType>
					<ProductValue>0.36</ProductValue>
					<ProductQuantity>6</ProductQuantity>
					<ProductWeight>0.06</ProductWeight>
					<ProductCountryOfOrigin>GB</ProductCountryOfOrigin>
					<ProductHarmonisedCode>48191000</ProductHarmonisedCode>
				</ItemLevelDeclaration>
			</ItemLevelDeclarations>
		</Package>
	</Packages>
</Shipment>
```



The `CreateXmlString` method generates an XML shipment request using data from a `DataTable`. It dynamically constructs shipment details, recipient information, package dimensions, customs information, and item-level details. The resulting XML is well-formed, API-compatible, and secure.

``` cs
// Create XML String ------------------------------------------------------------------------------------------------------------------
        public static string CreateXmlString(DataTable dt, string? ServiceCustomerUID = null, int domestic = 1, int ddp = 0)
        {
            if (dt.Rows.Count == 0) return string.Empty;  // Ensure there's data

            DataRow firstRow = dt.Rows[0];  // Use first row for shipment-level data

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

                // DeliveryAddress //
                new XElement(ns + "DeliveryAddress",
                    new XElement(ns + "ContactName", new XCData(firstRow["ContactName"].ToString())),
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
                    domestic == 0 && ddp == 1 ? new XElement(ns + "DutyBillingTerm", firstRow["DutyBillingTerm"]) : null
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
```

---

## 1.3 - Methods - XML Data Extraction

### 1.3.1 - ExtractTrackingNumber

The `ExtractTrackingNumber` method extracts the courier tracking number from an XML response string returned by the Parcelhub API. It looks for the `<CourierTrackingNumber>` element in a known XML namespace and returns its value.

#### Input parameters:
* `xmlResponse` - The full XML response string received from the API, expected to contain a `<CourierTrackingNumber>` node.

#### Step-by-Step Execution:
* Parse the XML String into a Document
	* Converts the input XML string into an `XDocument` for querying.
	* This step enables structured access to XML elements and attributes.
* Declare XML Namespace
	* Sets the namespace used by the Parcelhub API schema.
	* This is required when accessing namespaced elements like <CourierTrackingNumber>.
* Locate the `<CourierTrackingNumber>` Element
	* Searches the entire XML tree for the first `<CourierTrackingNumber>` element within the declared namespace.
	* Uses `FirstOrDefault()` to handle cases where the element may not be present.
* Return the Tracking Number or Fallback Message
	* If the element is found, it returns its value (the tracking number).
	* If not found, returns a user-friendly message `"Tracking number not found"`.
* Handle and Log Exceptions Gracefully
	* If the XML is malformed or parsing fails, logs the exception using the `SessionMaintenance.LogBook` method.
	* Returns a fallback string `"Error extracting tracking number"` to ensure calling code can handle failures safely.

In the example below the method would return “1Z999AA10123456784” from the XML

```xml
<ShipmentResponse xmlns="http://api.parcelhub.net/schemas/api/parcelhub-api-v0.4.xsd">
    <CourierTrackingNumber>1Z999AA10123456784</CourierTrackingNumber>
</ShipmentResponse>
```

```cs
// Get Tracking from XML -----------------------------------------------------------------------------------------
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
```

---

### 1.3.2 - ExtractLabelData

The `ExtractLabelData` method extracts label data (PDFs) from an XML API response. These labels are typically encoded in base64 format within `<LabelData>` elements, and the method decodes and saves each as a PDF file in a temporary folder, returning a list object containing the file paths.

#### Input Parameters:

* `xmlResponse` - The XML string returned by the API containing one or more `<LabelData>` elements.

#### Step-by-Step Execution:

* Initialize Output List
	* Prepares an empty list to collect the file paths of all successfully extracted label PDFs.
```cs
List<string> savedPdfFiles = new List<string>();
```

* Parse the XML
	* Parses the raw XML string into a structured `XDocument`.
	* Declares the Parcelhub XML namespace so that namespaced elements like `<LabelData>` can be properly identified.

```cs
XDocument doc = XDocument.Parse(xmlResponse);
XNamespace ns = "http://api.parcelhub.net/schemas/api/parcelhub-api-v0.4.xsd";
```

* Find All `<LabelData>` Elements
	* Retrieves all `<LabelData>` elements using the correct namespace.
	* Each element is expected to contain a base64-encoded PDF string.
```cs
var labelElements = doc.Descendants(ns + "LabelData");
```

* Create Temp Folder to Store PDFs
	* Creates (if needed) a temporary directory named `“CarrierLabels”` within the system's temp path.
	* This is where all decoded PDFs will be stored.
```cs
string tempFolder = Path.Combine(Path.GetTempPath(), "CarrierLabels");
if (!Directory.Exists(tempFolder))
    Directory.CreateDirectory(tempFolder);
```

* Loop Through `<LabelData>` Elements
	* Iterates over each base64-encoded label found in the XML.
```cs
foreach (var label in labelElements)
```

* Decode Each Label and Save as PDF
	* Extracts the base64 string from the `<LabelData>` element.
	* Decodes the string into raw PDF bytes.
	* Saves the byte array to disk as a `.pdf` file using an indexed filename (`Label_1_xyz.pdf`, `Label_2_xyz.pdf`, etc.).
	* Appends the full file path to `savedPdfFiles`.

```cs
string base64Label = label.Value;
byte[] pdfBytes = Convert.FromBase64String(base64Label);

string filePath = Path.Combine(tempFolder, $"Label_{labelIndex}_{tcarRef}.pdf");
File.WriteAllBytes(filePath, pdfBytes);
savedPdfFiles.Add(filePath);
```

* Handle Decoding Failures Gracefully
	* Logs any exceptions (e.g., malformed base64) without crashing the method.

```cs
catch (Exception ex)
{
    SessionMaintenance.LogBook("", "[WcmsApi]", "[ExtractLabelData]", $"Error decoding label: {ex.Message}");
}
```

* Catch and Log XML Parsing Errors
	* Handles and logs high-level issues such as malformed XML input.

```cs
catch (Exception ex)
{
    SessionMaintenance.LogBook("", "[WcmsApi]", "[ExtractLabelData]", $"Error parsing XML: {ex.Message}");
}
```

* Return List of Saved PDF File Paths
	* Returns a list of all labels successfully extracted and saved as files.
```cs
return savedPdfFiles;
```




```cs
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
```

---

### 1.3.4 - ExtractAccessToken

The `ExtractAccessToken` method extracts the access token from a raw XML string, returned as part of an authentication response from an API. The method returns the access token as a string.

#### Input Parameters:

* `xmlResponse` - The raw XML string returned by the authentication API.

#### Step-by-Step Execution:
* Parse the XML Input
	* Converts the raw xmlResponse string into a structured `XDocument`.
	* This allows for easy querying of XML elements using LINQ to XML.

```cs
XDocument doc = XDocument.Parse(xmlResponse);
```

* Search for the `<access_token>` Element
	* Searches for the first occurrence of the `<access_token>` element in the document.
	* Does not use a namespace, assuming the token response does not include one.
	* `FirstOrDefault()` safely returns null if the element is not found.

```cs
XElement accessTkn = doc.Descendants("access_token").FirstOrDefault();
```

* Return the Token or a Fallback Message
	* If the token element was found, its Value (the token string) is returned.
	* If not, it returns a fallback message: `"Access Token Not found"`.

```cs
return accessTkn != null ? accessTkn.Value : "Access Token Not found";
```

* Error Handling with Logging
	* Catches any exceptions (e.g., malformed XML).
	* Logs the error using `SessionMaintenance.LogBook` for audit/troubleshooting.
	* Returns a generic error message: `"Error extracting Access Token"`.

```cs
catch (Exception ex)
{
    SessionMaintenance.LogBook("", "[WcmsApi]", "[ExtractAccessToken]", $"Error: {ex.Message}");
    return "Error extracting Access Token";
}
```

```cs
// Get Access from XML --------------------------------------------------------------------------------------
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
```

---

### 1.3.5 - ExtractRefreshToken
The `ExtractRefreshToken` method extracts a refresh token from a given XML string. This token is used to obtain a new access token without requiring the user to log in again. The method returns the refresh token as a string.

#### Input Parameters:
* `xmlResponse` - The raw XML response string returned by the token API.

#### Step-by-Step Execution:
* Parse the XML Response
	* Parses the input string `xmlResponse` into an `XDocument`, allowing XML element navigation using LINQ to XML.
	* If the XML is malformed, it will trigger the catch block.

```cs
XDocument doc = XDocument.Parse(xmlResponse);
```

* Locate the `<refreshToken>` Element
	* Searches for the first `<refreshToken>` element in the entire XML document.
	* `FirstOrDefault()` returns the first match or null if not found.
	* Note: This assumes that the `<refreshToken>` element does not use a namespace.

```cs
XElement refreshTkn = doc.Descendants("refreshToken").FirstOrDefault();
```

* Return the Token or an Error Message
	* If the element was found, the method returns the text content of the `<refreshToken>`.
	* If it wasn't found, a fallback message `"Refresh Token Not found"` is returned.

```cs
return refreshTkn != null ? refreshTkn.Value : "Refresh Token Not found";
```

* Exception Handling and Logging
	* If any error occurs during XML parsing or element access, it logs the error via `SessionMaintenance.LogBook`.
	* Then, it returns a generic fallback message: `"Error extracting Refresh Token"`.

```cs
catch (Exception ex)
{
    SessionMaintenance.LogBook("", "[WcmsApi]", "[ExtractRefreshToken]", $"Error: {ex.Message}");
    return "Error extracting Refresh Token";
}
```

```cs
// Get Refresh from XML -----------------------------------------------------------------------------------------
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
```

---

### 1.3.6 - ExtractErrorMessage
The `ExtractErrorMessage` method extracts an error message from an XML response returned by an external API. It’s used when the API call fails and you need to display or log the reason for the failure. The method returns the error message as a string.

#### Input Parameters:
* `xmlResponse` - The raw XML string returned from the API when an error has occured.

#### Step-by-Step Execution:

* Parse the XML Response
	* Parses the raw XML string into an XDocument so LINQ can be used to navigate the structure.
	* If the string is not valid XML, an exception will be thrown and handled.

```cs
XDocument doc = XDocument.Parse(xmlResponse);
```

* Locate the `<Message>` Element
	* Searches for the first `<Message>` element in the XML.
	* This element is expected to contain the error message returned by the API.
	* It does not consider namespaces—so this assumes that `<Message>` exists outside any specific XML namespace.

```cs
XElement accessTkn = doc.Descendants("Message").FirstOrDefault();
```

* Return the Error Message or Fallback
	* If the `<Message>` element was found, its content (the actual error message) is returned.
	* Otherwise, a fallback string `"Error Not found"` is returned to indicate the absence of the element.

```cs
return accessTkn != null ? accessTkn.Value : "Error Not found";
```

* Handle Any Parsing or Lookup Errors
	* If any error occurs (e.g., bad XML format, null references), the exception is caught.
	* The error is logged using `SessionMaintenance.LogBook`.
	* A generic fallback message `"Error extracting Error"` is returned.

```cs
catch (Exception ex)
{
    SessionMaintenance.LogBook("", "[WcmsApi]", "[ExtractErrorMessage]", $"Error: {ex.Message}");
    return "Error extracting Error";
}
```

```cs
// Get Error Message from XML ----------------------------------------------------------------------------
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
```

---

### 1.3.7 - ExtractCustomerUID

The `ExtractCustomerUID` method retrieves the `ServiceCustomerUID` from a XML response. This UID is a unique reference for the carrier and service for the customer and is a requirement for international shipments. The `CustomerUID` is returned as a string by the method.

#### Input Parameters:

* `xmlResponse` - The raw XML response string, typically from an external service or API.

#### Step-by-Step Execution:

* Parse the XML Response
	* Converts the raw XML string into an `XDocument` object.
	* Enables traversal and querying of the XML structure using LINQ.
	* If parsing fails (invalid XML), the method jumps to the catch block.

```cs
XDocument doc = XDocument.Parse(xmlResponse);
```

* Extract the XML Namespace
	* Retrieves the default namespace declared in the root element of the XML.
	* This is crucial because XML elements in a namespace must be referenced with that namespace to be found using LINQ.

```cs
XNamespace ns = doc.Root.GetDefaultNamespace();
```

* Search for `<ServiceCustomerUID>`
	* Searches the document for the first occurrence of the `<ServiceCustomerUID>` element within the namespace.
	* If not found, the method proceeds to return a fallback value.

```cs
XElement ServiceCustomerUID = doc.Descendants(ns + "ServiceCustomerUID").FirstOrDefault();
```

* Return the UID or Default Value
	* If the element is found, its value (the actual UID string) is returned.
	* If it’s not found, the method returns `"00000"` as a safe default.

```cs
return ServiceCustomerUID != null ? ServiceCustomerUID.Value : "00000";
```

* Catch Exceptions and Log Errors
	* If an error occurs at any stage (e.g., malformed XML, null reference), it’s caught here.
	* The error is logged for debugging and auditing.
	* A fallback message `"Error extracting ServiceCustomerUID"` is returned.

```cs
catch (Exception ex)
{
    SessionMaintenance.LogBook("", "[WcmsApi]", "[ExtractCustomerUID]", $"Error:{ex.Message}");
    return "Error extracting ServiceCustomerUID";
}
```

```cs
// Get CustomerUID from XML ----------------------------------------------------------------------------------
        public static string ExtractCustomerUID(string xmlResponse)
        {
            try
            {
                XDocument doc = XDocument.Parse(xmlResponse);

                // Get the namespace from the root element
                XNamespace ns = doc.Root.GetDefaultNamespace();

                // Search for ServiceCustomerUID using the namespace
                XElement ServiceCustomerUID = doc.Descendants(ns + "ServiceCustomerUID").FirstOrDefault();

                return ServiceCustomerUID != null ? ServiceCustomerUID.Value : "00000";
            }
            catch (Exception ex)
            {
                SessionMaintenance.LogBook("", "[WcmsApi]", "[ExtractCustomerUID]", $"Error: {ex.Message}");
                return "Error extracting ServiceCustomerUID";
            }
        }
```

---

### 1.3.8 - PrintPdf

The `PrintPdf` method attempts to silently print a given PDF file using Adobe Acrobat Reader, without user interaction. It supports multiple installation paths for Adobe Reader and logs the process steps and any errors encountered.

#### Input Parameters:

* `filePath` - The full path to the PDF file that needs to be printed.

#### Step-by-Step Execution:

* Define Default Adobe Reader Path
	* A default path to Adobe Reader is specified.
	* This may or may not exist depending on system configuration.

```cs
string adobeReaderPath = @"C:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader\AcroRd32.exe";
```

* Create a List of Possible Reader Paths
	* A list of likely installation paths for Adobe Acrobat Reader is defined, accounting for different OS setups (32-bit vs. 64-bit, `AcroRd32.exe` vs. `AcroRd.exe`).

```cs
List<string> adobeReaderPaths = new List<string> { ... };
```

* Check for the First Existing Reader
	* Iterates through the possible paths.
	* If a valid Adobe Reader executable is found, it’s used as the print engine.

```cs
foreach (var path in adobeReaderPaths) { if (File.Exists(path)) { ... } }
```


* Handle Missing Adobe Reader
	* If no valid executable is found, the method throws an exception.
	* This is caught later to alert the user and log the failure.

```cs
if (!File.Exists(adobeReaderPath)) { throw new Exception(...); }
```

* Prepare the Print Process
	* Sets up a background process to launch Adobe Reader with the `/t` argument, which initiates silent printing.
	* Suppresses any UI windows (`CreateNoWindow` and `Hidden` styles).

```cs
Process printProcess = new Process { StartInfo = new ProcessStartInfo { ... } };
```

* Start the Print Job
	* Launches the Adobe Reader print command in the background.
	* Logs the attempt using the `SessionMaintenance.LogBook`.

```cs
printProcess.Start();
```

* Wait for Print Job Completion
	* Waits up to 15 seconds for the print job to complete.
	* If it doesn’t exit in time, logs a timeout warning.

```cs
bool exited = printProcess.WaitForExit(15000);
```

* Close Acrobat Reader
	* Attempts to gracefully close all open `AcroRd32` processes (Adobe Reader).
	* If a graceful shutdown fails, it forcibly terminates the process.

```cs
foreach (var process in Process.GetProcessesByName("AcroRd32")) { ... }
```

* Error Handling
	* Catches any exceptions (e.g., file not found, Adobe not installed, access issues).
	* Logs the error.
	* Displays a custom error message via `CustomMessageBox`.

```cs
catch (Exception ex) { ... }
```

```cs
// Print PDF ------------------------------------------------------------------------------------------------------------------
        public static void PrintPdf(string filePath)
        {
            try
            {
                string adobeReaderPath = @"C:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader\AcroRd32.exe";

                // Create a list of possible Adobe Reader paths
                List<string> adobeReaderPaths = new List<string>
                {
                    @"C:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader\AcroRd32.exe",
                    @"C:\Program Files\Adobe\Acrobat Reader DC\Reader\AcroRd32.exe",                    
                    @"C:\Program Files\Adobe\Acrobat Reader DC\Reader\AcroRd.exe",
                    @"C:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader\AcroRd.exe",
                };

                // Find the first existing path
                foreach (var path in adobeReaderPaths)
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
                SessionMaintenance.LogBook("", "[WcmsApi]", "[PrintPdf]", $"Printing {filePath}");

                // Wait up to 15 seconds for the printing process to finish
                bool exited = printProcess.WaitForExit(15000);
                if (!exited)
                {
                    SessionMaintenance.LogBook("", "[WcmsApi]", "[PrintPdf]", "Printing process is taking too long.");
                }

                // Try closing Acrobat gracefully first
                foreach (var process in Process.GetProcessesByName("AcroRd32"))
                {
                    if (!process.CloseMainWindow()) // Graceful close attempt
                    {
                        process.Kill(); // Force close if necessary
                        SessionMaintenance.LogBook("", "[WcmsApi]", "[PrintPdf]", "Process forcibly terminated: AcroRd32");
                    }
                    else
                    {
                        SessionMaintenance.LogBook("", "[WcmsApi]", "[PrintPdf]", "Process closed normally: AcroRd32");
                    }
                }

                SessionMaintenance.LogBook("", "[WcmsApi]", "[PrintPdf]", "Print Process Complete");
            }
            catch (Exception ex)
            {
                SessionMaintenance.LogBook("", "[WcmsApi]", "[PrintPdf]", $"Error printing: {ex.Message}");
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"Error printing: {ex.Message}");
            }
        }
```


