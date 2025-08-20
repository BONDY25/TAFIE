[Back](ClassIndex.md)

# TAFIE - SessionMaintenance - Methods

## 9.1 - Methods

### 9.1.1 - Initialization and declaration

The `SessionMaintenance` class is declared as `internal static`, signifying its role as a utility class whose members can be accessed directly without creating an instance of the class, and primarily within the same assembly. The variables declared within this class are all `public static`, making them globally accessible throughout the "TAFIE" application. These variables are fundamental for managing application settings, user session details, UI aesthetics, and backend connectivity, underpinning the robust and consistent operation of "TAFIE" in a fulfillment warehouse environment.

#### Global Variable Declarations

* `public static int debugMode = 0;`
    * Type: `int`
    * Accessibility: `public static`
    * Purpose: This variable serves as a flag to control the application's debugging behavior. A value of `0` indicates that debug mode is off. When `debugMode` is set to `1`, the application ignores the version number logic, allowing updates or changes to be tested without impacting live versions. This is invaluable for troubleshooting issues during development or in a controlled testing environment without requiring recompilation.

* `public static string userName { get; set; }`
    * Type: `string`
    * Accessibility: `public static`
    * Purpose: This static property is designed to store the `userName` of the currently logged-in operator using "TAFIE." Being `static`, it provides a single, globally accessible point to retrieve the username from anywhere in the application. This is crucial for logging user actions (as seen with `SessionMaintenance.LogBook`), personalizing the UI, and associating operations with specific personnel for auditing purposes.

* `public static string sessionId { get; set; }`
    * Type: `string`
    * Accessibility: `public static`
    * Purpose: Similar to `userName`, this static property holds a unique identifier for the user's current session within "TAFIE." This `sessionId` can be utilized across various forms and backend processes to maintain context, track sequential user activity, or link actions to a specific session in the application's audit logs. Its global accessibility ensures consistency across the application.

* `public static Color accentColor = Color.FromArgb(11, 159, 161);`
    * Type: `Color`
    * Accessibility: `public static`
    * Purpose: This variable defines the primary `accentColor` used throughout "TAFIE"'s user interface. Initialized with an RGB value (11, 159, 161), this color is applied to highlight interactive elements, provide focus cues (as seen in `SessionMaintenance.ControlEnter`), or serve as a key element of the application's branding and visual identity. Centralizing this color promotes a consistent and professional aesthetic.

    * `public static Color backColor = Color.Black;`
    * Type: `Color`
    * Accessibility: `public static`
    * Purpose: This variable defines the primary `backColor` for various UI elements in "TAFIE," initialized to `Color.Black`. It serves as the standard background color for controls or forms, contributing to the application's overall color scheme and visual consistency.

* `public static Color foreColor = Color.DarkGray;`
    * Type: `Color`
    * Accessibility: `public static`
    * Purpose: This variable defines the primary `foreColor` (foreground color, typically text color) for UI elements, initialized to `Color.DarkGray`. It is used for text and other foreground elements, ensuring readability against the `backColor` and maintaining a consistent visual style across the "TAFIE" interface.

* `public static string currentVersion = "1.3.1";`
    * Type: `string`
    * Accessibility: `public static`
    * Purpose: This string holds the `currentVersion` number of the "TAFIE" application. This global variable is invaluable for:
        * Logging the application version for debugging and support purposes.
        * Being used in version control mechanisms or update checks.
        * Ensuring that warehouse operators and support staff are always using the latest version of the application.

* `public static string appName = "TAFIE";`
    * Type: `string`
    * Accessibility: `public static`
    * Purpose: This string stores the `appName`, "TAFIE". Centralizing the application's name.

* `public const string connectionString = "Server=SQL-SSRS;Database=Appz;Integrated Security=True;Encrypt=False;";`
    * Type: `string`
    * Accessibility: `public const`
    * Purpose: This is a critical configuration constant. It defines the database connection string that all parts of "TAFIE" use to connect to the `Appz` database.
        * `public static`: Makes it globally accessible.
        * `const`: Ensures its value is fixed at compile-time and cannot be changed during runtime, guaranteeing that "TAFIE" always attempts to connect to the same designated database server (`SQL-SSRS`) and database (`Appz`).
        * Its presence here, means `SessionMaintenance` acts as the central repository for database connection details, making it easier to manage and update across the entire application.

* `public static int factCount = 0;`
    * Type:`int`
    * Accessibility: `public static`
    * Purpose: This variable is explicitly part of an "Easter egg" function. It serves as a counter to track how many interactions with the easter egg have occurred.

#### Summary

The global variables declared within the `SessionMaintenance` class form the backbone of "TAFIE"'s application-wide configuration and state management. They encompass essential elements such as debugging controls (`debugMode`), user session context (`userName`, `sessionId`), comprehensive UI styling (`accentColor`, `backColor`, `foreColor`), application metadata (`currentVersion`, `appName`), and, crucially, the centralized database `connectionString`. Additionally, `factCount` contributes to a minor, non-critical "Easter egg" feature. Their `public static` accessibility ensures that these vital parameters are consistently available and applied throughout the "TAFIE" application, contributing significantly to its stability, maintainability, and user experience for warehouse operators.

```cs
internal class SessionMaintenance
    {
        public static int debugMode = 0;
        public static string userName { get; set; }
        public static string sessionId { get; set; }

        public static Color accentColor = Color.FromArgb(11, 159, 161);
        public static Color backColor = Color.Black;
        public static Color foreColor = Color.DarkGray;

        public static string currentVersion = "1.3.1"; // Current Version of the Application
        public static string appName = "TAFIE"; //Name of the Application

        public const string connectionString = "Server=SQL-SSRS;Database=Appz;Integrated Security=True;Encrypt=False;";

        public static int factCount = 0;
```

---

### 9.1.2 - LogBook

The `LogBook` method is paramount to the operational transparency and debugging capabilities of "TAFIE." In a production environment like a fulfillment warehouse, comprehensive logging is essential for tracking user activities, monitoring application health, diagnosing issues, and providing an audit trail. This method standardizes how events are recorded, ensuring consistency across different forms and functionalities within "TAFIE" by capturing details such as the user, session, specific form, event type, and detailed notes, all timestamped and linked to a unique transaction key. Its robust error handling also ensures that even if the primary database logging fails, critical error information is still captured via a file-based fallback.

#### Step-by-Step Breakdown

* Method Signature and Parameters:
   * `public static`: Makes the method globally accessible without needing a `SessionMaintenance` object, facilitating easy logging from any part of "TAFIE."
   * `void`: The method does not return any value; its purpose is to perform a logging action.
   * `string tKey`: A transaction key that can be pre-provided (e.g., to link related log entries) or generated by the method if empty or "ERROR".
   * `string Form`: The name of the form or class from which the log entry originates (e.g., "[ClientManager]", "[CarrierForm]").
   * `string Event`: The method, specific event or action being logged (e.g., "[FormLoad]", "[SaveClient]", "[API Call]").
   * `string notes`: Detailed information or messages associated with the event.
   * `Label? label = null`: An optional parameter. If a `Label` control is provided, it will be updated after a successful log entry, providing direct UI feedback.

```cs
public static void LogBook(string tKey, string Form, string Event, string notes, Label? label = null):
```

* Session ID Check:
   * `CheckSessionID(sessionId);`: This line calls another method within `SessionMaintenance` to ensure that the global `sessionId` variable is valid or initialized. This is crucial for correctly associating log entries with the current user session.

* Variable Initialization for Logging:
   * The method defines the SQL query to execute a stored procedure named `[Appz - Insert_LogBook]`. This procedure is responsible for inserting the log data into the database, accepting several parameters.
   * It captures the full domain user (`Domain_User`) by combining the machine name and the current Windows username. This provides robust identification of the source of the log entry.
   * A suffix for the `tKey` is generated using the first two (or fewer, if shorter) uppercase characters of the current username. This adds user-specific context to the transaction key.
```cs
string query = $"[Appz - Insert_LogBook] @tKey, @Session_Id, @Domain_User, @User_Created, @Form, @Event, @Notes, @Application";

string user = $"{Environment.MachineName}.{Environment.UserName}";

string tKeySufix = Environment.UserName.Length >= 2 ? Environment.UserName.Substring(0, 2).ToUpper() : Environment.UserName.ToUpper();  
```

* Transaction Key (`tKey`) Generation Logic:
   * If no `tKey` is provided by the caller, a new one is generated using the current timestamp (year, month, day, hour, minute, second, millisecond) concatenated with the `tKeySufix`.
   * If the `tKey` is explicitly "ERROR", a new one is generated with the timestamp and suffix, but with an additional "-ER" appended. This clearly marks error-related transaction keys.

```cs
if (string.IsNullOrEmpty(tKey))
            {
                tKey = DateTime.Now.ToString("yyyyMMddHHmmssfff") + tKeySufix;
            }
            else if (tKey == "ERROR")
            {
                tKey = DateTime.Now.ToString("yyyyMMddHHmmssfff") + tKeySufix + "-ER";
            }
```

* Database Logging (Primary `try` Block):
   * The method establishes a connection to the `Appz` database using the globally defined `connectionString` from `SessionMaintenance`. The using statement ensures proper resource disposal.
   * `conn.Open();`: Opens the database connection.
   * A `SqlCommand` is created to execute the `[Appz - Insert_LogBook]` stored procedure.
      * `cmd.Parameters.AddWithValue(...)`: Each parameter required by the stored procedure (`@tKey`, `@Session_Id`, `@Domain_User`, `@User_Created`, `@Form`, `@Event`, `@Notes`, `@Application`) is added with its corresponding value. Notably, `SessionMaintenance.sessionId`, `SessionMaintenance.userName`, and `SessionMaintenance.appName` global variables are used here, reinforcing the class's role as a central information hub.
   * The stored procedure is executed. `ExecuteNonQuery` is used as no data is expected to be returned, only an insertion operation is performed.
   * `conn.Close();`: The database connection is explicitly closed.
   * UI Status Update (Optional):
      * If an optional `Label` control was provided, the `UpdateStatusLabel` method is called. This allows for real-time status updates on the UI.

```cs
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
```

* Robust Error Handling (Primary `catch` Block):
   * Catches any exceptions that occur during the primary database logging attempt.
   * Fallback File Logging:
      * A detailed error message is constructed, including timestamp, parameters, and the exception details.
      * A file path is generated dynamically to store the error log on a network drive (`\\elucid9\elucid\Apps\Error_Logs`), named uniquely with the app name, `tKey`, and user.
      * A nested `try-catch` block attempts to write the constructed error message to the specified file path. This is a critical fallback mechanism; even if the database is unreachable, these severe errors are still recorded. If even file writing fails, a `CustomMessageBox` with error code "106" is shown.
   * User Notification and Application Exit:
      * This `finally` block executes regardless of whether the file write succeeded or failed. It displays a `CustomMessageBox` with a generic error code "102" and the original exception message to the user, and then forcibly exits the application. This aggressive approach indicates that a failure to log a message (especially critical ones) is deemed a fatal error, preventing "TAFIE" from running in a state where crucial events might not be recorded.

```cs
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
```

#### Summary

The `LogBook` method is a cornerstone of the `SessionMaintenance` class and the "TAFIE" application's operational integrity. It provides a robust, centralized mechanism for capturing detailed log entries to a database, encompassing user actions, system events, and application status. Its intelligent `tKey` generation, use of global session/user parameters, and optional UI feedback contribute to a comprehensive logging solution. Crucially, its sophisticated error handling, including a critical file-based fallback logging system and an immediate application exit on failure, underscores its importance in ensuring that "TAFIE" maintains a thorough audit trail and diagnostic capability, even in adverse circumstances within a busy fulfillment warehouse environment.

```cs
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
```

---

### 9.1.3- GetSessionID

The `GetSessionID` method plays a critical role in "TAFIE" by ensuring that each running instance of the application has a distinct identifier. This session ID is crucial for tracking individual user sessions, auditing activities, and linking all subsequent log entries (via `LogBook`) to a specific application run. By recording this ID along with the domain user and application name in the database, "TAFIE" maintains a comprehensive record of application usage, vital for troubleshooting, performance monitoring, and compliance in a busy fulfillment warehouse environment.

#### Step-by-Step Breakdown

* Method Signature:
   * `public static`: Makes the method globally accessible without needing an object instance, allowing any part of "TAFIE" to request a session ID.
   * `string`: The method returns the generated session ID as a string.

```cs
public static string GetSessionID():
```

* Information Gathering for Session ID Generation:
   * Retrieves the current user's operating system username (e.g., `JSMITH`) and converts it to uppercase.
   * Retrieves the current machine's hostname (e.g., `WAREHOUSEPC01`) and converts it to uppercase.
   * Captures the precise current date and time down to milliseconds, formatted as a string. This ensures high uniqueness and chronological order.
   * Initializes a new `Random` number generator.
   * Generates a random integer between 1000 (inclusive) and 10000 (exclusive), ensuring a four-digit number.
   * Converts the generated random number to its string representation.

```cs
string domainUser = Environment.UserName.ToUpper();
string machine = Environment.MachineName.ToUpper();
string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
string query = $"EXECUTE [Appz - Insert_Update_Session] @Session_Id, @Domain_User, @Application";

Random random = new Random();

// Generate a random number between 1000 and 9999
int randomNumber = random.Next(1000, 10000);
// Convert the random number to a string and use it
string randomNumberString = randomNumber.ToString();
```

* SQL Query Definition:
   * Defines the SQL command to execute a stored procedure named `[Appz - Insert_Update_Session]`. This procedure is designed to insert a new session record, taking parameters for the session ID, domain user, and application name.

```cs
string query = $"EXECUTE [Appz - Insert_Update_Session] @Session_Id, @Domain_User, @Application";
```

* Session ID Construction:
   * Concatenates the collected information (machine name, random number, domain user, and timestamp) into a single, highly unique `sessionId` string. The combination of these elements makes collisions extremely improbable.

```cs
string sessionId = $"{machine}{randomNumberString}{domainUser}{timeStamp}";
```

* Database Interaction (Try-Catch Block):
   * The entire database operation is enclosed in a `try-catch` block to manage potential errors gracefully.
   * The method creates and manages a database connection using the globally defined `connectionString` from the `SessionMaintenance` class.
   * `conn.Open();`: Opens the connection to the `Appz` database.
   * A `SqlCommand` is created for the stored procedure with the following parameters:
      * Adds the newly generated `sessionId`.
      * Adds the `domainUser`.
      * Adds the global `appName` from `SessionMaintenance`.
   * The method executes the stored procedure. `ExecuteNonQuery` is used as no data is returned, only an insertion/update is performed.
   * `conn.Close();`: Closes the database connection.

```cs
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
```

* Error Handling (Catch Block):
   * If any exception occurs during the database interaction (e.g., connection issues, SQL errors), this block is executed.
   * The block resets the mouse cursor to its default state, ensuring it's not stuck as a "wait" cursor.
   * Displays a `CustomMessageBox` with a predefined error code "104" and the specific exception message, informing the user about the failure to obtain or record the session ID.
   * Critically, it calls the `LogBook` method (within `SessionMaintenance` itself) to log this severe error to the fallback logging system (as seen in `LogBook`'s description), ensuring that the failure to establish a session is recorded.
   * Forces the application to exit. This indicates that failing to generate and record a session ID is considered a critical error that prevents "TAFIE" from operating correctly or being auditable.

```cs
 catch (Exception ex) // Catch any errors
            {
                Cursor.Current = Cursors.Default;
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("104", $"\n{ex.Message}");
                LogBook("ERROR", "[SessionMaintenance]", "[GetSessionID]", $"FAILED: Code 104 ( {ex.Message} )");
                Application.Exit();
            }
```

* Return Value:
   * After successfully generating and recording the session ID in the database, the method returns the generated `sessionId` string to the caller, allowing it to be stored in `SessionMaintenance.sessionId` for global use.

```cs
return sessionId;
```

#### Summary

The `GetSessionID` method is a foundational component of "TAFIE"'s auditing and session management capabilities. It programmatically generates a robustly unique identifier for each application instance, incorporating machine, user, and time-based elements. This `sessionId` is then immediately inserted to the `Appz` database via a stored procedure, providing a crucial record of application launches. The method's aggressive error handling, which includes logging the failure and immediately exiting the application, underscores the paramount importance of ensuring that every "TAFIE" session is properly accounted for, which is vital for operational transparency and debugging in a demanding warehouse environment.

```cs
// Get Session ID -----------------------------------------------------------------------------------
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
```

---

### 9.1.4 - CheckSessionID

The `CheckSessionID` method serves as a critical security and integrity control point in "TAFIE." Its primary purpose is to confirm that the `sessionId` currently in use by the application instance corresponds to a single, valid, and active session record within the `Appz` database. This validation step is essential for ensuring that "TAFIE" sessions are properly tracked and that operations are performed under a legitimate and recorded application context. Failure to validate a session through this check results in immediate termination of the application, reinforcing the importance of session traceability in a warehouse environment.

#### Step-by-Step Breakdown
* Method Signature:
   * `public static`: Makes the method globally accessible.
   * `void`: The method does not return a value; its purpose is to perform a check and potentially trigger an action (like exiting the application) if the check fails.
   * `string sessionId`: Takes the session ID string to be validated as a parameter. This would typically be the `SessionMaintenance.sessionId` global variable.

```cs
public static void CheckSessionID(string sessionId)
```

* SQL Query Definition:
   * Defines an SQL query that counts the number of rows in a database table where the `Session_Id` column matches the provided `@Session_Id` parameter.
   * The table name is dynamically constructed using the global `appName` variable (e.g., `TAFIE_Sessions`). This query verifies if exactly one record exists for the given session ID.

```cs
string query = $"SELECT COUNT(*) FROM {appName}_Sessions WHERE Session_Id = @Session_Id";
```

* Database Interaction (Try-Catch Block):
   * The entire database operation is encapsulated within a `try-catch` block for robust error handling.
   * The method establishes a database connection using the globally defined `connectionString` from `SessionMaintenance`. The using statement ensures the connection is properly closed.
   * `conn.Open();`: Opens the connection to the `Appz` database.
   * A `SqlCommand` is created. With the following parameter:
      * The session ID passed to the method is added as a parameter to the SQL query, preventing SQL injection.
   * The method executes the query. `ExecuteScalar()` is used because the query returns a single scalar value (the count of matching rows). The result is cast to an int.

```cs
using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Session_Id", sessionId);
                        int count = (int)cmd.ExecuteScalar();
```
       
* Session ID Validation Logic:
   * This is the core validation. The method expects exactly `1` row to match the `sessionId`, indicating that the current session is uniquely active and recorded.
   * If `count` is not `1` (meaning zero records found, or unexpectedly, more than one record found for the same ID):
      * A `CustomMessageBox` is displayed to the user with a predefined error code "166", indicating a critical session integrity issue.
      * The method calls another method (`ClearSessionID`), which is responsible for attempting to remove the problematic session ID from the database.
      * The method then forces the application to immediately terminate. This drastic action is a security measure, preventing the application from operating under an invalid or compromised session, ensuring data integrity and system control in a production environment.

```cs
 if (count != 1)
 {
     CustomMessageBox messageBox = new CustomMessageBox();
     messageBox.ShowDefError("166", $"");
     ClearSessionID(sessionId);
     Application.Exit();
 }
```

* Error Handling (Catch Block):
   * Catches any exceptions that occur during the database interaction (e.g., connection issues, SQL query errors).
   * Displays a `CustomMessageBox` with a predefined error code "122" and the specific exception message, informing the user about the database error that prevented the session check.
   * Logs this critical error using the `LogBook` method, ensuring that the failure to check the session is recorded for debugging and auditing purposes.

```cs
 catch (Exception ex)
{
    CustomMessageBox messageBox = new CustomMessageBox();
    messageBox.ShowDefError("122", $"\n{ex.Message}");
    LogBook($"ERROR", "[SessionMaintenance]", "[CheckSessionID]", $"FAILED (  {ex.Message}  )");
}
```

#### Summary
The `CheckSessionID` method is a vital safeguard within "TAFIE"'s `SessionMaintenance` class. It enforces session integrity by querying the database to ensure that the application's current `sessionId` is valid and uniquely registered. This proactive validation is critical for maintaining an auditable trail of all application activities. Should the session ID not be found (or be duplicated), "TAFIE" responds with an immediate, definitive action: logging the failure, attempting to clear the invalid session, and terminating the application. This strict approach ensures that "TAFIE" operates only under verified contexts, enhancing its reliability and security in a demanding warehouse operation.

```cs
 // Check Session -------------------------------------------------------------------------------------
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
```

---

### 9.1.5 - ClearSessionID
The `ClearSessionID` method is a vital component of "TAFIE"'s session management framework. Its primary purpose is to ensure that database records corresponding to a particular application session are properly cleaned up or invalidated. This is especially crucial in scenarios where a session is deemed invalid (as seen in `CheckSessionID`) or when an application instance is terminated abnormally. By actively removing outdated or problematic session entries, `ClearSessionID` helps maintain the integrity and cleanliness of "TAFIE"'s session trackin, which is important for accurate auditing and resource management in a warehouse environment.

#### Step-by-Step Breakdown

* Method Signature:
   * `public static`: Makes the method globally accessible.
   * `void`: The method does not return a value; its purpose is to perform a database deletion operation.
   * `string sessionId`: Takes the specific session ID string to be cleared/deleted from the database as a parameter.

```cs
public static void ClearSessionID(string sessionId):
```

* SQL Query Definition:
   * Defines an SQL `DELETE` statement. It targets the `_Sessions` table (e.g., `TAFIE_Sessions`) whose name is dynamically constructed using the global `appName` variable.
   * The `WHERE` clause ensures that only the record corresponding to the provided `sessionId` is deleted, preventing unintended data loss.

```cs
string query = $"DELETE {appName}_Sessions WHERE Session_Id = @Session_Id";
```

* Database Interaction (Try-Catch Block):
   * The entire database operation is encapsulated within a `try-catch` block to handle potential errors.
   * The method establishes a database connection using the globally defined `connectionString` from `SessionMaintenance`. The using statement guarantees proper disposal of the connection resource.
   * `conn.Open();`: Opens the connection to the `Appz` database.
   * A `SqlCommand` is created with the `DELETE` query.:
      * The `sessionId` to be cleared is added as a parameterized value, preventing SQL injection vulnerabilities.
   * The method executes the `DELETE` command. `ExecuteNonQuery()` is used as the command performs a data modification and does not return any data rows.
   * `conn.Close();`: Closes the database connection.

```cs
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
```

* Error Handling (Catch Block):
   * Catches any exceptions that occur during the database deletion process (e.g., connection failures, database errors).
   * Displays a `CustomMessageBox` with a predefined error code "111" and the specific exception message, informing the user about the failure to clear the session.
   * Logs this error using the `LogBook` method. This is crucial as a failure to clear a session might indicate persistent database issues or a problem with the `sessionId` itself, requiring further investigation.

```cs
  // Catch Errors
            catch (Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("111", $"\n{ex.Message}");
                LogBook($"ERROR", "[SessionMaintenance]", "[ClearSessionID]", $"FAILED (  {ex.Message}  )");
            }
```

#### Summary

The `ClearSessionID` method is an integral part of "TAFIE"'s robust session management within the `SessionMaintenance` class. Its sole purpose is to remove specified session records from the database, primarily used to clean up invalid or terminated application sessions. By ensuring that only active and valid sessions are tracked, this method contributes to the accuracy of "TAFIE"'s auditing capabilities and helps maintain the integrity of its operational data. The inclusion of error logging ensures that any failures in clearing sessions are recorded for diagnostics, underscoring the importance of this cleanup function in a critical application context.

```cs
// Clear Session ---------------------------------------------------------------------
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
```

---

### 9.1.6 - CheckVersion

The `CheckVersion` method is a vital maintenance and control mechanism in "TAFIE." In a dynamic warehouse environment where software updates are periodic, it's essential to prevent users from operating with outdated or unsupported versions that might contain bugs, lack new features, or integrate incorrectly with updated APIs or database schemas. This method performs a critical check at application startup by comparing the application's current compiled version against the latest approved version recorded in the database. If a discrepancy is found, it forces the application to terminate, ensuring all operations are performed on a consistent and supported software base.

#### Step-by-Step Breakdown

* Method Signature:
   * `public static`: Makes the method globally accessible without requiring an object instance, allowing it to be called early in the application's lifecycle.
   * `void`: The method does not return a value; its primary purpose is to perform a check and potentially trigger an application exit.

```cs
public static void CheckVersion():
```

* Variable Declaration and SQL Query Definition:
   * Declares a local variable to store the version retrieved from the database.
   * Defines the SQL query to retrieve the latest version. It selects the `[Version]` from a table named `VERSION_CTRL`.
   * Filters results to only the current "TAFIE" application.
   * Ensures that the most recently updated version for the application is retrieved.
   * Guarantees only one record is fetched, representing the absolute latest version.

```cs
// Declare Variables
string latestVersion = null;
string query = $"SELECT TOP 1 [Version] FROM VERSION_CTRL WHERE [Application] = '{appName}' ORDER BY Last_Updated DESC";
```

* Retrieve Latest Version from Database (Primary `try` Block):
   * The database interaction is enclosed in a `try-catch` block.
   * The method establishes a connection to the `Appz` database using the global `connectionString` from `SessionMaintenance`.
   * `conn.Open();`: Opens the database connection.
   * A `SqlCommand` is created to execute the query.
   * The method executes the query and obtains a `SqlDataReader` to read the results.
   * It then checks if a row was returned by the query.
   * If a row is found, the value from the "Version" column is read and stored in the `latestVersion` variable.
   * `conn.Close();`: Closes the database connection.

```cs
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
```

* Database Connection Error Handling (Primary `catch` Block):
   * Catches any exceptions that occur during the database query (e.g., connection failures, table not found).
   * Resets the mouse cursor.
   * Displays a `CustomMessageBox` to the user with a predefined error code "105" and the exception message, indicating a problem retrieving version information.
   * Logs this critical error using the `LogBook` method, ensuring the failure to check the version is recorded.
   * Forces the application to terminate. This stringent response ensures that "TAFIE" does not run if it cannot verify its version against the central control system, highlighting the importance of this check.

```cs
 catch (Exception ex) // Catch any errors
            {
                Cursor.Current = Cursors.Default;
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("105", $"\n{ex.Message}");
                LogBook("ERROR", "[SessionMaintenance]", "[CheckVersion]", $"FAILED ( {ex.Message} )");
                Application.Exit();
            }
```

* Version Comparison and Application Control:
   * After successfully retrieving the `latestVersion` from the database, this condition compares it against the `currentVersion` (a global static variable in `SessionMaintenance` that holds the application's compiled version).
   * If the versions do not match:
      * Displays a `CustomMessageBox` to the user, explicitly stating that the current version is outdated/unsupported and showing both the running version and the latest available version. The error code "172" signifies this specific version mismatch.
      * Logs this critical version mismatch error using `LogBook` for auditing and diagnostic purposes.
      * Immediately terminates the application. This is a critical control mechanism to enforce the use of the correct and supported "TAFIE" version, preventing potentially problematic operations with outdated software in the warehouse.

```cs
// Compare latest version to current version
            if (latestVersion != currentVersion)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("172", $"\nCurrent Version:{currentVersion} Lastest Version: {latestVersion}");
                LogBook("ERROR", "[SessionMaintenance]", "[CheckVersion]", $"This is an outdated/unsupported version of the application, please use the latest version. This version: v{currentVersion} Latest Version: v{latestVersion}");
                Application.Exit();
            }
```

#### Summary

The `CheckVersion` method within "TAFIE"'s `SessionMaintenance` class is a robust and essential version control enforcement utility. It proactively queries a central `VERSION_CTRL` database table to retrieve the latest application version and compares it against the locally running `currentVersion`. This mechanism guarantees that warehouse operators are always using a supported and up-to-date instance of "TAFIE." In cases of version mismatch or a failure to even perform the check, the method provides clear error messages, logs the incident for audit, and critically, forces the application to exit. This strict approach is crucial for maintaining the integrity, reliability, and proper functionality of "TAFIE" in a dynamic operational environment.

```cs
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
```

---

### 9.1.7 - UpdateStatusLabel

The `UpdateStatusLabel` method provides real-time feedback to the "TAFIE" user, typically a warehouse operator, about the application's current state or the last performed action. By querying the `LogBook` table in the database for the most recent entry related to the current session, it constructs a concise status message and displays it directly on a provided `Label` control. This continuous visual update is invaluable for keeping the user informed of background processes, successful operations, or recent events, enhancing the overall transparency and user experience of "TAFIE" in a busy fulfillment environment.

#### Step-by-Step Breakdown

* Method Signature:
   * `public static`: Makes the method globally accessible without requiring an object instance.
   * `void`: The method does not return a value; its purpose is to directly modify the `Text` property of the `Label` object passed to it.
   * `Label status`: This parameter accepts a `System.Windows.Forms.Label` control from the UI. This allows the method to update any designated status label on any form that calls it.

```cs
public static void UpdateStatusLabel(Label status):
```

* SQL Query Definition:
   * Defines the SQL query to retrieve the most recent log entry for the current session.
   * Ensures only the single latest log entry is retrieved.
   * Concatenates several fields from the log entry (`Form`, `Event`, `Notes`) along with a "Status: " prefix and the timestamp into a single output string alias `[OUTPUT]`. `RTRIM` is used to remove trailing spaces.
   * Specifies the target table, dynamically named using the global `appName` variable (e.g., TAFIE_LogBook).
   * Filters the results to only include log entries relevant to the current application session, using the globally stored `sessionId`.
   * Orders the results by the creation timestamp in descending order, ensuring that `TOP 1` retrieves the most recent entry.
   * Initializes a string variable to store the retrieved status message.

```cs
 string query = "SELECT TOP 1 " +
                "'Status: ' + RTRIM(FORMAT(GETDATE(), 'yyyy-MM-dd HH:mm:ss')) + ' - ' + RTRIM([Form]) + ' - ' + RTRIM([Event]) + ' - ' + RTRIM([Notes]) as [OUTPUT] " +
                $"FROM  {appName}_LogBook " +
                "WHERE Session_Id = @Session_Id " +
                "ORDER BY DT_Created DESC";
            string output = "";
```

* Database Interaction (Try-Catch Block):
   * The database operation is contained within a `try-catch` block for robust error handling.
   * The method establishes a database connection using the global `connectionString`.
   * `conn.Open();`: Opens the database connection.
   * Creates an SqlCommand with the defined query..
      * The global `sessionId` is passed as a parameter to filter the log entries for the current session.
   * Executes the query and obtains a `SqlDataReader` to fetch the result.
      * If a row is returned (i.e., a log entry exists for the current session), the concatenated status string from the `[OUTPUT]` alias is read and assigned to the `output` variable.
   * `conn.Close();`: Closes the database connection.

```cs
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
```

* UI Update:
   * After successfully retrieving the latest log entry (or if no entry was found, `output` remains an empty string), the `Text` property of the provided `status` `Label` control is updated with the `output` string. This visible change provides direct feedback to the user.

```cs
status.Text = output;
```

* Error Handling (Catch Block):
   * Catches any exceptions occurring during the database interaction (e.g., connection issues, query errors).
   * Displays a `CustomMessageBox` with a predefined error code "117" and the exception message, informing the user that the status label could not be updated.
   * Logs this error using the `LogBook` method, ensuring that any failures in updating the status label are recorded for diagnostic purposes.

```cs
 // Catch Errors
            catch (Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowDefError("117", $"\n{ex.Message}");
                LogBook($"ERROR", "[SessionMaintenance]", "[UpdateStatusLabel]", $"FAILED: Code 117 (  {ex.Message}  )");
            }
```

#### Summary

The `UpdateStatusLabel` method within "TAFIE"'s `SessionMaintenance` class is a crucial utility for enhancing user awareness and application transparency. It dynamically fetches the most recent relevant activity from the application's database-backed logbook and displays it prominently on the user interface. By presenting real-time operational status derived from the `LogBook` data, this method keeps warehouse operators informed about ongoing processes and recent events. The method's overall purpose of providing immediate, session-specific feedback and its robust error logging contribute significantly to the usability and diagnostic capabilities of the "TAFIE" application.

```cs
// update Status Label --------------------------------------------------------------------------
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
```

---

### 9.1.8 - ShowFact

The `ShowFact` method provides a playful and unexpected interaction for "TAFIE" users. As an Easter egg, it offers a brief diversion from the core operational tasks by presenting a random "fact" when a specific, non-interactive UI element is clicked. This small feature adds a touch of personality to the application. The method includes a mechanism to limit how many facts can be viewed per session, preventing overuse and maintaining its status as a novelty.

#### Step-by-Step Breakdown

* Method Signature:
   * `public static`: Makes the method globally accessible, allowing it to be triggered by an event handler associated with the "Easter egg" UI element.
   * `void`: The method does not return a value; its purpose is to display a message box.

```cs
public static void ShowFact():
```

* Fact Display Limit Check:
   * This is the primary control flow for the Easter egg. The method checks the global `factCount` variable (also in `SessionMaintenance`). If `factCount` is less than 3, it proceeds to retrieve and display a fact. This limits the user to viewing a maximum of three facts per application session.

```cs
if (factCount < 3)
```

* Fact Retrieval (If `factCount` is within limit - try block):
   * Initializes a string to hold the retrieved fact.
   * Defines the SQL query to execute a stored procedure named `[Appz - GetFactz]`. This stored procedure is responsible for providing the actual "interesting fact." by selecting a random fact from a database table.
   * Database Interaction: The process of retrieving the fact from the database is enclosed in a `try-catch` block.
      * Establishes a database connection using the global `connectionString`.
      * `conn.Open();`: Opens the connection.
      * Creates a `SqlCommand` for the stored procedure and uses a `SqlDataReader` to read the result.
      * If the stored procedure returns a row, the fact is read from the "OUTPUT" column and stored in the `fact` variable.
      * `conn.Close();`: Closes the database connection.

```cs
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
```

* Displaying the Fact (Success Path):
   * Creates a new instance of the custom message box.
   * Temporarily sets the client size of the message box to a larger dimension (525x599 pixels). This suggests that some facts might be longer and require a larger display area than the default message box size.
   * Displays the retrieved `fact` in the custom message box with the title "Did you know?".
   * Resets the message box client size to a smaller dimension (525x342 pixels) after displaying the message, allowing the size to be reset for the next time it's used.
   * Increments the global `factCount` variable. This is crucial for enforcing the limit of 3 facts per session.

```cs
CustomMessageBox messageBox = new CustomMessageBox();
messageBox.ClientSize = new Size(525, 599);
messageBox.ShowMessage(fact, "Did you know?");
messageBox.ClientSize = new Size(525, 342);
factCount++;
```

* Fact Retrieval Error Handling (Catch Block):
   * Catches any exceptions that occur during the database interaction (e.g., connection issues, stored procedure errors).
   * Displays a `CustomMessageBox` with a predefined error code "117" and the exception message, indicating a problem retrieving the fact.
   * Logs this error using the `LogBook` method, ensuring that failures to retrieve Easter egg content are recorded.

```cs
 // Catch Errors
                catch (Exception ex)
                {
                    CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.ShowDefError("117", $"\n{ex.Message}");
                    LogBook($"ERROR", "[SessionMaintenance]", "[ShowFact]", $"FAILED: Code 117 (  {ex.Message}  )");
                }
```

* Fact Limit Exceeded (Else Block):
   * This block is executed if `factCount` is not less than 3 (i.e., `factCount` is 3 or more).
   * Displays a humorous message to the user, indicating that the fact limit has been reached and encouraging them to return later for more. This provides a friendly way to manage the Easter egg's usage.

```cs
else
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.ShowMessage("Right, I get that you wanna learn all these cool facts but save some for the rest, come back later if you're still in the mood for big facts!", "Calm Down");

            }
```

#### Summary

The `ShowFact` method within "TAFIE"'s `SessionMaintenance` class implements a delightful "Easter egg" feature. It allows users to retrieve and view random facts by interacting with a specific UI element. The method intelligently manages this feature by:
* Limiting access: Only allowing a maximum of three facts to be viewed per application session using the `factCount` global variable.
* Database integration: Fetching facts from the `Appz` database via the `[Appz - GetFactz]` stored procedure, providing dynamic content.
* User-friendly messaging: Displaying facts in a custom message box and providing a polite, humorous message when the fact limit is reached.
* Robust error handling: Logging any issues encountered during fact retrieval, ensuring even non-critical functionality is traceable.
This method exemplifies how small, engaging features can be integrated into a business application to enhance the user experience and add a touch of personality.

```cs
// Get & Show Fact --------------------------------------------------------------------------
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
```

---

### 9.1.9 - Button Enter/Leave

In user interface design, providing clear visual cues for interactive elements is essential for enhancing usability and user experience. The `ButtonEnter` and `ButtonLeave` methods in "TAFIE" serve this purpose by implementing a "hover" effect for `System.Windows.Forms.Button` controls. By programmatically altering the `BackColor` and `ForeColor` of buttons based on mouse interaction, these methods give immediate feedback to warehouse operators, indicating that a button is interactable and reinforcing the application's consistent visual style. Centralizing this UI logic in `SessionMaintenance` ensures uniform behavior across all forms in "TAFIE."

#### Step-by-Step Breakdown: `ButtonEnter`

* Method Signature:
   * `public static`: Makes the method globally accessible, allowing it to be easily attached as an event handler (e.g., to a button's `MouseEnter` event) from any form.
   * `void`: The method does not return any value; its purpose is to modify the properties of the passed Button object directly.
   * `System.Windows.Forms.Button button`: This parameter accepts a reference to the specific Button control whose appearance needs to be changed when the mouse enters its area.

```cs
public static void ButtonEnter(System.Windows.Forms.Button button):
```

* Visual Transformation on Mouse Enter:
   * `button.BackColor = backColor;`: Sets the background color of the `button` to the global `backColor` defined in `SessionMaintenance` (which is `Color.Black`). This typically makes the button's background dark when hovered over.
   * `button.ForeColor = accentColor;`: Sets the foreground color of the `button` to the global `accentColor` defined in `SessionMaintenance` (e.g., `Color.FromArgb(11, 159, 161`)). This makes the text or icon on the button pop with the application's accent color when hovered over.
   * Effect: When the mouse pointer enters the button's area, the button's background becomes the default background color, and its text/icon color changes to the application's accent color, providing a clear visual indication of interactivity.

```cs
public static void ButtonEnter(System.Windows.Forms.Button button)
        {
            button.BackColor = backColor;
            button.ForeColor = accentColor;
        }
```

#### Step-by-Step Breakdown: `ButtonLeave`

* Method Signature:
   * `public static`: Similar to `ButtonEnter`, this makes the method globally accessible for attachment as an event handler (e.g., to a button's `MouseLeave` event).
   * `void`: The method does not return any value; its purpose is to modify the properties of the passed `Button` object directly.
   * `System.Windows.Forms.Button button`: This parameter accepts a reference to the specific `Button` control whose appearance needs to be restored when the mouse leaves its area.

```cs
public static void ButtonLeave(System.Windows.Forms.Button button):
```

* Visual Reversion on Mouse Leave:
   * `button.BackColor = accentColor;`: Sets the background color of the `button` back to the global `accentColor` (e.g., `Color.FromArgb(11, 159, 161)`). This restores its default appearance when the mouse moves away.
   * `button.ForeColor = backColor;`: Sets the foreground color of the button back to the global `backColor` (`Color.Black`). This restores its default text/icon color.
   * Effect: When the mouse pointer leaves the button's area, the button reverts to its original color scheme (accent background, dark foreground), indicating that it is no longer being actively interacted with by the mouse.

```cs
public static void ButtonLeave(System.Windows.Forms.Button button)
        {
            button.BackColor = accentColor;
            button.ForeColor = backColor;
        }
```

#### Summary
The `ButtonEnter` and `ButtonLeave` methods in `SessionMaintenance` are elegant examples of centralized UI logic. They provide a standardized and consistent "hover" effect for all buttons across the "TAFIE" application. By leveraging the globally defined `accentColor` and `backColor`, these methods ensure that button interactions are visually clear, intuitive, and aligned with the application's overall design language. This enhances user experience for warehouse operators by providing immediate and consistent feedback on interactive elements.

```cs
 // Change Button Colours ---------------------------------------------
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
```

---

### 9.1.10 - Control Enter/Leave

Similar to the button-specific hover effects, `ControlEnter` and `ControlLeave` extend this consistent visual feedback to a broader range of `System.Windows.Forms.Control` types (e.g., `TextBox`, `ComboBox`, etc., as they inherit from `Control`). These methods modify the `BackColor` and `ForeColor` of a control as the user’s focus enters or leaves the control. This enhances usability in "TAFIE" by visually indicating active input fields or interactive elements, ensuring that warehouse operators receive clear cues about which control they are currently focused on.

#### Step-by-Step Breakdown: `ControlEnter`

* Method Signature:
   * `public static`: Makes the method globally accessible, allowing it to be easily attached as an event handler for various types of UI controls.
   * `void`: The method does not return any value; its purpose is to directly modify the properties of the passed `Control` object.
   * `Control control`: This parameter accepts a reference to any `System.Windows.Forms.Control` object. This is a generic parameter, meaning it can accept `TextBox`, `ComboBox`, `Panel`, or any other control that inherits from the base `Control` class.

```cs
public static void ControlEnter(Control control)
```

* Visual Transformation on Mouse Enter:
   * `control.BackColor = accentColor;`: Sets the background color of the control to the global `accentColor` defined in `SessionMaintenance` (e.g., `Color.FromArgb(11, 159, 161)`). This highlights the control with the application's accent color when the control is in focus.
   * `control.ForeColor = backColor;`: Sets the foreground color (usually text color) of the control to the global `backColor` defined in `SessionMaintenance` (which is `Color.Black`). This makes the text within the control dark, providing contrast against the accent background.
   * Effect: When the user’s focus enters the control, its background changes to the application's accent color, and its text becomes the default dark background color, drawing the user's attention to that specific input field or element.

```cs
  public static void ControlEnter(Control control)
        {
            control.BackColor = accentColor;
            control.ForeColor = backColor;
        }
```

#### Step-by-Step Breakdown: `ControlLeave`

* Method Signature:
   * `public static`: Similar to `ControlEnter`, this makes the method globally accessible for attachment as an event handler.
   * `void`: The method does not return any value; its purpose is to directly modify the properties of the passed `Control` object.
   * `Control control`: This parameter accepts a reference to the `Control` object whose appearance needs to be restored when the mouse leaves its area.

```cs
public static void ControlLeave(Control control):
```

* Visual Reversion on Mouse Leave:
   * `control.BackColor = Color.White;`: Sets the background color of the `control` back to `Color.White`. This is a significant difference from `ButtonLeave`, which reverts to `accentColor`. For typical input controls like text boxes, `Color.White` is a standard, neutral background.
   * `control.ForeColor = backColor;`: Sets the foreground color of the control back to the global `backColor` (`Color.Black`). This restores its default text color.
   * Effect: When the user’s focus leaves the control's area, the control's background reverts to white, and its text becomes black, restoring its default, non-focused appearance.

```cs
 public static void ControlLeave(Control control)
        {
            control.BackColor = Color.White;
            control.ForeColor = backColor;
        }
```

#### Summary

The `ControlEnter` and `ControlLeave` methods in `SessionMaintenance` provide a unified and consistent approach to visual feedback for a wide array of UI controls within the "TAFIE" application. By changing background and foreground colors on user interaction, they effectively guide the user's attention and indicate active elements. While `ButtonEnter/Leave` uses an accent-to-background color swap, `ControlEnter/Leave` uses the accent color for focused and a default white for the non-focused state, catering to the typical appearance of input fields. This centralized control over UI styling contributes significantly to a polished and intuitive user experience for warehouse operators using "TAFIE."

```cs
 // Change texbox colors Colours --------------------------------------------
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
```
