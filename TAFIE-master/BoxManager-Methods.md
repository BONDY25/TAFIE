[Back](ClassIndex.md)

# TAFIE Carrier Module - BoxManager - Methods

## 7.1 - Methods - Initialization

### 7.1.1 - Initialization and declaration

This section sets up the `BoxManager` form, defining its required properties, establishing the database connection, and preparing for data interaction between this form and the parent form.

#### Signature:

```cs
public partial class BoxManager : Form
```

#### Breakdown:

* Public Properties:
  * These allow the calling (parent) form to pass relevant context to the `BoxManager` form:
    * `sessionId` - Used for session-aware logging or security filtering.
    * `userName` - Identifies the user performing the box entry action.
    * `tcarRef` - A unique carrier reference that ties the box data to a specific shipment.

```cs
public string sessionId { get; set; }
public string userName { get; set; }
public string tcarRef { get; set; }
```

* Public Field:
  * Defaults the number of boxes to `1`, allowing this to be changed based on user input or retrieved data.

```cs
public int boxQty = 1;
```

* Callback Action:
  * A delegate that the parent form can assign to handle updated box data.
  * Provides a clean way for the form to return a `DataTable` of box data after saving.

```cs
public Action<DataTable> OnBoxDataSaved;
```

* Private Constant:
  * Looks at the `SessionMaintenance` class's global variable `connectionString`
  * Stores the SQL Server connection string for interacting with the `Appz` database.
  * Uses Windows Authentication (`Integrated Security=True`).
  * Disables encryption (`Encrypt=False`), suitable for internal LAN scenarios.

```cs
private const string connectionString = SessionMaintenance.connectionString;
```

* Constructor:
  * Initializes the form and loads its controls.

```cs
public BoxManager()
{
    InitializeComponent();
}
```

```cs
    public partial class BoxManager : Form
    {
//======================================================================================================================
//-- Initialization --//
//======================================================================================================================

        public string sessionId { get; set; }
        public string userName { get; set; }

        public string tcarRef { get; set; }

        public int boxQty = 1;

        public Action<DataTable> OnBoxDataSaved; // This will be set by the parent form

        private const string connectionString = "Server=SQL-SSRS;Database=Appz;Integrated Security=True;Encrypt=False;";
        public BoxManager()
        {
            InitializeComponent();
        }
```

---

### 7.1.2 - BoxManager_Load

The `BoxManager_Load` method runs when the `BoxManager` form is first loaded. It sets up the UI, logs the form launch, and initializes control states with passed-in or default data.

#### Signature:

```cs
private void BoxManager_Load(object sender, EventArgs e)
```

#### Breakdown:
* Logs that the `BoxManager` form has been opened. Helps with audit trails and support diagnostics.

```cs
SessionMaintenance.LogBook("", "[BoxManager]", "[FormLoad]", $"Form Started");	
```

* Sets the form’s title bar to include the current user's name and app context. Enhances user awareness of their session.

```cs
Text = $"{Environment.UserName.ToUpper()} - {SessionMaintenance.appName} Box Manager";	
```

* Hides the `dgContents` `DataGridView` by default and prevents user interaction with an empty table on load.

```cs
dgContents.Visible = false;	
```

* Initializes the textbox for box quantity using the `boxQty` value set externally or defaulted to `1`. Ensures the UI reflects the correct starting quantity.

```cs
txbBoxQty.Text = boxQty.ToString();	
```

#### Summary:
This load event is responsible for:
* Logging form access
* Updating the UI with session-specific and contextual data
* Prepping form controls to be in a clean and informative state before user interaction begins

```cs
 private void BoxManager_Load(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[BoxManager]", "[FormLoad]", $"Form Started");
            Text = $"{Environment.UserName.ToUpper()} - {SessionMaintenance.appName} Box Manager";
            dgContents.Visible = false;
            txbBoxQty.Text = boxQty.ToString();
        }
```

---


## 7.2 - Methods - Operational

### 7.2.1 - PopulateDataGrid

This method queries the database for the contents of a specific shipment request reference (identified by `tcarRef`) and loads the results into a `DataGridView` on the form.

#### Signature:

```cs
private void PopulateDataGrid()
```

#### Breakdown:
* Define the SQL stored procedure call to retrieve result rows for the given `TCAR` reference. 

```cs
string query = "EXECUTE [TCAR_Get_Results] @TCAR_Ref, 5";	
```

* Creates an in-memory table to hold the data retrieved from SQL Server.

```cs
DataTable dataTable = new DataTable();	
```

* Opens a secure SQL Server connection using the predefined credentials and server info.

```cs
using (SqlConnection conn = new SqlConnection(connectionString))
```

* Passes the reference number of the `TCAR` record as a parameter to the stored procedure.

```cs
cmd.Parameters.AddWithValue("@TCAR_Ref", tcarRef);
```

* Executes the SQL and returns a stream of rows from the result set.

```cs
SqlDataReader reader = cmd.ExecuteReader();	
```

* Fills the `DataTable` with data returned from SQL Server.

```cs
dataTable.Load(reader);	
```

* Binds the loaded data to the `DataGridView` for display.

```cs
dgContents.DataSource = dataTable;	
```

* Ensures the UI updates with the new data immediately.

```cs
dgContents.Refresh();	
```

* Catch Block - Handles and logs any exceptions, showing an error to the user and writing a detailed message to the app's log system for debugging or support.

#### Summary:
This method populates the on-screen grid with shipment or box-level data tied to the `tcarRef`.
It uses a stored procedure to fetch information from the database, fills a `DataTable`, and binds that table to the `DataGridView`. Robust error handling ensures that both the user and developer are informed in case of failure.

```cs
// Populate DataGridView with Results from SQL Server -----------------------------------------
        private void PopulateDataGrid()
        {
            string query = "EXECUTE [TCAR_Get_Results] @TCAR_Ref, 5";

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

### 7.2.2 - PopulateDgComboBox
This method populates the `BoxNo` column of a `DataGridView` (`dgContents`) with a list of combo box options representing box numbers from `1` to the user-specified `boxQty`.

#### Signature:

```cs
private void populateDgComboBox()
```

#### Breakdown:
* Validates that a meaningful number of boxes is defined. If not, an error message is shown and the method exits early.

```cs
if (boxQty <= 0)	
```

* Generates a sequential list of strings `1` through `boxQty` representing valid box number options.

```cs
List<string> boxNumbers = Enumerable.Range(1, boxQty).Select(i => i.ToString()).ToList();	
```

* Ensures that the `BoxNo` column exists and is of the correct type (`ComboBox`). If not, the user is alerted.

```cs
if (dgContents.Columns["BoxNo"] is DataGridViewComboBoxColumn comboBoxColumn)	
```

* Resets the combo box items and adds the fresh list of box numbers. This updates the template used when new rows are added.

```cs
comboBoxColumn.Items.Clear(); 
comboBoxColumn.Items.AddRange(...)	
```

* Iterates through each row of the `DataGridView`. Skips the placeholder "new row" used by the UI.

```cs
foreach (DataGridViewRow row in dgContents.Rows)	
```

* Ensures each cell in the column is a `ComboBoxCell`. If it's not yet initialized, it is created and inserted.

```cs
var cell = row.Cells["BoxNo"] as DataGridViewComboBoxCell;	
```

* Assigns a fresh list of box numbers to the cell’s `DataSource`. A copy is used to prevent shared reference issues.

```cs
cell.DataSource = new List<string>(boxNumbers);	
```

* The `else` branch If the `BoxNo` column is not a `DataGridViewComboBoxColumn`, shows a user error and stops execution.

#### Summary:
This method ensures that the `BoxNo` column in the `dgContents` grid is populated with a combo box containing all valid box numbers (from `1` to `boxQty`). It dynamically binds each cell to an independent data source to avoid shared state issues between rows. Robust validation and user messaging are included to handle missing columns or invalid inputs.

```cs 
// Populate DataGridView ComboBox Column with Box Numbers -------------------------------------------------------------
        private void populateDgComboBox()
        {
            if (boxQty <= 0)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("Please enter a valid positive number of boxes.");
                return;
            }

            // Generate list of box numbers as strings
            List<string> boxNumbers = Enumerable.Range(1, boxQty).Select(i => i.ToString()).ToList();

            // Check that the BoxNo column exists and is a combo box column
            if (dgContents.Columns["BoxNo"] is DataGridViewComboBoxColumn comboBoxColumn)
            {
                // Clear any existing items in the column template
                comboBoxColumn.Items.Clear();
                comboBoxColumn.Items.AddRange(boxNumbers.ToArray());

                // Populate each row's cell with a combo box if not already
                foreach (DataGridViewRow row in dgContents.Rows)
                {
                    if (row.IsNewRow) continue;

                    // Set the cell value to null or default
                    var cell = row.Cells["BoxNo"] as DataGridViewComboBoxCell;
                    if (cell == null)
                    {
                        cell = new DataGridViewComboBoxCell();
                        row.Cells["BoxNo"] = cell;
                    }

                    cell.DataSource = new List<string>(boxNumbers); // fresh list to prevent reference issues
                }
            }
            else
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("BoxNo column is not a ComboBox column.");
                return;
            }
        }
```

---

### 7.2.3 - SaveDataGrid
Extracts and formats user-edited data from the `dgContents` `DataGridView` into a `DataTable`, which can then be passed to other parts of the application or persisted to the database.

#### Signature:

```cs
private DataTable SaveDataGrid()
```

#### Breakdown:
* Creates a new `DataTable` instance:
    * This table will be used to hold the values pulled from the `dgContents` grid. It's returned at the end of the method.

```cs
DataTable dt = new DataTable();
```

* Defines the expected columns:
    * These match the expected structure of the data in `dgContents`. All are stored as strings regardless of their original data type to maintain consistency and reduce type conversion issues during export or further processing.

```cs
dt.Columns.Add("BoxNo", typeof(string));
dt.Columns.Add("Part", typeof(string));
dt.Columns.Add("Qty", typeof(string));
```

* Loops through each row in the `DataGridView`:
    * Skips any new/placeholder rows used for UI entry.

```cs
foreach (DataGridViewRow row in dgContents.Rows)
```

* Extracts data from each cell and adds to `DataTable`:
    * The method uses the cell name (e.g. `BoxNo`) to access the correct field.
    * The null-conditional operator (?.) protects against null references, avoiding runtime errors.
    * Each `DataRow` is added to the `DataTable`.

```cs
dr["BoxNo"] = row.Cells["BoxNo"].Value?.ToString();
dr["Part"] = row.Cells["Part"].Value?.ToString();
dr["Qty"] = row.Cells["Qty"].Value?.ToString();
```

* Exception Handling:
    * If an error occurs while accessing cell data or during table population, it logs the issue and shows an error message:
    * Uses `CustomMessageBox` to alert the user.
    * Logs error with `SessionMaintenance.LogBook` using code `119` for easier tracking.

* Returns:
    * Returns the constructed `DataTable` for use elsewhere, typically in form data submission or inter-form communication (e.g., through `OnBoxDataSaved` callback).

#### Summary:
The `SaveDataGrid` method Extracts structured box/part/quantity data from the grid into a table, safely handles empty values and logs errors on failure. It is used for saving user input for further processing or callback delivery. 

```cs    
    // Save Data from DataGridView to DataTable -------------------------------------------------------------------
        private DataTable SaveDataGrid()
        {
            DataTable dt = new DataTable();

            try
            {
                // Define columns
                dt.Columns.Add("BoxNo", typeof(string));
                dt.Columns.Add("Part", typeof(string));
                dt.Columns.Add("Qty", typeof(string));

                foreach (DataGridViewRow row in dgContents.Rows)
                {
                    if (row.IsNewRow) continue;

                    DataRow dr = dt.NewRow();

                    // Assuming correct cell names/indices
                    dr["BoxNo"] = row.Cells["BoxNo"].Value?.ToString();
                    dr["Part"] = row.Cells["Part"].Value?.ToString();
                    dr["Qty"] = row.Cells["Qty"].Value?.ToString();

                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex) // Catch Errors
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("Error Saving Data");
                SessionMaintenance.LogBook("ERROR", "[CarrierForm]", "[SaveDataGrid]", $"FAILED: Code 119 ( {ex.Message} )");
            }

            return dt;
        }  
```

---

## 7.3 - Methods - Environment Events

### 7.3.1 - BoxQty Field

These handlers manage focus behavior and validation logic for the Box Quantity input field. When the user finishes entering a box quantity, the value is validated, the `DataGridView` is populated, and the field is locked to prevent further changes.

#### Signature(s):

```cs
private void txbBoxQty_Enter(object sender, EventArgs e)
private void txbBoxQty_Leave(object sender, EventArgs e)
```

#### Breakdown:

* `txbBoxQty_Enter` – Highlighting the control
    * This is a standard UI helper from the `SessionMaintenance` class that changes the background/foreground styling to indicate focus.

```cs
SessionMaintenance.ControlEnter(txbBoxQty);
```

* `txbBoxQty_Leave` – Validation and flow control
    * Marks the end of focus and reverts any visual styling applied during Enter.

```cs
SessionMaintenance.ControlLeave(txbBoxQty);
```

* Validation:
    * Ensures the user input is a positive integer.

```cs
if (int.TryParse(txbBoxQty.Text, out int qty) && qty > 0)
```

* If valid:
    * Stores the new value in `boxQty`.
    * Makes the `DataGridView` visible.
    * Calls `PopulateDataGrid()` to fetch SQL results.
    * Calls `populateDgComboBox()` to update the box number dropdowns.
    * Disables the input field so the user can't re-enter a different value.

```cs
txbBoxQty.ReadOnly = true;
txbBoxQty.Enabled = false;
```

* If invalid:
    * An error message is shown using `CustomMessageBox`.
    * The field is reset to the last valid `boxQty`.
    * The textbox remains editable so the user can try again.

```cs
txbBoxQty.Text = boxQty.ToString();
txbBoxQty.ReadOnly = false;
txbBoxQty.Enabled = true;
```

#### Summary:
These handlers handle UI Events for the `txbBoxQty` Texbox, gracefully handles invalid input and guides users to correct it. When the control enters focus the handlers Apply visual focus styling. When the control leaves focus, the handler validates input, populates the grid, and disables further editing.

```cs
// Box Quantity TextBox ---------------------------------------------------------------------
        private void txbBoxQty_Enter(object sender, EventArgs e)
        {
            SessionMaintenance.ControlEnter(txbBoxQty);
        }

        private void txbBoxQty_Leave(object sender, EventArgs e)
        {
            SessionMaintenance.ControlLeave(txbBoxQty);
            if (int.TryParse(txbBoxQty.Text, out int qty) && qty > 0)
            {
                boxQty = qty;
                dgContents.Visible = true; // Show DataGridView if boxQty is valid
                PopulateDataGrid(); // Populate the DataGridView with data
                populateDgComboBox();
                txbBoxQty.ReadOnly = true;// Make the TextBox read-only after entering a valid value
                txbBoxQty.Enabled = false;
            }
            else
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("Please enter a valid positive number of boxes.");
                txbBoxQty.Text = boxQty.ToString(); // Reset to previous valid value
                txbBoxQty.ReadOnly = false;
                txbBoxQty.Enabled = true;
                return;
            }
        }
```

---

### 7.3.2 - Exit Button
These handlers manage visual UI behavior when the mouse pointer enters or leaves the Exit button, providing a consistent hover/focus experience across the application.

#### Signature(s):

```cs
private void btnExit_MouseEnter(object sender, EventArgs e)
private void btnExit_MouseLeave(object sender, EventArgs e)
```

#### Breakdown:
* `btnExit_MouseEnter`
    * Applies hover styling to the button.
    * Changes foreground/background color to visually indicate that the button is interactive and currently hovered.

```cs
SessionMaintenance.ButtonEnter(btnExit);
```

* `btnExit_MouseLeave`
    * Reverts any visual styling applied during `MouseEnter`.
    * Ensures the UI returns to a neutral state when the user moves the mouse away.

```cs
SessionMaintenance.ButtonLeave(btnExit);
```

#### Summary:
These handlers handle UI Events for the Exit Button and Aligns with visual behavior applied to buttons across the application. Upon the mouse entering the control the handler applies focus styling using `SessionMaintenance.ButtonEnter`. Upon the mouse leaving the control the handler reverts styling with `SessionMaintenance.ButtonLeave`.

```cs
// Exit Button -----------------------------------------------------------------------------
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

### 7.3.3 - Save Button
These handlers manage visual UI behavior when the mouse pointer enters or leaves the Save button, providing a consistent hover/focus experience across the application.

#### Signature(s):

```cs
private void btnSave_MouseEnter(object sender, EventArgs e)
private void btnSave_MouseLeave(object sender, EventArgs e)
```

#### Breakdown:
* `btnSave_MouseEnter`
    * Applies hover styling to the button.
    * Changes foreground/background color to visually indicate that the button is interactive and currently hovered.

```cs
SessionMaintenance.ButtonEnter(btnSave);
```

* `btnSave_MouseLeave`
    * Reverts any visual styling applied during `MouseEnter`.
    * Ensures the UI returns to a neutral state when the user moves the mouse away.

```cs
SessionMaintenance.ButtonLeave(btnSave);
```

#### Summary:
These handlers handle UI Events for the Save Button and Aligns with visual behavior applied to buttons across the application. Upon the mouse entering the control the handler applies focus styling using `SessionMaintenance.ButtonEnter`. Upon the mouse leaving the control the handler reverts styling with `SessionMaintenance.ButtonLeave`.

```cs
        // Save Button ------------------------------------------------------------------------------------------------
        private void btnSave_MouseEnter(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonEnter(btnSave);
        }

        private void btnSave_MouseLeave(object sender, EventArgs e)
        {
            SessionMaintenance.ButtonLeave(btnSave);
        }
```

---

## 7.4 - Methods - Button Click Events

### 7.4.2 - Exit Button

The `btnExit_Click` method handles the event when the user clicks the Exit button on the `BoxManager` form. It logs the action for auditing or diagnostics and then closes the form.

#### Signature:

```cs
private void btnExit_Click(object sender, EventArgs e)
```

#### Breakdown:
* Log the Exit Action:
    * Records the form closure in the application's logbook.
    * Useful for audit trails, user tracking, and debugging.
    * Includes context tags such as the form name and the method.

```cs
SessionMaintenance.LogBook("", "[BoxManager]", "[btnExit_Click]", $"Exiting Box Manager");
```

* Close the Form:
    * Terminates the `BoxManager` form.
    * Control is returned to the parent form, if applicable.

```cs
this.Close();
```

#### Summary:
The `btnExit_Click` method handles the button click event for the Exit button. It logs the exit event with context to and closes the current form of the session log, helping to maintain clear session tracking and a smooth user experience.

```cs
 // Exit Button ----------------------------------------------------------------------
        private void btnExit_Click(object sender, EventArgs e)
        {
            SessionMaintenance.LogBook("", "[BoxManager]", "[btnExit_Click]", $"Exiting Box Manager");
            this.Close();
        }
```

---

### 7.4.3 - Save Button
The `btnSave_Click` method is the event handler for the "Save" button in a Windows Forms application. It validates user input in a `DataGridView`, ensures data integrity, saves the contents to a SQL Server database, and then triggers any follow-up actions (such as label printing). This method is part of the data entry and persistence workflow for box management within the application.

#### Signature:

```cs
private void btnSave_Click(object sender, EventArgs e)
```

#### Breakdown:
* Iterate Through `DataGridView` Rows
    * The method begins by looping through each row in the `dgContents` grid, ignoring any new (empty) rows that have not yet been edited.

```cs
 foreach (DataGridViewRow row in dgContents.Rows)
```

* Validate `BoxNo` ComboBox Cell
    * For each valid row, it attempts to retrieve the value of the `BoxNo` cell, which is expected to be a `DataGridViewComboBoxCell`. If this cell is null, has no value, or contains an empty string, an error message is shown using a custom message box, and the method exits early without saving.

```cs
 var cell = row.Cells["BoxNo"] as DataGridViewComboBoxCell;
                if (cell == null || cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowError("Please ensure all BoxNo fields are filled.");
                    return;
                }
```

* Convert `DataGridView` to `DataTable`
    * Assuming all `BoxNo` cells are valid, the method calls `SaveDataGrid()` to transform the grid contents into a `DataTable`.

```cs
DataTable dt = SaveDataGrid();
```

* Check for Empty Data
    * If the resulting `DataTable` contains no rows, an error message is displayed indicating there is no data to save, and the method exits early.

```cs
if (dt.Rows.Count == 0)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("No data to save. Please ensure the DataGridView is populated.");
                return;
            }
```

* Insert Data into SQL Server
    * If the data is valid and not empty, the `InsertDataGrid` method of the `TcarControl` class is called to insert the data into a SQL Server database, along with the `userName` to track who performed the operation.

```cs
TcarControl.InsertDataGrid(dt, userName);
```

* Log the Save Action
    * A log entry is made using the `SessionMaintenance.LogBook` method to record the successful save operation, referencing the current `tcarRef`.

```cs
SessionMaintenance.LogBook("", "[BoxManager]", "[btnSave_Click]", $"Data saved successfully for TCAR Ref: {tcarRef}");
```

* Trigger Save Event
    * If any components are subscribed to the `OnBoxDataSaved` event, the method invokes this delegate, passing in the saved `DataTable`.

```cs
 OnBoxDataSaved?.Invoke(dt);
```

* Close the Form
    * Finally, the form is closed, concluding the save process and allowing any post-save actions (e.g., printing) to continue.

```cs
this.Close();
```

#### Summary:
The `btnSave_Click` method ensures that user input is complete and valid before saving DataGridView content to a SQL Server database. It includes input validation, data transformation, database insertion, logging, and event triggering. Its primary purpose is to persist box-related data in a structured and secure manner while providing user feedback and maintaining application state integrity.


```cs
 // Save Button -----------------------------------------------------------------------
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check all comboboxes have a value
            foreach (DataGridViewRow row in dgContents.Rows)
            {
                if (row.IsNewRow) continue;

                var cell = row.Cells["BoxNo"] as DataGridViewComboBoxCell;
                if (cell == null || cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowError("Please ensure all BoxNo fields are filled.");
                    return;
                }
            }

            // Save Data from DataGridView to DataTable
            DataTable dt = SaveDataGrid();
            if (dt.Rows.Count == 0)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowError("No data to save. Please ensure the DataGridView is populated.");
                return;
            }
            // Insert Data from DataTable to SQL Server
            TcarControl.InsertDataGrid(dt, userName);

            SessionMaintenance.LogBook("", "[BoxManager]", "[btnSave_Click]", $"Data saved successfully for TCAR Ref: {tcarRef}");

            OnBoxDataSaved?.Invoke(dt);

            this.Close();
            // carry on printing label// 
        }
```
