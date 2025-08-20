[Back](ClassIndex.md)

# TAFIE Carrier Module - RePrint - Methods

## 4.1 - Methods - Initialization

### 4.1.1 - Initialization and declaration

This section sets up the `RePrint` form—defining required properties, establishing a database connection string, and configuring the form's basic behavior.

#### Signature

```cs
public partial class RePrint : Form
```

#### Breakdown

* Public Properties:
	* These properties allow the calling form to pass contextual data (e.g., user info or session tracking) into the `RePrint` form.
	* This enables features like logging, permissions, or filtering label reprint records per user/session.

```cs
public string sessionId { get; set; }
public string userName { get; set; }
```

* Private Constant
	* Stores the SQL Server connection string.
	* This constant will be used when querying the label print history from the `Appz` database.
	* `Integrated Security=True` uses Windows authentication.
	* `Encrypt=False` avoids TLS encryption (for internal LAN use).

```cs
private const string connectionString = "Server=SQL-SSRS;Database=Appz;Integrated Security=True;Encrypt=False;";
```

* Constructor:
	* `InitializeComponent()`: Standard Windows Forms call to wire up the designer-generated UI controls.
	* `this.KeyPreview = true;`: Allows the form to intercept key presses before they’re passed to any individual control—enables implementing global keyboard shortcuts (e.g., Esc to close the form).

```cs
public RePrint()
{
    InitializeComponent();
    this.KeyPreview = true;
}
```

#### Summary

The `RePrint` form is set up with:
* Public properties for external context (username & session).
* A static SQL connection string for querying the `Appz` database.
* Initialization that prepares the form and enables keyboard event handling

```cs

 public partial class RePrint : Form
    {
//=====================================================================================================
//-- Initialization --//
//=====================================================================================================

        public string sessionId { get; set; }
        public string userName { get; set; }
        private const string connectionString = "Server=SQL-SSRS;Database=Appz;Integrated Security=True;Encrypt=False;";

        public RePrint()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }
...

```

---

### 4.1.2 - Initialization and declaration

Handles initialization tasks that should occur when the `RePrint` form is loaded. Primarily used for logging and setting the form's window title dynamically.

#### Signature:

```cs
private void RePrint_Load(object sender, EventArgs e)
```

#### Breakdown:

* Log the form load:
	 * Logs an entry that the RePrint form has been opened.
	 * Uses `SessionMaintenance.LogBook` to track UI activity, useful for diagnostics or auditing.
	 * Tagging includes the form and method name (`[RePrint]`, `[FormLoad]`) and a clear message.

```cs
SessionMaintenance.LogBook("", "[RePrint]", "[FormLoad]", $"Form Started");
```

* Set the form's title:
	 * Sets the window title dynamically based on the current Windows user and app name.
	 * Pulls `Environment.UserName` from the system, converts it to uppercase for consistency.
	 * `SessionMaintenance.appName` is a static property storing the application’s display name.
	 * Makes the title user-aware and contextual: useful in multi-user environments.

```cs
Text = $"{Environment.UserName.ToUpper()} - {SessionMaintenance.appName} Reprint Label";
```

#### Summary:

This method executes when the `RePrint` form loads. It:
* Writes a log entry noting the form start.
* Dynamically updates the window title with the username and app name to personalize and contextualize the UI.

```cs
       // Form Load ----------------------------------------------------------
        private void RePrint_Load(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[RePrint]", "[FormLoad]", $"Form Started");
            Text = $"{Environment.UserName.ToUpper()} - {SessionMaintenance.appName} Reprint Label";
        }
```

---

## 4.2 - Methods - Operational

### 4.2.1 - PopulateDataGrid

The `PopulateDataGrid` method retrieves label print records from the database using a stored procedure and populates the `dgReprint` `DataGrid` with the results. It's used to allow the user to view and select previously printed labels for reprinting.

#### Signature
* Access: `Private` – only used within the `RePrint` form class.

#### Parameters:
* `search`(`string`): A search term to filter label records.

#### Step-by-Step Breakdown

* Query Definition
	* A SQL query is prepared to call the stored procedure `[TCAR_Rpnt_Srch]` with two parameters: session ID and search term.

```cs
string query = "EXECUTE [TCAR_Rpnt_Srch] @Session_Id, @Search";
```

* Prepare DataTable
	 * This will hold the results retrieved from the database.

```cs
DataTable dataTable = new DataTable();
```

* Try-Catch for Error Handling
	 * Ensures any errors during DB access are caught and handled gracefully.


* Open SQL Connection and Execute Command
	 * A `SqlConnection` is opened using the class-level connection string.
	 * `SqlCommand` is prepared with the query and parameters.
	 * The stored procedure is executed and the returned data is loaded into dataTable.

```cs
using (SqlConnection conn = new SqlConnection(connectionString))
{
    conn.Open();
    using (SqlCommand cmd = new SqlCommand(query, conn))
    {
        cmd.Parameters.AddWithValue("@Session_Id", sessionId);
        cmd.Parameters.AddWithValue("@Search", search);

        cmd.ExecuteNonQuery(); // Not required here - optional if stored procedure does not return data

        SqlDataReader reader = cmd.ExecuteReader();
        dataTable.Load(reader);
    }
    conn.Close();
}
```

* Update UI – DataGrid
	 * Sets the `DataGridView` (`dgReprint`) to display the contents of the dataTable.

```cs
dgReprint.DataSource = dataTable;
dgReprint.Refresh();
```

* Exception Handling
	 * Displays a custom error message if something goes wrong.
	 * Logs the error for later review.

```cs
CustomMessageBox messageBox = new CustomMessageBox();
messageBox.ShowDefError("117", $"\n{ex.Message}");
SessionMaintenance.LogBook("ERROR", "[RePrint]", "[PopulateDataGrid]", $"FAILED: Code 117 ( {ex.Message} )");
```

#### Summary

This method encapsulates the logic for retrieving reprintable label data from the database and displaying it in the form's data grid. It’s designed with robust error handling and makes use of consistent logging and UI updates. Its separation into a standalone method improves clarity and reusability within the form.

```cs
// Populate Datagrid ----------------------------------------------------
        private void PopulateDataGrid(string search)
        {
            string query = "EXECUTE [TCAR_Rpnt_Srch] @Session_Id, @Search";

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
                        cmd.Parameters.AddWithValue("@Session_Id", sessionId);
                        cmd.Parameters.AddWithValue("@Search", search);

                        // Execute Query
                        cmd.ExecuteNonQuery();

                        // Execute Data Reader
                        SqlDataReader reader = cmd.ExecuteReader();

                        // Populate DataTable From Reader
                        dataTable.Load(reader);
                    }

                    conn.Close(); // Close SQL Connection

                    // Populate Data Grid
                    dgReprint.DataSource = dataTable;
                    dgReprint.Refresh();
                }
            }
            catch (Exception ex) // Catch Errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("117", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[RePrint]", "[PopulateDataGrid]", $"FAILED: Code 117 ( {ex.Message} )");
            }
        }
```

---

### 4.2.2 - GetXML

The `GetXML` method retrieves the original XML message associated with a specific tracking number and session ID from the `[TCAR_Srch]` table. It is likely used to support the reprint process by fetching the original label data.

#### Signature

* Access: `private` – only used internally by the `RePrint` form.
* Returns: `string` – the XML data as a string.

#### Parameters:

* `tracking`(`string`): The tracking number to search for in the database.

#### Step-by-Step Breakdown

* Initialize Default XML Value
	* Sets a default placeholder value in case the database query does not return data.

```cs
string xml = "PLACEHOLDER";
```

* Define SQL Query
	* Retrieves the `XML_MESSAGE` for a given session and tracking number.
	* Uses `ISNULL` to return `"NO DATA"` if the field is `NULL`.

```cs
string query = "SELECT ISNULL([XML_MESSAGE], 'NO DATA') FROM [TCAR_Srch] WHERE Session_Id = @Session_Id AND Tracking = @Tracking";
```

* Database Access Using ADO.NET
	* Connects to SQL Server.
	* Uses parameterized query to avoid SQL injection.
	* Executes the reader and retrieves the XML if available.

```cs
using (SqlConnection conn = new SqlConnection(connectionString))
{
    conn.Open();
    using (SqlCommand cmd = new SqlCommand(query, conn))
    {
        cmd.Parameters.AddWithValue("@Session_Id", sessionId);
        cmd.Parameters.AddWithValue("@Tracking", tracking);
        ...
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                xml = reader[0].ToString();
            }
        }
    }
    conn.Close();
}
```

* Logging
	* Logs parameter values, whether data was found, and the final XML string.

```cs
SessionMaintenance.LogBook("", "[RePrint]", "[GetXML]", $"Parameters set: [{sessionId}], [{tracking}]");
SessionMaintenance.LogBook("", "[RePrint]", "[GetXML]", $"Data found in reader");
SessionMaintenance.LogBook("", "[RePrint]", "[GetXML]", $"XML Retrieved:\n{xml}");
```

* Error Handling
	* Displays an error message and logs the failure if an exception occurs.

```cs
catch (Exception ex)
{
    CustomMessageBox messageBox = new CustomMessageBox();
    messageBox.ShowDefError("230", $"\n{ex.Message}");
    SessionMaintenance.LogBook("ERROR", "[RePrint]", "[GetXML]", $"FAILED Code: 230 ( {ex.Message} )");
}
```

* Return XML
	* Returns the fetched or fallback XML value using `return xml;`

#### Summary

The `GetXML` method is a clean and focused utility for safely retrieving XML data from the database, using proper error handling and verbose logging. It ensures that if something goes wrong, the user is notified, and details are available in logs for debugging.

```cs
// Get XML -----------------------------------------------------------------------------------------
        private string GetXML(string tracking)
        {
            string xml = "PLACEHOLDER";
            string query = "SELECT ISNULL([XML_MESSAGE], 'NO DATA') FROM [TCAR_Srch] WHERE Session_Id = @Session_Id AND Tracking = @Tracking";

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {

                        cmd.Parameters.AddWithValue("@Session_Id", sessionId);
                        cmd.Parameters.AddWithValue("@Tracking", tracking);

                        SessionMaintenance.LogBook("", "[RePrint]", "[GetXML]", $"Parameters set: [{sessionId}], [{tracking}]");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                xml = reader[0].ToString(); // Access by column index
                                SessionMaintenance.LogBook("", "[RePrint]", "[GetXML]", $"Data found in reader");
                            }
                        }
                    }
                    conn.Close(); // Close SQL Connection
                }
            }
            catch (Exception ex) // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("230", $"\n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[RePrint]", "[GetXML]", $"FAILED Code: 230 ( {ex.Message} )");
            }

            SessionMaintenance.LogBook("", "[RePrint]", "[GetXML]", $"XML Retrieved:\n{xml}");
            return xml;
        }
```

---

### 4.2.3 - UpdateTCAR

The `UpdateTCAR` method updates shipment-related records in the database by calling the stored procedure `[TCAR_Rpnt]` with the tracking number and current user. This is likely used to log or flag that a reprint has occurred.

#### Signature

* Access: `private` – intended for internal use within the `RePrint` form.

#### Parameters:

* `tracking`(`string`): the tracking number for which the `TCAR` record should be updated.

#### Step-by-Step Breakdown

* SQL Command Definition
	* Prepares a call to the stored procedure that performs the update.

```cs
string query = "EXECUTE [TCAR_Rpnt] @Tracking, @User";
```

* Open SQL Connection
	* Establishes a connection to the SQL Server using the defined connection string.

```cs
using (SqlConnection conn = new SqlConnection(connectionString))
{
    conn.Open();
    ...
}
```

* Create and Configure Command
	* Sets the two required parameters:
		* `@Tracking`: the shipment's tracking number.
		* `@User`: the `username` performing the update.
		* Executes the stored procedure with `ExecuteNonQuery`, as no data is returned.

```cs
using (SqlCommand cmd = new SqlCommand(query, conn))
{
    cmd.Parameters.AddWithValue("@Tracking", tracking);
    cmd.Parameters.AddWithValue("@User", userName);
    cmd.ExecuteNonQuery();
}
```

* Close Connection
	* Closes the database connection after the update using `conn.Close();`

* Error Handling
	* If an exception occurs:
		* A message box displays a friendly error to the user.
		* The issue is logged to the session log with error code `226`.

```cs
catch (Exception ex)
{
    CustomMessageBox messageBox = new CustomMessageBox();
    messageBox.ShowError($"An error occured updating shipment records \n{ex.Message}");
    SessionMaintenance.LogBook("ERROR", "[RePrint]", "[UpdateTCAR]", $"FAILED: Code 226 ( {ex.Message} )");
}
```

#### Summary

`UpdateTCAR` is a reliable and concise method for updating shipment records in the database. It uses parameterized SQL to avoid injection risks, handles errors gracefully, and logs problems for troubleshooting. This is a good example of how to isolate responsibility: it focuses purely on updating data, with no side effects or UI logic beyond error messaging.

```cs
// Update TCAR -------------------------------------------------------------------------------------------------------------
        private void UpdateTCAR(string tracking)
        {
            string query = "EXECUTE [TCAR_Rpnt] @Tracking, @User";

            try
            {
                // Execute SQL Query
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open SQL Connection
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Tracking", tracking);
                        cmd.Parameters.AddWithValue("@User", userName);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close(); // Close SQL Connection
                }
            }
            catch (Exception ex)  // Catch any errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError($"An error occured updating shipment records \n{ex.Message}");
                SessionMaintenance.LogBook("ERROR", "[RePrint]", "[UpdateTCAR]", $"FAILED: Code 226 ( {ex.Message} )");
            }
        }
```

---

### 4.2.4 - RePrintLabel

The `RePrintLabel` method extracts label file paths from a given XML message and sends each label to the printer. It handles scenarios where no labels are present and logs/report errors as needed.

#### Signature

* Access: `private` – internal use within the `RePrint` form.

#### Parameters:

* `xml`(`string`): the raw XML string containing the label data to be extracted and reprinted.

#### Step-by-Step Breakdown

* Extract Label Data from XML
	* Delegates to the `WcmsApi` class to parse the XML and retrieve a list of file paths pointing to label PDFs.

```cs
List<string> labelFiles = WcmsApi.ExtractLabelData(xml);
```

* Check if Label Data Exists
	* Safeguards against null results or empty extractions.

```cs
if (labelFiles == null || labelFiles.Count == 0)
```

* Log and Alert on Failure
	* If no label files were extracted:
		* An error is logged.
		* A user-friendly message box is shown.
		* The method exits early.

```cs
SessionMaintenance.LogBook("ERROR", "[RePrint]", "[RePrintLabel]", "Failed to extract any labels.");
CustomMessageBox messageBox = new CustomMessageBox();
messageBox.ShowError("No label data found in API response.");
return;
```

* Loop Through and Print Labels
	* Iterates through each file in the label list.
	* Sends each one to the printer via `WcmsApi.PrintPdf`.

```cs
foreach (var labelFile in labelFiles)
{
    WcmsApi.PrintPdf(labelFile);
}
```

#### Summary

`RePrintLabel` is a focused utility for handling the label reprinting process. It responsibly:
* Validates that label data is available,
* Logs issues and informs the user when necessary,
* And uses dedicated helper methods to print each label file.

All printing logic is neatly encapsulated and delegated, which keeps the form code tidy and readable.

```cs
// Reprint Label ---------------------------------------------------------------------------------
        private void RePrintLabel(string xml)
        {
            List<string> labelFiles = WcmsApi.ExtractLabelData(xml);

            if (labelFiles == null || labelFiles.Count == 0)
            {
                SessionMaintenance.LogBook("ERROR", "[RePrint]", "[RePrintLabel]", "Failed to extract any labels.");
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("No label data found in API response.");
                return;
            }

            // Print all labels
            foreach (var labelFile in labelFiles)
            {
                WcmsApi.PrintPdf(labelFile);
            }
        }
```

---

### 4.2.5 - ReprintFunction

`ReprintFunction` handles the reprint process for a selected shipment label in the `RePrint` form. It retrieves the tracking number from the selected row, fetches the corresponding XML, reprints the label, logs the reprint, and updates the backend to reflect the reprint action.

#### Signature

* Access: `private` – intended for internal use within the `RePrint` form.

#### Step-by-Step Breakdown

* Initial Log Entry
	* Logs the start of the reprint attempt.

```cs
SessionMaintenance.LogBook("", "[RePrint]", "[ReprintFunction]", $"Reprint attempted");
```

* Check for Selected Row
	* Ensures that a row in the data grid is selected before proceeding.

```cs
if (dgReprint.SelectedRows.Count > 0)
```

* Retrieve Tracking Number
	* Grabs the first selected row.
	* Extracts the tracking number from column index 2 (assumed to be the correct field for tracking).

```cs
DataGridViewRow selectedRow = dgReprint.SelectedRows[0];
string tracking = selectedRow.Cells[2].Value.ToString();
```

* Reprint Label
	* Calls `GetXML` to retrieve the XML associated with the tracking number.
	* Passes it to `RePrintLabel` to handle the actual print process.

```cs
RePrintLabel(GetXML(tracking));
```

* Log Reprint Event
	* Records details of the successful reprint action.

```cs
SessionMaintenance.LogBook("", "[RePrint]", "[ReprintFunction]", $"Carrier Label Re-Printed: {tracking}, {userName}, {sessionId}");
```

* Update Backend Record
	* Updates the database to reflect the reprint via a stored procedure.

```cs
UpdateTCAR(tracking);
```

* Error Handling (No Selection)
	* If no row is selected:
		* Shows a default error box.
		* Logs error code 205.

```cs
else
{
    CustomMessageBox messageBox = new CustomMessageBox();
    messageBox.ShowDefError("205", $"");
    SessionMaintenance.LogBook("", "[RePrint]", "[ReprintFunction]", "Error Triggered: 205");
    return;
}
```

#### Summary

`ReprintFunction` cleanly coordinates the label reprinting process based on a user's selection. It:
* Validates user interaction,
* Retrieves relevant label data,
* Sends it to the print pipeline,
* And reflects the reprint in both logs and backend records.

This method benefits from a modular design approach by delegating work to dedicated helpers like `GetXML`, `RePrintLabel`, and `UpdateTCAR`.

```cs
// Reprint Function ---------------------------------------------------------
        private void ReprintFunction()
        {
            SessionMaintenance.LogBook("", "[RePrint]", "[ReprintFunction]", $"Reprint attempted");

            if (dgReprint.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgReprint.SelectedRows[0];

                string tracking = selectedRow.Cells[2].Value.ToString();

                RePrintLabel(GetXML(tracking));

                SessionMaintenance.LogBook("", "[RePrint]", "[ReprintFunction]", $"Carrier Label Re-Printed: {tracking}, {userName}, {sessionId}");

                UpdateTCAR(tracking);
            }
            else
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("205", $"");
                SessionMaintenance.LogBook("", "[RePrint]", "[ReprintFunction]", "Error Triggered: 205");
                return;
            }
        }
```

## 4.3 - Methods - Environment Events

### 4.3.1 - Search Field

These methods manage the visual behavior and interaction feedback when the Search text box (`txbSearch`) gains or loses focus, helping to maintain a consistent and user-friendly UI.

#### Signatures

```cs
private void txbSearch_Enter(object sender, EventArgs e)
private void txbSearch_Leave(object sender, EventArgs e)
```

#### Parameters:

* `sender`(`object`): the control that triggered the event (`txbSearch`).
* `e`(`EventArgs`): standard event arguments (not used directly).

#### Step-by-Step Breakdown

* `txbSearch_Enter`
	* Called when the user focuses on the Search field (e.g. clicks or tabs into it).
	* Delegates to `SessionMaintenance.ControlEnter`, which will:
		* Highlight the control (e.g. background color change).
		* Log or audit interaction (if implemented that way).
		* Provide consistent behavior across all forms.

* `txbSearch_Leave`
	* Called when the user leaves the Search field (e.g. tabs or clicks out).
	* Delegates to `SessionMaintenance.ControlLeave` to reverse visual changes or perform validation triggers if needed.

#### Summary

These handlers keep the UI feedback consistent and centralized using shared `SessionMaintenance` methods. This modular approach helps ensure easy maintenance and styling changes across all input fields in the application.

```cs
 // Search Field ----------------------------------------------------------
        private void txbSearch_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbSearch);
        }

        private void txbSearch_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbSearch);
        }
```

--- 

### 4.3.2 - Search Button

These methods handle the mouse hover effects for the `btnSearch` button. When the user moves the cursor over the button or away from it, the button's appearance changes to provide visual feedback — improving the overall user experience and consistency across the app.

#### Signatures

```cs
private void btnSearch_MouseEnter(object sender, EventArgs e)
private void btnSearch_MouseLeave(object sender, EventArgs e)
```

#### Parameters:

* `sender`(`object`): the control that triggered the event (here, always `btnSearch`).
* `e`(`EventArgs`): standard event data (unused but required by the event handler signature).

#### Step-by-Step Breakdown

* `btnSearch_MouseEnter`
	* Triggered when the mouse pointer enters the bounds of `btnSearch`.
	* Calls `SessionMaintenance.ButtonEnter()` to apply the standard hover style (e.g., background color, text color).

* `btnSearch_MouseLeave`
	* Triggered when the mouse pointer leaves the button area.
	* Calls `SessionMaintenance.ButtonLeave()` to revert the visual style back to its default state.

#### Summary

These handlers are a clean and consistent way to manage hover styling across buttons using shared UI logic in the `SessionMaintenance` class. 

```cs
// Search Button -------------------------------------------------------
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

### 4.3.3 - Exit Button

These methods manage visual feedback when the mouse interacts with the `btnExit` control. Specifically, they provide a consistent hover effect that helps users understand the button is interactive and ready to be clicked — contributing to a polished and intuitive UI.

#### Signatures

```cs
private void btnExit_MouseEnter(object sender, EventArgs e)
private void btnExit_MouseLeave(object sender, EventArgs e)
```

#### Parameters:

* `sender`(`object`): the source of the event (here, always `btnExit`).
* `e`(`EventArgs`): standard event args, not directly used in this method.

#### Step-by-Step Breakdown

* `btnExit_MouseEnter`
	* Triggered when the cursor enters the exit button’s bounds.
	* Calls `SessionMaintenance.ButtonEnter()` to apply hover styling such as background and/or text color changes.

* `btnExit_MouseLeave`
	* Triggered when the cursor leaves the button area.
	* Calls `SessionMaintenance.ButtonLeave()` to revert the button to its default styling.

#### Summary

These handlers provide a consistent and responsive hover effect for the `btnExit` button using shared methods from `SessionMaintenance`. 

```cs
// Exit Button ---------------------------------------------------------------
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

### 4.3.4 - Reprint Button

These methods handle visual feedback for the `btnReprint` control when the user interacts with it via mouse hover. The goal is to enhance the UI experience by signaling interactivity through styling changes.

#### Signatures

```cs
private void btnReprint_MouseEnter(object sender, EventArgs e)
private void btnReprint_MouseLeave(object sender, EventArgs e)
```

#### Parameters:

* `sender`(`object`): the UI control that triggered the event (`btnReprint`).
* `e`(`EventArgs`): standard event arguments (not directly used here).

#### Step-by-Step Breakdown

* `btnReprint_MouseEnter`
	* Triggered when the mouse enters the `btnReprint` area.
	* Calls the shared `ButtonEnter()` method to apply hover-specific styling (e.g., highlight color or font).

* `btnReprint_MouseLeave`
	* Triggered when the mouse leaves the button area.
	* Calls `ButtonLeave()` to restore the default appearance.

#### Summary

These hover event handlers are part of a consistent UI behavior strategy across your forms. They leverage centralized styling logic from `SessionMaintenance` to reduce code duplication and ensure that hover feedback is applied uniformly to all interactive buttons.

```cs
        // Reprint Button --------------------------------------------
        private void btnReprint_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnReprint);
        }

        private void btnReprint_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnReprint);
        }
```

## 4.4 - Methods - Button Click Events

### 4.4.1 - Exit Button

`btnExit_Click` is the event handler for the Exit button on the RePrint form. It logs the closure action and closes the form when the user clicks the Exit button.

#### Signature

* Access: `private` – used only within the `RePrint` form.

```cs
private void btnExit_Click(object sender, EventArgs e)
```

#### Parameters:

* `sender`(`object`): the control that triggered the event (in this case, `btnExit`).
* `e`(`EventArgs`): standard event arguments (not used in this method).

#### Step-by-Step Breakdown

* Log Form Closing
	* Records that the RePrint form is being closed in the log book.

```cs
SessionMaintenance.LogBook("", "[RePrint]", "[FormClosing]", $"Form Closed");
```

* Close the Form
	* Triggers the built-in `Close()` method of the form.
	* Disposes of the current instance and removes it from view.

#### Summary

The `btnExit_Click` method provides a clean and consistent way to exit the `RePrint` form. It ensures:
* Proper logging for audit and traceability,
* Immediate closure of the form interface upon user action.
* This kind of centralized logging, even for something as simple as form closure, is a strong practice, it reinforces the app’s reliability and traceability.

```cs
        // Exit Button ---------------------------------------------------------------------------
        private void btnExit_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[RePrint]", "[FormClosing]", $"Form Closed");
            this.Close();
        }
```

---

### 4.4.2 - Search Button
`btnSearch_Click` is the click event handler for the Search button on the `RePrint` form. It executes a search using the provided search term and refreshes the data grid with the results.

#### Signature

* Access: `private` – scoped within the `RePrint` form.

```cs
private void btnSearch_Click(object sender, EventArgs e)
```

#### Parameters:

* `sender`(`object`): the source of the event (`btnSearch`).
* `e`(`EventArgs`): event data (not used here).

#### Step-by-Step Breakdown

* Retrieve Search Input
	* Captures the user-entered search term from the text box (`txbSearch`).

```cs
string search = txbSearch.Text;
```

* Clear Current Data Grid Contents
	* Removes any existing data from the grid to prepare for new results.
	* Ensures the UI visually reflects that it's been reset.

```cs
dgReprint.DataSource = null;
dgReprint.Refresh();
```

* Repopulate Grid with Filtered Results
	* Calls a previously defined method to query the database using the search parameter.
	* Updates the grid (`dgReprint`) with fresh search results.

```cs
PopulateDataGrid(search);
```

* Clear the Search Box
	* Clears the input field for user convenience.
	* Helps avoid duplicate or accidental repeated searches.

```cs
txbSearch.Text = "";
```

#### Summary

The `btnSearch_Click` method provides users a responsive way to filter previous shipments by search term. It ensures:
* Clean visual feedback (by clearing and refreshing the grid),
* Encapsulation of logic (via `PopulateDataGrid`),
* A seamless and user-friendly experience (by clearing the input box after each search).


```cs
// Search Button ----------------------------------------------------------------------------------
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txbSearch.Text;

            dgReprint.DataSource = null;
            dgReprint.Refresh();

            PopulateDataGrid(search);

            txbSearch.Text = "";
        }
```

---
 
### 4.4.3 - Reprint Button
`btnReprint_Click` is the click event handler for the Reprint button on the `RePrint` form. It initiates the process of reprinting a previously generated carrier label based on the user’s current selection in the data grid.

#### Signature

* Access: `private` – scoped within the `RePrint` form.

```cs
private void btnReprint_Click(object sender, EventArgs e)
```

#### Parameters:

* `sender`(`object`): the button triggering the event.
* `e`(`EventArgs`): standard event arguments (not used in this handler).

#### Step-by-Step Breakdown

* Call Reprint Function
	* Delegates the core logic to the `ReprintFunction` method.
	* This encapsulates everything needed to:
		* Get the selected row from the data grid.
		* Extract the tracking number.
		* Fetch the original XML label data.
		* Send the label back to the printer.
		* Update tracking records in the database.
		* Log success or failure.

```cs
ReprintFunction();
```

#### Summary

The `btnReprint_Click` method acts as a simple trigger that offloads the actual work to the ReprintFunction. This separation of concerns keeps the click handler clean and maintains a tidy, testable architecture.

```cs
// Reprint Button -----------------------------------------------
        private void btnReprint_Click(object sender, EventArgs e)
        {
            ReprintFunction();
        }
```

