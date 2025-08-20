[Back](ClassIndex.md)

# TAFIE Carrier Module - CarrierForm - Methods

## 2.1 - Methods - API Functions

### 2.1.1 - Initialization and declaration

This section sets up the initial structure of the `CarrierForm` class, defining its key variables, properties, and constructor behavior. These are used throughout the class to manage form state, store session-related data, and configure shipment and carrier info.

#### Class-Level Properties and Fields

Public Properties:
* These are accessible from outside the class and typically passed in during form initialization:
* `sessionId` (string) – Stores the user's current session ID.
* `userName` (string) – The name of the user operating the form.
* `passedClient` (string) – Client reference passed into the form.
* `passedLoadNote` (string) – Load note ID passed in to initiate the shipment process.

#### Nullable Strings for Shared Data:

These hold various shipment, tracking, and carrier-related values:
* `tcarRef` – Temporary carrier reference.
* `apiAccCode` – API account code for carrier integration.
* `deltaCheck` – Likely a flag or checksum used in backend logic.

#### Constant:

* `connectionString` – A hardcoded connection string to the SQL Server database (Appz), used for data access throughout the form.

#### Private Nullable Strings (prefixed with pb):

These strings are used to store the data when it is first loaded into the form after the user has entered a load note. The strings are used to compare against when the if/when the user edits values in the form’s fields.:
* Carrier and service information: `pbCarrier`, `pbService`, `pbServiceDescr`, `pbInco`, `pbEori`, `pbIoss`
* Sender or receiver details: `pbName`, `pbCompany`, `pbPostcode`, `pbCity`, `pbAddr1`, `pbAddr2`, `pbCounty`, `pbCountry`
* Contact details: `pbEmail`, `pbPhone`
* Shipment metadata: `pbRef1`, `pbRef2`, `pbTotWeight`, `pbTotValue`, `pbVolume`, `pbBoxes`
These are all initially null and will be populated during the form's execution based on the user’s input or data fetched from the backend.

#### Shipment Flags:

These integer flags are used to control logic flow and behavior:
* `delta` – Used to indicate a change in one of the input fields by the user
* `ddp` – Duty Paid flag (1 = DDP enabled, 0 = not).
* `domestic` – Shipment type flag (1 = domestic, 0 = international).

#### Constructor: CarrierForm()

```cs
public CarrierForm()
{
    InitializeComponent();
    this.KeyPreview = true;
}
```

* `InitializeComponent()` – Standard method to initialize all Windows Form controls and their event bindings. It wires up the form’s UI based on its `.Designer.cs` file.
* `KeyPreview = true` – Allows the form to intercept keyboard events before they reach any individual control. This is often used to implement keyboard shortcuts or global key handlers.

```cs
namespace TAFIE
{
    public partial class CarrierForm : Form
    {
        //=============================================================================================================================================================================================
        //-- Initialization --//
        //=============================================================================================================================================================================================

        public string sessionId { get; set; }
        public string userName { get; set; }

        public string passedClient { get; set; }
        public string passedLoadNote { get; set; }

        public string? tcarRef = null;
        public string? apiAccCode = null;
        public string? deltaCheck = null;

        private const string connectionString = "Server=SQL-SSRS;Database=Appz;Integrated Security=True;Encrypt=False;";

        string? pbCarrier = null;
        string? pbService = null;
        string? pbServiceDescr = null;
        string? pbInco = null;
        string? pbEori = null;
        string? pbIoss = null;
        string? pbName = null;
        string? pbCompany = null;
        string? pbPostcode = null;
        string? pbCity = null;
        string? pbAddr1 = null;
        string? pbAddr2 = null;
        string? pbCounty = null;
        string? pbCountry = null;
        string? pbEmail = null;
        string? pbPhone = null;
        string? pbRef1 = null;
        string? pbRef2 = null;
        string? pbTotWeight = null;
        string? pbTotValue = null;
        string? pbVolume = null;
        string? pbBoxes = null;

        int delta = 0;
        int ddp = 0; // Flag for duties paid
        int domestic = 1; // flag for domestic shipments

        public CarrierForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }
```

---

### 2.1.2 - CarrierForm_Load

The `CarrierForm_Load` method is an event handler triggered automatically when the `CarrierForm` loads. It initializes the UI state, sets default values, and prepares the form for user interaction. This method plays a key role in setting up the application environment for label creation.

#### Event Handler:

* This method is wired to the form's `Load` event and executes when the form is first displayed.

```cs
private void CarrierForm_Load(object sender, EventArgs e)
```

#### Step-by-Step Execution:

* Log Form Start
    * Logs the launch of the form for diagnostic and auditing purposes.

```cs
SessionMaintenance.LogBook("", "[CarrierForm]", "[FormLoad]", $"Form Started");
```

* Set Form Title
    * Sets the title of the form window using the current Windows user and application name.

```cs
Text = $"{Environment.UserName.ToUpper()} - {SessionMaintenance.appName} Label Creation (Beta)";
```

* Populate Combo Boxes
    * Populates client and country dropdowns with values, likely pulled from a database or config file.

```cs
PopulateComboBoxes(cbClient, "CLIENT");
PopulateComboBoxes(cbCountry, "COUNTRY");
```

* Set Initial Field Values
    * Displays the current username in the UI.
    * Pre-fills the client and load note fields if they were passed to the form externally.
    * Initializes the tracking reference (`tcarRef`) to blank.

```cs
lblUsername.Text = userName;
cbClient.Text = passedClient;
txbLoadNote.Text = passedLoadNote;
tcarRef = "";
```

* Clear Totals and Labels
    * Clears any existing shipment totals and label details to ensure a clean slate when the form loads.

```cs
lblTotWeight.Text = "";
lblTotValue.Text = "";
lblVol.Text = "";
lblBoxes.Text = "";
lblServiceDescr.Text = "";
lblDdp.Text = "";
lblDomestic.Text = "";
```

* UI Styling and Visibility
    * Resets the color and visibility of UI elements.
    * Hides form sections and the print button until valid data is loaded.

```cs
lblCountry.BackColor = Color.FromArgb(11, 159, 161);
btnPrint.Visible = false;
cntCarr.Visible = false;
cntDel.Visible = false;
cntShip.Visible = false;
cntCont.Visible = false;
cntContact.Visible = false;
pbLogoBack.Visible = true;
lblInstruct.Visible = true;
```

* Display Instructions
    * Displays a multi-line instructional message to guide the user through the process of using the form.

```cs
lblInstruct.Text = "This form can be used to create carrier labels for orders where the carrier integration provided by Elucid has failed. ..."
```

* Auto Load LoadNote if Provided
    * If the form was launched with a `LoadNote` and `Client` provided, it automatically retrieves the load note data.
    * Enables the print button if data is found.

```cs
if (!string.IsNullOrEmpty(passedLoadNote) && !string.IsNullOrEmpty(passedClient))
{
    GetLoadNote(passedLoadNote, passedClient);
    btnPrint.Visible = true;
}
```

* Show Development Warning
    * Alerts the user that this form/module is still in beta and may not be fully stable.

```cs
CustomMessageBox messageBox = new CustomMessageBox();
messageBox.ShowWarning("This module is still under development...");
```

#### Summary:

The `CarrierForm_Load` method is responsible for preparing the carrier label form when it first opens. It ensures UI elements are correctly populated, applies styling, loads any pre-passed data, and displays instructional and warning messages to the user. This foundational method ensures the form is ready for interaction and sets the tone for user experience.

```cs
// Form Load -------------------------------------------------------------------------------------------
        private void CarrierForm_Load(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[FormLoad]", $"Form Started");
            Text = $"{Environment.UserName.ToUpper()} - {SessionMaintenance.appName} Label Creation (Beta)";
            PopulateComboBoxes(cbClient, "CLIENT");
            PopulateComboBoxes(cbCountry, "COUNTRY");
            lblUsername.Text = userName;
            cbClient.Text = passedClient;
            txbLoadNote.Text = passedLoadNote;
            tcarRef = "";

            lblTotWeight.Text = $"";
            lblTotValue.Text = $"";
            lblVol.Text = $"";
            lblBoxes.Text = $"";
            lblServiceDescr.Text = "";
            lblDdp.Text = "";
            lblDomestic.Text = "";
            lblCountry.BackColor = Color.FromArgb(11, 159, 161);
            btnPrint.Visible = false;
            cntCarr.Visible = false;
            cntDel.Visible = false;
            cntShip.Visible = false;
            cntCont.Visible = false;
            cntContact.Visible = false;
            pbLogoBack.Visible = true;
            lblInstruct.Visible = true;
            lblInstruct.Text = "This form can be used to create carrier labels for orders where the carrier integration provided by Elucid has failed. " +
                "Select a client, enter a load note and click search to bring up the details" +
                "\nEdit any details as necessary, if there are errors you should be able to fix them here." +
                "\nClick print label and watch the magic of an Application programming interface happen right before your eyes!";

            if (!string.IsNullOrEmpty(passedLoadNote) && !string.IsNullOrEmpty(passedClient))
            {
                GetLoadNote(passedLoadNote, passedClient);
                btnPrint.Visible = true;
            }

            CustomMessageBox messageBox = new CustomMessageBox();
            messageBox.ShowWarning("This module is still under development.\nSome features or functions may not work as expected and could result in errors, bugs, or application crashes.");
        }
```

---

## 2.2 - Methods - Operational

### 2.2.1 - CreateTKey

The `CreateTKey` method generates a unique transaction key (`tKey`) based on the current timestamp and the user's name input. This key is likely used as a unique identifier for shipment processing, label tracking, or API calls.

#### Method Signature:

private string `CreateTKey()`
Access: `private` – Only accessible within the `CarrierForm` class.
Returns: `string` – A unique identifier string.

#### Step-by-Step Execution:

* Get Name from Input Field
    * Retrieves the text entered in the name textbox (`txbName`).
    * This is typically the recipient or customer name involved in the shipment.
```cs
string name = txbName.Text;
```

* Generate Suffix from Name
    * Extracts the first two characters from the name (if available) and converts them to uppercase.
    * If the name is shorter than two characters, the entire name is used.
    * This adds a personalized, human-readable component to the key.
```cs
string tKeySufix = name.Length >= 2 ? name.Substring(0, 2).ToUpper() : name.ToUpper();
```

* Generate Timestamp
    * Captures the current date and time down to milliseconds, formatted as: `yyyyMMddHHmmssfff` Example: `20250403101530827`
    * This ensures uniqueness, even across rapid successive calls.
```cs
string tKeySufix = name.Length >= 2 ? name.Substring(0, 2).ToUpper() : name.ToUpper();
```

* Concatenate Key
* Combines the timestamp with the name-based suffix to create the final `tKey`.
```cs
string tKey = tKeyTimeStamp + tKeySufix;
```

* Return the Key
    * Returns the newly generated unique transaction key for use elsewhere in the form or API.
    * Example Output:
    * If the name is `Smith`, and the method is called on April 3rd, 2025 at `10:15:30.827 AM`, the output might be: `20250403101530827SM`
```cs
return tKey;
```

#### Summary:

The `CreateTKey` method constructs a unique, timestamp-based identifier with a name-derived suffix. This identifier is used to track transactions or shipments in a way that is both system-unique and partially human-readable.

```cs  
      // Create tKey -------------------------------------------------------------------------------
        private string CreateTKey()
        {
            string name = txbName.Text;
            string tKeySufix = name.Length >= 2 ? name.Substring(0, 2).ToUpper() : name.ToUpper();
            string tKeyTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string tKey = tKeyTimeStamp + tKeySufix;
            return tKey;
        }
``` 

---

### 2.2.2 - PopulateComboBoxes

The `PopulateComboBoxes` method populates a given ComboBox control with values retrieved from a database. Depending on the field parameter (e.g., `"CLIENT"` or `"COUNTRY"`), it dynamically runs a SQL query and binds the results to the ComboBox.

#### Method Signature:

* Access: `private` – Used only within the `CarrierForm` class.

```cs
private void PopulateComboBoxes(ComboBox comboBox, string field)
```

#### Parameters:

* `comboBox` (`ComboBox`) – The UI ComboBox control to populate.
* `field` (`string`) – The type of data to load (`"CLIENT"` or `"COUNTRY"`).

#### Step-by-Step Execution:

* Declare Query Variable
    * Initializes an empty query string which will be built based on the `field` value.

```cs
string query = "";
```

* Build SQL Query Based on Field Type
    * If the field is `"CLIENT"`, it queries the `TAFIE_Clients` table for active clients.
    * If the field is `"COUNTRY"`, it queries the `TAFIE_Ctry` table for active countries.
    * Results are ordered alphabetically by `Description`.
  
```cs
if (field == "CLIENT")
{
    query = "SELECT [Description] FROM TAFIE_Clients WHERE [Active] = '1' ORDER BY [Description]";
}
else if (field == "COUNTRY")
{
    query = "SELECT [Description] FROM TAFIE_Ctry WHERE [Active] = '1' ORDER BY [Description]";
}
```

* Connect to SQL Database
    * Establishes a connection to the SQL Server using the provided `connectionString`.
    * Opens the connection to begin executing commands.

```cs
using (SqlConnection conn = new SqlConnection(connectionString))
{
    conn.Open();
    ...
}
```

* Execute SQL Command and Read Data
    * A `SqlCommand` is created using the query string and open connection.
    * Executes a SqlDataReader to fetch results row-by-row.
    * Clears any existing items in the ComboBox to prevent duplicates.
    * Loops through the results and adds each `Description` value to the ComboBox.

```cs
using (SqlCommand cmd = new SqlCommand(query, conn))
{
    using (SqlDataReader reader = cmd.ExecuteReader())
    {
        comboBox.Items.Clear();
        while (reader.Read())
        {
            comboBox.Items.Add(reader["Description"].ToString());
        }
    }
}
```

* Exception Handling
    * If an error occurs:
        * Resets the cursor.
        * Displays a custom error message box with error code `112`.
        * Logs the error to the system log.
        * Closes the application to prevent further issues.

```cs
catch (Exception ex)
{
    Cursor.Current = Cursors.Default;
    CustomMessageBox messageBox = new CustomMessageBox();
    messageBox.ShowDefError("112", $"\n{ex.Message}");
    SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[PopulateComboBoxes]",$"FAILED: Code 112 ( {ex.Message} )");
    SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[PopulateComboBoxes]","Application Closed");
    Application.Exit();
}
```

#### Summary:

The `PopulateComboBoxes` method dynamically fills UI ComboBox controls with either a list of active clients or countries from the database. It’s a reusable method that encapsulates database access and control population in a clean and structured way, with error handling included to catch issues during execution.

```cs
// Populate Combo Boxes -----------------------------------------------------------------------------------------------------------------------
        private void PopulateComboBoxes(ComboBox comboBox, string field)
        {
            // Declare Variables
            string query = "";

            if (field == "CLIENT")
            {
                query = "SELECT [Description] FROM TAFIE_Clients WHERE [Active] = '1' ORDER BY [Description]";
            }
            else if (field == "COUNTRY")
            {
                query = "SELECT [Description] FROM TAFIE_Ctry WHERE [Active] = '1' ORDER BY [Description]";
            }

            try
            {
                // Execute SQL Command 
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open(); // Open SQL Connection

                    // Combo Box //
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Execute Data Reader
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            comboBox.Items.Clear(); // Clear Combo box ready for new data

                            // Populate ComboBox from reader
                            while (reader.Read())
                            {
                                comboBox.Items.Add(reader["Description"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex) // Catch any errors
            {
                Cursor.Current = Cursors.Default;
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("112", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[PopulateComboBoxes]", $"FAILED: Code 112 ( {ex.Message} )");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[PopulateComboBoxes]", "Application Closed");
                Application.Exit();
            }
        }
```

---

### 2.2.3 - PopulateCarrierComboBoxes

The `PopulateCarrierComboBoxes` method populates either the carrier (`cbCarrier`) or service (`cbService`) ComboBox with values fetched from a stored procedure in the database, using the selected client and, if applicable, the carrier as parameters.

#### Method Signature:

* Access: `private` – Only accessible within the `CarrierForm` class.

```cs
private void PopulateCarrierComboBoxes(ComboBox comboBox, string client)
```

#### Parameters:

* `comboBox` (`ComboBox`) – The UI control to populate (either `cbCarrier` or `cbService`).
* `client` (`string`) – The selected client name used in the SQL query.

#### Step-by-Step Execution:

* Initial Setup & Logging
    * Logs the start of the method.
    * Declares a placeholder SQL query string.
    * Retrieves the currently selected carrier (used if populating services).

```cs
SessionMaintenance.LogBook("", "[CarrierForm]", "[PopulateCarrierComboBoxes]", "Method Started");
string query = "";
string carrier = cbCarrier.Text ?? "";
```

* Determine SQL Query
    * Decides which version of the `TCAR_Get_Combos` stored procedure to run:
        * `@Type = 3` returns a list of carriers.
        * `@Type = 2` returns a list of delivery methods for a specific carrier.

```cs
if (comboBox == cbCarrier)
{
    query = "EXECUTE [TCAR_Get_Combos] @Client, 3";
}
else if (comboBox == cbService)
{
    query = "EXECUTE [TCAR_Get_Combos] @Client, 2, @Carrier";
}
```

* SQL Connection and Command Execution
    * Opens a connection to the SQL Server using the class-level `connectionString`.
    * Prepares a SQL command using the built query.
    * Adds required parameters to the command:
        * Always includes `@Client`.
        * Includes `@Carrier` if applicable.

```cs
using (SqlConnection conn = new SqlConnection(connectionString))
{
    conn.Open();
    using (SqlCommand cmd = new SqlCommand(query, conn))
    {
        cmd.Parameters.AddWithValue("@Client", client);
        cmd.Parameters.AddWithValue("@Carrier", carrier);
        ...
    }
}
```

* Read and Populate Data
    * Executes the command and opens a `SqlDataReader` using `using (SqlDataReader reader = cmd.ExecuteReader())`.
    * Clears existing items from the ComboBox before adding new ones using `comboBox.Items.Clear();`.
    * Depending on the target ComboBox:
        * If populating the carrier ComboBox, adds values from the `Carrier` column.
        * If populating the service ComboBox, adds values from the `Delivery Method` column.

```cs
if (comboBox == cbCarrier)
{
    while (reader.Read())
    {
        comboBox.Items.Add(reader["Carrier"].ToString());
    }
}
else if (comboBox == cbService)
{
    while (reader.Read())
    {
        comboBox.Items.Add(reader["Delivery Method"].ToString());
    }
}
```

* Error Handling
    * If an error occurs:
        * Resets the cursor.
        * Displays a custom error message using a CustomMessageBox.
        * Logs the error with code 112 and relevant details.

```cs
catch (Exception ex)
{
    Cursor.Current = Cursors.Default;
    CustomMessageBox messageBox = new CustomMessageBox();
    messageBox.ShowDefError("112", $"\n{ex.Message}");
    SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[PopulateComboBoxes]", $"FAILED: Code 112 ( {ex.Message} )");
}
```

#### Summary:

`PopulateCarrierComboBoxes` is responsible for dynamically loading available carrier or delivery method options based on the client and optionally the carrier. It interfaces directly with the backend via a stored procedure, offering a flexible way to bind data to UI controls while also including full error reporting and logging.

```cs
// Populate Carrier Combo Boxes --------------------------------------------------------------------------------------
        private void PopulateCarrierComboBoxes(ComboBox comboBox, string client)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[PopulateCarrierComboBoxes]", "Method Started");
            string query = "";
            string carrier = cbCarrier.Text ?? "";

            if (comboBox == cbCarrier)
            {
                query = "EXECUTE [TCAR_Get_Combos] @Client, 3";

            }
            else if (comboBox == cbService)
            {
                query = "EXECUTE [TCAR_Get_Combos] @Client, 2, @Carrier";
            }

            try
            {
                // Execute SQL Command 
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open(); // Open SQL Connection

                    // Combo Box //
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Client", client);
                        cmd.Parameters.AddWithValue("@Carrier", carrier);
                        // Execute Data Reader
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            comboBox.Items.Clear(); // Clear Combo box ready for new data

                            if (comboBox == cbCarrier)
                            {
                                // Populate ComboBox from reader
                                while (reader.Read())
                                {
                                    comboBox.Items.Add(reader["Carrier"].ToString());
                                }
                            }
                            else if (comboBox == cbService)
                            {
                                // Populate ComboBox from reader
                                while (reader.Read())
                                {
                                    comboBox.Items.Add(reader["Delivery Method"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) // Catch any errors
            {
                Cursor.Current = Cursors.Default;
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("112", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[PopulateComboBoxes]", $"FAILED: Code 112 ( {ex.Message} )");
            }
        }
```

---

### 2.2.4 - UpdateCarrDescr

The `UpdateCarrDescr` method updates the carrier service description label on the UI based on the delivery method selected. It fetches the corresponding description from the database and then performs additional data updates and UI refresh actions.

#### Input Parameter:

* `delMethod` (`string`) – The delivery method code used to query the corresponding service description from the database.

#### Step-by-Step Execution:

* Define SQL Query
    * The method defines a SQL query to retrieve the Description from the `TCAR_CMAP` table where the `E9_Del_Method` matches the provided `delMethod`.

```sql
SELECT [Description] FROM TCAR_CMAP WHERE E9_Del_Method = @Del_Method
```

* Open SQL Connection
    * A `SqlConnection` object is created using the `connectionString`.
    * The connection to the database is opened.

* Prepare and Execute SQL Command
    * A `SqlCommand` object is initialized with the query and connection.
    * The `@Del_Method` parameter is populated using the provided `delMethod`.
    * The command is executed using a `SqlDataReader`.

* Read and Apply Result
    * If a record is found, the `Description` is retrieved from the result and assigned to the `lblServiceDescr.Text` label on the form.
    * If no result is found, the label remains empty or unchanged.

* Trigger Downstream Actions
    * After updating the description label, three additional operations are triggered:
        * `SaveData()` – To save any changes to the `TCAR` record relating to the shipment.
        * `RecalculateBoxes()` – Recalculates box-related metrics.
        * `PopulateUI(2)` – Updates parts of the UI.

* Error Handling
    * If any exceptions occur:
        * A custom error message is shown via `CustomMessageBox`.
        * An error log entry is written using `SessionMaintenance.LogBook`.

```cs      
 // Update Carrier Description --------------------------------------------------------------------------------------------
        private void UpdateCarrDescr(string delMethod)
        {
            string query = "SELECT [Description] FROM TCAR_CMAP WHERE E9_Del_Method = @Del_Method";

            try
            {
                // Execute SQL Command 
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open(); // Open SQL Connection

                    // Combo Box //
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Del_Method", delMethod);
                        // Execute Data Reader
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Populate ComboBox from reader
                            if (reader.Read())
                            {
                                lblServiceDescr.Text = reader["Description"].ToString() ?? "";
                            }
                        }
                    }
                }

                SaveData();
                RecalculateBoxes();
                PopulateUI(2);
            }
            catch (Exception ex) // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("An error occured getting delivery description");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[UpdateCarrDescr]", $"FAILED: Code 112 ( {ex.Message} )");
            }
        }
```

---

### 2.2.5 - GetLoadNote

The `GetLoadNote` method is responsible for initializing and populating all relevant data fields on the form using the provided load note and client values. It acts as a central routine that pulls together various dependent data sources and updates the UI accordingly.
The `GetLoadNote` method orchestrates all necessary data lookups and UI updates when a user enters a load note and client. It transitions the form from idle state to an active, populated state, ready for user review or interaction.

#### Function Signature:

* Access Modifier: private – Only accessible within the `CarrierForm` class.

#### Parameters:

* `loadNote` (`string`) – The Elucid Load Note Reference used to look up shipment-specific data.
* `client` (`string`) – The client identifier used to filter and retrieve associated data.

#### Step-by-Step Breakdown:

* Cursor Feedback
    * The method sets the cursor to a "wait" state to visually indicate that a data-fetching operation is in progress:

```cs
Cursor = Cursors.WaitCursor;
```

* Populate Carrier Dropdowns
    * Calls the `PopulateCarrierComboBoxes` method twice:
        * First for `cbCarrier` – the carrier selection dropdown.
        * Then for `cbService` – the service (delivery method) dropdown. These ensure the dropdowns are populated based on the selected client.

* Retrieve Supporting Data
    * Several helper methods are called to populate other key pieces of information:
        * `GetTcarRef`(`client`) – Retrieves a reference for the shipment.
        * `GetHeaderDetails`(`loadNote`, `client`) – Pulls header-level shipment details based on the load note.
        * `GetDelDetails()` – Retrieves delivery-specific information.
        * `GetComp()` – Retrieves component related metadata.

* Update UI Elements
    * Calls `PopulateUI` twice (with arguments `1` and `2`), for different stages of UI field updates. Also calls `PopulateDataGrid()` to refresh and populate the components table on the form.

* Reveal Form Sections
    * Several grouped UI components are made visible:
        * `cntCarr`
        * `cntDel`
        * `cntShip`
        * `cntCont`
        * `cntContact`
    * These correspond to different sections such as carrier details, delivery info, shipment details, contact info, etc.
    * Simultaneously, the logo background and instruction label are hidden to shift from a "pre-search" view into the active data-entry state:

```cs
pbLogoBack.Visible = false;
lblInstruct.Visible = false;
```

* Cleanup and Reset
    * In the `finally` block:
        * The cursor is reset to default.
        * A status label is hidden, signaling completion of the operation.

```cs
 // Get Load Note ----------------------------------------------------
        private void GetLoadNote(string loadNote, string client)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                PopulateCarrierComboBoxes(cbCarrier, client);
                PopulateCarrierComboBoxes(cbService, client);
                GetTcarRef(client);
                GetHeaderDetails(loadNote, client);
                GetDelDetails();
                GetComp();
                PopulateUI(1);
                PopulateUI(2);
                PopulateDataGrid();

                //btnBoxes.Visible = true;

                cntCarr.Visible = true;
                cntDel.Visible = true;
                cntShip.Visible = true;
                cntCont.Visible = true;
                cntContact.Visible = true;
                pbLogoBack.Visible = false;
                lblInstruct.Visible = false;
            }
            finally
            {
                Cursor = Cursors.Default;
                lblStatus.Visible = false;
            }
        }
```   

---

### 2.2.6 - GetTcarRef

The `GetTcarRef` method is responsible for retrieving the `TCAR_Ref`—a unique shipment reference ID generated by a SQL stored procedure on the database, based on the specified client. This reference is central to tracking and saving carrier shipment data throughout the application.
The GetTcarRef method ensures that a valid shipment reference ID is generated for the selected client. This reference is essential for associating all subsequent shipment-related data (like addresses, services, and package details) with a unique identifier.

#### Function Signature:

* Access Modifier: `private` – Used internally within the `CarrierForm` class.

#### Parameters:

* `client` (`string`) – A client identifier used to filter the reference being retrieved.

#### Step-by-Step Breakdown:

* SQL Query Preparation
    * The method prepares a stored procedure call to `TCAR_Get_Ref`, which is expected to return a unique shipment reference (`TCAR_Ref`) for the given client:

```cs
string query = "EXECUTE [TCAR_Get_Ref] @Client";
```

* Reset Existing Reference
    * Before querying the database, the method explicitly resets the existing `tcarRef` to `null`, ensuring no old or invalid values persist:

```cs
tcarRef = null;
```

* Execute SQL Command
    * A SQL connection is established, and the stored procedure is executed using a parameterized command to prevent SQL injection and ensure safe data handling:

```cs
cmd.Parameters.AddWithValue("@Client", client);
```

* Read Query Results
    * A `SqlDataReader` is used to process the result:
        * If data is returned, it reads the `TCAR_Ref` value from the result set and stores it in the class-level tcarRef variable.
        * This value is then used by other methods (such as `SaveData`) to associate the form's data with the correct record in the database.

* Error Handling
    * Any exceptions during the database operation are caught, and the following actions are taken:
        * An error message is shown to the user via `CustomMessageBox`.
        * The issue is logged to the application log using the `SessionMaintenance.LogBook` method, with an appropriate error code (`226`) for traceability:

```cs
SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetTcarRef]", $"FAILED: Code 226 ( {ex.Message} )");
```

  ```cs    
  // Get TCAR_Ref ---------------------------------------------------------------------------------------------
        private void GetTcarRef(string client)
        {
            string query = "EXECUTE [TCAR_Get_Ref] @Client";

            tcarRef = null;

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {

                        cmd.Parameters.AddWithValue("@Client", client);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tcarRef = reader["TCAR_Ref"].ToString();
                            }
                        }
                    }
                    conn.Close(); // Close SQL Connection
                }
            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured getting TCAR Reference \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetTcarRef]", $"FAILED: Code 226 ( {ex.Message} )");
            }
        }
```

---

### 2.2.7 - GetHeaderDetails

The `GetHeaderDetails` method is responsible for retrieving shipment header data from the database based on the provided client and load note. This header data forms the foundation of the shipment record and is used to pre-populate fields in the carrier form.
The `GetHeaderDetails` method ensures that key shipment information is retrieved and initialized before further processing. By running the `TCAR_Get_Headers` procedure, the application guarantees that all required data is in place to proceed with label creation, editing, or validation.

#### Function Signature:

* Access Modifier: `private` – Used internally within the `CarrierForm` class.

#### Parameters:

* `loadNote` (`string`) – An Elucid reference to a picking list for an order prior to shipment.
* `client` (`string`) – The client for which the shipment is being prepared.

#### Step-by-Step Breakdown:

* SQL Stored Procedure Call
    * The method prepares to execute a stored procedure named `TCAR_Get_Headers`, which populates header-level information for the current shipment. This includes key details such as origin, destination, and shipment metadata.

```cs  
string query = "EXECUTE [TCAR_Get_Headers] @TCAR_Ref, @Session_Id, @Client, @Load_Note, @User";
```

* Database Execution
    * The method opens a SQL connection and constructs a `SqlCommand` with all the required parameters.
    * This ensures the stored procedure is properly parameterized, which protects against SQL injection and supports safer, dynamic query execution.

```cs  
cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
cmd.Parameters.AddWithValue("@Session_Id", sessionId);
cmd.Parameters.AddWithValue("@Client", client);
cmd.Parameters.AddWithValue("@Load_Note", loadNote);
cmd.Parameters.AddWithValue("@User", userName);
``` 

* Execution and Cleanup
    * The command is executed using `cmd.ExecuteNonQuery()` since no data is directly returned to the application (the procedure saves the retrieved data to a table for later evaluation).
    * After the operation, the SQL connection is closed.

* Error Handling
    * If any exception occurs during the operation:
        * A custom message box is shown to the user to report the issue.
        * The error is logged in the system log with an appropriate code (`226`) for tracking and diagnostics.

```cs  
SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetHeaderDetails]", $"FAILED: Code 226 ( {ex.Message} )");
```

```cs       
// Get Header ------------------------------------------------------------------------------------------------------------------
        private void GetHeaderDetails(string loadNote, string client)
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
                        cmd.Parameters.AddWithValue("@Session_Id", sessionId);
                        cmd.Parameters.AddWithValue("@Client", client);
                        cmd.Parameters.AddWithValue("@Load_Note", loadNote);
                        cmd.Parameters.AddWithValue("@User", userName);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close(); // Close SQL Connection
                }
            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured getting shipment headers \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetHeaderDetails]", $"FAILED: Code 226 ( {ex.Message} )");
            }
        }
```

---

### 2.2.8 - GetDelDetails

The `GetDelDetails` method is responsible for retrieving delivery-specific details for the current shipment using the internal shipment reference (`tcarRef`). These details are used to populate and update the form UI and ensure accurate delivery information is linked to the shipment record.
The `GetDelDetails` method acts as a backend data refresh for delivery information. It ensures that up-to-date delivery data associated with the shipment (`tcarRef`) is retrieved from the database and made available to the form. This helps maintain data integrity and accuracy before generating carrier labels or saving form state.

#### Function Signature:

* Access Modifier: `private` – This method is only accessible within the `CarrierForm` class.

#### Step-by-Step Breakdown:

* SQL Stored Procedure Call
    * The method calls the stored procedure `TCAR_Get_Del`, which is responsible for fetching delivery-specific fields associated with the shipment referenced by `tcarRef`.

```cs
string query = "EXECUTE [TCAR_Get_Del] @TCAR_Ref";
```

* Database Connection and Execution
    * A SQL connection is opened.
    * A `SqlCommand` is constructed with the required parameter (`@TCAR_Ref`) set to the current shipment reference.
    * The procedure is executed via `ExecuteNonQuery()` since it performs an operation but does not return data directly to the application.

```cs
cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
cmd.ExecuteNonQuery();
```

* Cleanup
    * The SQL connection is properly closed after the command completes.

* Error Handling
    * If an error occurs:
        * A custom error message is displayed to the user.
        * The issue is logged via `SessionMaintenance.LogBook()` for troubleshooting, including a unique error code (`226`).

```cs
SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetDelDetails]", $"FAILED: Code 226 ( {ex.Message} )");
```

```cs      
 // Get Delivery Details ----------------------------------------------------------------------------------------
        private void GetDelDetails()
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
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetDelDetails]", $"FAILED: Code 226 ( {ex.Message} )");
            }
        }
```

---

### 2.2.9 - GetComp

The `GetComp` method retrieves component-level details associated with a shipment, identified by the internal reference tcarRef. These components represent the individual items that make up the shipment contents.
`GetComp`’s purpose is to call a stored procedure that fetches detailed data about the components tied to a specific shipment, such as parcel items, pallet contents, or SKU-level breakdowns.

#### Function Signature:

* Access Modifier: `private` – Only accessible within the `CarrierForm` class.

#### Step-by-Step Breakdown:

* SQL Command Preparation
    * The method prepares a call to a stored procedure named `TCAR_Get_Comp`, passing in the unique reference of the shipment (`tcarRef`):

```cs
string query = "EXECUTE [TCAR_Get_Comp] @TCAR_Ref";
```

* Database Operation
    * A connection to the SQL Server is established using `connectionString`.
    * The command object is configured with the `@TCAR_Ref` parameter, dynamically set based on the current shipment reference.
    * `ExecuteNonQuery()` is used, as the stored procedure does not return a result set, but performs an internal update and prepares data for subsequent operations (like populating UI).

```cs
cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
cmd.ExecuteNonQuery();
```

* Cleanup
    * The SQL connection is properly closed after the query is executed:

```cs
conn.Close();
```

* Error Handling
    * If the database operation fails, the method:
        * Shows a custom error message to the user
        * Logs the detailed error in the application log via `SessionMaintenance.LogBook`, using a distinct error code for traceability:

```cs
SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetComp]", $"FAILED: Code 226 ( {ex.Message} )");
```

#### Summary

The `GetComp` method acts as a backend data call that brings in component-level shipment details for a specific transaction. Although it doesn't update the UI directly, it populates SQL tables used later when filling out the form or generating labels. It ensures that all related product or parcel information is fetched and synchronized.

```cs
// Get Components  ------------------------------------------------------------------------------
        private void GetComp()
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
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetComp]", $"FAILED: Code 226 ( {ex.Message} )");
            }
        }
```

---

### 2.2.10 - RecalculateBoxes

The `RecalculateBoxes` method triggers a recalculation of box quantities for the current shipment, based on the shipment’s internal reference `tcarRef`. This is typically used when changes in delivery shipping services require an updated calculation of how many boxes are needed for dispatch.
The purpose of the method is to call a stored procedure that performs backend logic to recalculate how the shipment should be split into boxes — adjusting weight, dimensions, or grouping based on current shipment contents.

#### Function Signature

* Access Modifier: `private` – Only accessible within the `CarrierForm` class.

#### Step-by-Step Breakdown

* Logging Start of Operation
    * Before executing any SQL, a log entry is created to indicate that box calculation is starting:

```cs
SessionMaintenance.LogBook("", "[CarrierForm]", "[RecalculateBoxes]", $"Box Calculations Started");
```

* SQL Command Preparation
    * The method defines a stored procedure call, using the internal reference of the shipment (`tcarRef`). This procedure handles the recalculation logic in SQL.

```cs
string query = "EXECUTE [TCAR_RECAL] @TCAR_Ref";
```

* Database Operation
    * A connection to the SQL Server is opened using `connectionString`.
    * A `SqlCommand` object is created and configured with the required parameter.
    * `ExecuteNonQuery()` is used, indicating the procedure performs an action without returning rows.
    * The recalculation likely affects temporary tables or updates related metadata in the database for future UI updates.

```cs
cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
cmd.ExecuteNonQuery();
```

* Cleanup
    * The SQL connection is explicitly closed after execution to release resources using `conn.Close();`

* Error Handling
    * If something goes wrong (e.g., network issues, SQL errors), the method:
        * Displays a custom error message to the user using `messageBox.ShowError`
        * Logs the error for debug and review using `SessionMaintenance.LogBook`

#### Summary

The `RecalculateBoxes` method ensures the backend system reflects an accurate number of boxes required for shipping a specific transaction. It’s a backend data action that updates the shipment packaging structure, often following updates to components or services. Although it doesn’t directly affect the UI, it plays a crucial role in ensuring that the logistics data is correct and ready for label generation or dispatch planning.

```cs    
 // Recalculate Boxes ------------------------------------------------------------------------------------------
        private void RecalculateBoxes()
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[RecalculateBoxes]", $"Box Calculations Started");

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
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[RecalculateBoxes]", $"FAILED: Code 226 ( {ex.Message} )");
            }
        }
```

---

### 2.2.11 - PopulateUI

The `PopulateUI` method is responsible for retrieving and populating UI form fields with either shipment header information or summary statistics, depending on the mode passed in.
It acts as a bridge between the data retrieved from SQL Server and the visual elements on the form (e.g., text boxes and labels), ensuring that the user interface reflects the latest data stored in the database.

#### Function Signature:

* Access Modifier: `private` – Only accessible within the `CarrierForm` class.

#### Parameters:

* `int` mode – Controls which data set is retrieved and how the UI is updated:
    * `mode = 1`: Populates shipment header fields (customer name, address, etc.).
    * `mode = 2`: Populates summary stats (total weight, value, volume, box count, DDP info).

#### Step-by-Step Breakdown:

* SQL Command Selection
    * Based on the `mode` argument, the SQL command is constructed to retrieve different result sets:

```cs
switch (mode)
{
    case 1: query = "EXECUTE [TCAR_Get_Results] @TCAR_Ref, 1"; break;
    case 2: query = "EXECUTE [TCAR_Get_Results] @TCAR_Ref, 3"; break;
}
```

* Database Query Execution
    * A connection is opened to SQL Server using `connectionString`.
    * A parameterized SQL command is prepared, passing in the current shipment reference `tcarRef`.
    * The `SqlDataReader` reads the result returned from the stored procedure.

```cs
cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
using (SqlDataReader reader = cmd.ExecuteReader())
```

* Data Assignment
    * If the reader returns a result, values are extracted and stored in class-level variables depending on the mode:
        * Mode 1: Customer details, shipment method, and contact information.
        * Mode 2: Shipment summary metrics (weight, value, volume, etc.).


* UI Control Population
    * After reading the database values, the corresponding UI fields are updated:
    * Text boxes (`txbName`, `txbCompany`, etc.)
    * Labels (`lblTotWeight`, `lblVol`, etc.)
    * Combo boxes (`cbCarrier`, `cbService`, etc.)
    * This ensures the form reflects accurate and current data.

```cs
cbCarrier.Text = pbCarrier;
txbName.Text = pbName;
// ...
lblTotWeight.Text = $"Total Weight: {pbTotWeight} KG";
```

* Error Handling
    * If an exception occurs during the SQL operation:
        * A custom error message is shown to the user.
        * The issue is logged using `SessionMaintenance.LogBook`, with code `227` for traceability.

```cs
messageBox.ShowDefError("227", $"\n{ex.Message}");
SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[PopulateUI]", $"FAILED: Code 227 ( {ex.Message} )");
```

#### Summary

The `PopulateUI` method serves as a key link between backend data and the front-end user interface. It uses a mode-switching mechanism to determine which dataset to fetch and how to apply it to the form controls. This method ensures the form is populated with either header information or calculated totals, depending on the stage in the workflow.
By structuring the logic based on the mode, it keeps the method reusable and prevents redundancy. It’s a critical part of making the form reactive to underlying shipment data.

```cs
// Populate UI -----------------------------------------------------------------------------------------------------------
        private void PopulateUI(int mode)
        {
            string query = "";

            switch (mode)
            {
                case 1: query = "EXECUTE [TCAR_Get_Results] @TCAR_Ref, 1"; break;
                case 2: query = "EXECUTE [TCAR_Get_Results] @TCAR_Ref, 3"; break;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (mode == 1)
                                {
                                    pbCarrier = reader["Carrier"].ToString();
                                    pbService = reader["Del_Method"].ToString();
                                    pbServiceDescr = reader["Service_Descr"].ToString();
                                    pbInco = reader["Inco"].ToString();
                                    pbEori = reader["Eori"].ToString();
                                    pbIoss = reader["IOSS"].ToString();
                                    pbName = reader["Name"].ToString();
                                    pbCompany = reader["Company"].ToString();
                                    pbPostcode = reader["Postcode"].ToString();
                                    pbCity = reader["City"].ToString();
                                    pbAddr1 = reader["Addr1"].ToString();
                                    pbAddr2 = reader["Addr2"].ToString();
                                    pbCounty = reader["County"].ToString();
                                    pbCountry = reader["Country"].ToString();
                                    pbEmail = reader["Email"].ToString();
                                    pbPhone = reader["Phone"].ToString();
                                    pbRef1 = reader["Ref1"].ToString();
                                    pbRef2 = reader["Ref2"].ToString();
                                    apiAccCode = reader["Acc_Code"].ToString();
                                }
                                else if (mode == 2)
                                {
                                    pbTotWeight = reader["Total_Weight"].ToString();
                                    pbTotValue = reader["Total_Value"].ToString();
                                    pbVolume = reader["Volume"].ToString();
                                    pbBoxes = reader["Boxes"].ToString();
                                    ddp = (int)reader["DDP"];
                                    domestic = (int)reader["Domestic"];
                                }
                            }
                        }
                    }
                }

                if (mode == 1)
                {
                    cbCarrier.Text = pbCarrier;
                    cbService.Text = pbService;
                    txbInco.Text = pbInco;
                    txbEORI.Text = pbEori;
                    txbIOSS.Text = pbIoss;
                    txbName.Text = pbName;
                    txbCompany.Text = pbCompany;
                    txbPostcode.Text = pbPostcode;
                    txbCity.Text = pbCity;
                    txbAddr1.Text = pbAddr1;
                    txbAddr2.Text = pbAddr2;
                    txbCounty.Text = pbCounty;
                    cbCountry.Text = pbCountry;
                    txbEmail.Text = pbEmail;
                    txbPhone.Text = pbPhone;
                    txbRef1.Text = pbRef1;
                    txbRef2.Text = pbRef2;
                    lblServiceDescr.Text = pbServiceDescr;
                }
                else if (mode == 2)
                {
                    lblTotWeight.Text = $"Total Weight: {pbTotWeight} KG";
                    lblTotValue.Text = $"Total Value: {pbTotValue} GBP";
                    lblVol.Text = $"Volume: {pbVolume}";
                    lblBoxes.Text = $"Boxes: {pbBoxes}";
                    lblDdp.Text = ddp == 1 ? "Duties Paid" : "Duties Unpaid";
                }


            }
            catch (Exception ex) // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("227", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[PopulateUI]", $"FAILED: Code 227 ( {ex.Message} )");
            }
        }
```

---

### 2.2.12 - PopulateDataGrid

The `PopulateDataGrid` method is responsible for retrieving detailed shipment content data from the database and displaying it in a `DataGridView` control (`dgContents`) on the UI. This includes a list of shipment items such as products, SKUs, weights, or dimensions, allowing users to visually inspect or validate the load contents.

#### Function Signature:

* Access Modifier: `private` – Only accessible within the `CarrierForm` class.

#### Step-by-Step Breakdown:

* SQL Command Preparation
    * The method constructs a SQL query to retrieve result set type 2 from the stored procedure `TCAR_Get_Results`, using the current transaction reference (`tcarRef`):

```cs
string query = "EXECUTE [TCAR_Get_Results] @TCAR_Ref, 2";
```

* Data Container Setup
    * An empty `DataTable` object is created to store the query results for later binding to the UI grid:

```cs
DataTable dataTable = new DataTable();
```

* SQL Query Execution
    * A connection to the SQL Server is opened using `connectionString`.
    * The stored procedure is executed via a `SqlCommand`.
    * The `SqlDataReader` reads the result set from the database.
    * The result set is loaded into the `DataTable`.

```cs
cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
SqlDataReader reader = cmd.ExecuteReader();
dataTable.Load(reader);
```

* Data Binding to UI
    * Once the `DataTable` is populated, it is bound to the `DataGridView` (`dgContents`) for display on the UI:

```cs
dgContents.DataSource = dataTable;
dgContents.Refresh();
```

* Column Handling
    * After binding, the last column in the grid is hidden. This is done to suppress an error status returned from the stored procedure, not meant for user display:

```cs
int errorColumnIndex = dgContents.Columns.Count - 1;
dgContents.Columns[errorColumnIndex].Visible = false;
```

* Conditional Formatting
    * Additional formatting is applied to the grid by calling the `FormatDataGrid()` method. This highlights rows based on specific rules (e.g., hidden error status column, missing weights, flags, or incorrect quantities):

```cs
FormatDataGrid();
```

* Error Handling
    * If the database operation fails:
        * A custom error message is shown via `CustomMessageBox`.
        * The error is logged with code `117` for traceability:

```cs
messageBox.ShowDefError("117", $"\n{ex.Message}");
SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[PopulateDataGrid]", $"FAILED: Code 117 ( {ex.Message} )");
```

#### Summary

The `PopulateDataGrid` method retrieves detailed shipment content data using the TCAR reference and displays it in a structured table within the UI. It enables users to verify or inspect the line items associated with a shipment. Key UI tasks like hiding sensitive columns and formatting are also handled here, ensuring the data is both functional and user-friendly.

```cs
// Populate DataGrid ---------------------------------------------------------------------------------------------
        private void PopulateDataGrid()
        {
            string query = "EXECUTE [TCAR_Get_Results] @TCAR_Ref, 2";

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

                        // Execute Query
                        SqlDataReader reader = cmd.ExecuteReader();

                        // Populate DataTable From Reader
                        dataTable.Load(reader);
                    }

                    conn.Close(); // Close SQL Connection

                    // Populate Data Grid
                    dgContents.DataSource = dataTable;
                    dgContents.Refresh();

                    // Hide the last column
                    if (dgContents.Columns.Count > 0)
                    {
                        int errorColumnIndex = dgContents.Columns.Count - 1; // Last column index
                        dgContents.Columns[errorColumnIndex].Visible = false;
                    }

                    // Apply conditional formatting
                    FormatDataGrid();
                }
            }
            catch (Exception ex) // Catch Errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("117", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[PopulateDataGrid]", $"FAILED: Code 117 ( {ex.Message} )");
            }
        }
```

---

### 2.2.13 - FormatDataGrid

The `FormatDataGrid` method applies conditional formatting to the rows in the `dgContents` `DataGridView` control, based on the error flags returned from the database. This helps visually highlight rows that may contain warnings or errors, allowing users to quickly identify problematic shipment items.

#### Function Signature:

* Access Modifier: `private` – Only accessible within the `CarrierForm` class.

#### Step-by-Step Breakdown:

* Row Iteration
    * The method loops through every row in the `dgContents` grid:

```cs
foreach (DataGridViewRow row in dgContents.Rows)
```

* Column Safety Check
    * Before formatting, it confirms that each row contains at least one cell to prevent runtime errors:

```cs
if (row.Cells.Count > 0)
```

* Identify Error Column
    * The last column in the grid is assumed to contain an integer "error flag" that determines formatting. This column was previously hidden in PopulateDataGrid:

```cs
int errorColumnIndex = dgContents.Columns.Count - 1;
```

* Conditional Formatting Based on Error Flag
    * The method checks the error flag value and applies formatting as follows:
      
1. Error Flag = 2 (Warning)
    * Background: Yellow
    * Text: Black
    * This usually signals a warning that requires user attention but not necessarily a critical failure.

```cs
if (errorFlag == 2)
{
    row.DefaultCellStyle.BackColor = Color.Yellow;
    row.DefaultCellStyle.ForeColor = Color.Black;
}
```

2. Error Flag = 1 (Error)
    * Background: Red
    * Text: Black
    * Indicates a more serious issue in the row’s data that likely needs correction.

```cs
else if (errorFlag2 == 1)
{
    row.DefaultCellStyle.BackColor = Color.Red;
    row.DefaultCellStyle.ForeColor = Color.Black;
}
```
3. No Error / Default State
    * Background: White
    * Text: Black
    * Applied when the flag is neither 1 nor 2.

```cs
else
{
    row.DefaultCellStyle.BackColor = Color.White;
    row.DefaultCellStyle.ForeColor = Color.Black;
}
```

* Note: `int.TryParse()` is used for robust parsing of cell values into integers, ensuring null or malformed data doesn’t crash the loop.

#### Summary

The `FormatDataGrid` method visually distinguishes error states in the shipment line items by color-coding rows in the `DataGridView`. It uses a hidden error flag column to determine whether a row contains an error (`Red`), warning (`Yellow`), or no issue (`White`). This provides quick feedback for users inspecting or editing shipment data in the interface.

```cs
// Apply Conditional Formatting Based on Error Flag ----------------------------------------------------------------------------
        private void FormatDataGrid()
        {
            foreach (DataGridViewRow row in dgContents.Rows)
            {
                if (row.Cells.Count > 0)
                {
                    int errorColumnIndex = dgContents.Columns.Count - 1; // Last column index

                    if (int.TryParse(row.Cells[errorColumnIndex].Value?.ToString(), out int errorFlag) && errorFlag == 2)
                    {
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else if (int.TryParse(row.Cells[errorColumnIndex].Value?.ToString(), out int errorFlag2) && errorFlag2 == 1)
                    {
                        row.DefaultCellStyle.BackColor = Color.Red;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }
```

---

### 2.2.14 - CompleteTCAR

The `CompleteTCAR` method finalizes the shipment process for a transaction (TCAR), recording the tracking number and the box reference in the system. This marks the shipment as complete in the database, updating its status and associating relevant logistics information.

#### Function Signature:

* Access Modifier: `private` – Only accessible within the `CarrierForm` class.

#### Parameters:

* `tracking`(`string`) – The tracking number assigned to the shipment.
* `BoxRef`(`int`) – The reference number for the box being used.

#### Step-by-Step Breakdown:
* SQL Command Setup
    * A SQL stored procedure is called to complete the TCAR transaction, with three parameters passed to it:

```cs
string query = "EXECUTE [TCAR_Complete_V2] @TCAR_Ref, @Tracking, @Box_Ref";
```

* Database Execution
    * A new SQL connection is opened using the `connectionString`.
    * A `SqlCommand` is created and configured with the stored procedure and required parameters:
        * `@TCAR_Ref`: The internal reference number for the shipment.
        * `@Tracking`: The tracking number provided by the carrier.
        * `@Box_Ref`: The numeric ID of the box or container used.
    * Since the procedure doesn’t return any result set, `ExecuteNonQuery()` is used.
  
```cs
cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
cmd.Parameters.AddWithValue("@Tracking", tracking);
cmd.Parameters.AddWithValue("@Box_Ref", BoxRef);
cmd.ExecuteNonQuery();
```

* Cleanup
    * The connection is closed after the procedure executes using `conn.Close()`;

* Error Handling
    * If an exception occurs during the SQL operation:
        * A custom error message is displayed to the user.
        * The error is also logged using `SessionMaintenance.LogBook` with the method context and exception details.

```cs
CustomMessageBox messageBox = new CustomMessageBox();
messageBox.ShowError($"An error occured Completing TCAR \n{ex.Message}");
SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[CompleteTCAR]", $"FAILED: ( {ex.Message} )");
```

#### Summary

The `CompleteTCAR` method finalizes a shipment by executing the `TCAR_Complete_V2` stored procedure. It attaches a tracking number and box reference to the shipment and updates its status in the database. This is typically one of the final steps in the label generation workflow, ensuring the transaction is marked as complete and ready for dispatch or handover to a courier.

```cs
  // Complete TCAR -------------------------------------------------------------------------------------------
        private void CompleteTCAR(string tracking, int BoxRef)
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
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[CompleteTCAR]", $"FAILED: ( {ex.Message} )");
            }
        }
```

---

### 2.2.15 - CheckLoadNote

The `CheckLoadNote` method verifies the existence of an Elucid Load Note associated with a specific client. This method checks whether a particular Load Note exists in the database.

#### Function Signature:

* Access Modifier: `private` – Only accessible within the `CarrierForm` class.
* Return Type: `int` – Returns an integer result (`1` for exists, `0` for not found).

#### Parameters:

* `loadNote` (`string`) – An Elucid reference to a picking list for an order prior to shipment.
* `client` (`string`) – The client for which the shipment is being prepared.

#### Step-by-Step Breakdown:

* Initialize Return Variable
    * An integer variable `check` is initialized to store the result:

```cs
int check = 0;
```

* SQL Query Setup
    * The method prepares a stored procedure call that checks for the presence of a Load Note in the system:

```cs
string query = "EXECUTE TAFIE_Check_Load @Session_Id, @Client, @Load_Note";
```

* Database Execution
    * A SQL connection is opened.
    * A `SqlCommand` is created and parameters are passed in:
        * `@Session_Id`: The current session identifier (`sessionId`).
        * `@Client`: The client to which the Load Note belongs.
        * `@Load_Note`: The Load Note value to validate.
    * `ExecuteScalar()` is used here because the stored procedure returns a single value (e.g., `1` or `0`).
    * The result is cast to int and stored in check.
    * Connection is closed afterward using `conn.Close();`.
      
```cs
// Assign parameters
cmd.Parameters.AddWithValue("@Session_Id", sessionId);
cmd.Parameters.AddWithValue("@Client", client);
cmd.Parameters.AddWithValue("@Load_Note", loadNote);
```

```cs
// Store Result
int result = (int)cmd.ExecuteScalar();
check = result;
```

* Error Handling
    * If the query fails:
        * The cursor is reset to default (in case it was set to waiting).
        * A custom error message is displayed to the user.
        * The error is logged with a specific error code:

```cs
Cursor.Current = Cursors.Default;
CustomMessageBox messageBox = new CustomMessageBox();
messageBox.ShowDefError("109", $"\n{ex.Message}");
SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[CheckLoadNote]", $"FAILED: Code 109 ( {ex.Message} )");
```

* Return Value
    * Finally, the result of the check is returned:

```cs
return check;
```

#### Summary

The `CheckLoadNote` method verifies whether a specific Load Note exists in the database for a given client and session. It calls the stored procedure `TAFIE_Check_Load` and returns a result code. This method is often used to prevent duplicate records or validate inputs before proceeding with shipment grouping or dispatch preparation.

```cs
// Check Load Note Exists ----------------------------------------------------------------------------------------------
        private int CheckLoadNote(string loadNote, string client)
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
                        cmd.Parameters.AddWithValue("@Session_Id", sessionId);
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
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[CheckLoadNote]", $"FAILED: Code 109 ( {ex.Message} )");
            }

            return check;
        }
```

---

### 2.2.16 - OpenURL

The `OpenURL` method launches the user's default web browser and navigates to the Royal Mail postcode finder. This is used to assist users in quickly validating or looking up postal codes during data entry or shipment processing.

#### Function Signature:

* Access Modifier: `private` – The method is only accessible within the `CarrierForm` class.

#### Step-by-Step Breakdown:

* Define Target URL
    * The method defines the URL for the Royal Mail postcode finder:

```cs
string url = "https://www.royalmail.com/find-a-postcode";
```

* Prepare Process Start Info
    * A `ProcessStartInfo` object is configured to open the URL using the system’s default web browser:
        * `FileName`: The web address to open.
        * `UseShellExecute = true`: Required to launch a URL or file with the default system handler (in this case, the web browser).

```cs
ProcessStartInfo processStartInfo = new ProcessStartInfo
{
    FileName = url,
    UseShellExecute = true
};
```

* Launch Browser
    * The process is started, effectively opening the URL in a new browser window/tab:

```cs
Process.Start(processStartInfo);
```

* Logging
    * After attempting to open the browser, the method logs a success message to the session log:

```cs
SessionMaintenance.LogBook($"", "[CarrierForm]", "[OpenURL]", $"Process Executed");
```

* Error Handling
    * If anything goes wrong (e.g., browser not found, malformed URL), the error is caught and handled:
        * A custom error message is shown to the user.
        * The issue is logged with a distinct error code for traceability:

```cs
CustomMessageBox messageBox = new CustomMessageBox();
messageBox.ShowDefError("121", $"\n{ex.Message}");
SessionMaintenance.LogBook($"ERROR", "[CarrierForm]", "[OpenURL]", $"FAILED: Code 121 (  {ex.Message}  )");
```

#### Summary

The `OpenURL` method provides a simple utility for launching a specific web page in the user’s default browser. In this case, it's used to assist with postcode lookup via the Royal Mail website. It enhances user workflow by offering quick access to external resources and logs both success and error events for auditability.

```cs
  // Open URL ------------------------------------------------------------------------------------------------------------------
        private void OpenURL()
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

                SessionMaintenance.LogBook($"", "[CarrierForm]", "[OpenURL]", $"Process Executed");

            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("121", $"\n{ex.Message}");
                SessionMaintenance.LogBook($"ERROR", "[CarrierForm]", "[OpenURL]", $"FAILED: Code 121 (  {ex.Message}  )");

            }
        }
```

---

### 2.2.17 - ClearFields

The `ClearFields` method resets the form to its initial, empty state by clearing all user inputs, variables, and UI elements associated with the label generation process. This is typically used when starting a new transaction, cancelling an entry, or resetting after submission.

#### Function Signature:

Access Modifier: `private` – Only accessible within the `CarrierForm` class.

#### Step-by-Step Breakdown:

* Clear Backend Variables (Persistent Data Holders)
    * Clears all private fields that hold shipment or transaction-specific data:
    * Also clears the main reference identifier:

```cs
pbCarrier = null;
pbService = null;
// ...
pbBoxes = null;
```

```cs
tcarRef = null;
```

* Clear UI Controls (TextBoxes, ComboBoxes, Labels)
1. All visual fields in the form are reset to a blank state using `Text = null` or `.Clear()` where applicable:

```cs
cbCarrier.Items.Clear();
cbCarrier.Text = clear;
txbName.Text = clear;
lblTotValue.Text = clear;
// ...
lblServiceDescr.Text = clear;
```

2. Special handling is applied to controls like `lblStatus` and `lblCountry`:

```cs
lblStatus.Visible = false;
lblCountry.BackColor = Color.FromArgb(11, 159, 161);
```

* Reset Field Colors
    * Ensures all input fields and visual labels revert to their default (white) background color for clarity:
    * This helps visually clear any prior formatting (e.g., red for errors).
  
```cs
txbLoadNote.BackColor = Color.White;
cbCarrier.BackColor = Color.White;
// ...
lblDomestic.BackColor = Color.White;
```

* Hide Buttons and Non-Essential Controls
1. Hides form buttons and panels that should only be visible during an active or valid transaction:

```cs
btnBoxes.Visible = false;
btnPrint.Visible = false;
cntCarr.Visible = false;
cntDel.Visible = false;
cntShip.Visible = false;
cntCont.Visible = false;
cntContact.Visible = false;
```

2. Re-displays the logo background and instructions label, assuming this is the default view state:

```cs
pbLogoBack.Visible = true;
lblInstruct.Visible = true;
```

* Clear the Data Grid
    * Removes any existing box or content entries from the `DataGridView`:

```cs
dgContents.DataSource = null;
dgContents.Refresh();
```

* Reset Focus
    * Puts the cursor focus back to the `txbLoadNote` field for new input:

```cs
txbLoadNote.Focus();
```

* Reset Internal State Flags
    * Clears or resets key logic flags used during processing:

```cs
delta = 0;
ddp = 0;
domestic = 1;
```

#### Summary

The `ClearFields` method is a comprehensive reset utility that clears all shipment-related data, UI input fields, styling, and session variables within the form. It's designed to return the `CarrierForm` to a clean state, ready for new data entry or idle waiting. This is especially useful for maintaining consistent behavior after a transaction completes or is cancelled.

```cs      
  // Clear Fields -----------------------------------------------------------------------------------------------------------------------
        private void ClearFields()
        {
            // Clear OG Parameters
            pbCarrier = null;
            pbService = null;
            pbInco = null;
            pbEori = null;
            pbIoss = null;
            pbName = null;
            pbCompany = null;
            pbPostcode = null;
            pbCity = null;
            pbAddr1 = null;
            pbAddr2 = null;
            pbCounty = null;
            pbCountry = null;
            pbEmail = null;
            pbPhone = null;
            pbRef1 = null;
            pbRef2 = null;
            pbTotWeight = null;
            pbTotValue = null;
            pbVolume = null;
            pbBoxes = null;

            // Clear fields
            tcarRef = null;
            string? clear = null;
            lblStatus.Visible = false;
            txbLoadNote.Text = clear;
            cbCarrier.Items.Clear();
            cbService.Items.Clear();
            cbCarrier.Text = clear;
            cbService.Text = clear;
            txbInco.Text = clear;
            txbEORI.Text = clear;
            txbIOSS.Text = clear;
            txbName.Text = clear;
            txbCompany.Text = clear;
            txbPostcode.Text = clear;
            txbCity.Text = clear;
            txbAddr1.Text = clear;
            txbAddr2.Text = clear;
            txbCounty.Text = clear;
            cbCountry.Text = clear;
            txbEmail.Text = clear;
            txbPhone.Text = clear;
            txbRef1.Text = clear;
            txbRef2.Text = clear;
            lblTotWeight.Text = clear;
            lblTotValue.Text = clear;
            lblVol.Text = clear;
            lblBoxes.Text = clear;
            lblServiceDescr.Text = clear;
            lblCountry.BackColor = Color.FromArgb(11, 159, 161);
            lblDdp.Text = clear;
            lblDomestic.Text = clear;

            // Reset field back colours
            txbLoadNote.BackColor = Color.White;
            cbCarrier.BackColor = Color.White;
            cbService.BackColor = Color.White;
            txbInco.BackColor = Color.White;
            txbEORI.BackColor = Color.White;
            txbIOSS.BackColor = Color.White;
            txbName.BackColor = Color.White;
            txbCompany.BackColor = Color.White;
            txbPostcode.BackColor = Color.White;
            txbCity.BackColor = Color.White;
            txbAddr1.BackColor = Color.White;
            txbAddr2.BackColor = Color.White;
            txbCounty.BackColor = Color.White;
            cbCountry.BackColor = Color.White;
            txbEmail.BackColor = Color.White;
            txbPhone.BackColor = Color.White;
            txbRef1.BackColor = Color.White;
            txbRef2.BackColor = Color.White;
            lblTotWeight.BackColor = Color.White;
            lblTotValue.BackColor = Color.White;
            lblVol.BackColor = Color.White;
            lblBoxes.BackColor = Color.White;
            lblDdp.BackColor = Color.White;
            lblDomestic.BackColor = Color.White;

            // Hide Buttons
            btnBoxes.Visible = false;
            btnPrint.Visible = false;

            // Clear Box Contents
            dgContents.DataSource = null;
            dgContents.Refresh();

            txbLoadNote.Focus();

            // Reset UI Elements
            btnPrint.Visible = false;
            cntCarr.Visible = false;
            cntDel.Visible = false;
            cntShip.Visible = false;
            cntCont.Visible = false;
            cntContact.Visible = false;
            pbLogoBack.Visible = true;
            lblInstruct.Visible = true;

            delta = 0;
            ddp = 0;
            domestic = 1;
        }
```

---

### 2.2.18 - CheckField

The `CheckField` method performs validation on individual fields of the form based on a provided field ID. It dynamically checks the content of a control (e.g., a text box) against expected rules such as required input, regex patterns, or length limits. It also provides user feedback and highlights issues where found.

#### Function Signature:

* Access Modifier: `private` – Accessible only within the containing class (`CarrierForm`).
* Return Type: `int` – Returns `1` if an error is found; `0` if validation passes.

Parameters:

* `field`(`int`) – The numeric ID representing which field is being validated.
* `control`(`Control`) – The UI control (e.g., `TextBox`, `ComboBox`) whose value should be checked.

#### Step-by-Step Breakdown:

* Extract and Prepare Input
    * The value to be checked is trimmed.
    * A `CustomMessageBox` is initialized for displaying error messages.
    * Variable `c` is used to indicate whether the check fails (`1`) or passes (`0`).

```cs
string value = control.Text.Trim();
CustomMessageBox messageBox = new CustomMessageBox();
int c = 0;
```

* Field-Specific Validation via Switch Statement
    * Each case corresponds to a specific field on the form. Validation logic varies by case:

```cs
switch (field)
{
    case 0: // Carrier
        if (string.IsNullOrEmpty(value)) { ... }
        break;
    case 1: // Del Method
        ...
}
```

* Validation Examples:

<table>
  <tr>
   <td>
Case
   </td>
   <td>Field
   </td>
   <td>Validation Rule
   </td>
  </tr>
  <tr>
   <td>0
   </td>
   <td>Carrier
   </td>
   <td>Must not be empty.
   </td>
  </tr>
  <tr>
   <td>1
   </td>
   <td>Delivery
   </td>
   <td>Must not be empty.
   </td>
  </tr>
  <tr>
   <td>2
   </td>
   <td>IncoTerms
   </td>
   <td>(No validation)
   </td>
  </tr>
  <tr>
   <td>3
   </td>
   <td>EORI
   </td>
   <td>Must match alphanumeric regex (10–17 characters), if entered.
   </td>
  </tr>
  <tr>
   <td>4
   </td>
   <td>IOSS
   </td>
   <td>Must start with IM followed by 10 digits.
   </td>
  </tr>
  <tr>
   <td>5
   </td>
   <td>Name
   </td>
   <td>Must be between 1 and 35 characters.
   </td>
  </tr>
  <tr>
   <td>6
   </td>
   <td>Company
   </td>
   <td>Must not exceed 35 characters.
   </td>
  </tr>
  <tr>
   <td>7
   </td>
   <td>Postcode
   </td>
   <td>For GB, must match standard UK postcode regex.
   </td>
  </tr>
  <tr>
   <td>8
   </td>
   <td>City
   </td>
   <td>Must be between 1 and 35 characters.
   </td>
  </tr>
  <tr>
   <td>9
   </td>
   <td>Addr1
   </td>
   <td>Must be between 1 and 35 characters.
   </td>
  </tr>
  <tr>
   <td>10
   </td>
   <td>Addr2
   </td>
   <td>Must not exceed 35 characters (if entered).
   </td>
  </tr>
  <tr>
   <td>11
   </td>
   <td>County
   </td>
   <td>Must not exceed 35 characters (if entered).
   </td>
  </tr>
  <tr>
   <td>12
   </td>
   <td>Email
   </td>
   <td>Required for international shipments; must match standard email format.
   </td>
  </tr>
  <tr>
   <td>13
   </td>
   <td>Phone
   </td>
   <td>Required for international shipments; must match phone regex (optional +).
   </td>
  </tr>
</table>


* Common Actions on Validation Failure:
    * If validation fails:
        * An error message is shown using `messageBox.ShowError(...)`
        * Focus is moved to the invalid control via `control.Focus()`
        * The return code `c` is set to `1`

* Return Result
    * Returns 1 if the field failed validation.
    * Returns 0 if the field passed all checks.

```cs
return c;
```

#### Summary

The `CheckField` method provides context-sensitive validation of user inputs in the form. Each field is handled individually using a field ID, allowing reusable logic and easy extensibility. It ensures data accuracy and user guidance before proceeding with form submission or backend logic.

```cs
// Check Field ------------------------------------------------------------------------------------------------------------------       
        private int CheckField(int field, Control control)
        {
            string value = control.Text.Trim();
            CustomMessageBox messageBox = new CustomMessageBox();
            int c = 0;
            switch (field)
            {
                case 0: // Carrier
                    if (string.IsNullOrEmpty(value))
                    {
                        messageBox.ShowError("Carrier cannot be empty.");
                        control.Focus();
                        c = 1;
                    }
                    break;
                case 1: // Del Method
                    if (string.IsNullOrEmpty(value))
                    {
                        messageBox.ShowError("Delivery Method cannot be empty.");
                        control.Focus();
                        c = 1;
                    }
                    break;
                case 2: // IncoTerms
                    break;
                case 3: // EORI
                    if (!Regex.IsMatch(value, @"^[A-Z0-9]{10,17}$", RegexOptions.IgnoreCase) && !string.IsNullOrEmpty(value))
                    {
                        messageBox.ShowError("Invalid EORI number format.");
                        control.Focus();
                        c = 1;
                    }
                    break;
                case 4: // IOSS
                    if (!Regex.IsMatch(value, @"^IM\d{10}$") && !string.IsNullOrEmpty(value))
                    {
                        messageBox.ShowError("Invalid IOSS number format (IM followed by 10 digits).");
                        control.Focus();
                        c = 1;
                    }
                    break;
                case 5: // Name
                    if (value.Length < 1 || value.Length > 35)
                    {
                        messageBox.ShowError("Name must be between 1 and 35 characters.");
                        control.Focus();
                        c = 1;
                    }
                    break;

                case 6: // Company
                    if (value.Length > 35)
                    {
                        messageBox.ShowError("Company name must be less than 35 characters.");
                        control.Focus();
                        c = 1;
                    }
                    break;
                case 7: // Postcode
                    if (cbCountry.SelectedItem != null && cbCountry.SelectedItem.ToString() == "Great Britain")
                    {
                        if (!Regex.IsMatch(value, @"^[A-Z]{1,2}\d[A-Z\d]? ?\d[A-Z]{2}$", RegexOptions.IgnoreCase))
                        {
                            messageBox.ShowError("Invalid UK postcode format.");
                            control.Focus();
                            c = 1;
                        }
                    }
                    break;
                case 8: // City
                    if (value.Length < 1 || value.Length > 35)
                    {
                        messageBox.ShowError("City must be between 1 and 35 characters.");
                        control.Focus();
                        c = 1;
                    }
                    break;
                case 9: // Addr1
                    if (value.Length < 1 || value.Length > 35)
                    {
                        messageBox.ShowError("Address Line 1 must be between 1 and 35 characters.");
                        control.Focus();
                        c = 1;
                    }
                    break;
                case 10: // Addr2
                    if (value.Length > 35 && !string.IsNullOrEmpty(value))
                    {
                        messageBox.ShowError("Address line 2 must be less than 35 characters.");
                        control.Focus();
                        c = 1;
                    }
                    break;
                case 11: // County
                    if (value.Length > 35 && !string.IsNullOrEmpty(value))
                    {
                        messageBox.ShowError("County must be less than 35 characters.");
                        control.Focus();
                        c = 1;
                    }
                    break;
                case 12: // Email
                    {
                        if (cbCountry.SelectedItem != null && cbCountry.SelectedItem.ToString() != "Great Britain" && string.IsNullOrEmpty(value))
                        {
                            messageBox.ShowError("Email is required for international shipments");
                            control.Focus();
                            c = 1;
                        }
                        if (!Regex.IsMatch(value, @"^[\w\.-]+@[\w\.-]+\.\w+$") && !string.IsNullOrEmpty(value))
                        {
                            messageBox.ShowError("Invalid email format.");
                            control.Focus();
                            c = 1;
                        }
                    }
                    break;
                case 13: // Phone
                    {
                        if (cbCountry.SelectedItem != null && cbCountry.SelectedItem.ToString() != "Great Britain" && string.IsNullOrEmpty(value))
                        {
                            messageBox.ShowError("Phone is required for international shipments");
                            control.Focus();
                            c = 1;
                        }
                        if (!Regex.IsMatch(value, @"^\+?[0-9]{1,14}$") && !string.IsNullOrEmpty(value))
                        {
                            messageBox.ShowError("Invalid phone number format.");
                            control.Focus();
                            c = 1;
                        }
                    }
                    break;
            }

            return c;
        }
```



---

### 2.2.19 - CheckAllFields

The `CheckAllFields` method is used to validate all key user input fields in the form by calling the `CheckField` method sequentially for each one. It determines whether any validation errors are present before the form can proceed with further actions such as submission.

#### Function Signature:

* Access Modifier: `private` – Only accessible within the class it's defined in (`CarrierForm`).
* Return Type: `bool` – Returns `true` if any field fails validation; otherwise `false`.

#### Step-by-Step Breakdown:

* Initialize Validation Flag
    * A flag `c` is initialized to track if any field fails validation.

```cs
int c = 0;
```

* Validate Each Field
    * Each call to `CheckField` validates a specific form control based on its index.
    * If the field fails validation, `CheckField` returns `1`, which is added to `c`.
    * If the field passes, it returns `0`.
    * This version accumulates the results, ensuring that all fields are checked and any combination of failures is captured.

```cs
c += CheckField(0, cbCarrier);
c += CheckField(1, cbService);
...
c += CheckField(13, txbPhone);
```

* Evaluate Validation Outcome
    * If `c == 0`: All fields passed validation: returns `false` (no errors).
    * If `c > 0`: At least one field failed: returns `true` (errors found).

```cs
if (c == 0)
{
    return false;
}
else
{
    return true;
}
```

#### Summary

The `CheckAllFields` method ensures comprehensive validation of form input fields by aggregating the results of individual field checks. It identifies whether any field fails validation by summing all individual error results, returning true if issues are found.

```cs
// Check all Fields --------------------------------------------------------------------------------     
        private bool CheckAllFields()
        {
            int c = 0;
            c += CheckField(0, cbCarrier);
            c += CheckField(1, cbService);
            c += CheckField(2, txbInco);
            c += CheckField(3, txbEORI);
            c += CheckField(4, txbIOSS);
            c += CheckField(5, txbName);
            c += CheckField(6, txbCompany);
            c += CheckField(7, txbPostcode);
            c += CheckField(8, txbCity);
            c += CheckField(9, txbAddr1);
            c += CheckField(10, txbAddr2);
            c += CheckField(11, txbCounty);
            c += CheckField(12, txbEmail);
            c += CheckField(13, txbPhone);

            if (c == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
```

---

### 2.2.20 - GetBoxes

The `GetBoxes` method is responsible for retrieving a list of box reference numbers (`Box_Ref`) associated with a specific `TCAR_Ref` from the database. It queries the `TCAR_Boxes` table and returns the results as a list of integers.

#### Function Signature:

* Access Modifier: `private` – Accessible only within the class it belongs to.
* Return Type: `List<int>` – A list of box reference numbers (integers).

#### Step-by-Step Breakdown:

* Define SQL Query
    * SQL statement that selects all `Box_Ref` values where the `TCAR_Ref` matches the one passed into the method.

```cs
string query = "SELECT Box_Ref FROM TCAR_Boxes WHERE TCAR_Ref = @TCAR_Ref";
```

* Initialize Return Object
    * An empty list that will hold the retrieved box reference numbers.

```cs
List<int> boxRefs = new List<int>();
```

* Execute SQL Command
    * Opens a connection to the database using the class's `connectionString`.
    * Closes it once the operation is complete.

```cs
using (SqlConnection conn = new SqlConnection(connectionString))
{
    conn.Open();
    ...
    conn.Close();
}
```

* Prepare and Execute the Command
    * Prepares the command with the SQL query.
    * Binds the current `tcarRef` as a parameter to prevent SQL injection and allow dynamic querying.

```cs
using (SqlCommand cmd = new SqlCommand(query, conn))
{
    cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
```

* Read and Populate Results
    * Executes the query and opens a data reader.
    * Iterates over the result set, extracting each `Box_Ref` and adding it to the list.

```cs
using (SqlDataReader reader = cmd.ExecuteReader())
{
    while (reader.Read())
    {
        boxRefs.Add((int)reader["Box_Ref"]);
    }
}
```

* Exception Handling
    * Catches and logs any exceptions.
    * Displays a custom error message to the user and logs the failure in the system.

```cs
catch (Exception ex)
{
    Cursor.Current = Cursors.Default;
    CustomMessageBox messageBox = new CustomMessageBox();
    messageBox.ShowError($"An error occured getting box references.\n{ex.Message}");
    SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetBoxes]", $"FAILED: ( {ex.Message} )");
}
```



* Return the Result
    * Returns the list of box reference numbers retrieved from the database.

```cs
return boxRefs;
```

#### Summary

The `GetBoxes` method retrieves all Box_Ref values from the `TCAR_Boxes` table associated with the current `tcarRef`. It returns these as a list of integers for further use (e.g., processing, display, or updating). The method includes robust error handling and safe SQL parameterization.

```cs
// Get Boxes -----------------------------------------------------------------------------------       
        private List<int> GetBoxes()
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
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetBoxes]", $"FAILED: ( {ex.Message} )");
            }

            return boxRefs;
        }
```

---

### 2.2.21 - SaveData 

The `SaveData` method is responsible for persisting the current shipment header information from the form into the database. It collects values from various UI controls (such as textboxes and combo boxes), constructs a parameterized SQL query, and executes a stored procedure to save the data.
This method ensures that any data entered or modified by the user in the shipment form is accurately and securely saved to the database.

#### Function Signature:

Access Modifier: `private` – Only accessible within the `CarrierForm` class.

#### Step-by-Step Breakdown:

* Collect Form Data
    * The method starts by extracting user input from form fields including:
        * Carrier, service, and Incoterms (INCO)
        * Contact and shipment details (e.g., name, address, city, postcode)
        * Regulatory information (EORI, IOSS)
        * Additional references and communication fields (e.g., ref1, email, phone)
    * The country is separately handled via the selected item in the country combo box (`cbCountry`).

* Prepare SQL Command
    * A string `query` is declared to call the stored procedure `[TCAR_Save]`, which is expected to handle the saving of all provided fields:

```sql
EXECUTE [TCAR_Save] @TCAR_Ref, @Carrier, @Service, ...
```

* Establish SQL Connection
    * A `SqlConnection` is opened using the predefined `connectionString`.
    * A `SqlCommand` is initialized with the query and connection.

* Bind Parameters
    * Each field from the form is bound to a corresponding SQL parameter using `cmd.Parameters.AddWithValue`. These parameters match the stored procedure’s expected inputs.

* Execute Query
    * Once all parameters are added, the command is executed with:
    * This runs the stored procedure without expecting a return result.

```cs
cmd.ExecuteNonQuery();
```

* Error Handling
    * If any exception occurs:
        * A custom error message is displayed to the user.
        * An error log entry is written using the `SessionMaintenance.LogBook` method.

```cs
 // Save Data ---------------------------------------------------------------------------------------------------
        private void SaveData()
        {
            string? carrier = cbCarrier.Text;
            string? service = cbService.Text;
            string? inco = txbInco.Text;
            string? eori = txbEORI.Text;
            string? ioss = txbIOSS.Text;
            string? name = txbName.Text;
            string? company = txbCompany.Text;
            string? postcode = txbPostcode.Text;
            string? city = txbCity.Text;
            string? addr1 = txbAddr1.Text;
            string? addr2 = txbAddr2.Text;
            string? county = txbCounty.Text;
            string? country = "";
            string? email = txbEmail.Text;
            string? phone = txbPhone.Text;
            string? ref1 = txbRef1.Text;
            string? ref2 = txbRef2.Text;

            if (cbCountry.SelectedItem != null)
            {
                country = cbCountry.SelectedItem.ToString();
            }

            string query = "EXECUTE [TCAR_Save]" +
                "@TCAR_Ref " +
                ",@Carrier " +
                ",@Service " +
                ",@Inco " +
                ",@Eori " +
                ",@IOSS " +
                ",@Name " +
                ",@Company " +
                ",@Postcode " +
                ",@City " +
                ",@Addr1 " +
                ",@Addr2 " +
                ",@County " +
                ",@Country " +
                ",@Email " +
                ",@Phone " +
                ",@Ref1 " +
                ",@Ref2 " +
                ",@User";

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
                        cmd.Parameters.AddWithValue("@Carrier", carrier);
                        cmd.Parameters.AddWithValue("@Service", service);
                        cmd.Parameters.AddWithValue("@Inco", inco);
                        cmd.Parameters.AddWithValue("@Eori", eori);
                        cmd.Parameters.AddWithValue("@IOSS", ioss);
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Company", company);
                        cmd.Parameters.AddWithValue("@Postcode", postcode);
                        cmd.Parameters.AddWithValue("@City", city);
                        cmd.Parameters.AddWithValue("@Addr1", addr1);
                        cmd.Parameters.AddWithValue("@Addr2", addr2);
                        cmd.Parameters.AddWithValue("@County", county);
                        cmd.Parameters.AddWithValue("@Country", country);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@Ref1", ref1);
                        cmd.Parameters.AddWithValue("@Ref2", ref2);
                        cmd.Parameters.AddWithValue("@User", userName);

                        cmd.ExecuteNonQuery();
                    }
                    conn.Close(); // Close SQL Connection
                }
            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured saving shipment headers \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[SaveData]", $"FAILED: ( {ex.Message} )");
                return;
            }
        }
```

---

## 2.3 - Methods - API

### 2.3.1 - GetCustomerUid

The `GetCustomerUid` method is responsible for retrieving the Customer UID for a shipment’s service level via an asynchronous API call. It constructs an XML request, sends it to an external service, and extracts the Customer UID from the response.

#### Function Signature:

* Access Modifier: `private` – Only accessible within the current class (`CarrierForm`).
* Return Type: `Task<string>` – An asynchronous task that returns a string (the Customer UID).

#### Parameters: 

* `boxRef`(`int`) – The box reference number used to generate the request.

#### Step-by-Step Breakdown:

* Create the XML Request
    * Calls `WcmsApi.GetXmlData` to gather data for the specified `boxRef` and the current `tcarRef`.
    * Passes that data to `CreateXmlString`, which wraps the data in the required XML structure.
    * Parameters like `domestic` and `ddp` determine the shipment context.

```cs
string callXml = WcmsApi.CreateXmlString(WcmsApi.GetXmlData(tcarRef, boxRef), null, domestic, ddp);
```

* Make Asynchronous API Call
    * Sends the generated XML via an async method `MakeServiceApiCall`.
    * Awaits the response string, which should be an XML containing shipping details.

```cs
string respXml = await WcmsApi.MakeServiceApiCall(callXml);
```

* Extract the Customer UID
    * Parses the returned XML to extract the `CustomerUID` using a helper method.

```cs
string ServiceCustomerUID = WcmsApi.ExtractCustomerUID(respXml);
```

* Log the Retrieved UID
    * Logs the result for traceability, indicating the operation was successful and which UID was retrieved.

```cs
SessionMaintenance.LogBook("", "[CarrierForm]", "[GetCustomerUid]", $"Customer UID Retrived: {ServiceCustomerUID}");
```

* Return the UID
    * Returns the extracted UID for further use (e.g., for printing labels).

```cs
return ServiceCustomerUID;
```

#### Summary
The `GetCustomerUid` method builds an XML request using current shipping data, makes an asynchronous API call to retrieve label info, and extracts the unique Customer UID from the response. This method is essential for interacting with external services (like WCMS) to support shipping workflows. Its async nature ensures the UI remains responsive during network communication.

 ```cs    
   // Get Customer UID -----------------------------------------------------------------------------------       
        private async Task<string> GetCustomerUid(int boxRef)
        {
            string callXml = WcmsApi.CreateXmlString(WcmsApi.GetXmlData(tcarRef, boxRef), null, domestic, ddp);

            // Make API Call to get label data
            string respXml = await WcmsApi.MakeServiceApiCall(callXml);

            string ServiceCustomerUID = WcmsApi.ExtractCustomerUID(respXml);

            SessionMaintenance.LogBook("", "[CarrierForm]", "[GetCustomerUid]", $"Customer UID Retrived: {ServiceCustomerUID}");

            return ServiceCustomerUID;
        }
```

---

### 2.3.2 - GetLabel
The `GetLabel` method is responsible for retrieving and printing a shipping label for a specified box. It interacts with the WCMS API to retrieve label data and tracking information, logs important events throughout the process, and handles errors gracefully.

#### Function Signature:

* Access Modifier: `private`

#### Parameters:

* `boxRef`(`int`) – The reference number of the box to label.
* `boxCount`(`int`) – The total number of boxes (used for user feedback).

#### Step-by-Step Breakdown:
* Set API Context
    * Assigns values to the `WcmsApi` static properties to identify the user and transaction for the API call.

```cs
WcmsApi.userName = userName;
WcmsApi.tcarRef = tcarRef;
```

* Log Start
    * Logs the beginning of the label process.

```cs
SessionMaintenance.LogBook("", "[CarrierForm]", "[GetLabel]", "Method Started");
```

* Validate TCAR Reference
    * Checks if an order is loaded. If not, displays an error and exits.

```cs
if (string.IsNullOrEmpty(tcarRef))
```

* Begin Try Block
    * Shows a loading cursor to indicate processing.

```cs
try
{
    Cursor = Cursors.WaitCursor;
```

* Create Request XML
    * Generates a transaction key.
    * Builds the full XML request for the label, including data for the specified box and customer UID.

```cs
string tkey = CreateTKey();
string callXml = WcmsApi.CreateXmlString(WcmsApi.GetXmlData(tcarRef, boxRef), await GetCustomerUid(boxRef), domestic, ddp);
```

* Log XML
    * Logs the request XML for traceability and support.

```cs
WcmsApi.InsertXml(callXml, tkey, 1);
```

* Make API Call
    * Sends the request to the carrier API and awaits the response.

```cs
string respXml = await WcmsApi.MakeCarrierApiCall(callXml, tkey);
```

* Handle Empty Response
    * If the API returns nothing, logs and displays an error.

```cs
if (string.IsNullOrEmpty(respXml))
```

* Extract and Log Tracking Number
    * Attempts to retrieve the tracking number from the API response.
    * Logs an error if it's missing.

```cs
string trackingNumber = WcmsApi.ExtractTrackingNumber(respXml);
```

* Extract Labels
    * Parses the XML to obtain a list of PDF file paths or base64-encoded labels.
    * Displays an error and exits if no labels are found.

```cs
List<string> labelFiles = WcmsApi.ExtractLabelData(respXml);
```

* Print Labels
    * Loops through each label and sends it to the printer.

```cs
foreach (var labelFile in labelFiles)
{
    WcmsApi.PrintPdf(labelFile);
}
```

* Finalize Box Record
    * Marks the box as completed in the system, associating it with its tracking number.

```cs
CompleteTCAR(trackingNumber, boxRef);
```

* Exception Handling
    * Displays an error message and logs it if any exceptions are thrown during the process.

```cs
catch (Exception ex)
```

* Always Reset Cursor
    * Ensures the cursor is reset to normal whether the process succeeds or fails.

```cs
finally
{
    Cursor = Cursors.Default;
}
```

* Show Completion Message
    * Provides user feedback to indicate which label was printed.

```cs
CustomMessageBox messageBoxfin = new CustomMessageBox();
messageBoxfin.ShowInfo($"Label Printed: {boxRef} of {boxCount}");
```

#### Summary
The `GetLabel` method orchestrates the full label printing flow:

* Validates input
* Builds and sends an XML request
* Parses the API response
* Prints the resulting labels
* Logs all steps
    
Handles user feedback and errors throughout
It’s a critical part of the shipping workflow and ensures that every box is properly labeled and tracked.

```cs
  // Get Label Process ------------------------------------------------------------------------------------------------------------------       
        private async Task GetLabel(int boxRef, int boxCount)
        {

            WcmsApi.userName = userName;
            WcmsApi.tcarRef = tcarRef;

            SessionMaintenance.LogBook("", "[CarrierForm]", "[GetLabel]", "Method Started");

            if (string.IsNullOrEmpty(tcarRef))
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("No Order data loaded");
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                string tkey = CreateTKey();
                string callXml = WcmsApi.CreateXmlString(WcmsApi.GetXmlData(tcarRef, boxRef), await GetCustomerUid(boxRef), domestic, ddp);

                // Insert Call XML for reference (logging)
                WcmsApi.InsertXml(callXml, tkey, 1);

                // Make API Call to get label data
                string respXml = await WcmsApi.MakeCarrierApiCall(callXml, tkey);

                if (string.IsNullOrEmpty(respXml))
                {
                    SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetLabel]", "Received empty response XML.");
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowError("Failed to retrieve label data. Empty response from API.");
                    return;
                }

                // Extract tracking number and label files
                string trackingNumber = WcmsApi.ExtractTrackingNumber(respXml);
                List<string> labelFiles = WcmsApi.ExtractLabelData(respXml);

                // Log details
                if (string.IsNullOrEmpty(trackingNumber))
                {
                    SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetLabel]", "Failed to extract tracking number.");
                }
                else
                {
                    SessionMaintenance.LogBook("", "[CarrierForm]", "[GetLabel]", $"Tracking Number: {trackingNumber}, {boxRef}");
                }

                if (labelFiles == null || labelFiles.Count == 0)
                {
                    SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetLabel]", "Failed to extract any labels.");
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowError("No label data found in API response.");
                    return;
                }

                // Print all labels
                foreach (var labelFile in labelFiles)
                {
                    WcmsApi.PrintPdf(labelFile);
                }

                CompleteTCAR(trackingNumber, boxRef);

            }
            catch (Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occurred during label processing: {ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetLabel]", $"Exception: {ex.Message}");
                return;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            SessionMaintenance.LogBook("", "[CarrierForm]", "[GetLabel]", "Method Finished");

            CustomMessageBox messageBoxfin = new CustomMessageBox();
            messageBoxfin.ShowInfo($"Label Printed: {boxRef} of {boxCount}");

        }
```

---

### 2.3.3 - GetAccessToken

The `GetAccessToken` method is responsible for securely retrieving API credentials from the database and then requesting an access token from an external API using those credentials. This token is used for authenticating future API requests.

#### Function Signature:

* Access Modifier: `private`

#### Parameters:

* `accCode`(`string`) – The account code used to retrieve API credentials from the database.

#### Step-by-Step Breakdown:

* Define Query and Variables
    * SQL query retrieves the API username and password for the given account.
    * `tkey` is generated to log or associate this transaction uniquely.

```cs
string query = "SELECT TOP 1 RTRIM([API_UserName]) AS API_UserName, RTRIM([API_Password]) AS API_Password FROM TCAR_Acct WHERE Acc_Code = @Acc_Code";
string apiUsername = string.Empty;
string apiPassword = string.Empty;
string tkey = CreateTKey();
```

* Run SQL Query
    * Opens a connection to the database asynchronously using the configured connection string.
    * Add the account code as a parameter to prevent SQL injection using: `cmd.Parameters.AddWithValue("@Acc_Code", accCode);`.
    * Reads the first result (if any) and extracts the username and password into local variables.

```cs
if (await reader.ReadAsync().ConfigureAwait(false))
```

* Validate Retrieved Credentials
    * Ensures that the credentials are not empty; if they are, an exception is thrown.

```cs
if (string.IsNullOrEmpty(apiUsername) || string.IsNullOrEmpty(apiPassword))
{
    throw new Exception("API credentials not found for the given account code.");
}
```

* Generate XML and Call API
    * Constructs an XML request using the credentials.
    * Sends it to the API to get an access token asynchronously.

```cs
string callXml = WcmsApi.GetTknXml(apiUsername, apiPassword);
string respXml = await WcmsApi.MakeTKNApiCall(callXml).ConfigureAwait(false);
```

* Process API Response
    * Validates that the API returned a non-empty response using `if (!string.IsNullOrEmpty(respXml))`
    * Extracts and stores the access token for immediate use.
    * Extracts and logs the refresh token (presumably for later token renewal).
    * The transaction key (`tkey`) is used when saving this to ensure traceability.

```cs
WcmsApi.accessToken = WcmsApi.ExtractAccessToken(respXml);
string refreshTkn = WcmsApi.ExtractRefreshToken(respXml);
WcmsApi.InsertAccessTkn(tkey, accCode, refreshTkn);
```

* Exception Handling
    * Catches any errors that occur during:
        * Database access
        * XML generation
        * API interaction
        * Response parsing
        * Displays the error to the user and logs it in the system.

```cs
messageBox.ShowError($"An error occurred while getting the Access Token: \n{ex.Message}");
SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetAccessToken]", $"FAILED: {ex.Message}");
```

#### Summary

The `GetAccessToken` method handles:
* Secure retrieval of API credentials from the database
* Construction and submission of an access token request
* Extraction and storage of the access and refresh tokens
* Full logging and error handling throughout the process
It ensures that authentication for external API requests is handled dynamically and securely, enabling seamless integration with external systems.

```cs    
   // Get Access Token (Async) ---------------------------------------------------------------------------------------------------
        private async Task GetAccessToken(string accCode)
        {
            string query = "SELECT TOP 1 RTRIM([API_UserName]) AS API_UserName, RTRIM([API_Password]) AS API_Password FROM TCAR_Acct WHERE Acc_Code = @Acc_Code";
            string apiUsername = string.Empty;
            string apiPassword = string.Empty;
            string tkey = CreateTKey();

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
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[GetAccessToken]", $"FAILED: {ex.Message}");
                return;
            }
        }
```

---

## 2.4 - Methods - Environment Events

### 2.4.1 - Exit Button

The `btnExit_MouseEnter` and `btnExit_MouseLeave` methods handle user interactions when the mouse pointer enters or leaves the Exit button area on the form, enhancing user experience through visual feedback.

#### Signature

```cs
private void btnExit_MouseEnter(object sender, EventArgs e)
private void btnExit_MouseLeave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `btnExit_MouseEnter`:
	* Trigger: Runs when the mouse pointer enters the `btnExit` button area.
	* Action: Calls `SessionMaintenance.ButtonEnter(btnExit)`:
		* Sets the button’s `BackColor` to `backColor`.
		* Sets the `ForeColor` (text color) to `accentColor` for contrast.

* `btnExit_MouseLeave`:
	* Trigger: Runs when the mouse pointer leaves the `btnExit` button.
	* Action: Calls `SessionMaintenance.ButtonLeave(btnExit)`:
		* Reverts the `BackColor` to `accentColor`.
		* Reverts the `ForeColor` to `backColor`, restoring its default visual style.

#### Summary

These methods ensure that when users hover over or leave the Exit button, the button provides consistent visual cues using centralized styling logic in `SessionMaintenance`. This improves user experience by making the app feel more responsive and cohesive.

```cs
// Exit Button ---------------------------------------------------------
        private void btnExit_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnExit);
        }

        private void btnExit_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnExit);
        }
```

---

### 2.4.2 - Search Button

The event handler methods `btnSearch_MouseEnter` and `btnSearch_MouseLeave` change the appearance of the Search button when the user hovers over or moves away from the button, using standardized visual behavior to improve UI feedback and consistency.

#### Signature

```cs
private void btnSearch_MouseEnter(object sender, EventArgs e)
private void btnSearch_MouseLeave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `btnSearch_MouseEnter`:
	* Trigger: Called when the mouse pointer enters the `btnSearch` button area.
	* Action:
		* Invokes `SessionMaintenance.ButtonEnter(btnSearch)`.
		* This sets:
			* `BackColor` to `backColor` → `Color.Black`
			* `ForeColor` to `accentColor` → `Color.FromArgb(11, 159, 161)` (a teal-like color)

* `btnSearch_MouseLeave`:
	* Trigger: Called when the mouse pointer leaves the `btnSearch` button area.
	* Action:
		* Invokes `SessionMaintenance.ButtonLeave(btnSearch)`.
		* This reverts the colors:
			* `BackColor` to `accentColor` → teal
			* `ForeColor` to `backColor` → black

#### Summary

These handlers use the `SessionMaintenance` helper methods to apply consistent hover and leave styling to the Search button. When hovered, it inverts the color scheme to highlight interactivity; when the mouse leaves, it returns to its base visual state. This promotes a responsive and clean UI aligned with your app’s theme.

```cs
 // Search Button ------------------------------------------------------------
        private void btnSearch_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnSearch);
        }

        private void btnSearch_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnSearch);
        }
```

---

### 2.4.3 - Print Button

The event handler methods `btnPrint_MouseEnter` and `btnPrint_MouseLeave` manage the visual styling of the Print button when a user hovers over or moves the mouse away, ensuring consistent user experience across the form’s interactive elements.

#### Signature

```cs
private void btnPrint_MouseEnter(object sender, EventArgs e)
private void btnPrint_MouseLeave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `btnPrint_MouseEnter`:
	* Trigger: When the mouse cursor enters the `btnPrint` button area.
	* Action:
		* Calls `SessionMaintenance.ButtonEnter(btnPrint)`
		* Sets:
			* `BackColor` to `backColor` → `Color.Black`
			* `ForeColor` to `accentColor` → `Color.FromArgb(11, 159, 161)`

* `btnPrint_MouseLeave`:
	* Trigger: When the mouse cursor leaves the `btnPrint` button area.
	* Action:
		* Calls `SessionMaintenance.ButtonLeave(btnPrint)`
		* Sets:
			* `BackColor` to `accentColor` → teal
			* `ForeColor` to `backColor` → black

#### Summary

These methods give the Print button a consistent hover effect by flipping foreground and background colors when hovered. It aligns with the styling used for other buttons like Search and Exit, promoting a unified look and feel across the interface.

```cs    
    // Print Button -------------------------------------------------------
        private void btnPrint_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnPrint);
        }

        private void btnPrint_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnPrint);
        }
```

---

### 2.4.4 - Load Note Field

The `txbLoadNote_Enter` and `txbLoadNote_Leave` methods handle the focus styling and activity logging for the Load Note textbox when the user enters or leaves the field. They improve the user experience through visual cues and maintain a usage audit trail.

#### Signatures

```cs
private void txbLoadNote_Enter(object sender, EventArgs e)
private void txbLoadNote_Leave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `txbLoadNote_Enter`
	* Trigger: User focuses on (clicks into or tabs to) the `txbLoadNote` textbox.
	* Actions:
		* `SessionMaintenance.ControlEnter(txbLoadNote)`:
		* Sets `BackColor` to `accentColor` → `Color.FromArgb(11, 159, 161)`
		* Sets `ForeColor` to `backColor` → `Color.Black`
		* Logs the event with `SessionMaintenance.LogBook("", "[CarrierForm]", "[txbLoadNote_Enter]", "Field Entered")`

* `txbLoadNote_Leave`
	* Trigger: User leaves (clicks or tabs away from) the `txbLoadNote` textbox.
	* Actions:
		* `SessionMaintenance.ControlLeave(txbLoadNote)`:
		* Resets `BackColor` to `white`
		* Sets `ForeColor` to `backColor` → `Color.Black`
		* Logs the event using `SessionMaintenance.LogBook("", "[CarrierForm]", "[txbLoadNote_Leave]", "Field Left")`

#### Summary

These event handlers enhance the UI by visually indicating when the Load Note field is active, and log the interaction to aid in support or diagnostics. They follow the same styling approach used across the form for consistency.

```cs
// Load Note Field --------------------------------------------------------------------------------
        private void txbLoadNote_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbLoadNote);
            SessionMaintenance.LogBook("", "[CarrierForm]", "[txbLoadNote_Enter]", $"Field Entered");
        }

        private void txbLoadNote_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbLoadNote);
            SessionMaintenance.LogBook("", "[CarrierForm]", "[txbLoadNote_Enter]", $"Field Left");
        }
```

---

### 2.4.5 - Client Field

The `cbClient_Enter` and `cbClient_Leave` methods handle the visual feedback for when the user interacts with the Client combo box (`cbClient`). They improve usability by providing consistent focus styling.

#### Signatures

```cs
private void cbClient_Enter(object sender, EventArgs e)
private void cbClient_Leave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `cbClient_Enter`
	* Trigger: User focuses on the `cbClient` combo box (via click or keyboard navigation).
	* Action:
		* Calls `SessionMaintenance.ControlEnter(cbClient)`:
		* Changes background color to `accentColor` → `Color.FromArgb(11, 159, 161)`
		* Changes foreground color to `backColor` → `Color.Black`

* `cbClient_Leave`
	* Trigger: User leaves the `cbClient` field.
	* Action:
		* Calls `SessionMaintenance.ControlLeave(cbClient)`:
		* Resets background color to white
		* Sets foreground color to `backColor` → `Color.Black`

#### Summary

These focus event handlers enhance user interaction by providing visual cues when the Client combo box is active or inactive. Unlike `txbLoadNote`, these methods don't log entry/exit to the field because logging isn't needed here.

 ```cs
       // Client Field --------------------------------------------------------------------------------
        private void cbClient_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(cbClient);
        }

        private void cbClient_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(cbClient);
        }
```

---

### 2.4.6 - Carrier Field

These methods handle visual feedback and dynamic data population for the `cbCarrier` combo box. They help users understand when the control is active and ensure that related service options are updated accordingly when the selected carrier changes.

#### Signatures

```cs
private void cbCarrier_Enter(object sender, EventArgs e)
private void cbCarrier_Leave(object sender, EventArgs e)
private void cbCarrier_TextChanged(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `cbCarrier_Enter`
	* Trigger: When the `cbCarrier` combo box gains focus.
	* Action:
		* Applies focus styling using `SessionMaintenance.ControlEnter`.

* `cbCarrier_Leave`
	* Trigger: When the `cbCarrier` combo box loses focus.
	* Action:
		* Reverts the control's visual styling to its default using `ControlLeave`.

* `cbCarrier_TextChanged`
	* Trigger: When the text in the `cbCarrier` combo box changes (either user-typed or selected from the list).
	* Actions:
		* Clears the service description label to reset any previously shown data using `lblServiceDescr.Text = "";`
		* Populates the `cbService` combo box with carrier-specific options based on the current client using `PopulateCarrierComboBoxes(cbService, * cbClient.Text ?? "");`
		* Logs the change in carrier for tracking/debugging purposes with `SessionMaintenance.LogBook("", "[CarrierForm]", "[cbService_TextChanged]", $"Carrier Changed {cbCarrier.Text ?? ""}");`

#### Summary

These three handlers manage both UI responsiveness and data consistency for the carrier selection process:
* The `Enter` and `Leave` methods enhance user experience with visual feedback.
* The `TextChanged` method ensures the corresponding service options are relevant to the selected carrier and logs the interaction for audit or debugging.

```cs
// Carrier Field ---------------------------------------------------------------------------------------------
        private void cbCarrier_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(cbCarrier);
        }

        private void cbCarrier_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(cbCarrier);
        }
        private void cbCarrier_TextChanged(object sender, EventArgs e)
        {
            lblServiceDescr.Text = "";
            PopulateCarrierComboBoxes(cbService, cbClient.Text ?? "");
            SessionMaintenance.LogBook("", "[CarrierForm]", "[cbService_TextChanged]", $"Carrier Changed {cbCarrier.Text ?? ""}");
        }
```

---

### 2.4.7 - Service Field

These handlers manage the UI behavior and service description updates when interacting with the `cbService` combo box. They help provide real-time feedback and logging as users select or modify a carrier service.

#### Signatures

```cs
private void cbService_Enter(object sender, EventArgs e)
private void cbService_Leave(object sender, EventArgs e)
private void cbService_TextChanged(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `cbService_Enter`
	* Trigger: When the `cbService` combo box receives focus.
	* Action:
		* Highlights the control by changing its colors using `SessionMaintenance.ControlEnter`.

* `cbService_Leave`
	* Trigger: When the control loses focus.
	* Action:
		* Reverts the color styling back to default using `ControlLeave`.

* `cbService_TextChanged`
	* Trigger: When the text in the service field changes.
	* Actions:
		* Retrieves the currently selected or entered service using `string Service = cbService.Text ?? "";`
		* Logs the service change for auditing or diagnostics using `SessionMaintenance.LogBook("", "[CarrierForm]", "[cbService_TextChanged]", $"Service Changed {Service}");`
		* Updates the service description label (`lblServiceDescr`) to reflect the selected service with `UpdateCarrDescr(Service);`

#### Summary

These handlers ensure a smooth and visually clear experience when users interact with the `cbService` combo box, while also dynamically updating relevant UI and maintaining a log of user interactions:
* `Enter` / `Leave`: provide visual cues.
* `TextChanged`: logs user choice and updates associated descriptive info.

```cs
// Service Field ------------------------------------------------------------------------------------------
        private void cbService_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(cbService);
        }

        private void cbService_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(cbService);
        }

        private void cbService_TextChanged(object sender, EventArgs e)
        {
            string Service = cbService.Text ?? "";
            SessionMaintenance.LogBook("", "[CarrierForm]", "[cbService_TextChanged]", $"Service Changed {Service}");
            UpdateCarrDescr(Service);
        }
```

---

### 2.4.8 - INCO Field

These event handlers control visual feedback and validation checks when the user interacts with the `txbInco` text box, which stores INCO terms used for shipments.

#### Signatures

```cs
private void txbInco_Enter(object sender, EventArgs e)
private void txbInco_Leave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `txbInco_Enter`
	* Trigger: When the user focuses on the INCO field.
	* Action:
		* Calls `SessionMaintenance.ControlEnter` to apply the active input styling (usually a color change) to indicate that the field is in focus.

* `txbInco_Leave`
	* Trigger: When the user leaves the INCO field.
	* Actions:
		* Reverts the field's visual state back to normal
		* Calls `DeltaCheck()` to assess if any changes have been made to the form that may require tracking or flagging.
		* Runs a validation or state-checking method using `CheckField(2, txbInco);` This checks whether the value entered in the INCO field is valid or complete, based on a rule set associated with field index `2`.

#### Summary

These handlers make sure the `txbInco` field is both user-friendly and data-reliable:
* Enter: highlights the field to show it is active.
* Leave: resets styling, performs a change check, and validates or flags the field content.

```cs
// INCO Field ----------------------------------------------------------------
        private void txbInco_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbInco);
        }

        private void txbInco_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbInco);
            DeltaCheck();
            CheckField(2, txbInco);
        }
```

---

### 2.4.9 - EORI Field

These handlers manage visual state and validation checks when the user interacts with the `txbEORI` input field, which holds the Economic Operators Registration and Identification (EORI) number used for customs processing.

#### Signatures

```cs
private void txbEORI_Enter(object sender, EventArgs e)
private void txbEORI_Leave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `txbEORI_Enter`
	* Trigger: When the user enters the `txbEORI` field.
	* Action:
		* Applies focus styling using `SessionMaintenance.ControlEnter(txbEORI);`
		* This visually indicates the field is active (likely by changing the background and text colors).

* `txbEORI_Leave`
	* Trigger: When the user leaves the field.
	* Actions:
		* Reverts visual styling
		* Checks for changes in form data using `DeltaCheck();`
		* Validates the EORI field using `CheckField(3, txbEORI);`

#### Summary

These handlers provide a clean, responsive user experience with `txbEORI`:
* Enter: Highlights the field to show it's focused.
* Leave: Resets visuals, checks for changes, and validates the input.

```cs
        // EORI Field ------------------------------------------------------
        private void txbEORI_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbEORI);
        }

        private void txbEORI_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbEORI);
            DeltaCheck();
            CheckField(3, txbEORI);
        }
```

---

### 2.4.10 - IOSS Field

These handlers manage user interaction, visual styling, and validation for the Import One-Stop Shop (IOSS) field, commonly used for VAT processing in cross-border trade.

#### Signatures

```cs
private void txbIOSS_Enter(object sender, EventArgs e)
private void txbIOSS_Leave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `txbIOSS_Enter`
	* Trigger: User focuses or clicks into the `txbIOSS` textbox.
	* Action:
		* Applies focused styling using `SessionMaintenance.ControlEnter(txbIOSS);`
		* Highlights the field visually with a background color to indicate it's active.

* txbIOSS_Leave
	* Trigger: User exits the `txbIOSS` textbox.
	* Actions:
		* Reverts styling using `SessionMaintenance.ControlLeave(txbIOSS);`
		* Restores the default visual appearance (e.g. white background).
		* Tracks changes to form state using `DeltaCheck();`
		* Detects whether the value in the field has changed since entry.
		* Validates the IOSS field using `CheckField(4, txbIOSS);`

#### Summary

These event handlers ensure the `txbIOSS` field:
* Responds visually when active.
* Tracks user changes on exit.
* Validates the field to maintain correct data entry for the IOSS value.

```cs  
   // IOSS Field ----------------------------------------------------------
        private void txbIOSS_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbIOSS);
        }

        private void txbIOSS_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbIOSS);
            DeltaCheck();
            CheckField(4, txbIOSS);
        } 
```

---

### 2.4.11 - Name Field

These methods manage UI highlighting, change tracking, and validation for the `txbName` field, which holds the recipient name on a carrier form.

#### Signatures

```cs
private void txbName_Enter(object sender, EventArgs e)
private void txbName_Leave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `txbName_Enter`
	* Triggered when the user focuses into the `txbName `textbox.
	* Action:
		* Highlights the field visually (typically with a contrasting background and text color).
		* Indicates that the field is currently active.

* `txbName_Leave`
	* Triggered when the user leaves the textbox (e.g. tabs out or clicks elsewhere).
	* Actions:
		* Reverts visual styling using `SessionMaintenance.ControlLeave(txbName);`
		* Checks if data has changed using `DeltaCheck();`
		* Validates the field content using` CheckField(5, txbName);`

#### Summary

These handlers ensure the `txbName` field:
* Responds visually to focus.
* Flags any edits through change tracking.
* Runs validation checks when the field is exited.

```cs
// Name Field ---------------------------------------------------------------
private void txbName_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbName);
        }

        private void txbName_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbName);
            DeltaCheck();
            CheckField(5, txbName);
        }
```

---

### 2.4.12 - Company Field

These handlers manage UI feedback, change tracking, and field validation for the `txbCompany` textbox, which stores the recipient’s company name on the carrier form.

#### Signatures

```cs
private void txbCompany_Enter(object sender, EventArgs e)
private void txbCompany_Leave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `txbCompany_Enter`
	* Triggered when the user enters the `txbCompany` field.
	* Action:
		* Visually highlights the field (using accent and background colors).
		* Helps the user identify which field is currently active.

* `txbCompany_Leave`
	* Triggered when the user exits the field.
	* Actions:
		* Restore field styling using `SessionMaintenance.ControlLeave(txbCompany);`
		* Check for changes using `DeltaCheck();`
		* Run validation logic `CheckField(6, txbCompany);`

#### Summary

These methods enhance UX by:
* Giving visual cues for field focus.
* Detecting and tracking any user edits.
* Running field-specific validation once the field is left.

```cs
// Company Field ------------------------------------------------------------
        private void txbCompany_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbCompany);
        }

        private void txbCompany_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbCompany);
            DeltaCheck();
            CheckField(6, txbCompany);
        }
```

---

### 2.4.13 - Postcode Field

These methods provide visual feedback, change tracking, and validation for the postcode field, which is a required field for delivery/shipping logic.

#### Signatures

```cs
private void txbPostcode_Enter(object sender, EventArgs e)
private void txbPostcode_Leave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `txbPostcode_Enter`
	* Triggered when the `txbPostcode` field receives focus.
	* Action:
		* Applies visual styling to indicate the field is active (e.g. highlight background/foreground colors).
		* Enhances usability by showing clearly where the user is typing.

* `txbPostcode_Leave`
	* Triggered when the user navigates away from the field.
	* Actions:
		* Restore default styling using `SessionMaintenance.ControlLeave(txbPostcode);`
		* Check for data changes using `DeltaCheck();`
		* Validate the field using `CheckField(7, txbPostcode);`

#### Summary

These methods:
* Improve user experience with focus indicators.
* Track modifications for possible form submission or audit logging.
* Run postcode-specific validation when the field is exited.

```cs
// Postcode Field ----------------------------------------------------------
        private void txbPostcode_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbPostcode);
        }

        private void txbPostcode_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbPostcode);
            DeltaCheck();
            CheckField(7, txbPostcode);
        }
```

---

### 2.4.14 - City Field

These methods manage UI focus styling, change detection, and validation for the City input field—essential for ensuring location data is accurately and clearly entered.

#### Signatures

```cs
private void txbCity_Enter(object sender, EventArgs e)
private void txbCity_Leave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `txbCity_Enter`
	* Triggered when the user focuses on the `txbCity` field.
	* Action:
		* Visually highlights the field using the app’s accent colors.
		* Provides a clear indication that the field is active.

* `txbCity_Leave`
	* Triggered when the user leaves the field.
	* Actions:
		* Reset the visual style using `SessionMaintenance.ControlLeave(txbCity);`
		* Detect any changes using `DeltaCheck();`
		* Perform validation using `CheckField(8, txbCity);`

#### Summary

These handlers ensure that:
* Users receive visual cues when interacting with the City field.
* Changes are monitored for save or discard functionality.
* Validation is applied to maintain data integrity for the City input.

```cs
 // City Field ----------------------------------------------------------
        private void txbCity_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbCity);
        }

        private void txbCity_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbCity);
            DeltaCheck();
            CheckField(8, txbCity);
        }
```

---

### 2.4.15 - Addr1 Field

These methods control user interface highlighting, change detection, and field validation for the first address line input field (`Addr1`). This ensures a smoother user experience and helps validate key location data.

#### Signatures

```cs
private void txbAddr1_Enter(object sender, EventArgs e)
private void txbAddr1_Leave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `txbAddr1_Enter`
	* Triggered when the user enters the `txbAddr1` field.
	* Action:
		* Visually highlights the input field using the session-defined accent styling, giving feedback that the field is active.

* `txbAddr1_Leave`
	* Triggered when the user exits the field.
	* Actions:
		* Reset styling using `SessionMaintenance.ControlLeave(txbAddr1);`
		* Detect field changes using `DeltaCheck();`
		* Validate the address line using `CheckField(9, txbAddr1);`

#### Summary

These event handlers help users clearly see when `Addr1` is selected, track whether the value has been changed, and validate it for correctness when the user leaves the field—helping maintain accuracy in address data entry.

```cs
 // Addr1 Field ----------------------------------------------------------------
        private void txbAddr1_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbAddr1);
        }

        private void txbAddr1_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbAddr1);
            DeltaCheck();
            CheckField(9, txbAddr1);
        }
```

---

### 2.4.16 - Addr2 Field

These methods provide visual feedback, change detection, and validation for the second address line (`Addr2`). While often optional, it's treated with the same care as required fields to ensure consistent user input tracking.

#### Signatures

```cs
private void txbAddr2_Enter(object sender, EventArgs e)
private void txbAddr2_Leave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `txbAddr2_Enter`
	* When triggered: User clicks into or tabs into the `txbAddr2` input field.
	* Action:
		* Applies visual styling (accent background, swapped text color) to indicate focus.

* `txbAddr2_Leave`
	* When triggered: User exits the field.
	* Actions:
		* Reset field visuals using `SessionMaintenance.ControlLeave(txbAddr2);`
		* Track changes using `DeltaCheck();`
		* Validate input using `CheckField(10, txbAddr2);`

#### Summary

Even though `Addr2` may be optional, these handlers ensure it gets the same UX attention as other fields. The combination of visual cues, change detection, and validation makes the data entry process more intuitive and reliable.

```cs
// Addr2 Field --------------------------------------------------------------
        private void txbAddr2_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbAddr2);
        }

        private void txbAddr2_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbAddr2);
            DeltaCheck();
            CheckField(10, txbAddr2);
        }
```

---

### 2.4.17 - County Field

These methods handle the user interaction, visual feedback, and change tracking for the County input field. While counties may not always be required, the logic ensures consistent UI behavior and data validation.

#### Signatures

```cs
private void txbCounty_Enter(object sender, EventArgs e)
private void txbCounty_Leave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown
* `txbCounty_Enter`
	* Triggered when: The user focuses on the `txbCounty` field.
	* Action:
		* Applies accent background and foreground styling to indicate focus.

* `txbCounty_Leave`
	* Triggered when: The user moves focus away from the field.
	* Actions:
		* Reset visuals using `SessionMaintenance.ControlLeave(txbCounty);`
		* Detect value change using `DeltaCheck();`
		* Validate input using `CheckField(11, txbCounty);`

#### Summary

This control is part of the consistent user experience design across the form. Visual cues help guide the user, while change tracking and validation enforce data integrity—even for less critical fields like county.

```cs
// County Field --------------------------------------------------------
        private void txbCounty_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbCounty);
        }

        private void txbCounty_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbCounty);
            DeltaCheck();
            CheckField(11, txbCounty);
        }
```

---

### 2.4.18 - Country Field

These methods manage the focus behavior, visual cues, change tracking, and domestic/international logic when the user interacts with the country dropdown (`cbCountry`).

#### Signatures

```cs
private void cbCountry_Enter(object sender, EventArgs e)
private void cbCountry_Leave(object sender, EventArgs e)
private void cbCountry_TextChanged(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `cbCountry_Enter`
	* Triggered when: User focuses on the `cbCountry` `ComboBox`.
	* Action:
		* Applies active color scheme to indicate focus.

* `cbCountry_Leave`
	* Triggered when: User leaves the field.
	* Actions:
		* Resets visual state using `SessionMaintenance.ControlLeave(cbCountry);`
		* Checks whether the field value has changed compared to a previously saved state using `DeltaCheck();`

* `cbCountry_TextChanged`
	* Triggered when: The selected country text changes.
	* Logic Dynamically adjusts the `domestic` flag and the label based on the country selection.
	* If `"Great Britain"` is selected:
		* Sets `domestic` = `1`
		* Updates label to Domestic
	* Otherwise:
		* Sets domestic = `0`
		* Updates label to International

#### Summary

This field goes beyond styling and validation—it influences how the rest of the form may behave depending on whether a shipment is Domestic or International. It also maintains consistency in UI feedback and tracks user changes for further logic down the line.

```cs
 // Country Field -----------------------------------------------------------
        private void cbCountry_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(cbCountry);
        }

        private void cbCountry_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(cbCountry);
            DeltaCheck();
        }

        private void cbCountry_TextChanged(object sender, EventArgs e)
        {
            string country = cbCountry.Text ?? "";
            if (country == "Great Britain")
            {
                domestic = 1;
                lblDomestic.Text = "Domestic";
            }
            else
            {
                domestic = 0;
                lblDomestic.Text = "International";
            }
        }
```

---

### 2.4.19 - Email Field

These methods handle the focus behavior and validation logic for the email field (`txbEmail`), providing feedback and ensuring changes are tracked.

#### Signatures

```cs
private void txbEmail_Enter(object sender, EventArgs e)
private void txbEmail_Leave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* txbEmail_Enter
	* Triggered when: User focuses on the `txbEmail` text box.
	* Action:
		* Applies active color scheme (sets the background and foreground colors) to indicate focus on the email field.

* txbEmail_Leave
	* Triggered when: User leaves the `txbEmail` text box.
	* Actions:
		* Resets the visual state of the email field (background/foreground color) using `SessionMaintenance.ControlLeave(txbEmail);`
		* Checks if the value in the email field has changed from a previous state using `DeltaCheck();`
		* Performs additional validation or processing using `CheckField(12, txbEmail);`

#### Summary

This field ensures the user’s interaction with the email input is clearly reflected in the UI through color changes, and it performs necessary validation once the user leaves the field. The `DeltaCheck` and `CheckField` methods ensure any changes are tracked and validated.

```cs
// Email Field -----------------------------------------------------------
        private void txbEmail_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbEmail);
        }

        private void txbEmail_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbEmail);
            DeltaCheck();
            CheckField(12, txbEmail);
        }
```

---

### 2.4.20 - Phone Field

These methods handle the focus behavior and validation logic for the phone field (`txbPhone`), ensuring a consistent UI experience and checking for any changes or validation after the user leaves the field.

#### Signatures

```cs
private void txbPhone_Enter(object sender, EventArgs e)
private void txbPhone_Leave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `txbPhone_Enter`
	* Triggered when: User focuses on the `txbPhone` text box.
	* Action:
		* Applies the active focus color scheme (changes the background and foreground color) to visually highlight the phone field.

* `txbPhone_Leave`
	* Triggered when: User leaves the `txbPhone` text box.
	* * Actions:
		* Resets the visual styling of the phone field using `SessionMaintenance.ControlLeave(txbPhone);`
		* checks if the value in the phone field has been modified using `DeltaCheck();`
		* Performs validation or further processing on the phone field using `CheckField(13, txbPhone);`

#### Summary

This field ensures a consistent UI experience with color changes for focus and performs validation or processing upon leaving the field. The DeltaCheck method tracks changes, and `CheckField` validates or processes the phone input.

```cs
// Phone Field --------------------------------------------------------------
        private void txbPhone_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbPhone);
        }

        private void txbPhone_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbPhone);
            DeltaCheck();
            CheckField(13, txbPhone);
        }
```

---

### 2.4.21 - Ref 1 Field

These methods manage the focus behavior and validation logic for the "Ref 1" field (`txbRef1`), ensuring that the field is visually highlighted when focused and validated after losing focus.

#### Signatures

```cs
private void txbRef1_Enter(object sender, EventArgs e)
private void txbRef1_Leave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `txbRef1_Enter`
	* Triggered when: User focuses on the `txbRef1` text box.
	* Action:
		* Changes the background and foreground color of the `txbRef1` field to indicate focus, making it visually prominent.

* `txbRef1_Leave`
	* Triggered when: User leaves the `txbRef1` text box.
	* Actions:
		* Resets the visual styling of the `txbRef1` field back to its default state when it loses focus using `SessionMaintenance.ControlLeave(txbRef1);`
		* Checks if the value in the `txbRef1` field has been modified or needs to be validated using `DeltaCheck();`

#### Summary

These handlers ensure the UI is updated when the user interacts with the "Ref 1" field by highlighting it during focus and checking for changes or required validation when the field is left. The `DeltaCheck` method checks for any modifications after leaving the field.

```cs
// Ref 1 Field ----------------------------------------------------------
        private void txbRef1_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbRef1);
        }

        private void txbRef1_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbRef1);
            DeltaCheck();
        }
```

---

### 2.4.22 - Ref 2 Field

These methods handle the focus behavior and validation logic for the "Ref 2" field (`txbRef2`), ensuring the field is visually highlighted when focused and validated after losing focus.

#### Signatures

```cs
private void txbRef2_Enter(object sender, EventArgs e)
private void txbRef2_Leave(object sender, EventArgs e)
```

#### Step-by-Step Breakdown

* `txbRef2_Enter`
	* Triggered when: User focuses on the `txbRef2` text box.
	* Action:
		* Changes the background and foreground color of the txbRef2 field to indicate focus, making it visually prominent.

* `txbRef2_Leave`
	* Triggered when: User leaves the `txbRef2` text box.
	* Actions:
		* Resets the visual styling of the `txbRef2` field back to its default state when it loses focus using `SessionMaintenance.ControlLeave(txbRef2);`
		* Checks if the value in the `txbRef2` field has been modified or needs to be validated using `DeltaCheck();`

#### Summary

These handlers ensure the UI is updated when the user interacts with the "Ref 2" field by highlighting it during focus and checking for changes or required validation when the field is left. The `DeltaCheck` method checks for any modifications after leaving the field.

```cs
        // Ref 2 Field -----------------------------------------------------
        private void txbRef2_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbRef2);
        }

        private void txbRef2_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbRef2);
            DeltaCheck();
        }
```

---

## 2.5 - Methods - Button Click Events

### 2.5.1 - Exit Button

The `btnExit_Click` method is the event handler for the Exit button on the form. It handles the logic required to cleanly close the form when the button is clicked, while also logging this action for traceability.

#### Function Signature:

* Access Modifier: `private`

#### Parameters:

* `sender`(`object`) – The control that triggered the event (in this case, the Exit button).
* `e`(`EventArgs`) – Contains event data.

#### Step-by-Step Breakdown:

* Log the Form Closing Action
    * This line writes a log entry indicating that the form was closed.
    * The log is written to the session log using a standard structure:
        * Class: [CarrierForm]
        * Method: [FormClosing]
        * Message: "Form Closed"

```cs
SessionMaintenance.LogBook("", "[CarrierForm]", "[FormClosing]", $"Form Closed");
```

* Close the Form
    * Invokes the Close() method on the current form, which triggers the form’s shutdown sequence.

```cs
this.Close();
```

#### Summary

The btnExit_Click method is a simple, clean event handler that:

* Logs the form closure action for auditing/debugging purposes.
* Closes the current form.
    
This ensures traceability and a consistent exit point from the CarrierForm.

```cs
        // Exit Button -----------------------------------------------------------------
        private void btnExit_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[CarrierForm]", "[FormClosing]", $"Form Closed");
            this.Close();
        }
```

---

### 2.5.2 - Search Button

The `btnSearch_Click` method handles the logic behind the Search button. It validates the input fields (client and load note), performs a status check on the load note, and either displays relevant error messages or proceeds to load the data if the conditions are met.

#### Function Signature:

* Access Modifier: `private`

#### Parameters:

* `sender`(`object`) – The control that triggered the event (Search button).
* `e`(`EventArgs`) – Contains event data.

#### Step-by-Step Breakdown:

* Retrieve Input
    * Gets the text entered in the Load Note textbox.
    * Initializes the `client` variable.

```cs
string loadNote = txbLoadNote.Text;
string client = "";
```

* Set Client (If Selected)
    * If a client is selected from the dropdown, its value is assigned to `client`.

```cs
if (cbClient.SelectedItem != null)
{
    client = cbClient.SelectedItem.ToString();
}
```

* Validate Inputs
    * If no client is selected, shows predefined error `"130"`.
    * If load note is empty, shows predefined error `"228"`.
    * Both cases halt execution early via return.

```cs
if (string.IsNullOrEmpty(client))
{
    messageBox.ShowDefError("130", $"");
    return;
}
else if (string.IsNullOrEmpty(loadNote))
{
    messageBox.ShowDefError("228", $"");
    return;
}
```

* Display Status Label
    * Temporarily shows a loading/status label while processing the request.

```cs
lblStatus.Visible = true;
```

* Check Load Note Status
    * Calls a method to check the status of the load note.
    * The result is stored as an integer `status`.

```cs
int status = CheckLoadNote(loadNote, client);
```

* Handle Status Results
    * Depending on the returned `status`, one of the following actions is taken:
        * `0`: Not found → Show error and hide the Print button.
        * `2`: Closed → Show error and hide the Print button.
        * `4`: Label already generated → Show warning, get load note data and enable the Print button.
        * Default: Assume valid → get load note and enable the Print button.

```cs
switch (status)
{
    case 0:
        messageBox.ShowError($"load note not found: {loadNote}");
        btnPrint.Visible = false;
        break;
    case 2:
        messageBox.ShowError($"Load note: {loadNote} is no longer open.");
        btnPrint.Visible = false;
        break;
    case 4:
        messageBox.ShowWarning($"A Carrier label has been created for Load note: {loadNote} using the TAFIE application");
        GetLoadNote(loadNote, client);
        btnPrint.Visible = true;
        break;
    default:
        GetLoadNote(loadNote, client);
        btnPrint.Visible = true;
        break;
}
```

* Hide Status Label
    * Hides the status label once processing is done.

```cs
lblStatus.Visible = false;
```

#### Summary

The `btnSearch_Click` method:
* Validates user input for client and load note.
* Checks the status of the load note in the system.
* Displays appropriate errors or proceeds with loading data.
* Manages UI feedback through the status label and Print button visibility.

```cs 
      // Search Button ------------------------------------------------------------------------------------------------------------------
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string loadNote = txbLoadNote.Text;
            string client = "";

            CustomMessageBox messageBox = new CustomMessageBox();

            if (cbClient.SelectedItem != null)
            {
                client = cbClient.SelectedItem.ToString();
            }

            if (string.IsNullOrEmpty(client))
            {
                messageBox.ShowDefError("130", $"");
                return;
            }
            else if (string.IsNullOrEmpty(loadNote))
            {
                messageBox.ShowDefError("228", $"");
                return;
            }
            else
            {
                lblStatus.Visible = true;
                int status = CheckLoadNote(loadNote, client);

                switch (status)
                {
                    case 0:
                        messageBox.ShowError($"load note not found: {loadNote}");
                        btnPrint.Visible = false;
                        break;
                    case 2:
                        messageBox.ShowError($"Load note: {loadNote} is no longer open.");
                        btnPrint.Visible = false;
                        break;
                    case 4:
                        messageBox.ShowWarning($"A Carrier label has been created for Load note: {loadNote} using the TAFIE application");
                        GetLoadNote(loadNote, client);
                        btnPrint.Visible = true;
                        break;
                    default:
                        GetLoadNote(loadNote, client);
                        btnPrint.Visible = true;
                        break;
                }
                lblStatus.Visible = false;
            }
        }
```

---

### 2.5.3 - Print Label Button (Async)

The `btnPrint_Click` method is triggered when the Print Label button is clicked. It validates the form fields, optionally asks the user for confirmation, ensures an access token is available, retrieves all box references, and initiates label printing for each box.

#### Function Signature:

* Access Modifier: `private`

#### Parameters:

* `sender`(`object`) – The control that triggered the event (Print button).
* `e`(`EventArgs`) – Contains event data.

#### Step-by-Step Breakdown:

* Validate Fields
    * Calls `CheckAllFields()` to ensure all required form inputs are valid.
    * If any field is invalid, the block is executed.

```cs
if (!CheckAllFields())
```

* Show Confirmation Prompt
    * Prompts the user to confirm if they want to save and print labels.
    * Stores their choice in `result`.

```cs
bool result = messageBox.ShowQuestion("Print Label(s)?", "Are you sure you want to save current data and print all carrier labels?");
```

* If User Confirms
    * Continues only if the user selects Yes.

```cs
if (result == true)
```

* Prepare Variables
    * Retrieves the account code needed to authenticate with the API.
    * Initializes retry counter (`attmps`) for token retrieval.

```cs
string? accCode = apiAccCode;
int attmps = 0;
```

* Save Current Data
    * Saves any changes made to the form before continuing.

```cs
SaveData();
```

* Ensure Access Token is Available
    * Checks whether an access token is available for the account.
    * If not, attempts to fetch one using `GetAccessToken()`.
    * Will only retry once (loop runs max twice).

```cs
while (!WcmsApi.CheckAccessTkn(accCode) && attmps <= 1)
{
    await GetAccessToken(accCode);
    attmps++;
}
```

* Retrieve Box References
    * Fetches all related box references from the database.
    * Stores the total number of boxes in `boxCount`.

```cs
List<int> boxRefs = GetBoxes();
int boxCount = boxRefs.Count;
```

* Print Labels for Each Box
    * Loops through each box reference.
    * Calls `GetLabel()` asynchronously for each, initiating the API call and printing process.

```cs
foreach (var boxRef in boxRefs)
{
    await GetLabel(boxRef, boxCount);
}
```

* Clear the Form
    * Resets the form once label generation is complete.

```cs
ClearFields();
```

#### Summary

The `btnPrint_Click` method:
* Checks whether all fields are valid.
* Asks the user to confirm they want to proceed with printing.
* Saves current data and ensures an access token is available.
* Fetches all box references for the current order.
* Prints labels for each box via API calls.
* Clears the form after completion.

```cs
// Print Label Button (Async) ------------------------------------------------------------------------------------------
        private async void btnPrint_Click(object sender, EventArgs e)
        {
            if (!CheckAllFields())
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                bool result = messageBox.ShowQuestion("Print Label(s)?", "Are you sure you want to save current data and print all carrier labels?"); // Ask user if they want to exit
                if (result == true)
                {
                    string? accCode = apiAccCode;
                    int attmps = 0;

                    SaveData(); // Save any changes

                    // Ensure access token is available before proceeding
                    while (!WcmsApi.CheckAccessTkn(accCode) && attmps <= 1)
                    {
                        await GetAccessToken(accCode);// Wait for access token to be retrieved
                        attmps++;
                    }

                    // Proceed with label request
                    List<int> boxRefs = GetBoxes();
                    int boxCount = boxRefs.Count;

                    foreach (var boxRef in boxRefs)
                    {
                        await GetLabel(boxRef, boxCount);
                    }

                    ClearFields();
                }
            }
        }
```

---

## 2.6 - Methods - MenuStrip Click Events

### 2.6.1 - Close

The `closeToolStripMenuItem_Click` method is triggered when the user selects Close from the form’s MenuStrip. It simply delegates to the existing Exit button handler to close the form.

#### Function Signature:

* Access Modifier: `private`

#### Parameters:

* `sender`(`object`) – The control that triggered the event (menu item).
* `e`(`EventArgs`) – Contains event data.

#### Step-by-Step Breakdown:

* Trigger Exit Logic
    * Calls the `btnExit_Click` method directly.
    * This reuses the same logic already implemented for the Exit button:
    * Logs the closure in the session log.
    * Closes the form.

```cs
btnExit_Click(sender, e);
```

#### Summary

The `closeToolStripMenuItem_Click` method:
* Simply reuses the logic from the Exit button (`btnExit_Click`) to close the form.
* Helps keep behavior consistent between the Exit button and MenuStrip Close option.

```cs
        // Close ------------------------------------------------------------
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnExit_Click(sender, e);
        }
```
        
---

### 2.6.2 - Print Label 

The `printLabelToolStripMenuItem_Click` method is triggered when the user selects Print Label from the MenuStrip. It checks if the Print button is currently visible, and if so, reuses the same logic as clicking the button. If not, it shows an error message.

#### Function Signature:

* Access Modifier: `private`

#### Parameters:

* `sender`(`object`) – The control that triggered the event (menu item).
* `e`(`EventArgs`) – Event arguments.

#### Step-by-Step Breakdown:

* Check if the Print Button is Visible
    * Determines whether label data is available and the user is allowed to print.
    * Visibility of the button is used as a proxy for this condition.

```cs
if (btnPrint.Visible)
```

* Trigger Print Logic
    * If the Print button is visible, the method programmatically triggers the same async logic as the Print button (`btnPrint_Click`).
    * This ensures consistent behavior whether the user clicks the button or uses the menu.

```cs
btnPrint_Click(sender, e);
```

* Show Error if Print is Not Allowed
    * If the Print button is not visible, show an error message indicating that no label data is available to print.

```cs
CustomMessageBox messageBox = new CustomMessageBox();
messageBox.ShowError($"No Label data to print");
```

#### Summary
The `printLabelToolStripMenuItem_Click` method:
* Reuses the existing Print button logic to ensure consistent label printing behavior from the MenuStrip.
* Performs a basic visibility check to determine whether printing is currently possible.
* Displays an error message if label printing is not applicable.

```cs
// Print Label --------------------------------------------------------------------------
        private void printLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnPrint.Visible)
            {
                btnPrint_Click(sender, e);
            }
            else
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"No Label data to print");
                return;
            }
        }
```

---

### 2.6.3 - Clear

The `clearToolStripMenuItem_Click` method is triggered when the user selects Clear from the MenuStrip. It resets the selected client and clears all input fields on the form.

#### Function Signature:

* Access Modifier: `private`

#### Parameters:

* `sender`(`object`) – The control that triggered the event (in this case, the menu item).
* `e`(`EventArgs`) – Standard event arguments for the handler.

#### Step-by-Step Breakdown:

* Clear Client Selection
    * Manually clears the selected value in the `cbClient` ComboBox.
    * This resets the client selection to a blank/default state.

```cs
cbClient.Text = "";
```

Clear All Other Fields
* Invokes the `ClearFields()` method, which is assumed to:
    * Reset all textboxes, dropdowns, and relevant input controls on the form.
    * Possibly reset UI indicators (e.g., error highlights, label states).

```cs
ClearFields();
```

#### Summary

The `clearToolStripMenuItem_Click` method:
* Provides a quick way to reset the form’s input via the MenuStrip.
* Clears the selected client and all other user input fields.
* Is functionally similar to a “Reset” or “New” action, preparing the form for fresh input. 

```cs      
  // Clear ------------------------------------------------------------------------------------------
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cbClient.Text = "";
            ClearFields();
        }
```

---

### 2.6.4 - Reprint 

The `reprintToolStripMenuItem_Click` method is triggered when the user selects Reprint from the MenuStrip. It opens the `RePrint` form and passes in relevant session context.

#### Function Signature:

* Access Modifier: `private`

#### Parameters:

* `sender`(`object`) – The UI control that triggered the event.
* `e`(`EventArgs`) – Standard event arguments.

#### Step-by-Step Breakdown:

* Instantiate the `RePrint` Form
    * Creates a new instance of the `RePrint` form/class.

```cs
RePrint rePrint = new RePrint();
```

* Pass Session Information
    * Sets the `userName` and `sessionId` properties of the `RePrint` form.
    * This likely allows the `RePrint` form to:
    * Log activity under the current user.
    * Filter or retrieve data related to the current session.

```cs
rePrint.userName = userName;
rePrint.sessionId = sessionId;
```

* Show the `RePrint` Form
    * Displays the `RePrint` form as a non-modal window.
    * This allows the user to interact with it without blocking the main form.

```cs
rePrint.Show();
```

#### Summary

The `reprintToolStripMenuItem_Click` method:
* Opens the `RePrint` window.
* Provides session context by passing in the current username and session ID.
* Allows the user to access previous labels or documents for reprinting purposes.

```cs
 // Reprint ------------------------------------------------------------------------------------------
        private void reprintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RePrint rePrint = new RePrint();
            rePrint.userName = userName;
            rePrint.sessionId = sessionId;
            rePrint.Show();
        }
```


### 2.6.5 - Carrier Combos
The `combinationsToolStripMenuItem_Click` method is triggered when the user selects Combinations from the MenuStrip. It opens the `CarrierCombos` form with the currently selected client.

#### Function Signature:

* Access Modifier: `private`

#### Parameters:

* `sender`(`object`) – The UI control that triggered the event.
* `e`(`EventArgs`) – Standard event arguments.

#### Step-by-Step Breakdown:

* Initialize `client` string
    * Prepares a placeholder for the selected client.

```cs
string client = "";
```

* Check if a client is selected
    * Ensures the user has selected a client from the dropdown.
    * Prevents attempting to launch the form without valid input.

```cs
if (cbClient.SelectedItem != null)
```

* Retrieve the selected client
    * Retrieves the client's string value for use in the next form.

```cs
client = cbClient.SelectedItem.ToString();
```

* Initialize and configure the `CarrierCombos` form
    * Creates a new instance of the `CarrierCombos` form.
    * Passes current session data (`userName`, `sessionId`) and the selected client to it.

```cs
CarrierCombos carrierCombos = new CarrierCombos();
carrierCombos.userName = userName;
carrierCombos.sessionId = sessionId;
carrierCombos.passedClient = client;
```

* Show the `CarrierCombos` form
    * Displays the form to the user.

```cs
carrierCombos.Show();
```

* Handle case where no client is selected
    * If no client was selected, shows an error using the message box with code `"130"`.
    * Stops execution early.

```cs
CustomMessageBox messageBox = new CustomMessageBox();
messageBox.ShowDefError("130", $"");
return;
```

#### Summary

The `combinationsToolStripMenuItem_Click` method:
* Opens the `CarrierCombos` form.
* Passes in the currently selected client and session/user context.
* Displays an error if no client is selected.

```cs
// Carrier Combos ----------------------------------------------------------
        private void combinationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string client = "";
            if (cbClient.SelectedItem != null)
            {
                client = cbClient.SelectedItem.ToString();

                CarrierCombos carrierCombos = new CarrierCombos();
                carrierCombos.userName = userName;
                carrierCombos.sessionId = sessionId;
                carrierCombos.passedClient = client;
                carrierCombos.Show();
            }
            else
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("130", $"");
                return;
            }
        }
```

---

### 2.6.6 - Address Search

The `addressSearchToolStripMenuItem_Click` method is triggered when the user selects Address Search from the MenuStrip. It launches an external address search utility via the `OpenURL()` method.

#### Function Signature:

* Access Modifier: `private`

#### Parameters:

* `sender`(`object`) – The control that initiated the event.
* `e`(`EventArgs`) – Standard event argument data.

#### Step-by-Step Breakdown:

* Call the `OpenURL` method
    * Directly invokes a method named `OpenURL`.
    * Presumably opens a web browser or external tool to assist with address lookup.
    * There is no validation or condition here; the method simply executes the URL-opening logic.

```cs
OpenURL();
```

#### Summary

The `addressSearchToolStripMenuItem_Click` method:
* Responds to a MenuStrip click event for "Address Search".
* Calls a utility method to open an address search resource or page (`OpenURL`).
* Keeps the logic abstracted and simple by offloading the core function to `OpenURL()`.

```cs
// Address Search ---------------------------------------------------------------------
        private void addressSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenURL();
        }
```

---

### 2.6.7 - Client Manager

The `clientManagerToolStripMenuItem_Click` method handles the event triggered when the user selects Client Manager from the MenuStrip. It opens the `ClientManager` form and passes in session context data for tracking or access control purposes.

#### Function Signature:

* Access Modifier: `private`

#### Parameters:

* `sender`(`object`) – The control that initiated the event.
* `e`(`EventArgs`) – Standard event argument data.

#### Step-by-Step Breakdown:

* Instantiate the `ClientManager` form
    * Creates a new instance of the `ClientManager` form class.

```cs
ClientManager clientManager = new ClientManager();
```

* Pass user session data
    * Assigns the current form’s `userName` and `sessionId` to the new form instance.
    * Ensures the `ClientManager` form can reference the current session for logging or permissions.

```cs
clientManager.userName = userName;
clientManager.sessionId = sessionId;
```

* Show the form
    * Displays the `ClientManager` form non-modally, allowing users to interact with it while keeping the current form open.

```cs
clientManager.Show();
```

#### Summary

The `clientManagerToolStripMenuItem_Click` method:
* Launches the `ClientManager` form.
* Passes in session-specific values for continuity and tracking.
* Opens the form without blocking the current UI thread (non-modal display).

```cs
        // Client Manager ------------------------------------------------------
        private void clientManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClientManager clientManager = new ClientManager();
            clientManager.userName = userName;
            clientManager.sessionId = sessionId;
            clientManager.Show();
        }
```

---

## 2.7 - Methods - Key Down Events

### 2.7.1 - CarrierForm_KeyDown

The `CarrierForm_KeyDown` method allows users to interact with the form using keyboard shortcuts. It improves accessibility and efficiency by mapping common actions to Ctrl key combinations and the Escape key.

#### Function Signature:

* Access Modifier: `private`

#### Parameters:

* `sender`(`object`) – the control that raised the event.
* `e`(`KeyEventArgs`) – provides information about the key press (e.g., which key and modifiers were used).

#### Step-by-Step Breakdown:

* Ctrl + G → Clear the form
    * Clears the client selection (`cbClient`) and resets the form fields via `ClearFields()`.

```cs
if (e.Control && e.KeyCode == Keys.G)
{
    cbClient.Text = "";
    ClearFields();
}
```

* Ctrl + R → Perform search
    * Triggers the `btnSearch_Click` method to validate and load data based on the current load note and client selection.

```cs
if (e.Control && e.KeyCode == Keys.R)
{
    btnSearch_Click(sender, e);
}
```

* Ctrl + P → Print labels
    * Initiates the label printing process by calling the same logic used in the Print Label menu item click.

```cs
if (e.Control && e.KeyCode == Keys.P)
{
    printLabelToolStripMenuItem_Click(sender, e);
}
```

* Escape → Close the form
    * Invokes the `btnExit_Click` method to close the form and log the exit event.

```cs
if (e.KeyCode == Keys.Escape)
{
    btnExit_Click(sender, e);
}
```

#### Summary:

This method provides keyboard shortcuts for common form actions:

<table>
  <tr>
   <td>Key Combination
   </td>
   <td>Action
   </td>
  </tr>
  <tr>
   <td>Ctrl + G	
   </td>
   <td>Clear client selection and fields
   </td>
  </tr>
  <tr>
   <td>Ctrl + R	
   </td>
   <td>Run search
   </td>
  </tr>
  <tr>
   <td>Ctrl + P	
   </td>
   <td>Print carrier labels
   </td>
  </tr>
  <tr>
   <td>Escape
   </td>
   <td>Close the form
   </td>
  </tr>
</table>
	
This improves user workflow by reducing dependency on mouse clicks.

```cs
private void CarrierForm_KeyDown(object sender, KeyEventArgs e)
        {
            // ctrl + G
            if (e.Control && e.KeyCode == Keys.G)
            {
                cbClient.Text = "";
                ClearFields();
            }

            // ctrl + R
            if (e.Control && e.KeyCode == Keys.R)
            {
                btnSearch_Click(sender, e);
            }

            // ctrl + P
            if (e.Control && e.KeyCode == Keys.P)
            {
                printLabelToolStripMenuItem_Click(sender, e);
            }

            // Esc
            if (e.KeyCode == Keys.Escape)
            {
                btnExit_Click(sender, e);
            }
        }
```
