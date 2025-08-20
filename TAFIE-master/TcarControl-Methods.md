[Back](ClassIndex.md)

# TAFIE Carrier Module - TcarControl - Methods

## 6.1 - Methods - SQL Data Tasks

### 6.1.1 - GetHeaderDetails

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
                SessionMaintenance.LogBook("ERROR", "[TcarControl]", "[GetHeaderDetails]", $"FAILED: Code 226 ( {ex.Message} )");
            }
        }
```

---

### 6.1.2 - GetDelDetails

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
```

---

### 6.1.3 - GetComp

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
```

---

### 6.1.4 - RecalculateBoxes

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
```

---

### 6.1.5 - CompleteTCAR

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
```

---

### 6.1.6 - CheckLoadNote

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
// Check Load Note Exists------------------------------------------------------------------------------------------------------
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
                SessionMaintenance.LogBook("ERROR", "[TcarControl]", "[CheckLoadNote]", $"FAILED: Code 109 ( {ex.Message} )");
            }

            return check;
        }
```

---

### 6.1.7 - GetBoxes

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
```

---

### 6.1.8 - InsertDataGrid

The `InsertDataGrid` method is responsible for processing a `DataTable` containing box content records and inserting its data into the SQL Server database. It first clears any existing records associated with the current transaction reference (`TCAR_Ref`), then adds each new record from the `DataTable` using the `TCAR_Insert_New_BCON` stored procedure. Finally, it triggers a postage split calculation via the `TCAR_Split_Post` procedure.

This method is crucial for refreshing the database state with updated box contents, following user input or data editing with the box manager form.

#### Input Parameter:

* `dt`: The collection of box content data to be inserted. Each row represents a box.
* `user`: The username of the operator performing the data insert.

#### Clear Existing Records

* Executes a multi-statement SQL command to remove any existing `TCAR_BOXES` and `TCAR_BCON` entries for the given `TCAR_Ref`. This ensures the insert is clean and idempotent.

```cs
string queryDel = "DELETE TCAR_BOXES WHERE TCAR_Ref = @TCAR_Ref; DELETE TCAR_BCON WHERE TCAR_Ref = @TCAR_Ref";
```

#### Insert Each Row from DataTable

* Iterates over each `DataRow` in the provided `DataTable`.
* Validates the `BoxNo` field. If parsing fails, the row is skipped and logged.
* For valid rows, executes the `TCAR_Insert_New_BCON` stored procedure with the box number, part code, quantity, and user.
* Parameterized SQL commands are used to prevent injection and ensure safe execution.

```cs
if (!int.TryParse(row["BoxNo"]?.ToString(), out int boxNo))
{
    SessionMaintenance.LogBook("ERROR", "[TcarControl]", "[InsertDataGrid]",
        $"Skipping row due to invalid BoxNo: '{row["BoxNo"]}'");
    continue;
}
```

#### Postage Split Calculation

* After all inserts, the method runs `TCAR_Split_Post` to recalculate how postage is distributed across the new records.

```cs
string querySplit = "EXECUTE [TCAR_Split_Post] @TCAR_Ref";
```

#### Error Handling

* Any exception thrown during connection, command execution, or data iteration is caught and handled gracefully.
	* A custom error message is shown to the user via a message box.
	* A detailed error message is logged for diagnostics using `LogBook`, tagged with error code `118`.

```cs
SessionMaintenance.LogBook("ERROR", "[TcarControl]", "[InsertDataGrid]", $"FAILED: Code 118 ( {ex.Message} )");
```

#### Summary

The `InsertDataGrid` validats box data from a `DataTable` into the database for the current shipment, replacing any existing records and recalculating postage.


```cs
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
```

---

### 6.1.9 - InsertPayload

The `InsertPayload` method is a public static method that accepts three parameters: `payLoad` (a string), `tKey` (a string), and `mode` (an integer). This method is responsible for inserting the API Payloads data into the database by executing a stored procedure that saves the provided XML or JSON string along with relevant information such as the `TCAR` reference, user details, and operation mode.

#### Input Parameters:

* `payLoad`: This string parameter represents the API payload data that will be inserted into the database. It could be the XML or JSON response from the API or any other XML/JSON-related data used by the application.
* `tKey`: This string parameter is used as a key to identify the data being inserted. It's typically tied to a specific record in the system, such as a shipment or label.
* `mode`: This integer parameter determines the mode or operation type when inserting the payload data. Different modes are assigned depending on whether the API call was a success or not.

#### SQL Query Execution:

* The method constructs an SQL query that calls the stored procedure `[TCAR_INSERT_XML]` with the following parameters:
    * `@tkey`: The key that uniquely identifies the data to be inserted.
    * `@TCAR_Ref`: The `TCAR` reference, which is a global value that identifies the shipment label request in the application.
    * `@User`: The username of the user performing the operation, retrieved from the userName global property.
    * `@Payload_String`: The Payload string that will be stored in the database.
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

* If the insert operation is successful, a log entry is created using `SessionMaintenance.LogBook` to record the XML/JSON insertion along with the `mode` and `tKey` for reference. This helps in tracking the insertion process and identifying which data was inserted.

```cs
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
                 cmd.Parameters.AddWithValue("@User", userName);
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
```
---

## 6.2 - Methods - API Helpers

### 6.2.1 - CreateTKey

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
      // Create tKey -------------------------------------------------------------------------------------------------------------
        public static string CreateTKey(string name)
        {
            string tKeySufix = name.Length >= 2 ? name.Substring(0, 2).ToUpper() : name.ToUpper();
            string tKeyTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string tKey = tKeyTimeStamp + tKeySufix;
            return tKey;
        }

``` 

---

### 6.2.2 - OpenURL

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
```

---

### 6.2.3 - PrintLables

The `PrintLabels` method provides a flexible and scalable way to generate and print labels for multiple packages or "boxes" that are part of a single shipment. Instead of embedding the entire label generation and printing process directly within this method, it accepts a delegate (`printMethod`) that encapsulates the logic for processing an individual box. This design promotes modularity, reusability, and allows for different label printing workflows (e.g., for different carriers or scenarios) to be passed into the same generic printing orchestrator.

#### Function Signature:

* `public static`: Indicates that the method can be called directly from anywhere in the application using the class name (`TcarControl.PrintLabels`) and does not require an instance of `TcarControl`.
* `async Task`: Signifies that this method performs asynchronous operations and will return a `Task`, allowing the calling code to `await` its completion without blocking the main application thread. This is crucial for maintaining UI responsiveness in "TAFIE."
* `List<int> boxRefs`: The first parameter is a list of integer references, where each integer represents an identifier for a box that needs a label. This list defines the scope of boxes to be processed.
* `Func<int, int, Task> printMethod`: This is the most significant parameter. It's a delegate (specifically a Func delegate) that represents a method that:
	* Takes two int arguments (the current `boxRef` and the total `boxCount`).
	* Returns a `Task` (meaning it's an asynchronous method).
	* This delegate allows `PrintLabels` to call any method that matches this signature, effectively decoupling the "what to print" logic from the "how to iterate and print for all" logic.

```cs
public static async Task PrintLabels(List<int> boxRefs, Func<int, int, Task> printMethod)
```

#### Step-by-Step Breakdown

* Initialization:
	* The total number of boxes to be processed is determined by getting the count of items in the `boxRefs` list. This count is useful for displaying progress to the user (e.g., "1 of 5 labels printed").

```cs
int boxCount = boxRefs.Count;
```

* Iterating Through Boxes:
	* The method iterates through each `boxRef` (individual box identifier) in the provided `boxRefs` list.

```cs
foreach (var boxRef in boxRefs)
```

* Asynchronous Execution of Print Logic:
	* Inside the loop, for each `boxRef`, the `printMethod` delegate is invoked. The current `boxRef` and the overall `boxCount` are passed as arguments to this delegated method. The `await` keyword pauses the execution of the `PrintLabels` method until the `printMethod` (e.g., `GetLabelPhub`) for the current `boxRef` has completed its asynchronous operation. This ensures that labels are processed sequentially for each box.

```cs
await printMethod(boxRef, boxCount);
```

#### Summary
The `PrintLabels` method within "TAFIE"'s `TcarControl` class provides a powerful and flexible mechanism for batch processing of shipping label generation. By accepting a delegate (`printMethod`) that encapsulates the intricate details of fetching and printing a label for a single box, it abstracts the high-level orchestration of the process. This design promotes code reusability, simplifies the management of different label generation workflows (e.g., `GetLabelPhub` for a specific carrier), and ensures that "TAFIE" can efficiently handle the requirements of a warehouse needing to produce multiple labels for various packages.

```cs
// Print Labels for all Boxes --------------------------------------------------------------------
        public static async Task PrintLabels(List<int> boxRefs, Func<int, int, Task> printMethod)
        {
            int boxCount = boxRefs.Count;
            foreach (var boxRef in boxRefs)
            {
                await printMethod(boxRef, boxCount);
            }
        }
```

---

### 6.2.4 - PrintPdf

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
```
