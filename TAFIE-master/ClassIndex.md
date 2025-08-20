# TAFIE Carrier Module C# Class Index

![TAFIE LOGO](TAFIE/TAFIE_Logo.JPG)

## Methodology
The application's front-end and back-end UI (user interface) are built using `C# .NET Framework` within the Visual Studio IDE.

During the process of requesting a carrier label, the application invokes various methods and functions to:
* Validate data
* Execute SQL queries
* Perform logical operations
* Control UI elements

The application accepts user input in the form of an Elucid load note, which serves as a parameter for executing SQL stored procedures. These procedures update the database and retrieve necessary data to facilitate the carrier label request process.

### Process Overview
* User Input & Data Initialization
  * The user enters a load note.
  * The application passes the load note to an SQL stored procedure, which creates a `TCAR` record.
  * The `TCAR_Ref` (a unique reference ID) is returned and stored as a global parameter in the application.
* Data Retrieval & UI Interaction
  * Several stored procedures are executed to retrieve all necessary data.
  * This data is displayed in the UI, allowing the user to review and edit information before submitting a label request.
* Carrier Label Request Process
  * The application constructs an XML/JSON document containing the shipment details.
  * The XML/JSON document is stored in the SQL database.
  * An API call is made using the XML/JSON document to the carrier’s system to request a label.
  * The API response is received as an XML/JSON, containing tracking details and a Base64-encoded PDF label which are extracted by the application.
  * The label is decoded and sent to a PDF viewer for silent printing.
  * Additional SQL stored procedures are executed to store API requests/responses and update the `TCAR` record with tracking data.
* Error Handling & User Feedback
  * Comprehensive error handling is implemented to prevent application crashes.
  * Errors are managed gracefully and communicated to the user, ensuring smooth operation and troubleshooting.

---

## Classes

### 1.0 - [WcmsApi](WcmsApi-Methods.md)

The `WcmsApi` class contains all public methods and variables used by the application to request a carrier label via an API call using the Parcel Hub API. It includes methods to:
* Construct and parse XML documents
* Extract data from XML responses
* Make and handle API calls
* Log API interactions in the database
* Decode Base64-encoded PDF labels

This class plays a crucial role in integrating the application with external Parcel Hub carrier services by managing the entire API request and response cycle efficiently.

### 2.0 - [CarrierForm](CarrierForm-Methods.md)

The `CarrierForm` class is a Windows Form that manages the user interface (UI) and its associated logic for the carrier module. It contains methods that control UI elements, handle user interactions, and process input and output data. This form serves as the main interface for the carrier functionality, where users enter the loadnote and perform the core operations of the application, ultimately aiming to create and print a shipment carrier label.

### 3.0 - [CarrierCombos](CarrierCombos-Methods.md)

The `CarrierCombos` class is a Windows Form that allows users to view all valid carrier and service level combinations available for a specific client within the carrier module. When the form is loaded, it retrieves and displays these combinations in a data grid for easy reference and validation.


### 4.0 - [RePrint](RePrint-Methods.md)

The `RePrint` class is a form class that form allows users to search and reprint shipment labels created in the last 48 hours via the carrier module. Labels are displayed in a searchable grid, and selecting an entry retrieves the original XML payload and reprints the associated label files. Reprint activity is logged, and the user interface maintains consistent styling and feedback behavior.

### 5.0 - [ClientManager](ClientManager-Methods.md)

The `ClientManager` class, a `partial class` inheriting from `Form`, represents a crucial supplementary window within the "TAFIE" application. This form is dedicated to managing client-specific parameters such as IOSS (Import One Stop Shop) numbers, EORI (Economic Operators Registration and Identification) numbers, and client status. This dedicated management ensures that client data is accurately maintained for seamless label generation and compliance with international shipping regulations.

### 6.0 - [TcarControl](TcarControl-Methods.md)

The `TcarControl` class is the central backend class for the TAFIE Carrier module, it holds several key methods shared and used by multiple classes in order to execute the API Logic and label printing procedures

### 7.0 - [BoxManager](BoxManager-Methods.md)

The `BoxManager` class is a Form responsible for managing box-level shipment data for a specific carrier reference (`tcarRef`).
It allows users to input and save details about the number of boxes associated with a shipment. The form integrates with the parent form by exposing an `OnBoxDataSaved` callback, which enables the parent to receive the updated box data once it has been entered and saved.

### 8.0 - [ProCarrAPI](ProCarrApi-Methods.md)

The `ProCarrApi` class contains all public methods and variables used by the application to request a carrier label via an API call using the Pro Carrier API. It includes methods to:
* Construct and parse JSON documents
* Extract data from JSON responses
* Make and handle API calls
* Log API interactions in the database
* Decode Base64-encoded PDF labels

This class plays a crucial role in integrating the application with external Pro Carrier carrier services by managing the entire API request and response cycle efficiently.

### 9.0 - [SessionMaintenance](SessionMaintenance-Methods.md)

The `SessionMaintenance` class is one of the central backend utility classes within the "TAFIE" application. It provides core functionality related to application state, logging, consistent UI presentation, and critical application configuration.

This class:
* Manages all application logging (for debugging, auditing, and operational insights, as seen with `LogBook`).
* Facilitates consistent UI visual feedback by providing standardized methods for changing the colors of controls (like buttons and text boxes) on user interaction (e.g., `ButtonEnter`, `ControlEnter`).
* Stores key global parameters and configuration, including default background, foreground, and accent colors for UI consistency.
* Crucially, it centrally holds the public SQL database connection string (`connectionString`), which is then referenced by other classes throughout "TAFIE" to establish database connectivity. This makes `SessionMaintenance` the single source of truth for database access configuration.

This class plays a pivotal role in the core functionality of the "TAFIE" application by standardizing backend operations, centralizing critical configuration, and enhancing user interface consistency.


