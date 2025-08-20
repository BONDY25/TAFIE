# TAFIE Carrier Module SQL Database Dictionary

![TAFIE LOGO](TAFIE/TAFIE_Logo.JPG)

Aiden Bond - Whistl Fulfilment South West - 03/04/2025 - V4

## Methodology

The module's functionality relies heavily on an SQL database, which serves as both the primary data storage medium and the core of the data retrieval logic. The architecture consists of tables and stored procedures that facilitate the process of retrieving a carrier label from the Parcel Hub (PHUB) carrier API.

When a carrier label request is made, it is stored as a `TCAR` record, with the `TCAR_Ref` acting as the main reference throughout the application’s methods and SQL stored procedures. These procedures orchestrate the necessary steps to obtain a carrier label. The Carrier module's tables and stored procedures are integrated with the existing Elucid databases to gather all required data before making the API call.

Upon receiving an Elucid load note from the application, multiple stored procedures populate database tables with the relevant Elucid data needed to construct an XML request for the API. Both the XML request sent to the carrier API and the XML response received are stored in the Carrier module’s database tables for tracking and auditing purposes.

Before a label request can be made, an access token is required for authorization. This token is obtained via a separate API call using login credentials stored in the database. Stored procedures manage token storage to ensure that a valid token is always available before making a label request. The application executes a stored procedure that checks for valid tokens and returns predefined values, allowing it to either retrieve the last valid token or request a new one before proceeding with the label request.

Once a label has been successfully retrieved, the `TCAR` record is marked as complete, and the carrier tracking reference is stored against the `TCAR_Ref`. Additionally, this tracking reference is passed into the Elucid `load_dely` table. A note is also inserted into the Elucid `cust_hist` table under the customer's account, indicating that a carrier label was generated using the TAFIE Application. However, the Elucid order associated with the `TCAR` Reference remains unmarked as complete within Elucid.

## Tables

To find all the tables use this SQL

```SQL
SELECT
			RTRIM(TABLE_NAME) as [Table]
			,RTRIM(COLUMN_NAME) as [Column]
			,RTRIM(DATA_TYPE) as [Datatype]
			,ISNULL(ISNULL(RTRIM(CHARACTER_MAXIMUM_LENGTH), RTRIM(NUMERIC_PRECISION) + ',' + RTRIM(NUMERIC_SCALE)), '') as [Constraint]
			,RTRIM(IS_NULLABLE) as [Nullable]
FROM 
			INFORMATION_SCHEMA.COLUMNS 
WHERE 
			TABLE_NAME LIKE 'TCAR%'
```

---

#### TCAR (TAFIE Carrier Module Header)

This is the header table that will store header data for a label creation record

<table>
  <tr>
   <td><strong>Column</strong>
   </td>
   <td><strong>Datatype</strong>
   </td>
   <td><strong>Constraint</strong>
   </td>
   <td><strong>Nullable?</strong>
   </td>
   <td><strong>Description</strong>
   </td>
  </tr>
  <tr>
   <td>TCAR_Ref
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>NO
   </td>
   <td>This is the label creation reference for the record
   </td>
  </tr>
  <tr>
   <td>Session_Id
   </td>
   <td>varchar
   </td>
   <td>255
   </td>
   <td>NO
   </td>
   <td>This is the Session Id the record was created on
   </td>
  </tr>
  <tr>
   <td>Client
   </td>
   <td>varchar
   </td>
   <td>255
   </td>
   <td>NO
   </td>
   <td>This is the client the record relates to 
   </td>
  </tr>
  <tr>
   <td>Customer
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>NO
   </td>
   <td>This is the customer the record relates to 
   </td>
  </tr>
  <tr>
   <td>Ref_No
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>NO
   </td>
   <td>This is the Elucid order number the record  relates to
   </td>
  </tr>
  <tr>
   <td>Load_Note
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>NO
   </td>
   <td>This is the Elucid Load note the record relates to 
   </td>
  </tr>
  <tr>
   <td>Carrier
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>YES
   </td>
   <td>This is the Elucid carrier for the shipment
   </td>
  </tr>
  <tr>
   <td>Del_Method
   </td>
   <td>varchar
   </td>
   <td>50
   </td>
   <td>YES
   </td>
   <td>This is the Elucid delivery method for the shipment
   </td>
  </tr>
  <tr>
   <td>Customer_UID
   </td>
   <td>varchar
   </td>
   <td>50
   </td>
   <td>YES
   </td>
   <td>This is the PHUB customer service level id
   </td>
  </tr>
  <tr>
   <td>Service_Id
   </td>
   <td>varchar
   </td>
   <td>50
   </td>
   <td>YES
   </td>
   <td>This is the PHUB carrier identifier
   </td>
  </tr>
  <tr>
   <td>Service_Level
   </td>
   <td>varchar
   </td>
   <td>50
   </td>
   <td>YES
   </td>
   <td>This is the PHUB Delivery method identifier
   </td>
  </tr>
  <tr>
   <td>Service_Descr
   </td>
   <td>varchar
   </td>
   <td>100
   </td>
   <td>YES
   </td>
   <td>This is the Elucid description for the delivery method
   </td>
  </tr>
  <tr>
   <td>API_Acc
   </td>
   <td>varchar
   </td>
   <td>255
   </td>
   <td>YES
   </td>
   <td>This is client level account code (L number) for the PHUB account
   </td>
  </tr>
  <tr>
   <td>Acc_Code
   </td>
   <td>varchar
   </td>
   <td>8
   </td>
   <td>YES
   </td>
   <td>This is the site level account identifier
   </td>
  </tr>
  <tr>
   <td>Inco
   </td>
   <td>varchar
   </td>
   <td>255
   </td>
   <td>YES
   </td>
   <td>This is the shipments Incoterms
   </td>
  </tr>
  <tr>
   <td>EORI
   </td>
   <td>varchar
   </td>
   <td>255
   </td>
   <td>YES
   </td>
   <td>This is the shipments EORI number
   </td>
  </tr>
  <tr>
   <td>IOSS
   </td>
   <td>varchar
   </td>
   <td>255
   </td>
   <td>YES
   </td>
   <td>This is the shipment IOSS number
   </td>
  </tr>
  <tr>
   <td>Name
   </td>
   <td>varchar
   </td>
   <td>255
   </td>
   <td>YES
   </td>
   <td>This is the delivery recipient name
   </td>
  </tr>
  <tr>
   <td>Company
   </td>
   <td>varchar
   </td>
   <td>35
   </td>
   <td>YES
   </td>
   <td>This is be the delivery address company (If applicable)
   </td>
  </tr>
  <tr>
   <td>Postcode
   </td>
   <td>varchar
   </td>
   <td>10
   </td>
   <td>YES
   </td>
   <td>This is the delivery postcode or zipcode
   </td>
  </tr>
  <tr>
   <td>City
   </td>
   <td>varchar
   </td>
   <td>35
   </td>
   <td>YES
   </td>
   <td>This is the delivery city or town
   </td>
  </tr>
  <tr>
   <td>Addr1
   </td>
   <td>varchar
   </td>
   <td>35
   </td>
   <td>YES
   </td>
   <td>This is the first line of the delivery address
   </td>
  </tr>
  <tr>
   <td>Addr2
   </td>
   <td>varchar
   </td>
   <td>35
   </td>
   <td>YES
   </td>
   <td>This is the second line of the delivery address (If applicable)
   </td>
  </tr>
  <tr>
   <td>County
   </td>
   <td>varchar
   </td>
   <td>35
   </td>
   <td>YES
   </td>
   <td>This is the delivery county, state or province
   </td>
  </tr>
  <tr>
   <td>Country
   </td>
   <td>varchar
   </td>
   <td>35
   </td>
   <td>YES
   </td>
   <td>This is the delivery country
   </td>
  </tr>
  <tr>
   <td>Email
   </td>
   <td>varchar
   </td>
   <td>35
   </td>
   <td>YES
   </td>
   <td>This is the recipients email address
   </td>
  </tr>
  <tr>
   <td>Phone
   </td>
   <td>varchar
   </td>
   <td>12
   </td>
   <td>YES
   </td>
   <td>This is the recipient's Phone number
   </td>
  </tr>
  <tr>
   <td>Ref1
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>YES
   </td>
   <td>This is the shipment’s first reference
   </td>
  </tr>
  <tr>
   <td>Ref2
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>YES
   </td>
   <td>This is the shipment’s second reference
   </td>
  </tr>
  <tr>
   <td>Curr
   </td>
   <td>varchar
   </td>
   <td>3
   </td>
   <td>YES
   </td>
   <td>This is the currency the order is in 
   </td>
  </tr>
  <tr>
   <td>Status
   </td>
   <td>int
   </td>
   <td>10,0
   </td>
   <td>YES
   </td>
   <td>This is the status of the TCAR record
<p>
225 = Reprinted
<p>
220 = Complete, A carrier label has been created
<p>
200 = Cancelled, Shipment abandoned or succeeded
<p>
30 = Shipment Open
   </td>
  </tr>
  <tr>
   <td>Tracking
   </td>
   <td>varchar
   </td>
   <td>255
   </td>
   <td>YES
   </td>
   <td>This is the tracking number for the shipment (populated when a label has been created)
   </td>
  </tr>
  <tr>
   <td>DT_Created
   </td>
   <td>datetime
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the DateTime the record was created
   </td>
  </tr>
  <tr>
   <td>Last_Updated
   </td>
   <td>datetime
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the DateTime the record was last updated
   </td>
  </tr>
  <tr>
   <td>User_Created
   </td>
   <td>varchar
   </td>
   <td>100
   </td>
   <td>YES
   </td>
   <td>This is the username who created the record.
<p>
 
   </td>
  </tr>
  <tr>
   <td>Last_Upd_User
   </td>
   <td>varchar
   </td>
   <td>100
   </td>
   <td>YES
   </td>
   <td>This is the username of the last user who modified the record
   </td>
  </tr>
  <tr>
   <td>Postal_Charges
   </td>
   <td>numeric
   </td>
   <td>14,2
   </td>
   <td>YES
   </td>
   <td>This is the postal charges associated with the shipment
   </td>
  </tr>
</table>

---
#### TCAR_Line (TAFIE Carrier Module Line)

This table will store the line details of a label creation record

<table>
  <tr>
   <td><strong>Column</strong>
   </td>
   <td><strong>Datatype</strong>
   </td>
   <td><strong>Constraint</strong>
   </td>
   <td><strong>Nullable?</strong>
   </td>
   <td><strong>Description</strong>
   </td>
  </tr>
  <tr>
   <td>TCAR_Ref
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>NO
   </td>
   <td>This is the label creation reference for the record
   </td>
  </tr>
  <tr>
   <td>Line
   </td>
   <td>int
   </td>
   <td>10,0
   </td>
   <td>NO
   </td>
   <td>This is the line number for the record
   </td>
  </tr>
  <tr>
   <td>Part
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>NO
   </td>
   <td>This is the part on the line record
   </td>
  </tr>
  <tr>
   <td>Description
   </td>
   <td>varchar
   </td>
   <td>100
   </td>
   <td>YES
   </td>
   <td>This is the description of the part
   </td>
  </tr>
  <tr>
   <td>Type
   </td>
   <td>varchar
   </td>
   <td>35
   </td>
   <td>YES
   </td>
   <td>This is the product group for the part
   </td>
  </tr>
  <tr>
   <td>Qty
   </td>
   <td>int
   </td>
   <td>10,0
   </td>
   <td>YES
   </td>
   <td>This is the Qty of the part on the line
   </td>
  </tr>
  <tr>
   <td>Part_Value
   </td>
   <td>numeric
   </td>
   <td>25,13
   </td>
   <td>YES
   </td>
   <td>This is the unit value of the part 
   </td>
  </tr>
  <tr>
   <td>Line_Value
   </td>
   <td>numeric
   </td>
   <td>14,2
   </td>
   <td>YES
   </td>
   <td>This is the total line value
   </td>
  </tr>
  <tr>
   <td>Part_Weight
   </td>
   <td>numeric
   </td>
   <td>28,15
   </td>
   <td>YES
   </td>
   <td>This is the unit weight for the part
   </td>
  </tr>
  <tr>
   <td>Line_Weight
   </td>
   <td>numeric
   </td>
   <td>17,4
   </td>
   <td>YES
   </td>
   <td>This is the line weight
   </td>
  </tr>
  <tr>
   <td>CoO
   </td>
   <td>varchar
   </td>
   <td>35
   </td>
   <td>YES
   </td>
   <td>This is the part’s country of origin in 2 character format
   </td>
  </tr>
  <tr>
   <td>HS_Code
   </td>
   <td>varchar
   </td>
   <td>35
   </td>
   <td>YES
   </td>
   <td>This is the part’s commodity code
   </td>
  </tr>
  <tr>
   <td>DT_Created
   </td>
   <td>datetime
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the datetime the record was created
   </td>
  </tr>
  <tr>
   <td>Last_Updated
   </td>
   <td>datetime
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the datetime the record was last updated
   </td>
  </tr>
  <tr>
   <td>Last_Upd_User
   </td>
   <td>varchar
   </td>
   <td>100
   </td>
   <td>YES
   </td>
   <td>This is the user who last updated the record
<p>
 
   </td>
  </tr>
</table>

---
#### TCAR_BOXES (TAFIE Carrier Module Boxes)

This table will be the box header record for a label creation record


<table>
  <tr>
   <td><strong>Column</strong>
   </td>
   <td><strong>Datatype</strong>
   </td>
   <td><strong>Constraint</strong>
   </td>
   <td><strong>Nullable?</strong>
   </td>
   <td><strong>Description</strong>
   </td>
  </tr>
  <tr>
   <td>TCAR_Ref
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>NO
   </td>
   <td>This is the label creation reference for the record
   </td>
  </tr>
  <tr>
   <td>Box_Ref
   </td>
   <td>int
   </td>
   <td>10,0
   </td>
   <td>NO
   </td>
   <td>This is the box number reference
   </td>
  </tr>
  <tr>
   <td>Weight
   </td>
   <td>numeric
   </td>
   <td>17,4
   </td>
   <td>YES
   </td>
   <td>This is the total weight of the box contents in Kilograms
   </td>
  </tr>
  <tr>
   <td>Value
   </td>
   <td>numeric
   </td>
   <td>14,2
   </td>
   <td>YES
   </td>
   <td>This is the total values of the box contents
   </td>
  </tr>
  <tr>
   <td>Qty
   </td>
   <td>numeric
   </td>
   <td>14,2
   </td>
   <td>YES
   </td>
   <td>This is the qty of units in the box
   </td>
  </tr>
  <tr>
   <td>Dim_L
   </td>
   <td>float
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the box length in centimeters
   </td>
  </tr>
  <tr>
   <td>Dim_W
   </td>
   <td>float
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the box width in centimeters
   </td>
  </tr>
  <tr>
   <td>Dim_H
   </td>
   <td>float
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the box height in centimeters
   </td>
  </tr>
  <tr>
   <td>DT_Created
   </td>
   <td>datetime
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the datetime the record was created
   </td>
  </tr>
  <tr>
   <td>Last_Updated
   </td>
   <td>datetime
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the datetime the record was last updated
   </td>
  </tr>
  <tr>
   <td>Last_Upd_User
   </td>
   <td>varchar
   </td>
   <td>100
   </td>
   <td>YES
   </td>
   <td>This is the user who last updated the record
   </td>
  </tr>
  <tr>
   <td>Tracking
   </td>
   <td>varchar
   </td>
   <td>255
   </td>
   <td>YES
   </td>
   <td>This is the tracking number for the shipment (populated when a label has been created)
   </td>
  </tr>
</table>

---
#### TCAR_BCON (TAFIE Carrier Module Box Contents)

This table will store the box contents for a box header record

<table>
  <tr>
   <td><strong>Column</strong>
   </td>
   <td><strong>Datatype</strong>
   </td>
   <td><strong>Constraint</strong>
   </td>
   <td><strong>Nullable?</strong>
   </td>
   <td><strong>Description</strong>
   </td>
  </tr>
  <tr>
   <td>TCAR_Ref
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>NO
   </td>
   <td>This is the label creation reference for the record
   </td>
  </tr>
  <tr>
   <td>Box_Ref
   </td>
   <td>int
   </td>
   <td>10,0
   </td>
   <td>NO
   </td>
   <td>This is the box number reference
   </td>
  </tr>
  <tr>
   <td>Part
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>NO
   </td>
   <td>This is the part in the box
   </td>
  </tr>
  <tr>
   <td>Qty
   </td>
   <td>int
   </td>
   <td>10,0
   </td>
   <td>YES
   </td>
   <td>This is the qty of the part in the box
   </td>
  </tr>
  <tr>
   <td>DT_Created
   </td>
   <td>datetime
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the datetime the record was created
   </td>
  </tr>
  <tr>
   <td>Last_Updated
   </td>
   <td>datetime
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the datetime the record was last updated
   </td>
  </tr>
  <tr>
   <td>Last_Upd_User
   </td>
   <td>varchar
   </td>
   <td>100
   </td>
   <td>YES
   </td>
   <td>This is the user who last updated the record
   </td>
  </tr>
</table>

---
#### TCAR_Call (TAFIE Carrier Module API Call XMLs)

This table will store the XML Call we sent to the Parcel Hub API

<table>
  <tr>
   <td><strong>Column</strong>
   </td>
   <td><strong>Datatype</strong>
   </td>
   <td><strong>Constraint</strong>
   </td>
   <td><strong>Nullable?</strong>
   </td>
   <td><strong>Description</strong>
   </td>
  </tr>
  <tr>
   <td>tkey
   </td>
   <td>varchar
   </td>
   <td>255
   </td>
   <td>NO
   </td>
   <td>This is the unique transaction key of the API call to get the carrier label
   </td>
  </tr>
  <tr>
   <td>TCAR_Ref
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>NO
   </td>
   <td>This is the label creation reference for the record
   </td>
  </tr>
  <tr>
   <td>Status
   </td>
   <td>int
   </td>
   <td>10,0
   </td>
   <td>YES
   </td>
   <td>This is the status of the API Call 
	   <p>
200 = Ok
<p>
400 = Bad Request
   </td>
  </tr>
  <tr>
   <td>DT_Created
   </td>
   <td>datetime
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the datetime the record was created
   </td>
  </tr>
  <tr>
   <td>User_Created
   </td>
   <td>varchar
   </td>
   <td>100
   </td>
   <td>NO
   </td>
   <td>This is the user who last updated the record
   </td>
  </tr>
  <tr>
   <td>XML_MESSAGE
   </td>
   <td>nvarchar
   </td>
   <td>MAX
   </td>
   <td>NO
   </td>
   <td>This is the XML sent by the application to the PHUB API
   </td>
  </tr>
</table>

---
#### TCAR_Resp (TAFIE Carrier Module API Response XMLs)

This table will store the XML Response from the Parcel Hub API

<table>
  <tr>
   <td><strong>Column</strong>
   </td>
   <td><strong>Datatype</strong>
   </td>
   <td><strong>Constraint</strong>
   </td>
   <td><strong>Nullable?</strong>
   </td>
   <td><strong>Description</strong>
   </td>
  </tr>
  <tr>
   <td>tkey
   </td>
   <td>varchar
   </td>
   <td>255
   </td>
   <td>NO
   </td>
   <td>This is the unique transaction key of the API call to get the carrier label
   </td>
  </tr>
  <tr>
   <td>TCAR_Ref
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>NO
   </td>
   <td>This is the label creation reference for the record
   </td>
  </tr>
  <tr>
   <td>Status
   </td>
   <td>int
   </td>
   <td>10,0
   </td>
   <td>YES
   </td>
   <td>This is the status of the API Call
	   <p>
200 = Ok
<p>
400 = Bad Request
   </td>
  </tr>
  <tr>
   <td>DT_Created
   </td>
   <td>datetime
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the datetime the record was created
   </td>
  </tr>
  <tr>
   <td>User_Created
   </td>
   <td>varchar
   </td>
   <td>100
   </td>
   <td>NO
   </td>
   <td>This is the user who last updated the record
   </td>
  </tr>
  <tr>
   <td>XML_MESSAGE
   </td>
   <td>nvarchar
   </td>
   <td>MAX
   </td>
   <td>NO
   </td>
   <td>This is the XML response received by the PHUB API
   </td>
  </tr>
</table>

---
#### TCAR_TOKN (TAFIE Carrier Module Access Tokens)

This table will store the access tokens in order to make the API calls to get a label from the Parcel Hub API. Access tokens are valid for 4 hours. After 30 days the record will be deleted from the table

<table>
  <tr>
   <td><strong>Column</strong>
   </td>
   <td><strong>Datatype</strong>
   </td>
   <td><strong>Constraint</strong>
   </td>
   <td><strong>Nullable?</strong>
   </td>
   <td><strong>Description</strong>
   </td>
  </tr>
  <tr>
   <td>tkey
   </td>
   <td>varchar
   </td>
   <td>255
   </td>
   <td>NO
   </td>
   <td>This is the unique transaction key of the API call to get the token
   </td>
  </tr>
  <tr>
   <td>Acc_Code
   </td>
   <td>varchar
   </td>
   <td>8
   </td>
   <td>NO
   </td>
   <td>This is the site level account identifier
   </td>
  </tr>
  <tr>
   <td>Access_Tkn
   </td>
   <td>nvarchar
   </td>
   <td>MAX
   </td>
   <td>NO
   </td>
   <td>This is the access token used to make the carrier label API Request. This token is valid for 4 hours.
   </td>
  </tr>
  <tr>
   <td>Refresh_Tkn
   </td>
   <td>varchar
   </td>
   <td>255
   </td>
   <td>YES
   </td>
   <td>This is the refresh token used to get a new access token after it has expired. This token is valid for 30 days
   </td>
  </tr>
  <tr>
   <td>DT_Created
   </td>
   <td>datetime
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the datetime the record was created
   </td>
  </tr>
  <tr>
   <td>Status
   </td>
   <td>int
   </td>
   <td>10,0
   </td>
   <td>YES
   </td>
   <td>This is the status of the access token
<p>
20 = Active
<p>
90 = Expired
   </td>
  </tr>
</table>

---
#### TCAR_Acct (TAFIE Carrier Module API Accounts)

This Table stores the account information for a parcel hub account

<table>
  <tr>
   <td><strong>Column</strong>
   </td>
   <td><strong>Datatype</strong>
   </td>
   <td><strong>Constraint</strong>
   </td>
   <td><strong>Nullable?</strong>
   </td>
   <td><strong>Description</strong>
   </td>
  </tr>
  <tr>
   <td>Acc_Code
   </td>
   <td>varchar
   </td>
   <td>8
   </td>
   <td>NO
   </td>
   <td>This is the site level account identifier
   </td>
  </tr>
  <tr>
   <td>Acc_No
   </td>
   <td>varchar
   </td>
   <td>12
   </td>
   <td>NO
   </td>
   <td>This is the client level account number (L Number)
   </td>
  </tr>
  <tr>
   <td>Department_Id
   </td>
   <td>varchar
   </td>
   <td>12
   </td>
   <td>NO
   </td>
   <td>This is the client level department identifier
   </td>
  </tr>
  <tr>
   <td>API_UserName
   </td>
   <td>varchar
   </td>
   <td>255
   </td>
   <td>YES
   </td>
   <td>This is the Username for the account code
   </td>
  </tr>
  <tr>
   <td>API_Password
   </td>
   <td>varchar
   </td>
   <td>255
   </td>
   <td>YES
   </td>
   <td>This is the password for the account code
   </td>
  </tr>
  <tr>
   <td>DT_Created
   </td>
   <td>datetime
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the datetime the record was created
   </td>
  </tr>
  <tr>
   <td>Last_Updated
   </td>
   <td>datetime
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the datetime the record was last updated
   </td>
  </tr>
  <tr>
   <td>Last_Upd_User
   </td>
   <td>varchar
   </td>
   <td>100
   </td>
   <td>YES
   </td>
   <td>This is the user who last updated the record
   </td>
  </tr>
</table>

---
#### TCAR_Parameters (TAFIE Carrier Module Parameters)

This table is used to store default data for the apps functionality

<table>
  <tr>
   <td><strong>Column</strong>
   </td>
   <td><strong>Datatype</strong>
   </td>
   <td><strong>Constraint</strong>
   </td>
   <td><strong>Nullable?</strong>
   </td>
   <td><strong>Description</strong>
   </td>
  </tr>
  <tr>
   <td>ID
   </td>
   <td>varchar
   </td>
   <td>3
   </td>
   <td>NO
   </td>
   <td>This is the row identifier for the parameter
   </td>
  </tr>
  <tr>
   <td>Description
   </td>
   <td>varchar
   </td>
   <td>100
   </td>
   <td>YES
   </td>
   <td>This is the description of the parameter
   </td>
  </tr>
  <tr>
   <td>String_1
   </td>
   <td>varchar
   </td>
   <td>4000
   </td>
   <td>YES
   </td>
   <td>This is where a string can be stored
   </td>
  </tr>
  <tr>
   <td>String_2
   </td>
   <td>varchar
   </td>
   <td>4000
   </td>
   <td>YES
   </td>
   <td>This is where a string can be stored
   </td>
  </tr>
  <tr>
   <td>String_3
   </td>
   <td>varchar
   </td>
   <td>4000
   </td>
   <td>YES
   </td>
   <td>This is where a string can be stored
   </td>
  </tr>
  <tr>
   <td>Int_1
   </td>
   <td>int
   </td>
   <td>10,0
   </td>
   <td>YES
   </td>
   <td>This is where an int can be stored
   </td>
  </tr>
  <tr>
   <td>Int_2
   </td>
   <td>int
   </td>
   <td>10,0
   </td>
   <td>YES
   </td>
   <td>This is where an int can be stored
   </td>
  </tr>
  <tr>
   <td>Int_3
   </td>
   <td>int
   </td>
   <td>10,0
   </td>
   <td>YES
   </td>
   <td>This is where an int can be stored
   </td>
  </tr>
</table>

---
#### TCAR_CMAP (TAFIE Carrier Module Carrier Map)

This table is used to map Elucid carrier codes to parcel hub service Ids and service levels

<table>
  <tr>
   <td><strong>Column</strong>
   </td>
   <td><strong>Datatype</strong>
   </td>
   <td><strong>Constraint</strong>
   </td>
   <td><strong>Nullable?</strong>
   </td>
   <td><strong>Description</strong>
   </td>
  </tr>
  <tr>
   <td>E9_Carrier
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>NO
   </td>
   <td>This is the Elucid carrier code
   </td>
  </tr>
  <tr>
   <td>E9_Del_Method
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>NO
   </td>
   <td>This is the Elucid delivery method
   </td>
  </tr>
  <tr>
   <td>Description
   </td>
   <td>varchar
   </td>
   <td>255
   </td>
   <td>YES
   </td>
   <td>This is the Elucid description of the delivery method
   </td>
  </tr>
  <tr>
   <td>Service_Id
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>YES
   </td>
   <td>This is the PHUB service identifier
   </td>
  </tr>
  <tr>
   <td>Service_Level
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>YES
   </td>
   <td>This is the PHUB service level
   </td>
  </tr>
  <tr>
   <td>Weight_Limit
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>YES
   </td>
   <td>This is the service weight limit
   </td>
  </tr>
  <tr>
   <td>DT_Created
   </td>
   <td>datetime
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the datetime the record was created
   </td>
  </tr>
  <tr>
   <td>Last_Updated
   </td>
   <td>datetime
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the datetime the record was last updated
   </td>
  </tr>
  <tr>
   <td>Last_Upd_User
   </td>
   <td>varchar
   </td>
   <td>100
   </td>
   <td>YES
   </td>
   <td>This is the user who last updated the record
   </td>
  </tr>
  <tr>
   <td>Active
   </td>
   <td>int
   </td>
   <td>10,0
   </td>
   <td>YES
   </td>
   <td>This is the status of the record
<p>
1 = Active
<p>
0 - Inactive
   </td>
  </tr>
  <tr>
   <td>DDP
   </td>
   <td>int
   </td>
   <td>10,0
   </td>
   <td>YES
   </td>
   <td>This is the duties paid flag
<p>
1 = Duties paid
<p>
0 = Duties unpaid
   </td>
  </tr>
  <tr>
   <td>Domestic
   </td>
   <td>int
   </td>
   <td>10,0
   </td>
   <td>YES
   </td>
   <td>This is the domestic flag
<p>
1 = Domestic
<p>
0 = International
   </td>
  </tr>
</table>

---

#### TCAR_Srch (TAFIE Reprint Search Results)

This table is used to store a temporary results set used when reprinting carrier labels created through the TAFIE Carrier module.

<table>
  <tr>
   <td><strong>Column</strong>
   </td>
   <td><strong>Datatype</strong>
   </td>
   <td><strong>Constraint</strong>
   </td>
   <td><strong>Nullable?</strong>
   </td>
   <td><strong>Description</strong>
   </td>
  </tr>
  <tr>
   <td>Session_Id
   </td>
   <td>varchar
   </td>
   <td>255
   </td>
   <td>YES
   </td>
   <td>This is the Session Id the record was created on
   </td>
  </tr>
  <tr>
   <td>TCAR_Ref
   </td>
   <td>varchar
   </td>
   <td>24
   </td>
   <td>YES
   </td>
   <td>This is the label creation reference for the record
   </td>
  </tr>
  <tr>
   <td>Ref1
   </td>
   <td>varchar
   </td>
   <td>35
   </td>
   <td>YES
   </td>
   <td>This is the shipment’s first reference
   </td>
  </tr>
  <tr>
   <td>Ref2
   </td>
   <td>varchar
   </td>
   <td>35
   </td>
   <td>YES
   </td>
   <td>This is the shipment’s second reference
   </td>
  </tr>
  <tr>
   <td>Tracking
   </td>
   <td>varchar
   </td>
   <td>255
   </td>
   <td>YES
   </td>
   <td>This is the tracking number for the shipment
   </td>
  </tr>
  <tr>
   <td>DT_Created
   </td>
   <td>datetime
   </td>
   <td> 
   </td>
   <td>YES
   </td>
   <td>This is the datetime the record was created
   </td>
  </tr>
  <tr>
   <td>XML_MESSAGE
   </td>
   <td>nvarchar
   </td>
   <td>MAX
   </td>
   <td>YES
   </td>
   <td>This is the XML response received by the PHUB API
   </td>
  </tr>
</table>

---
## Stored Procedures

### TCAR_Get_Headers

The `TCAR_Get_Headers` stored procedure is responsible for retrieving and inserting carrier request data into the `TCAR` table. It dynamically constructs and executes an SQL query to extract relevant details from the Elucid databases, integrating them to facilitate the retrieval of carrier labels. The procedure ensures data consistency and completeness by handling abandoned records and retrieving header-level details.

#### Input Parameters


* `@TCAR_Ref` - VARCHAR(24) - Unique reference identifier for the `TCAR` record.
* `@Session_Id` - VARCHAR(255) - Session identifier for tracking transactions.
* `@Client` - VARCHAR(255) - Specifies the client associated with the `TCAR` record.
* `@Load_Note` - VARCHAR(24) - Elucid Load Note for the shipment.
* `@User` - VARCHAR(255) - The user initiating the request.

#### Process Flow

1. Variable Initialization
    Upon execution, the procedure initializes several key variables:

  * `@SQL` – Stores the dynamically generated SQL query.
  * `@Connection_String` – Dynamically constructs a reference to the client’s Elucid database based on the client name (`@Client`).
  * `@Client_Id` and `@Acc_Code`** – Retrieve client-specific identifiers from the `TAFIE_Clients` table.


2. Handling Abandoned Records
Before retrieving data, the procedure updates abandoned records in the `TCAR` table by setting their status to 200 (indicating cancellation). This update applies to records that:

  * Have a `status` of 30 (Open).
  * Match the provided ` @Load_Note` parameter.

This ensures that outdated or incomplete records do not interfere with subsequent processing.


3. Constructing the SQL Query
The procedure dynamically constructs an INSERT statement to populate the ` TCAR` table. The SQL query retrieves:

* Customer details, carrier information, and delivery method from the external Elucid database.
* API account details (`API_Acc`) from the Elucid` carr_dtls` table.
* Service level (`Service_Level`), service ID (`Service_Id`), and service description (`Service_Descr`) from the `TCAR_CMAP` table, based on the carrier and delivery method.
* If any values are missing, the procedure substitutes default placeholders ('' or 'ER').


4. Executing the SQL Query
The final dynamically generated SQL query is printed for logging and debugging purposes. The procedure then executes the query to insert the retrieved data into the `TCAR` table.


#### Data Flow and Integration
The stored procedure interacts with multiple database tables to gather and insert necessary information:

1. Extracts data from local tables and Elucid tables
* `load_dely`, `sord`, and `lnot_sord` tables in the Elucid database (via `@Connection_String`).
* `carr_dtls` table for API account details.
* `TCAR_CMAP` table for service-level mapping.

2. Inserts processed data into the `TCAR table`</strong>
* Stores carrier request details, including tracking references, service levels, and status updates.

3. Ensures accurate tracking and auditing
* Retains API requests and responses.
* Logs the user initiating the request.

#### Summary

The `TCAR_Get_Headers` stored procedure plays a crucial role in the carrier label retrieval by ensuring that all required data is retrieved and correctly inserted into the `TCAR` table. Through dynamic SQL execution, it seamlessly integrates external and internal data sources, maintaining system integrity and ensuring smooth processing of carrier requests.

```SQL
ALTER PROC [dbo].[TCAR_Get_Headers]

@TCAR_Ref VARCHAR(24)
,@Session_Id VARCHAR(255)
,@Client VARCHAR(255)
,@Load_Note VARCHAR(24)
,@User VARCHAR(255)

as

--=================================================================================================================================
-- DECLARE & SET VARIABLES
--=================================================================================================================================

DECLARE @SQL VARCHAR(MAX) = ''
DECLARE @Connection_String VARCHAR(MAX) 
DECLARE @Client_Id INT = (SELECT ID FROM TAFIE_Clients WHERE [Description] = @Client)
DECLARE @Acc_Code VARCHAR(255) = (SELECT Acc_Code FROM TAFIE_Clients WHERE [Description] = @Client)
DECLARE @Service_Provider VARCHAR(255) 
DECLARE @Service_Id VARCHAR(255)

SET @Connection_String = 
							(
								SELECT 
											'[' + [Server] + '].[' + [Database] + '].dbo' 
								FROM 
											TAFIE_Clients 
								WHERE 
											[Description] = @Client
							)

--=================================================================================================================================
-- CANCEL ABANDONDED RECORDS
--=================================================================================================================================

UPDATE 
		TCAR
SET 
		[Status] = 200
		,Last_Updated = GETDATE()
WHERE 
		[Status] = 30 
		AND Load_Note = @Load_Note

--=================================================================================================================================
-- GET & INSERT DATA
--=================================================================================================================================

SET @SQL = @SQL + '

INSERT INTO [TCAR]
(
	[TCAR_Ref]
	,[Session_Id]
	,[Client]
	,[Customer]
	,[Ref_No]
	,[Load_Note]
	,[Carrier]
	,[Del_Method]
	,[API_Acc]
	,[Inco]
	,[EORI]
	,[IOSS]
	,[Status]
	,[Ref1]
	,[Ref2]
	,[Curr]
	,[DT_Created]
	,[Last_Updated]
	,[User_Created]
	,[Service_Level]
	,[Service_Id]
	,[Service_Descr]
	,[Acc_Code]
	,[Postal_Charges]
)
'

SET @SQL = @SQL + '

SELECT
			''' + @TCAR_Ref + '''
			,''' + @Session_Id + '''
			,''' + @Client + '''
			,RTRIM(sord.customer) as [Customer]
			,RTRIM(sord.ref_no) as [Ref_No]
			,RTRIM(load_dely.load_note) as [Load_Note]
			,RTRIM(load_Dely.carrier) as [Carrier]
			,RTRIM(load_dely.del_method) as [Del_Method]
			,ISNULL(
					(
						SELECT TOP 1
									RTRIM(carr_dtls.account_no) as [Account_No]
						FROM 
									' + @Connection_String + '.load_dely 
									LEFT JOIN ' + @Connection_String + '.carr_dtls on load_dely.carrier = carr_dtls.carrier
						WHERE 
									load_dely.load_note = ''' + @Load_Note + '''
					), ''''
			) as [API_Acc]
			,'''' as [IncoTerms]
			,(SELECT EORI FROM TAFIE_Clients WHERE [Description] = ''' + @Client + ''') as [EORI]
			,(SELECT IOSS FROM TAFIE_Clients WHERE [Description] = ''' + @Client + ''') as [IOSS]
			,30
			,RTRIM(sord.ref_no) as [Ref1]
			,RTRIM(load_dely.load_note) as [Ref2]
			,RTRIM(sord.price_curr) as [Curr] 
			,GETDATE() as [DT_Created]
			,GETDATE() as [Last_Updated]
			,''' + ISNULL(@User, 'UNDEFINED') + ''' as [User_Created]

--=================================================================================================================================
-- *******GET SERVICE CODES & IDS******* --
--=================================================================================================================================

			,ISNULL(
					(
						SELECT TOP 1
									RTRIM(carr_ddtl.Service_Level) as [Service_Level]
						FROM 
									' + @Connection_String + '.load_dely 
									LEFT JOIN ' + @Connection_String + '.carr_ddtl on load_dely.carrier = carr_ddtl.carrier AND load_dely.del_method = carr_ddtl.del_method
						WHERE 
									load_dely.load_note = ''' + @Load_Note + '''
					), 
					ISNULL(
					(
						SELECT TOP 1 
									RTRIM(TCAR_CMAP.service_level) 
						FROM 
									TCAR_CMAP 
						WHERE 
									TCAR_CMAP.E9_Carrier = load_dely.carrier 
									AND TCAR_CMAP.E9_Del_Method = load_dely.del_method
					), ''ER''
			)
			) as [Service_Level]
			,ISNULL(
					(
						SELECT TOP 1
									RTRIM(carr_ddtl.service_id) as [Service_Id]
						FROM 
									' + @Connection_String + '.load_dely 
									LEFT JOIN ' + @Connection_String + '.carr_ddtl on load_dely.carrier = carr_ddtl.carrier AND load_dely.del_method = carr_ddtl.del_method
						WHERE 
									load_dely.load_note = ''' + @Load_Note + '''
					), 
					ISNULL(
							(
								SELECT TOP 1 
											RTRIM(TCAR_CMAP.service_id) 
								FROM 
											TCAR_CMAP
								WHERE 
											TCAR_CMAP.E9_Carrier = load_dely.carrier 
											AND TCAR_CMAP.E9_Del_Method = load_dely.del_method
							), ''ER''
						)
			) as [Service_Id]
			,ISNULL(
					(
						SELECT TOP 1
									RTRIM(dmtd.descr) as [Service_Descr]
						FROM 
									' + @Connection_String + '.load_dely 
									LEFT JOIN ' + @Connection_String + '.dmtd on load_dely.del_method = dmtd.del_method
						WHERE 
									load_dely.load_note = ''' + @Load_Note + '''
					), 
					ISNULL(
							(
								SELECT TOP 1 
											RTRIM(TCAR_CMAP.[Description]) 
								FROM 
											TCAR_CMAP
								WHERE 
											TCAR_CMAP.E9_Carrier = load_dely.carrier 
											AND TCAR_CMAP.E9_Del_Method = load_dely.del_method
							), ''ER''
						)
			) as [Service_Descr]
			,''' + ISNULL(@Acc_Code, '') + ''' as [Acc_Code]
			,ISNULL(sord_dely.std_charge_gross, 0) as [Postal_Charges]			
FROM 
			' + @Connection_String + '.load_dely
			LEFT JOIN ' + @Connection_String + '.lnot_sord on load_dely.load_note = lnot_sord.load_note
			LEFT JOIN ' + @Connection_String + '.sord on sord.ref_no = lnot_sord.ref_no
			LEFT JOIN ' + @Connection_String + '.sord_dely on sord.ref_no = sord_dely.ref_no
WHERE 
			load_dely.load_note = ''' + @Load_Note + '''
'

--=================================================================================================================================
-- PRINT & EXECUTE SQL
--=================================================================================================================================

PRINT(@SQL)
EXECUTE(@SQL)


```

---

### TCAR_Get_Del

The `TCAR_Get_Del` stored procedure is responsible for retrieving delivery address details for a specific carrier request (`TCAR_Ref`). It dynamically constructs and executes an SQL query to extract customer address details from the Elucid database and updates the corresponding `TCAR` record. This ensures that the correct delivery information is stored and available for subsequent carrier label retrieval.

#### Input Parameters

The procedure accepts a single input parameter:
* `@TCAR_Ref` - VARCHAR(24) - Unique reference identifier for the `TCAR` record.


#### Process Flow
1. Variable Initialization
Upon execution, the procedure initializes several key variables:
* `@SQL` – Stores the dynamically generated SQL query.
* `@Connection_String` – Dynamically constructs a reference to the Elucid database based on the client associated with the `TCAR_Ref`.
* `@Client` – Retrieves the client name linked to the provided `TCAR_Ref`.
* `@Load_Note` – Retrieves the load reference number associated with the `TCAR_Ref`.

2. Retrieving Address Data
* The procedure constructs an SQL query to extract customer address details from the Elucid `cust_addr` table. The query retrieves:
  * Customer name, company name, and contact details.
  * Address components (street, city, county, country, and postcode).
  * Primary contact phone number and email address.

* To ensure clean data formatting:
  * Newline characters (CHAR(13), CHAR(10)) are removed from text fields.
  * Address fields are limited to 35 characters to comply with system constraints.
  * Phone numbers prioritise available contact methods in the following order: `phone_day`, `phone_eve`, `phone_ext`, and `mobile`.

The retrieved data is stored in a temporary table (`#TEMP_UPDATE`) for further processing.


3. Updating the `TCAR` Table
The stored procedure updates the `TCAR` table with the retrieved customer address details:

* Fields such as `Name`, `Company`, `Addr1`, `Addr2`, `City`, `County`, `Country`, `Postcode`, `Phone`, and `Email` are updated based on values from `#TEMP_UPDATE`.
* The `Last_Updated` timestamp is refreshed to reflect the latest modification.

4. Cleanup and Execution
After updating the `TCAR` table, the temporary table (`#TEMP_UPDATE`) is dropped to free memory. The dynamically constructed SQL query is then executed to complete the process.


#### Data Flow and Integration
The stored procedure interacts with multiple tables to retrieve and update customer address data:

1. Extracts customer address data from the Elucid database
* Uses `cust_addr ` table to get address details.
* References `load_dely ` table to find the corresponding `del_addr_ref ` and customer values.

2. Updates delivery details in the `TCAR `table
* Ensures `TCAR ` records have the correct and formatted address details.
* Maintains consistency in carrier request processing.

3. Ensures data integrity and efficient processing
* Uses a temporary table to hold extracted data before updating.
* Cleans up temporary storage after execution.

#### Summary

The `TCAR_Get_Del ` stored procedure is crucial for maintaining accurate delivery address information in the `TCAR ` table. By dynamically retrieving and updating address details, it ensures that carrier label requests are processed with valid and formatted customer address information. This process plays a key role in the carrier label generation workflow, ensuring that shipment details are correctly recorded and transferred to external carrier systems.

```SQL
ALTER PROC [dbo].[TCAR_Get_Del]

@TCAR_Ref VARCHAR(24)

AS

--=================================================================================================================================
-- DECLARE & SET VARIABLES
--=================================================================================================================================

DECLARE @SQL VARCHAR(MAX) = ''
DECLARE @Connection_String VARCHAR(MAX) 
DECLARE @Client VARCHAR(255) = (SELECT Client FROM TCAR WHERE TCAR_Ref = @TCAR_Ref)
DECLARE @Load_Note VARCHAR(24) = (SELECT Load_Note FROM TCAR WHERE TCAR_Ref = @TCAR_Ref)

SET @Connection_String = 
							(
								SELECT 
											'[' + [Server] + '].[' + [Database] + '].dbo' 
								FROM 
											TAFIE_Clients 
								WHERE 
											[Description] = @Client
							)


--=================================================================================================================================
-- GET DATA
--=================================================================================================================================

SET @SQL = @SQL + '


SELECT
			LEFT(ISNULL(RTRIM(cust_addr.initials) + '' '', '''') + ISNULL(RTRIM(cust_addr.full_name), ''''), 35) AS [Name]
			,LEFT(RTRIM(REPLACE(REPLACE(cust_addr.organisation, CHAR(13), ''''), CHAR(10), '''')), 35) AS [Company]
			,LEFT(RTRIM(REPLACE(REPLACE(cust_addr.postcode, CHAR(13), ''''), CHAR(10), '''')), 10) AS [Postcode]
			,LEFT(RTRIM(REPLACE(REPLACE(cust_addr.city, CHAR(13), ''''), CHAR(10), '''')), 35) AS [City]
			,LEFT(
				RTRIM(
					REPLACE(
						REPLACE(
								CASE 
										WHEN CHARINDEX(''~'', cust_addr.[address]) > 0 
										THEN LEFT(cust_addr.[address], CHARINDEX(''~'', cust_addr.[address]) - 1)
										ELSE cust_addr.[address]
								END, CHAR(13), ''''
						), CHAR(10), ''''
					)
				), 35
			) AS [Addr1]
			,LEFT(
				RTRIM(
					REPLACE(
						REPLACE(
								CASE 
										WHEN CHARINDEX(''~'', [address]) > 0 
										THEN SUBSTRING(cust_addr.[address], CHARINDEX(''~'', cust_addr.[address]) + 1, LEN(cust_addr.[address]))
										ELSE NULL
								END, CHAR(13), ''''
						), CHAR(10), ''''
					)
				), 35
			) AS [Addr2]
			,LEFT(RTRIM(REPLACE(REPLACE(cust_addr.county, CHAR(13), ''''), CHAR(10), '''')), 35) AS [County]
			,RTRIM(REPLACE(REPLACE(cust_addr.country, CHAR(13), ''''), CHAR(10), '''')) AS [Country]
			,LEFT(RTRIM(ISNULL(cust_addr.phone_day, ISNULL(cust_addr.phone_eve, ISNULL(cust_addr.phone_ext, mobile)))), 12) as [Phone]
			,LEFT(RTRIM(cust_addr.email), 35) as [Email]
			INTO #TEMP_UPDATE
FROM 
			' + @Connection_String + '.cust_addr
WHERE 
			cust_addr.address_ref = (
										SELECT 
													load_Dely.del_addr_ref 
										FROM 
													' + @Connection_String + '.load_Dely 
										WHERE 
													load_Dely.load_note = ''' + @Load_Note + '''
									)			
			AND cust_addr.customer = (
										SELECT 
													load_Dely.customer 
										FROM 
													' + @Connection_String + '.load_Dely 
										WHERE 
													load_Dely.load_note = ''' + @Load_Note + '''
									)
'

--=================================================================================================================================
-- UPDATE TCAR RECORD
--=================================================================================================================================

SET @SQL = @SQL + '

UPDATE 
		TCAR
SET
		TCAR.Name = #TEMP_UPDATE.Name
		,TCAR.Company = #TEMP_UPDATE.Company
		,TCAR.Postcode = #TEMP_UPDATE.Postcode
		,TCAR.City = #TEMP_UPDATE.City
		,TCAR.Addr1 = LEFT(#TEMP_UPDATE.Addr1, 35)
		,TCAR.Addr2 = LEFT(#TEMP_UPDATE.Addr2, 35)
		,TCAR.County = #TEMP_UPDATE.County
		,TCAR.Country = #TEMP_UPDATE.Country
		,TCAR.Phone = #TEMP_UPDATE.Phone
		,TCAR.Email = #TEMP_UPDATE.Email
		,TCAR.Last_Updated = GETDATE()
FROM 
		#TEMP_UPDATE
WHERE 
		TCAR.Load_Note = ''' + @Load_Note + '''
'

--=================================================================================================================================
-- CLEAN UP
--=================================================================================================================================

SET @SQL = @SQL + '

DROP TABLE #TEMP_UPDATE

'

--=================================================================================================================================
-- PRINT & EXECUTE SQL
--=================================================================================================================================

PRINT(@SQL)
EXECUTE(@SQL)
	


```

---

### TCAR_Get_Comp

The stored procedure `TCAR_Get_Comp` is responsible for retrieving and processing shipment data associated with a given `TCAR_Ref`. It extracts relevant details from multiple database tables, compiles the information, and inserts records into `TCAR_Line`, `TCAR_BOXES`, and `TCAR_BCON ` tables. The procedure also handles packaging calculations, ensuring that items are optimally distributed across shipping boxes according to weight constraints.

#### Functional Breakdown

* Variable Initialization
The procedure initializes several variables, including:
  * `@Client` - Extracts the client associated with the `TCAR_Ref`.
  * `@Load_Note` - Retrieves the load note reference.
  * `@User` - Identifies the user who created the transaction.
  * `@Connection_String` - Constructs the connection string dynamically to query the appropriate database.

* Data Extraction and Insertion into `TCAR_Line`</strong>
A dynamic SQL query retrieves shipment line details from the corresponding client database and inserts them into `TCAR_Line`. The data includes:
  * Part number 
  * description
  * Product Group.
  * Quantity and weight calculations.
  * Country of origin and harmonized system (HS) codes.
  * Financial values associated with each line item.

* Box Assignment Logic
The procedure determines how shipment items should be distributed into packaging boxes, ensuring compliance with weight constraints.
Single Box Assignment:
  * If the total load weight is within the predefined maximum (`@Max_Box_Weight`), all items are packed into a single box.
  * A record is inserted into `TCAR_BOXES`, including weight, value, and dimensions.
  * `TCAR_BCON ` is updated to track box contents.

Multiple Box Assignment:
  * If the load exceeds `@Max_Box_Weight`, items are distributed across multiple boxes.
  * A cursor iterates through `TCAR_Line ` records, sorting by item weight in descending order.
  * Items are allocated to boxes while ensuring that each box remains within the weight limit.
  * The procedure dynamically creates new box records and updates the corresponding weight and value calculations.

* Execution and Logging
The constructed SQL query is printed for debugging and then executed dynamically. The procedure maintains accurate timestamps (`DT_Created`, `Last_Updated`) and user tracking (`Last_Upd_User`).

#### Summary

The `TCAR_Get_Comp `stored procedure is designed to retrieve and process shipment data for a given transaction reference (`TCAR_Ref`). It dynamically queries the appropriate client database, extracts relevant shipment details, and inserts the data into the `TCAR_Line ` table. The procedure then determines optimal packaging by assigning items to boxes based on weight constraints, ensuring that no box exceeds the predefined maximum weight. If a single box is sufficient, the entire shipment is packed together; otherwise, items are distributed across multiple boxes. The procedure utilizes dynamic SQL for flexible database access and a cursor-based approach for iterative box assignment. It ensures data integrity by maintaining accurate timestamps and user tracking while executing structured inserts and updates.

```SQL
ALTER PROC [dbo].[TCAR_Get_Comp]

@TCAR_Ref VARCHAR(24)

AS

--=================================================================================================================================
-- DECLARE & SET VARIABLES
--=================================================================================================================================

DECLARE @SQL VARCHAR(MAX) = ''
DECLARE @Connection_String VARCHAR(MAX) 
DECLARE @Client VARCHAR(255) = (SELECT TCAR.Client FROM TCAR WHERE TCAR.TCAR_Ref = @TCAR_Ref)
DECLARE @Load_Note VARCHAR(24) = (SELECT TCAR.Load_Note FROM TCAR WHERE TCAR.TCAR_Ref = @TCAR_Ref)
DECLARE @User VARCHAR(24) = (SELECT TCAR.User_Created FROM TCAR WHERE TCAR.TCAR_Ref = @TCAR_Ref)

SET @Connection_String = 
							(
								SELECT 
											'[' + [Server] + '].[' + [Database] + '].dbo' 
								FROM 
											TAFIE_Clients 
								WHERE 
											[Description] = @Client
							)

--=================================================================================================================================
-- GET & INSERT DATA [TCAR_Line]
--=================================================================================================================================

SET @SQL = '

INSERT INTO [TCAR_Line] 
(
	[TCAR_Ref]
	,[Line]
	,[Part]
	,[Description]
	,[Type]
	,[Line_Value]
	,[Qty]
	,[Line_Weight]
	,[CoO]
	,[HS_Code]
	,[DT_Created]
	,[Last_Updated]
	,[Last_Upd_User]
)

SELECT 
			''' + @TCAR_Ref + '''
			,RTRIM(Load_line.line)
			,RTRIM(part.part)
			,RTRIM(part.descr)
			,LEFT(ISNULL(RTRIM(pgrp.descr), RTRIM(part.descr)), 35)
			,CAST(sord_line.total_value AS numeric(14,2))
			,COALESCE(CAST(load_line.qty AS float), 0)
			,CAST(((ISNULL(part_dims.weight, 0) / 1000) * ISNULL(load_line.qty, 0)) AS numeric(17,4))
			,RTRIM(part_ctro.country)
			,RTRIM(part_hscd.harmonised_code)
			,GETDATE()
			,GETDATE()
			,''' + @User + '''
FROM 
			' + @Connection_String + '.load_dely
			INNER JOIN ' + @Connection_String + '.load_line ON load_dely.load_note = load_line.load_note 
			INNER JOIN ' + @Connection_String + '.sord_load ON load_line.load_note = sord_load.load_note AND load_line.line = sord_load.load_line 
			INNER JOIN ' + @Connection_String + '.sord_line ON sord_load.ref_no = sord_line.ref_no AND sord_load.dely_ref = sord_line.dely_ref AND sord_load.line = sord_line.line 
			INNER JOIN ' + @Connection_String + '.sord ON sord_line.ref_no = sord.ref_no 
			INNER JOIN ' + @Connection_String + '.part ON sord_line.part = part.part 
			LEFT JOIN ' + @Connection_String + '.part_dims ON part.part = part_dims.part 
			LEFT JOIN ' + @Connection_String + '.part_hscd ON part.part = part_hscd.part 
			LEFT JOIN ' + @Connection_String + '.part_ctro ON part.part = part_ctro.part 
			LEFT JOIN ' + @Connection_String + '.pgrp ON part.prod_group = pgrp.prod_group
WHERE 
			load_dely.load_note = ''' + @Load_Note + ''''

--=================================================================================================================================
-- PRINT & EXECUTE SQL
--=================================================================================================================================

PRINT(@SQL)
EXECUTE(@SQL)

--=================================================================================================================================
-- GET & INSERT BOX DATA
--=================================================================================================================================

DECLARE @Service_Level VARCHAR(12) = (SELECT TOP 1 Service_Level FROM TCAR WHERE TCAR_Ref = @TCAR_Ref)
DECLARE @Service_Id VARCHAR(12) = (SELECT TOP 1 Service_Id FROM TCAR WHERE TCAR_Ref = @TCAR_Ref)
DECLARE @Max_Box_Weight NUMERIC(18,2) = (SELECT TOP 1 Weight_Limit FROM TCAR_CMAP WHERE Service_Level = @Service_Level AND Service_Id = @Service_Id)
DECLARE @Total_Load_Weight NUMERIC(18,2) = (SELECT SUM(ISNULL([Line_Weight], 0)) FROM TCAR_Line WHERE TCAR_Ref = @TCAR_Ref)
DECLARE @Remaining_Weight NUMERIC(18,2) = @Total_Load_Weight
DECLARE @Box_Num INT = 1
DECLARE @Current_Box_Weight NUMERIC(18,2) = 0

--=================================================================================================================================
-- GET & INSERT BOX DATA - 1.0
--=================================================================================================================================

IF @Total_Load_Weight + 0.2 <= @Max_Box_Weight
	BEGIN
		INSERT INTO TCAR_BOXES -- INSERT SINGLE BOX IF WITHIN LIMIT
		(
			[TCAR_Ref]
			,[Box_ref]
			,[Weight]
			,[Value]
			,[Qty]
			,[Dim_L]
			,[Dim_W]
			,[Dim_H]
			,[DT_Created]
			,[Last_Updated]
			,[Last_Upd_User]
		)

		SELECT
					@TCAR_Ref
					,'1'
					,SUM(ISNULL([Line_Weight], 0)) + 0.2 -- ACCOUNT FOR PACKAGING WEIGHT
					,SUM(ISNULL([Line_Value], 0))
					,SUM(ISNULL([Qty], 0))
					,'1'
					,'1'
					,'1'
					,GETDATE()
					,GETDATE()
					,@User
		FROM 
					TCAR_Line 
		WHERE 
					TCAR_Ref = @TCAR_Ref

--=================================================================================================================================
-- GET & INSERT BOX DATA - 1.1
--=================================================================================================================================

		INSERT INTO TCAR_BCON -- INSERT BOX CONTENTS
		(
			[TCAR_Ref]
			,[Box_ref]
			,[Part]
			,[Qty]
			,[DT_Created]
			,[Last_Updated]
			,[Last_Upd_User]
		)
		SELECT
					@TCAR_Ref
					,'1'
					,Part
					,ISNULL(Qty, 1)
					,GETDATE()
					,GETDATE()
					,@User
		FROM 
					TCAR_Line 
		WHERE 
					TCAR_Ref = @TCAR_Ref
	END

--=================================================================================================================================
-- GET & INSERT BOX DATA - 2.0
--=================================================================================================================================

-- CREATE MULTIPLE BOXES IS NOT WITHIN LIMIT
ELSE
	BEGIN
		DECLARE cur CURSOR FOR 

		SELECT 
					[Part]
					,ISNULL([Line_Weight], 0)
					,ISNULL([Part_Value], 0)
					,ISNULL([Qty], 0)
		FROM 
					TCAR_Line 
		WHERE 
					TCAR_Ref = @TCAR_Ref
		ORDER BY 
					[Line_Weight] DESC  

		DECLARE @Part NVARCHAR(50)
		DECLARE @Part_Weight NUMERIC(14,2)
		DECLARE @Part_Value NUMERIC(14,2)
		DECLARE @Part_Qty NUMERIC(14,2)
		DECLARE @Weight_Per_Unit NUMERIC(18,2)
		DECLARE @Qty_To_Box NUMERIC(14,2)

--=================================================================================================================================
-- GET & INSERT BOX DATA - 2.1 - OPEN CURSOR
--=================================================================================================================================

		OPEN cur
		FETCH NEXT FROM cur INTO @Part, @Part_Weight, @Part_Value, @Part_Qty

		WHILE @@FETCH_STATUS = 0

		BEGIN
			
			SET @Weight_Per_Unit = (SELECT SUM(ISNULL([Part_Weight], 0)) FROM TCAR_Line WHERE TCAR_Ref = @TCAR_Ref AND part = @Part)
			-- DISTRIBUTE ITEMS ACROSS MULTI BOXES IF NEEDED
			SET @Qty_To_Box = @Part_Qty

			WHILE @Qty_To_Box > 0

--=================================================================================================================================
-- GET & INSERT BOX DATA - 2.2
--=================================================================================================================================

			BEGIN
				-- IF NEW BOX NEEDED
				IF @Current_Box_Weight = 0 OR @Current_Box_Weight + (@Qty_To_Box * @Weight_Per_Unit) > @Max_Box_Weight
					BEGIN                    
						INSERT INTO TCAR_BOXES -- INSERT NEW BOX RECORD
						(
							[TCAR_Ref]
							,[Box_ref]
							,[Weight]
							,[Value]
							,[Qty]
							,[Dim_L]
							,[Dim_W]
							,[Dim_H]
							,[DT_Created]
							,[Last_Updated]
							,[Last_Upd_User]
						)
						VALUES
						(
							@TCAR_Ref
							,@Box_Num
							,0 --PLACEHOLDER
							,0 --PLACEHOLDER
							,0 --PLACEHOLDER
							,'1'
							,'1'
							,'1'
							,GETDATE()
							,GETDATE()
							,@User
						)

						SET @Current_Box_Weight = 0
						SET @Box_Num = @Box_Num + 1
					END

--=================================================================================================================================
-- GET & INSERT BOX DATA - 2.3
--=================================================================================================================================

				-- DETERMINE HOW MUCH CAN FIT IN CURRENT BOX
				DECLARE @Max_Qty_In_Box NUMERIC(14,2)
				SET @Max_Qty_In_Box = FLOOR((@Max_Box_Weight - @Current_Box_Weight) / @Weight_Per_Unit)

				IF @Max_Qty_In_Box > @Qty_To_Box OR @Max_Qty_In_Box <= 0
				SET @Max_Qty_In_Box = @Qty_To_Box

				INSERT INTO TCAR_BCON -- INSERT CONTENTS RECORD
				(
					[TCAR_Ref]
					,[Box_ref]
					,[Part]
					,[Qty]
					,[DT_Created]
					,[Last_Updated]
					,[Last_Upd_User]
				)
				VALUES
				(
					@TCAR_Ref
					,@Box_Num - 1
					,@Part
					,@Max_Qty_In_Box
					,GETDATE()
					,GETDATE()
					,@User
				)

--=================================================================================================================================
-- GET & INSERT BOX DATA - 2.4
--=================================================================================================================================

				-- UPDATE BOX TOTALS
				UPDATE 
						TCAR_BOXES
				SET 
						[Weight] = [Weight] + (@Max_Qty_In_Box * @Weight_Per_Unit)
						,[Value] = [Value] + CAST((@Part_Value* @Max_Qty_In_Box) AS NUMERIC(14,2))
						,[Qty] = [Qty] + @Max_Qty_In_Box
				WHERE 
						TCAR_Ref = @TCAR_Ref 
						AND Box_ref = @Box_Num - 1
				
				-- ADJUST COUNTERS
				SET @Current_Box_Weight = @Current_Box_Weight + (@Max_Qty_In_Box * @Weight_Per_Unit)
				SET @Qty_To_Box = @Qty_To_Box - @Max_Qty_In_Box

				-- IF BOX IS FULL, RESET FOR NEW BOX
				IF @Current_Box_Weight >= @Max_Box_Weight
				SET @Current_Box_Weight = 0
			END

--=================================================================================================================================
-- GET & INSERT BOX DATA - 2.5 - FETCH NEXT FROM CURSOR
--=================================================================================================================================

			FETCH NEXT FROM cur INTO @Part, @Part_Weight, @Part_Value, @Part_Qty
		END

		CLOSE cur
		DEALLOCATE cur
	END


```

---

### TCAR_Get_Ref

The `TCAR_Get_Ref` stored procedure is responsible for generating a unique reference number (`TCAR_Ref`) for a new shipment request within the system. This reference number follows a standardised format that includes the client identifier and an incrementing numeric sequence.

#### Procedure Logic
1. Client Identification:
* The procedure takes a single input parameter, `@Client`, which represents the name of the client.
* It queries the `TAFIE_Clients`table to retrieve the corresponding ID for the given client name.
2. Reference Number Generation:
* The procedure retrieves the highest numerical sequence used in existing `TCAR_Ref ` values by extracting the last six digits from previously recorded references in the `TCAR`table.
* If no previous records exist, it defaults to 000000, ensuring that the first generated reference starts from 000001.
* The retrieved number is incremented by 1 to maintain sequential order.
3. Formatting the Reference:
* The new `TCAR_Ref` is constructed using the following format: TCAR-[Client_ID-6]-[digit sequence]
* The numeric sequence is left-padded with zeros to maintain a six-digit length.
4. Output:
* The generated reference is returned as a single-column result set.

#### Summary

The `TCAR_Get_Ref` stored procedure generates a unique reference number (`TCAR_Ref`) for new transactions by combining the client ID with a sequential six-digit number. It first retrieves the client’s unique identifier from the `TAFIE_Clients`table, then determines the highest existing sequence number in the `TCAR ` table and increments it. The final reference is formatted as TCAR-&lt;Client_ID>-&lt;6-digit sequence> and returned as output. This procedure ensures systematic numbering, prevents duplication, and maintains consistency in transaction tracking.

```SQL
ALTER PROC [dbo].[TCAR_Get_Ref]

@Client VARCHAR(255)

as

DECLARE @Client_Id INT = (SELECT ID FROM TAFIE_Clients WHERE [Description] = @Client)
DECLARE @Number INT = 
		ISNULL(
				(
					SELECT TOP 1 
								CAST(RIGHT(TCAR_Ref, 6) AS INT) 
					FROM 
								TCAR 
					ORDER BY 
								CAST(RIGHT(TCAR_Ref, 6) AS INT) DESC
				), 0
		) + 1

DECLARE @TCAR_Ref VARCHAR(24) = 'TCAR-' + RTRIM(@Client_Id) + '-' + RIGHT(CONCAT('000000', @Number), 6)

SELECT @TCAR_Ref as [TCAR_Ref]


```

---

### TCAR_Get_Results

The `TCAR_Get_Results` stored procedure retrieves data related to a specific `TCAR_Ref`, based on the mode of operation specified by the `@Mode` parameter. It enables the extraction of relevant information from different tables within the database, supporting various functional requirements of the system.

#### Functionality
The procedure operates in four distinct modes:

* Retrieving Form Field Data (`@Mode = 1`)
  * Retrieves shipment-related details from the `TCAR` table, including carrier information, delivery method, consignee details (name, address, and contact information), customs-related information (EORI, IOSS), and customer reference numbers.
  * The country name is derived using a lookup from the `TAFIE_Ctry` table.

* Retrieving Item Line Data for a Data Grid (`@Mode = 2`)
  * Fetches itemized shipment details from the `TCAR_Line` table, including part number, description, quantity, weight, and value.
  * This mode is useful for displaying a breakdown of the shipment contents in a tabular format.

* Retrieving Box Summary Data (`@Mode = 3`)
  * Aggregates packaging data from the `TCAR_BOXES` table, calculating total weight, total value, total volume, and the number of boxes used for the shipment.
  * This provides a high-level overview of the shipment’s physical characteristics.

* Retrieving Shipment Contents (`@Mode = 4`)
  * Extracts detailed information about the contents of each box, including the box reference, part number, description, and quantity.
  * Joins data from the `TCAR_BOXES` and `TCAR_BCON` tables to map each part to its respective box.

This stored procedure enhances data retrieval efficiency by structuring queries based on operational needs. It provides flexibility by allowing different levels of shipment details to be fetched dynamically.

#### Summary

The `TCAR_Get_Results` stored procedure retrieves shipment-related data based on the specified mode. It supports fetching form fields, item line details, packaging summaries, and shipment contents, ensuring efficient data access for various operational requirements. The procedure optimizes database queries by retrieving only the necessary information based on the provided `TCAR_Ref` and Mode.

```SQL
ALTER PROC [dbo].[TCAR_Get_Results]

@TCAR_Ref VARCHAR(24)
,@Mode INT

AS

--===================================================================================================
-- GET DATA FROM FORM FIELDS
--===================================================================================================

IF @Mode = 1

	BEGIN

		SELECT 
					[Carrier]
					,[Del_Method]
					,[Service_Descr]
					,[Inco]
					,[EORI]
					,[IOSS]
					,[Name]
					,[Company]
					,[Postcode]
					,[city]
					,[Addr1]
					,[Addr2]
					,[County]
					,(SELECT [Description] FROM TAFIE_Ctry WHERE TAFIE_Ctry.Ctry_Code = TCAR.[Country])as [Country]
					,[Email]
					,[Phone]
					,[Ref1]
					,[Ref2]
					,[Acc_Code] 
		FROM 
					TCAR
		WHERE 
					TCAR_Ref = @TCAR_Ref

	END

--===================================================================================================
-- GET DATA FOR DATAGRID
--===================================================================================================

ELSE IF @Mode = 2

	BEGIN

		SELECT
					[Part]
					,[Description]
					,[Qty]
					,[Line_Weight]
					,[Line_Value]
					,[HS_Code]
					,[CoO]
					,[Type]
					,CASE 
						WHEN [Type] IS NULL OR [Type] = '' THEN 1
						WHEN [CoO] IS NULL OR [CoO] = '' THEN 1
						WHEN [HS_Code] IS NULL OR [HS_Code] = '' THEN 1
						WHEN COALESCE(Part_Value, 0) < 0 OR COALESCE(Part_Value, 0) > 99999 THEN 2
						WHEN COALESCE(Line_Value, 0) < 0 OR COALESCE(Line_Value, 0) > 99999 THEN 2
						WHEN COALESCE(Line_Weight, 0) < 0 OR COALESCE(Line_Weight, 0) > 99999 THEN 1
						WHEN COALESCE(Qty, 0) < 1 OR COALESCE(Qty, 0) > 99999 THEN 1
						WHEN LEN(COALESCE([Description], '')) NOT BETWEEN 1 AND 35 THEN 2
						WHEN LEN(COALESCE([Type], '')) NOT BETWEEN 1 AND 35 THEN 1
						WHEN LEN(COALESCE([HS_Code], '')) NOT BETWEEN 1 AND 13 THEN 1
						ELSE 0
					END as [Error]
		FROM 
					TCAR_Line
		WHERE 
					TCAR_Ref = @TCAR_Ref

	END

--===================================================================================================
-- GET DATA FOR BOXES
--===================================================================================================

ELSE IF @Mode = 3

	BEGIN 

		SELECT 
					SUM(ISNULL([Weight], 0)) as [Total_Weight]
					,SUM(ISNULL(Value, 0)) as [Total_Value]
					,SUM(Dim_L * Dim_H * Dim_L) as [Volume]
					,COUNT(Box_Ref) as [Boxes]
					,ISNULL((SELECT TOP 1 DDP FROM TCAR_CMAP WHERE TCAR.Service_Level = TCAR_CMAP.Service_Level), 0) as [DDP]
					,ISNULL(
							CASE 
									WHEN ISNULL(TCAR.Country, 'GB') != 'GB' THEN 0
									ELSE 1
							END, 1
					) as [Domestic]
		FROM 
					TCAR
					LEFT JOIN TCAR_BOXES on TCAR.TCAR_Ref = TCAR_BOXES.TCAR_Ref
		WHERE 
					TCAR.TCAR_Ref = @TCAR_Ref
		GROUP BY
					TCAR.TCAR_Ref
					,TCAR.Country
					,TCAR.Service_Level

	END 


--===================================================================================================
-- GET DATA FOR SHIPMENT CONTENTS
--===================================================================================================

ELSE IF @Mode = 4

	BEGIN 

		SELECT 
					TCAR_BOXES.Box_Ref
					,TCAR_BCON.Part
					,(
						SELECT TOP 1 
									[Description] 
						FROM 
									TCAR_Line 
						WHERE 
									TCAR_Line.part = TCAR_BCON.Part 
									AND TCAR_Line.TCAR_Ref = TCAR_BOXES.TCAR_Ref
					) as [Description]
					,TCAR_BCON.Qty							
		FROM 
					TCAR_BOXES
					LEFT JOIN TCAR_BCON on TCAR_BOXES.Box_Ref = TCAR_BCON.Box_Ref AND TCAR_BOXES.TCAR_Ref = TCAR_BCON.TCAR_Ref					
		WHERE 
					TCAR_BOXES.TCAR_Ref = @TCAR_Ref
	END 

```

---

### TCAR_GET_XML
The `TCAR_GET_XML` stored procedure generates structured XML-like shipment data for a given `TCAR_Ref`. It consolidates shipping, customs, and package details from multiple tables into a single result set, allowing seamless integration with the application to construct the label request XML

#### Functionality
This procedure retrieves data from `TCAR`, `TCAR_Line`, and `TCAR_BOXES` tables and structures it into a format suitable for XML-based data exchange. The key sections generated include:

* Shipment Details
  * Extracts the account reference (`API_Acc`), references (`Ref1`, `Ref2`), and a brief contents description.
  * Includes shipment manifestation status (HasBeenManifested).

* Collection Details
  * Captures the collection date and time (CollectionDate, CollectionReadyTime) and specifies the latest pickup time (LocationCloseTime).

* Delivery Address
  * Retrieves recipient details such as contact name, email, phone number, full address, and country.
  * Classifies the address as Residential.

* Customs Declaration Information
  * Defines trade terms (TermsOfTrade) and categorizes the shipment (CategoryOfItem).

* Service Information
  * Identifies the service provider (ServiceId, ServiceCustomerUID, ServiceProviderId).

* Package Details
  * Extracts physical package attributes (PackageType, dimensions, weight, and value).
  * Defines the package contents and its customs declaration (PackageCustomsDeclaration).

* Item-Level Declaration
  * Provides SKU (ProductSKU), description, type, value, and weight of individual products in the shipment.
  * Specifies country of origin (ProductCountryOfOrigin) and harmonized code (ProductHarmonisedCode).
  * Determines item quantity per package dynamically using a subquery.

This stored procedure is crucial for generating structured shipping data, ensuring efficient integration with logistics providers and customs authorities.

#### Summary

The `TCAR_GET_XML` stored procedure retrieves and formats shipment-related data into a structured XML-compatible format. It organizes essential details such as collection information, delivery address, service specifications, package attributes, and item-level customs declarations. This procedure facilitates automated shipment processing and integration with external logistics systems. 

```SQL
ALTER PROC[dbo].[TCAR_GET_XML_V2]

@TCAR_Ref VARCHAR(24)
,@Box_Ref INT

as

--==============================================================================================================================================================
-- XML PATH
--==============================================================================================================================================================



SELECT
			--<Shipment>
				RTRIM([API_Acc]) as [Account]
				--<CollectionDetails>
					,CAST(GETDATE() as date) as [CollectionDate]
					,CAST(GETDATE() as time) as [CollectionReadyTime]
					,'17:00:00' as [LocationCloseTime]
				--</CollectionDetails>
				--<DeliveryAddress>
					,RTRIM(Name) as [ContactName]
					,RTRIM(email) as [Email]
					,RTRIM(Phone) as [Phone]
					,RTRIM(Addr1) as [Address1]
					,RTRIM(addr2) as [Address2]
					,RTRIM(City) as [City]
					,RTRIM(County) as [Area]
					,RTRIM(Postcode) as [Postcode]
					,RTRIM(Country) as [Country]
					,'Residential' as [AddressType]
				--</DeliveryAddress>
				,RTRIM(Ref1) as [Reference1]
				,RTRIM(Ref2) as [Reference2]
				,'' as [SpecialInstructions]
				,'Goods' as [ContentsDescription]
				,RTRIM(Curr) as [CurrencyCode]
				,'false' as [HasBeenManifested]
				--<CustomsDeclarationInfo>
					,CASE 
							WHEN (SELECT TOP 1 DDP FROM TCAR_CMAP WHERE TCAR.Service_Level = TCAR_CMAP.Service_Level) = 1 THEN 'DutiesAndTaxesPaid'
							ELSE 'DutiesAndTaxesUnpaid'
					END as [TermsOfTrade]
					,'Sold' as [CategoryOfItem]
					,ISNULL(TCAR.Postal_Charges, 0) as PostalCharges
					,'' as CarriageValue
					,'' as InsuranceValue
					,'' as RecipientTaxID
					,RTRIM(Country) as RecipientTaxIDCountry
					,RTRIM(TCAR.IOSS) as IOSSNumber
					,RTRIM(Country) as IOSSNumberCountry
					,RTRIM(EORI) as RecipientVATNumber
					,RTRIM(Country) as RecipientVATNumberCountry
					,'None' as DutyBillingTerm
				--<CustomsDeclarationInfo>
				--<ServiceInfo>
					,RTRIM(Service_Level) as [ServiceId]
					,RTRIM(ISNULL(Customer_UID, '00000')) as [ServiceCustomerUID]			
					,RTRIM(Service_Id) as [ServiceProviderId]
				--</ServiceInfo>
				--<Packages>
					--<Package>
						,'Parcel' as [PackageType]
						--<Dimensions>
							,CAST(TCAR_BOXES.Dim_L as int) as [Length]
							,CAST(TCAR_BOXES.Dim_W as int) as [Width]
							,CAST(TCAR_BOXES.Dim_H as int) as [Height]
						--</Dimensions>
						,CAST(ISNULL(TCAR_BOXES.[Weight], 0.1) as numeric(14,4)) as [Weight]
						,CAST(ISNULL(TCAR_BOXES.Value, 0) as numeric(14,2)) as [Value]
						,'Goods' as [Contents]
						--<PackageCustomsDeclaration>
							,CAST(ISNULL(TCAR_BOXES.Qty, 0) as int) as [Qty]
							,CAST(TCAR_BOXES.Value as numeric(14,2)) as [Value]
						--</PackageCustomsDeclaration>
						--<ItemLevelDeclaration>
							,RTRIM(TCAR_Line.Part) as [ProductSKU]
							,RTRIM(TCAR_Line.[Description]) as [ProductDescription]
							,RTRIM(TCAR_line.[Type]) as [ProductType]
							,CAST(TCAR_line.Part_Value as numeric(14,2)) as [ProductValue]
							--,CAST(TCAR_BCON.Qty as int) as [ProductQuantity]
							,ISNULL(
									(
										SELECT TOP 1
													TCAR_BCON.Qty 
										FROM 
													TCAR_BCON 
										WHERE 
													TCAR_BCON.part = TCAR_Line.part 
													AND TCAR_BOXES.TCAR_Ref = TCAR_BCON.TCAR_Ref 
													AND TCAR_BOXES.Box_Ref = TCAR_BCON.Box_Ref
													AND TCAR_BCON.Box_Ref = @Box_Ref
									), TCAR_Line.Qty
							) as [ProductQuantity]
							,CAST(TCAR_line.Part_Weight as numeric(14,2)) as [ProductWeight]
							,RTRIM(TCAR_Line.CoO) as [ProductCountryOfOrigin]
							,RTRIM(TCAR_Line.HS_Code) as [ProductHarmonisedCode]
						--</ItemLevelDeclaration>
					--</Package>
				--</Packages>
			--</Shipment>
FROM 
			TCAR
			INNER JOIN TCAR_Line ON TCAR_Line.TCAR_Ref = TCAR.TCAR_Ref
			INNER JOIN TCAR_BOXES ON TCAR_BOXES.TCAR_Ref = TCAR.TCAR_Ref		
			INNER JOIN TCAR_BCON ON TCAR.TCAR_Ref = TCAR_BCON.TCAR_Ref AND TCAR_BOXES.Box_Ref = TCAR_BCON.Box_Ref AND TCAR_line.Part = TCAR_BCON.Part	
WHERE 
			TCAR.TCAR_Ref = @TCAR_Ref
			AND TCAR_BOXES.Box_Ref = @Box_Ref

```

---

### TCAR_INSERT_XML
The `TCAR_INSERT_XML` stored procedure handles the insertion of API request and response data related to `TCAR`transactions. It processes XML messages received from the application and categorises them based on the mode of operation. The procedure inserts records into the `TCAR_Call` and `TCAR_Resp` tables while maintaining a status log for API interactions.

#### Functionality
This procedure operates in three modes:

* Mode 1 - API Call Logging
  * Inserts a record into the `TCAR_Call` table when an API request is made.
  * Stores the transaction key (`tkey`), reference (`TCAR_Ref`), user, XML message, and assigns a default status of 200 (OK).

* Mode 2 - Successful API Response Handling
  * Inserts a record into the `TCAR_Resp` table when a successful API response is received.
  * Stores the same attributes as in Mode 1, with a 200 status code indicating success.

* Mode 3 - Failed API Response Handling
  * Inserts a failure record into the `TCAR_Resp` table with a 400 (Bad Request) status.
  * Updates the corresponding request record in `TCAR_Call`, setting its status to 400 to indicate an error.

This procedure ensures structured logging of API interactions, enabling efficient tracking and troubleshooting of transaction requests and responses.

#### Summary

The `TCAR_INSERT_XML` stored procedure logs API requests and responses for `TCAR` transactions. It supports three modes: logging API calls, recording successful responses, and handling failed responses. The procedure ensures accurate tracking of API interactions by inserting records into `TCAR_Call` and `TCAR_Resp` tables and updating failure statuses when necessary.

```SQL
ALTER PROC [dbo].[TCAR_INSERT_XML]

@tkey VARCHAR(255)
,@TCAR_Ref VARCHAR(24)
,@User VARCHAR(100)
,@XML_String NVARCHAR(MAX)
,@Mode Int
as

--============================================================
-- MODE 1 API CALL
--============================================================

IF @Mode = 1
	BEGIN

		INSERT INTO [TCAR_Call]
		(
			[tkey]
			,[TCAR_Ref]
			,[Status]
			,[DT_Created]
			,[User_Created]
			,[XML_MESSAGE]
		)
		VALUES
		(
			@tkey
			,@TCAR_Ref
			,200
			,GETDATE()
			,@User
			,@XML_String
		)

	END

--============================================================
-- MODE 2 API RESPONSE (SUCCESS)
--============================================================

ELSE IF @Mode = 2

	BEGIN

		INSERT INTO [TCAR_Resp]
		(
			[tkey]
			,[TCAR_Ref]
			,[Status]
			,[DT_Created]
			,[User_Created]
			,[XML_MESSAGE]
		)
		VALUES
		(
			@tkey
			,@TCAR_Ref
			,200
			,GETDATE()
			,@User
			,@XML_String
		)

	END

--============================================================
-- MODE 3 API RESPONSE (FAIL)
--============================================================

ELSE IF @Mode = 3

	BEGIN

		INSERT INTO [TCAR_Resp]
		(
			[tkey]
			,[TCAR_Ref]
			,[Status]
			,[DT_Created]
			,[User_Created]
			,[XML_MESSAGE]
		)
		VALUES
		(
			@tkey
			,@TCAR_Ref
			,400
			,GETDATE()
			,@User
			,@XML_String
		)

		-- UDPATE CALL RECORD --

		UPDATE 
				[TCAR_Call]
		SET 
				[Status] = 400
		WHERE 
				[tkey] = @tkey

	END



```

---

### TCAR_Save
The SQL stored procedure, named `TCAR_Save`, is designed to update records in the `TCAR` table based on a given reference (`TCAR_Ref`) and several parameters. The stored procedure updates various details related to a specific entry in the `TCAR` table. The procedure accepts multiple parameters such as carrier details, service, customer information, and additional reference fields. If any parameter is not provided (i.e., it's null or defaults to "X"), the corresponding field in the `TCAR` table is set to NULL.

#### Functionality
* Variable Declarations and Setup:
  * The procedure begins by querying the `TCAR` table to retrieve the client and load note information related to the provided `TCAR_Ref`.
  * It also retrieves a `Client_Id ` from the `TAFIE_Clients` table, which maps the client description to an ID.
  * A dynamic `@Connection_String` is constructed based on the client's details, allowing the procedure to reference a specific Elucid database for the client.

* Dynamic SQL Construction:
  * The procedure constructs a dynamic SQL query (`@SQL`) using the input parameters. This query updates the fields in the `TCAR` table where the `TCAR_Ref` matches the given value.
  * For each field to be updated, the procedure checks whether the input is valid. If the input is X (representing no value), the field will be set to NULL. Otherwise, the field is updated with the provided value.

* Country Code Lookup:
  * The procedure looks up a country code from the `ctry ` table in the dynamically determined database schema based on the provided `@Country`.

* Service Information:
  * It also retrieves additional service-related information from the `TCAR_CMAP` table. Specifically, it looks for a `service_id`, `service_level`, and `service_description` based on the carrier and delivery method.
  * If no matching information is found, default values ('ER') are used.

* Execution:
  * After constructing the dynamic SQL statement, the procedure prints the generated SQL for debugging or auditing purposes.
  * Finally, the dynamic SQL is executed using EXECUTE(`@SQL`).

#### Summary

The `TCAR_Save` stored procedure is a flexible method for updating customer and service-related information in the `TCAR` table, with the added ability to adapt to different clients' schemas and perform conditional updates based on the input parameters.

```SQL
ALTER PROC [dbo].[TCAR_Save]

@TCAR_Ref VARCHAR(24)
,@Carrier VARCHAR(24)
,@Service VARCHAR(24)
,@Inco VARCHAR(24)
,@Eori VARCHAR(24)
,@IOSS VARCHAR(24)
,@Name VARCHAR(35)
,@Company VARCHAR(35)
,@Postcode VARCHAR(10)
,@City VARCHAR(35)
,@Addr1 VARCHAR(35)
,@Addr2 VARCHAR(35)
,@County VARCHAR(35)
,@Country VARCHAR(35)
,@Email VARCHAR(35)
,@Phone VARCHAR(12)
,@Ref1 VARCHAR(24)
,@Ref2 VARCHAR(24)
,@User VARCHAR(100) = ''

as

--=================================================================================================================================
-- DECLARE & SET VARIABLES
--=================================================================================================================================

DECLARE @Client VARCHAR(255) = (SELECT Client FROM TCAR WHERE TCAR_Ref = @TCAR_Ref)
DECLARE @Load_Note VARCHAR(24) = (SELECT Load_Note FROM TCAR WHERE TCAR_Ref = @TCAR_Ref)
DECLARE @SQL VARCHAR(MAX) = ''
DECLARE @Connection_String VARCHAR(MAX) 
DECLARE @Client_Id INT = (SELECT ID FROM TAFIE_Clients WHERE [Description] = @Client)

SET @Connection_String = 
							(
								SELECT 
											'[' + [Server] + '].[' + [Database] + '].dbo' 
								FROM 
											TAFIE_Clients 
								WHERE 
											[Description] = @Client
							)

--=================================================================================================================================
-- DECLARE & SET VARIABLES
--=================================================================================================================================

SET @SQL = @SQL +
'

DECLARE @Country_Code VARCHAR(12) = (SELECT country FROM ' + @Connection_String + '.ctry WHERE descr = ''' + ISNULL(@Country,'') + ''')

UPDATE 
		[TCAR]
SET
		[carrier] = CASE WHEN ''' + ISNULL(@Carrier, 'X') + ''' = ''X'' THEN NULL ELSE ''' + ISNULL(@Carrier, '') + ''' END
		,[Del_Method] = CASE WHEN ''' + ISNULL(@Service, 'X') + ''' = ''X'' THEN NULL ELSE ''' + ISNULL(@Service, '') + ''' END
		,[Inco] = CASE WHEN ''' + ISNULL(@Inco, 'X') + ''' = ''X'' THEN NULL ELSE ''' + ISNULL(@Inco, '') + ''' END
		,[EORI]= CASE WHEN ''' + ISNULL(@Eori, 'X') + ''' = ''X'' THEN NULL ELSE ''' + ISNULL(@Eori, '') + ''' END
		,[IOSS]= CASE WHEN ''' + ISNULL(@IOSS, 'X') + ''' = ''X'' THEN NULL ELSE ''' + ISNULL(@IOSS, '') + ''' END
		,[Name] = CASE WHEN ''' + ISNULL(@Name, 'X') + ''' = ''X'' THEN NULL ELSE ''' + ISNULL(@Name, '') + ''' END
		,[Company] = CASE WHEN ''' + ISNULL(@Company, 'X') + ''' = ''X'' THEN NULL ELSE ''' + ISNULL(@Company, '') + ''' END
		,[Postcode] = CASE WHEN ''' + ISNULL(@Postcode, 'X') + ''' = ''X'' THEN NULL ELSE ''' + ISNULL(@Postcode, '') + ''' END
		,[City] = CASE WHEN ''' + ISNULL(@City, 'X') + ''' = ''X'' THEN NULL ELSE ''' + ISNULL(@City, '') + ''' END
		,[Addr1] = CASE WHEN ''' + ISNULL(@Addr1, 'X') + ''' = ''X'' THEN NULL ELSE ''' + ISNULL(@Addr1, '') + ''' END
		,[Addr2] = CASE WHEN ''' + ISNULL(@Addr2, 'X') + ''' = ''X'' THEN NULL ELSE ''' + ISNULL(@Addr2, '') + ''' END
		,[County] = CASE WHEN ''' + ISNULL(@County, 'X') + ''' = ''X'' THEN NULL ELSE ''' + ISNULL(@County, '') + ''' END
		,[Country] = ISNULL(@Country_Code, '''')
		,[Email] = CASE WHEN ''' + ISNULL(@Email, 'X') + ''' = ''X'' THEN NULL ELSE ''' + ISNULL(@Email, '') + ''' END
		,[Phone] = CASE WHEN ''' + ISNULL(@Phone, 'X') + ''' = ''X'' THEN NULL ELSE ''' + ISNULL(@Phone, '') + ''' END
		,[Ref1] = CASE WHEN ''' + ISNULL(@Ref1, 'X') + ''' = ''X'' THEN NULL ELSE ''' + ISNULL(@Ref1, '') + ''' END
		,[Ref2] = CASE WHEN ''' + ISNULL(@Ref2, 'X') + ''' = ''X'' THEN NULL ELSE ''' + ISNULL(@Ref2, '') + ''' END
		,[Last_Updated] = GETDATE()
		,[Last_Upd_User] = User_Created
		,[Service_Id] = ISNULL(
								(
									SELECT TOP 1 
												RTRIM(TCAR_CMAP.service_id) 
									FROM 
												TCAR_CMAP 
									WHERE 
												TCAR_CMAP.E9_Carrier = ''' + ISNULL(@Carrier, '') + '''
												AND TCAR_CMAP.E9_Del_Method = ''' + ISNULL(@Service, '') + '''
								), ''ER''
							)
		,[Service_Level] = ISNULL(
									(
										SELECT TOP 1 
													RTRIM(TCAR_CMAP.service_level) 
										FROM 
													TCAR_CMAP 
										WHERE 
													TCAR_CMAP.E9_Carrier = ''' + ISNULL(@Carrier, '') + '''
													AND TCAR_CMAP.E9_Del_Method = ''' + ISNULL(@Service, '') + '''
									), ''ER''
								)
		,[Service_Descr] = ISNULL(
									(
										SELECT TOP 1 
													RTRIM(TCAR_CMAP.[Description]) 
										FROM 
													TCAR_CMAP 
										WHERE 
													TCAR_CMAP.E9_Carrier = ''' + ISNULL(@Carrier, '') + '''
													AND TCAR_CMAP.E9_Del_Method = ''' + ISNULL(@Service, '') + '''
									), ''ER''
								)
WHERE 
		TCAR.TCAR_Ref = ''' + @TCAR_Ref + '''
'

PRINT (@SQL)
EXECUTE(@SQL)

```

---

### TCAR_Complete
The SQL stored procedure, named `TCAR_Complete`, is designed to complete a transaction or process related to a specific entry in the `TCAR` table. It involves updating various details related to the Shipment's tracking number, as well as updating related data in external tables such as `load_dely` and cust_hist in the client-specific schema. The `TCAR_Complete` procedure finalises the tracking information for a specific load (represented by `TCAR_Ref`) and propagates this data across different tables. Specifically, it updates the `TCAR` table with the tracking number, updates the related delivery information in the Elucid database (`load_dely`), and logs this activity in the customer history table (`cust_hist`).

#### Variable Declarations and Setup:
The procedure declares several variables:
* `@SQL`: A variable to store the dynamically constructed SQL queries.
* `@Connection_String`: Stores the client-specific database connection string.
* Several values (`@Client`, `@Load_Note`, `@Customer`, `@User`, `@tkey`, and ` @Site`) are retrieved from various tables (`TCAR`, `TAFIE_Clients`, `TCAR_Resp`) based on the `TCAR_Ref`.
* The `@Connection_String` is dynamically constructed using the client-specific details found in the `TAFIE_Clients` table. This allows the procedure to interact with the relevant schema for the given client.

#### Completing the `TCAR`Entry:
* The `TCAR `table is updated with the provided `@Tracking` number and changes the `status ` to 220 (Complete). The `Last_Updated ` field is also set to the current date and time.

#### Updating Elucid Database:
* `load_dely` Table:
  * A dynamic SQL query is constructed to update the `load_dely` table in the client-specific database. It sets the `carrier_ref` (tracking number) and updates the last_updated and last_upd_user fields based on the provided `@Tracking number`, `@User`, and `@Load_Note`.

* `cust_hist` Table:
  * A second dynamic SQL query is constructed to insert a new record into the `cust_hist` table in the client-specific database. This entry logs a summary of the activity that has taken place, including the generation of the tracking label. The insertion includes information such as:
    * The transaction key (`tkey`).
    * User and time-related details (`@User`, GETDATE()).
    * Activity summary and description, such as "TAFIE - Label Generated," and information about the load note and tracking number.

#### Executing the Dynamic SQL:
* The constructed dynamic SQL queries for updating the `load_dely` and `cust_hist` tables are stored in the `@SQL` variable.
* The generated SQL is printed for debugging or auditing purposes.
* Finally, the dynamic SQL is executed using EXECUTE(`@SQL`).

#### Summary:
The `TCAR_Complete` stored procedure is a crucial component for completing the label generation process in the carrier module. It updates the main `TCAR` record with tracking information, propagates this information to Elucid, and logs the activity in the customer history table. The use of dynamic SQL allows the procedure to be flexible and work with multiple clients, ensuring scalability and maintainability across different client environments.

```SQL
ALTER PROC[dbo].[TCAR_Complete_V2]

@TCAR_Ref VARCHAR(24)
,@Tracking VARCHAR(255)
,@Box_Ref INT

AS

--=================================================================================================================================
-- DECLARE & SET VARIABLES
--=================================================================================================================================

DECLARE @SQL VARCHAR(MAX) = ''
DECLARE @Connection_String VARCHAR(MAX) 
DECLARE @Client VARCHAR(255) = (SELECT TCAR.Client FROM TCAR WHERE TCAR.TCAR_Ref = @TCAR_Ref)
DECLARE @Load_Note VARCHAR(24) = (SELECT TCAR.Load_Note FROM TCAR WHERE TCAR.TCAR_Ref = @TCAR_Ref)
DECLARE @Customer VARCHAR(24) = (SELECT TCAR.Customer FROM TCAR WHERE TCAR.TCAR_Ref = @TCAR_Ref)
DECLARE @User VARCHAR(24) = (SELECT TCAR.User_Created FROM TCAR WHERE TCAR.TCAR_Ref = @TCAR_Ref)
DECLARE @tkey VARCHAR(24) = (SELECT TOP 1 tkey FROM TCAR_Resp WHERE TCAR_Resp.TCAR_Ref = @TCAR_Ref ORDER BY DT_Created DESC)
DECLARE @Site VARCHAR(24) = (SELECT [Site] FROM TAFIE_Clients WHERE [Description] = @Client)

SET @Connection_String = 
							(
								SELECT 
											'[' + [Server] + '].[' + [Database] + '].dbo' 
								FROM 
											TAFIE_Clients 
								WHERE 
											[Description] = @Client
							)

--=============================================================
-- COMPLETE TCAR
--=============================================================

UPDATE 
		TCAR
SET 
		[Tracking] = @Tracking
		,[Last_Updated] = GETDATE()
		,[Status] = 220
WHERE 
		TCAR_Ref = @TCAR_Ref

--=============================================================
-- COMPLETE TCAR_BOXES
--=============================================================

UPDATE 
		TCAR_BOXES
SET 
		[Tracking] = @Tracking
		,[Last_Updated] = GETDATE()
WHERE 
		TCAR_Ref = @TCAR_Ref
		AND Box_Ref = @Box_Ref

--=============================================================
-- CANCEL ANY PREVIOUS TCARS 1/2
--=============================================================

UPDATE 
		TCAR
SET 		
		[Last_Updated] = GETDATE()
		,[Status] = 200
WHERE 
		TCAR_Ref != @TCAR_Ref
		AND Load_Note = @Load_Note

--=============================================================
-- CANCEL ANY PREVIOUS TCARS 2/2
--=============================================================

UPDATE 
		TCAR_BOXES
SET 
		[Tracking] = 'Cancelled'
		,[Last_Updated] = GETDATE()
WHERE 
		TCAR_Ref IN 
					(
						SELECT 
									TCAR_Ref 
						FROM 
									TCAR 
						WHERE 
									TCAR_Ref != @TCAR_Ref 
									AND Load_Note = @Load_Note
									AND [status] = 200
					)

--=============================================================
-- UPDATE ELUCID (LOAD_DELY)
--=============================================================

SET @SQL = @SQL +
'

UPDATE 
		' + @Connection_String + '.[load_dely]
SET
		[load_dely].[carrier_ref] = ''' + @Tracking + '''
		,[load_dely].[last_updated] = GETDATE()
		,[load_dely].[last_upd_user] = ''' + @User + '''
WHERE 
		[load_dely].[load_note] = ''' + @Load_Note + '''
		
'

--=============================================================
-- UPDATE ELUCID (CUST_HIST)
--=============================================================

SET @SQL = @SQL +
'
INSERT INTO ' + @Connection_String + '.[cust_hist]
(
	[tkey]
	,[u_version]
	,[last_updated]
	,[last_upd_user]
	,[dt_created]
	,[level_1]
	,[level_2]
	,[activity_date]
	,[activity_time]
	,[activity_summary]
	,[customer]
	,[primary_ref]
	,[internal_contact]
	,[notes]
)
VALUES
(
	''' + @tkey + '''
	,''!''
	,GETDATE()
	,''' + @User + '''
	,GETDATE()
	,''' + @Site + '''
	,''GROUP''
	,CAST(GETDATE() as date)
	,CAST(GETDATE() as time)
	,''TAFIE - Label Generated''
	,''' + @Customer + '''
	,''' + @Load_Note + '''
	,''' + @User + '''
	,''A label has been generated for load note: ' + @Load_Note + ' with the carrier reference ' + @Tracking + '''
)

'

--=============================================================
-- PRINT & EXECUTE SQL
--=============================================================

PRINT(@SQL)
EXECUTE(@SQL)
```

---

### TCAR_INSERT_TOKN
The `TCAR_INSERT_TOKN` stored procedure is responsible for inserting authentication tokens into the `TCAR_TOKN` table. These tokens are used for API authentication and session management, ensuring secure communication with the PHUB API.

#### Functionality
* Accepts four parameters:
  * `@tkey`: A unique transaction key.
  * `@Acc_Code`: The account code associated with the tokens.
  * `@Access_Tkn`: The access token used for authentication.
  * `@Refresh_Tkn`: The refresh token used for renewing the access token.

* Inserts these values into the `TCAR_TOKN`table along with:
  * `DT_Created`: A timestamp of when the record was inserted.
  * `Status`: A default value of 20, which represents an active state.

This procedure ensures that API tokens are securely stored and tracked for authentication purposes.

#### Summary
The `TCAR_INSERT_TOKN` stored procedure stores authentication tokens in the `TCAR_TOKN` table. It records transaction keys, account codes, access tokens, and refresh tokens along with a creation timestamp and status indicator. This procedure plays a crucial role in managing secure API authentication.

```SQL
ALTER PROC [dbo].[TCAR_INSERT_TOKN]

@tkey VARCHAR(255)
,@Acc_Code VARCHAR(8)
,@Access_Tkn NVARCHAR(MAX)
,@Refresh_Tkn NVARCHAR(1000)

as

INSERT INTO [TCAR_TOKN]
(
	[tkey]
	,[Acc_Code]
	,[Access_Tkn]
	,[Refresh_Tkn]
	,[DT_Created]
	,[Status]
)
VALUES
(
	@tkey
	,@Acc_Code
	,@Access_Tkn
	,@Refresh_Tkn
	,GETDATE()
	,20
)

```

---

### TCAR_Check_TOKN
The `TCAR_Check_TOKN` stored procedure is designed to manage and retrieve valid authentication tokens associated with an account code. It ensures that expired tokens are invalidated and retrieves the most recent valid token, if available.

#### Functionality
* Cancel Expired Tokens
  * Updates tokens in `TCAR_TOKN` by setting their `Status ` to 90 (Expired) if:
    * The token was created more than 240 minutes (4 hours) ago and is still active (`Status = 20`).
    * The token has an invalid placeholder value ('Access Token Not found').

* Retrieve a Valid Token
  * Selects the most recent valid token (`Status = 20`) created within the last 200 minutes for the given account code (`@Acc_Code`).
  * Stores the token in a temporary table (`#TEMP_TKN`).

* Return the Token Status
  * If a valid token exists, it returns the token details.
  * If no valid token is found, it returns 'NO TOKEN' as `Acc_Code`, indicating the need for a new authentication request.

#### Summary
The `TCAR_Check_TOKN` stored procedure manages authentication tokens by invalidating expired ones and retrieving the most recent valid token for an account. If no valid token is found, it returns a "NO TOKEN" response, prompting the application to get a new one. This procedure plays a crucial role in maintaining secure and efficient API authentication.

```SQL
ALTER PROC [dbo].[TCAR_Check_TOKN]

@Acc_Code VARCHAR(8)

as

--==========================================================================================
-- CANCEL EXPIRED TOKENS
--==========================================================================================

UPDATE 
		TCAR_TOKN
SET 
		[Status] = 90
WHERE 
		(
			DT_Created <= DATEADD(MINUTE, -240, GETDATE()) 
			AND [Status] = 20
		)
		OR 
		(
			TCAR_TOKN.Access_Tkn = 'Access Token Not found'
		)


--==========================================================================================
-- GET ANY VALID TOKENS 
--==========================================================================================

SELECT TOP 1
		RTRIM(Access_Tkn) as [Acc_Code]
		INTO #TEMP_TKN
FROM 
		TCAR_TOKN 
WHERE 
		DT_Created >= DATEADD(MINUTE, -200, GETDATE()) 
		AND [Status] = 20
		AND Acc_Code = @Acc_Code

IF EXISTS (SELECT 1 FROM #TEMP_TKN)
	BEGIN
		SELECT* FROM #TEMP_TKN
	END
ELSE
	BEGIN 
		SELECT 'NO TOKEN' as [Acc_Code]
	END

```

---

### TCAR_Rpnt_Srch
The `TCAR_Rpnt_Srch` stored procedure facilitates a search function within the carrier module’s reprint function, allowing users to search for printed labels within the last 24 hours in order to reprint them if required. The procedure first removes any previous search results associated with the session and then retrieves new results based on the provided search criteria.

#### Functionality
* Clear Previous Search Results
  * Deletes old search results from the `TCAR_Srch` table where `Session_Id` matches the current session.
  * Ensures that only fresh search results are available for the session.
* Retrieve and Store New Search Results
  * Searches the `TCAR` and `TCAR_Resp` tables for matching records based on:
    * `Ref1`, `Ref2`, `Tracking`, `TCAR_Ref`, or the response creation date (`DT_Created`).
  * Filters results to include only `TCAR` Records:
    * With statuses 220 or 225 in the `TCAR` table.
    * Having a response status of 200 in `TCAR_Resp`.
    * Created within the last 24 hours.
  * Inserts the matching records into `TCAR_Srch` for the session.
* Display Search Results
  * Selects and returns relevant details (`Ref1`, `Ref2`, `Tracking`, `DT_Created`) from `TCAR_Srch ` for the current session.
  * Outputs results with renamed columns for readability, e.g., `DT_Created ` as Print Date.

#### Summary
The `TCAR_Rpnt_Srch` stored procedure retrieves and displays shipment records based on user search input. It ensures up-to-date search results by deleting old entries and filtering transactions based on status and timestamp. The results are stored temporarily for the session and then displayed with relevant details, enhancing search efficiency in the carrier module’s reprint function.

```SQL
ALTER PROC [dbo].[TCAR_Rpnt_Srch]

@Session_Id VARCHAR(255)
,@Search VARCHAR(255)

as

--=====================================================================================
-- DELETE OLD SEARCH RESULTS
--=====================================================================================

DELETE 
		[TCAR_Srch]
WHERE 
		Session_Id = @Session_Id

--=====================================================================================
-- GET NEW RESULTS
--=====================================================================================

INSERT INTO [TCAR_Srch]
(
	[Session_Id]
	,[TCAR_Ref]
	,[Ref1]
	,[Ref2]
	,[Tracking]
	,[DT_Created]
	,[XML_MESSAGE]
)

SELECT
			@Session_Id
			,TCAR.TCAR_Ref
			,TCAR.Ref1
			,TCAR.Ref2
			,TCAR.Tracking
			,TCAR_Resp.DT_Created
			,TCAR_Resp.XML_MESSAGE
FROM 
			TCAR 
			INNER JOIN TCAR_Resp on TCAR.TCAR_Ref = TCAR_Resp.TCAR_Ref
WHERE 
			TCAR.[Status] IN (220, 225)
			AND TCAR_Resp.[Status] = 200
			AND TCAR_Resp.DT_Created > DATEADD(HOUR, -24, GETDATE())
			AND 
			(
				[Ref1] LIKE '%' + @Search + '%'
				OR [Ref2] LIKE '%' + @Search + '%'
				OR [Tracking] LIKE '%' + @Search + '%'
				OR TCAR.[TCAR_Ref] LIKE '%' + @Search + '%'
				OR RTRIM(CAST(TCAR_Resp.DT_Created as date)) = @Search
			)

--=====================================================================================
-- SHOW RESULTS
--=====================================================================================

SELECT
			[Ref1] as [Reference 1]
			,[Ref2] as [Reference 2]
			,[Tracking]
			,[DT_Created] as [Print Date]
FROM 
			[TCAR_Srch]
WHERE 
			Session_Id = @Session_Id

```

---

### TCAR_Rpnt

The `TCAR_Rpnt` stored procedure updates the `TCAR` table to mark a shipment as reprinted, maintaining a record each time a shipment label is reprinted.

#### Parameters
The procedure takes two parameters:
* `@Tracking` – The tracking number of the reprinted shipment label.
* `@User` – The user who reprinted the shipment label.

#### Functionality
The procedure updates the following fields in the `TCAR` table to indicate that a shipment label has been reprinted:
* `Last_Updated` – Stores the date and time when the shipment label was reprinted.
* `Last_Upd_User` – Records the user who performed the reprint.
* `Status` – Updated to 225 to indicate that the shipment label has been reprinted.

#### Summary
The `TCAR_Rpnt` stored procedure is a key component of the carrier module’s auditing system. By updating the `TCAR` table to reflect reprinted shipment labels, it plays a crucial role in tracking reprints, ensuring accountability, and aiding in troubleshooting potential shipment issues throughout the shipment lifecycle.

```SQL
ALTER PROC [dbo].[TCAR_Rpnt]

@Tracking VARCHAR(255)
,@User VARCHAR(100)

as

--==================================================
-- UPDATE TCAR TO INDICATE REPRINT
--==================================================

UPDATE 
		TCAR
SET 
		Last_Updated = GETDATE()
		,Last_Upd_User = @User
		,[status] = 225
WHERE 
		Tracking = @Tracking
```

---

### TCAR_Check_Lines
The `TCAR_Check_Lines` stored procedure is used to validate line items associated with a specific shipment reference (`TCAR_Ref`). It checks for missing or invalid values in key product fields to ensure data integrity before processing.

#### Parameters
The procedure accepts a single parameter:
* `@TCAR_Ref` – The reference number of the shipment whose line items need validation.

#### Functionality
The procedure performs the following operations:
* Data Validation Using a Common Table Expression (CTE)
	* A temporary dataset (`ErrorCheck`) is created to identify errors in the `TCAR_Line` table.
	* Various conditions are checked for required fields, including:
		* Missing or empty values (`Product Type`, `Product Country of Origin`, `Product Commodity Code`).
		* Numerical constraints (`Product Value`, `Line Value`, `Line Weight`, and `Quantity` must be between 1 and 99,999).
		* Length restrictions (`Product Description` and `Product Type` must be 1–35 characters, `Product Commodity Code` must be 1–13 characters).
	* If a field violates any of these rules, an `Error_Field` and corresponding `Error_State` code are assigned.

* Error Reporting
	* The procedure selects the first detected error and formats it into a human-readable message.
	* If no errors are found, it returns `"No Error"`.

#### Error Codes and Conditions
The following error codes are used to classify validation failures:
* `101` - Field cannot be null or empty.
* `102` - Value must be between 1 and 99,999.
* `103` - Text length must be between 1 and 35 characters.
* `104` - Text length must be between 1 and 13 characters.

#### Summary
The `TCAR_Check_Lines` stored procedure ensures that line items for a shipment contain valid data before further processing. By identifying errors early, it helps maintain data integrity, prevents downstream failures, and improves auditing capabilities within the carrier module.


```SQL
ALTER PROC [dbo].[TCAR_Check_Lines]

@TCAR_Ref VARCHAR(24)

AS

/*
ERROR STATE KEY
101 - CANNOT BE NULL OR EMPTY
102 - MUST BE BETWEEN 1 AND 99999
103 - MUST BE BETWEEN 1 AND 35 CHARACTERS
104 - MUST BE BETWEEN 1 AND 13 CHARACTERS
*/

--====================================================================================================================
-- GET DATA INTO CTE
--====================================================================================================================

;WITH ErrorCheck AS 
(
	SELECT 
				TCAR_Ref
				,Part
				,[Description]
				,[Error_Field] = 
				CASE 
					WHEN [Type] IS NULL OR [Type] = '' THEN 'Product Type'
					WHEN [CoO] IS NULL OR [CoO] = '' THEN 'Product Country of Origin'
					WHEN [HS_Code] IS NULL OR [HS_Code] = '' THEN 'Product Commodity Code'
					WHEN COALESCE(Part_Value, 0) < 0 OR COALESCE(Part_Value, 0) > 99999 THEN 'Product Value'
					WHEN COALESCE(Line_Value, 0) < 0 OR COALESCE(Line_Value, 0) > 99999 THEN 'Product Value'
					WHEN COALESCE(Line_Weight, 0) < 0 OR COALESCE(Line_Weight, 0) > 99999 THEN 'Product Weight'
					WHEN COALESCE(Qty, 0) < 1 OR COALESCE(Qty, 0) > 99999 THEN 'Product Qty'
					WHEN LEN(COALESCE([Description], '')) NOT BETWEEN 1 AND 35 THEN 'Product Description'
					WHEN LEN(COALESCE([Type], '')) NOT BETWEEN 1 AND 35 THEN 'Product Type'
					WHEN LEN(COALESCE([HS_Code], '')) NOT BETWEEN 1 AND 13 THEN 'Product Commodity Code'
					ELSE NULL
				END
				,[Error_State] =
				CASE 
					WHEN [Type] IS NULL OR [Type] = '' THEN 101
					WHEN [CoO] IS NULL OR [CoO] = '' THEN 101
					WHEN [HS_Code] IS NULL OR [HS_Code] = '' THEN 101
					WHEN COALESCE(Part_Value, 0) < 0 OR COALESCE(Part_Value, 0) > 99999 THEN 102
					WHEN COALESCE(Line_Value, 0) < 0 OR COALESCE(Line_Value, 0) > 99999 THEN 102
					WHEN COALESCE(Line_Weight, 0) < 0 OR COALESCE(Line_Weight, 0) > 99999 THEN 102
					WHEN COALESCE(Qty, 0) < 1 OR COALESCE(Qty, 0) > 99999 THEN 102
					WHEN LEN(COALESCE([Description], '')) NOT BETWEEN 1 AND 35 THEN 103
					WHEN LEN(COALESCE([Type], '')) NOT BETWEEN 1 AND 35 THEN 103
					WHEN LEN(COALESCE([HS_Code], '')) NOT BETWEEN 1 AND 13 THEN 104
					ELSE 0
				END
	FROM 
				TCAR_Line
	WHERE 
				TCAR_Ref = @TCAR_Ref
)

--====================================================================================================================
-- SELECT RESULTS
--====================================================================================================================

SELECT TOP 1
			CASE 
				WHEN ISNULL([Error_State], 0) > 0 
				THEN CONCAT('Part ', Part, ' - (', [Description], ') Has an error: ', Error_Field, ' ', 
				CASE 
					WHEN [Error_State] = 101 THEN 'cannot be null or empty'
					WHEN [Error_State] = 102 THEN 'must be between 1 and 99999'
					WHEN [Error_State] = 103 THEN 'must be between 1 and 35 characters'
					WHEN [Error_State] = 104 THEN 'must be between 1 and 13 characters'
				END, '.')
				ELSE 'No Error'
			END AS [Error]
FROM 
			ErrorCheck
WHERE 
			[Error_State] != 0


```

---

### TCAR_Get_Client

The `TCAR_Get_Client` stored procedure retrieves client-related customs information based on a given client description. It is designed to fetch key compliance and account details from the `TAFIE_Clients` table.

#### Parameters
The procedure accepts one parameter:
* `@Client` – A string representing the client description to be used as a search criterion.

#### Functionality
* Data Retrieval
	* The procedure queries the `TAFIE_Clients` table for a record where the `Description` field matches the provided `@Client` parameter.
	* It retrieves the following key fields:
		* `IOSS` – The Import One-Stop Shop (IOSS) number for EU VAT compliance.
		* `EORI` – The Economic Operators Registration and Identification (EORI) number for customs identification.
		* `AccCode` – The account code associated with the client.
		* `Active` – A status flag indicating whether the client account is active.

* Data Formatting
	* The `RTRIM` function is used on the `IOSS`, `EORI`, and `Acc_Code` fields to remove any trailing spaces, ensuring clean output.

#### Summary
The `TCAR_Get_Client` stored procedure is essential for retrieving regulatory and account information related to a specific client. This ensures that shipments and customs documentation are properly linked to the correct client records, supporting compliance and operational efficiency.

```SQL
ALTER PROC[dbo].[TCAR_Get_Client]

@Client VARCHAR(255)

as

--======================================================
-- GET CLIENT CUSTOMS DATA
--======================================================

SELECT
		RTRIM(IOSS) as [IOSS]
		,RTRIM(EORI) as [EORI]
		,RTRIM(Acc_Code) as [AccCode]
		,Active
FROM 
		TAFIE_Clients
WHERE 
		@Client = [Description]
```

---

### TCAR_Save_Client

The `TCAR_Save_Client` stored procedure updates client information in the `TAFIE_Clients` table. It ensures that key compliance and account details remain up to date for a given client.

#### Parameters
The procedure accepts six parameters:
* `@Client` – The client description used to identify the record to be updated.
* `@IOSS` – The Import One-Stop Shop (IOSS) number for EU VAT compliance.
* `@EORI` – The Economic Operators Registration and Identification (EORI) number for customs identification.
* `@AccCode` – The account code associated with the client.
* `@Active` – An integer flag indicating whether the client is active.
* `@User` – The username of the individual making the update.

#### Functionality
Updating Client Data
* The procedure searches for a client record where the Description field matches the provided `@Client` parameter.
* If a matching record is found, the following fields are updated:
	* `Last_Upd_User` – Updated with the username of the individual making the change.
	* `Last_Updated` – Set to the current timestamp using `GETDATE()`.
	* `IOSS`, `EORI`, and `Acc_Code` – Updated with the provided values to ensure correct compliance and account details.
	* `Active` – Updated to reflect the new account status.

#### Summary
The `TCAR_Save_Client` stored procedure is a critical part of client data management, ensuring that regulatory, account, and status details remain accurate. By maintaining updated records, this procedure supports compliance with customs regulations and smooth shipment processing.

```SQL
ALTER PROC [dbo].[TCAR_Save_Client]

@Client VARCHAR(255)
,@IOSS VARCHAR(255)
,@EORI VARCHAR(255)
,@AccCode VARCHAR(8)
,@Active INT
,@User VARCHAR(100)

AS

--=========================================
-- UPDATE CLIENT DATA
--=========================================

UPDATE
		TAFIE_Clients
SET
		Last_Upd_User = @User
		,Last_Updated = GETDATE()
		,IOSS = @IOSS
		,EORI = @EORI
		,Acc_Code = @AccCode
		,Active = @Active
WHERE
		[Description] = @Client
```

---

### TCAR_Get_Combos

The `TCAR_Get_Combos` stored procedure dynamically retrieves carrier and delivery method combinations from a client-specific database. It is designed to support flexible querying based on different modes.

#### Parameters
The procedure accepts three parameters:
* `@Client` – The client identifier used to determine the database connection.
* `@Mode` – An integer parameter that controls the type of query executed.
* `@Carrier` – (Optional) A specific carrier filter used in certain modes. Defaults to an empty string.

#### Functionality

* Establishing the Connection String
	* The procedure dynamically constructs a database reference using the `TAFIE_Clients` table.
	* It retrieves the server and database names for the specified `@Client` and formats them into a fully qualified table reference.
* Executing Queries Based on Mode - The procedure builds a dynamic SQL query based on the value of `@Mode`:
	* Mode 1: Retrieve all carriers and their corresponding delivery methods for the client.
		* Returns `Carrier`, `Delivery Method`, and `Description`.
		* Filters results to only include carriers integrated via `'PHUB_API'`.
	* Mode 2: Retrieve delivery methods for a specific carrier.
		* Similar to Mode 1 but includes an additional filter to return results for the given `@Carrier`.
	* Mode 3: Retrieve a distinct list of carriers.
		* Returns only unique `Carrier` values for the client.
		* Filters results to only include carriers integrated via `'PHUB_API'`.
* Executing the Dynamic SQL
	* The procedure prints the generated SQL query for debugging.
	* The constructed SQL is executed dynamically using `EXECUTE(@SQL)`.

#### Summary
The `TCAR_Get_Combos` stored procedure provides a flexible way to retrieve carrier and delivery method data for a client. By using dynamic SQL, it adapts to different query modes while ensuring compatibility with the client's specific database structure. This procedure plays a crucial role in carrier selection and delivery configuration.

```SQL
ALTER PROC [dbo].[TCAR_Get_Combos]

@Client VARCHAR(255)
,@Mode INT
,@Carrier VARCHAR(24) = ''

As

--=================================================================================================================================
-- DECLARE & SET VARIABLES
--=================================================================================================================================

DECLARE @SQL VARCHAR(MAX) = ''
DECLARE @Connection_String VARCHAR(MAX) 

SET @Connection_String = 
							(
								SELECT 
											'[' + [Server] + '].[' + [Database] + '].dbo' 
								FROM 
											TAFIE_Clients 
								WHERE 
											[Description] = @Client
							)

--=================================================================================================================================
-- SET DYNAMIC QUERY (MODE 1)
--=================================================================================================================================

IF @Mode = 1
	BEGIN

		SET @SQL = @SQL + '

		SELECT
					RTRIM(carr_ddtl.carrier) as [Carrier]
					,RTRIM(carr_ddtl.del_method) as [Delivery Method]
					,RTRIM(dmtd.descr) as [Description]
		FROM 
					' + @Connection_String + '.carr_ddtl
					INNER JOIN ' + @Connection_String + '.dmtd on dmtd.del_method = carr_ddtl.del_method
					INNER JOIN ' + @Connection_String + '.carr_intg on carr_ddtl.carrier = carr_intg.carrier
		WHERE 
					carr_intg.intg_code = ''PHUB_API''
	'
	END

--=================================================================================================================================
-- SET DYNAMIC QUERY (MODE 2)
--=================================================================================================================================

ELSE IF @Mode = 2
	BEGIN

		SET @SQL = @SQL + '

		SELECT
					RTRIM(carr_ddtl.carrier) as [Carrier]
					,RTRIM(carr_ddtl.del_method) as [Delivery Method]
					,RTRIM(dmtd.descr) as [Description]
		FROM 
					' + @Connection_String + '.carr_ddtl
					INNER JOIN ' + @Connection_String + '.dmtd on dmtd.del_method = carr_ddtl.del_method
					INNER JOIN ' + @Connection_String + '.carr_intg on carr_ddtl.carrier = carr_intg.carrier
		WHERE 
					carr_intg.intg_code = ''PHUB_API''
					AND carr_ddtl.carrier = ''' + @Carrier + '''
	'
	END

--=================================================================================================================================
-- SET DYNAMIC QUERY (MODE 3)
--=================================================================================================================================

ELSE IF @Mode = 3
	BEGIN

		SET @SQL = @SQL + '

		SELECT
					DISTINCT RTRIM(carr_ddtl.carrier) as [Carrier]
		FROM 
					' + @Connection_String + '.carr_ddtl
					INNER JOIN ' + @Connection_String + '.dmtd on dmtd.del_method = carr_ddtl.del_method
					INNER JOIN ' + @Connection_String + '.carr_intg on carr_ddtl.carrier = carr_intg.carrier
		WHERE 
					carr_intg.intg_code = ''PHUB_API''
	'
	END

--=================================================================================================================================
-- PRINT & EXECUTE SQL
--=================================================================================================================================

PRINT(@SQL)
EXECUTE(@SQL)
```

---

### TCAR_RECAL

The `TCAR_RECAL` stored procedure is designed to reset and recalculate shipment data by clearing existing records and reloading fresh data.

#### Parameters
* `@TCAR_Ref` – A unique reference identifier for the shipment that requires recalculating.

#### Functionality
* Clearing Old Records
	* Deletes existing records from three tables (`TCAR_Line`, `TCAR_BOXES`, `TCAR_BCON`) where `TCAR_Ref` matches the provided reference.
	* This ensures that outdated or incorrect shipment data is completely removed before recalculating.

* Recalculating Shipment Data
	* Calls the `TCAR_Get_Comp` stored procedure, which retrieves and repopulates the necessary data for the specified `@TCAR_Ref`.
	* This step ensures that the shipment data is refreshed and up to date.

#### Summary
The `TCAR_RECAL` stored procedure is a critical component of the shipment processing system, enabling the recalibration of shipment records. By removing outdated data and triggering a recalculation, it ensures that shipment information remains accurate and consistent. This is particularly useful in scenarios where modifications, corrections, or updates are needed for a shipment's details.

```SQL
ALTER PROC [dbo].[TCAR_RECAL]


@TCAR_Ref VARCHAR(24)

as

--=================================================================================================================================
-- CLEAR OLD RECORDS
--=================================================================================================================================

DELETE TCAR_Line WHERE TCAR_Ref = @TCAR_Ref
DELETE TCAR_BOXES WHERE TCAR_Ref = @TCAR_Ref
DELETE TCAR_BCON WHERE TCAR_Ref = @TCAR_Ref

--=================================================================================================================================
-- GET DATA AGAIN TO RECAL
--=================================================================================================================================

EXECUTE [TCAR_Get_Comp] @TCAR_Ref
```

---

## Useful Queries

Here are some useful SQL queries that may help you understand the relationships between tables.

### Call & Response

This query will give you the call and response for a given shipment based on either the Elucid load note or order number.

```sql
DECLARE @Load_Note VARCHAR(24) = '' -- Enter a Load Note
DECLARE @Ref_No VARCHAR(24) = '' -- Enter an Order Number

SELECT
			RTRIM([TCAR].[TCAR_Ref]) as [TCAR Reference]
			,[TCAR].[DT_Created] as [Date Created] -- Shipment Record Creation Date
			,RTRIM([TCAR].[Load_Note]) as [Load Note] -- Elucid Load note
			,RTRIM([TCAR].[Ref_No]) as [Order Number] -- Elucid Order Number
			,RTRIM([TCAR_Call].[XML_MESSAGE]) as [Call] -- XML Call made
			,RTRIM([TCAR_Resp].[XML_MESSAGE]) as [Response] -- XML Response received
FROM 
			[TCAR]
			LEFT JOIN [TCAR_Call] on [TCAR].[TCAR_Ref] = [TCAR_Call].[TCAR_Ref]
			LEFT JOIN [TCAR_Resp] on [TCAR].[TCAR_Ref] = [TCAR_Resp].[TCAR_Ref]
WHERE 
			[TCAR].[Load_Note] = @Load_Note
			OR [TCAR].[Ref_No] = @Ref_No

```

---

### Boxes

This query will give you all the boxes for a shipment and thier header data based on either the Elucid load note or order number.

```sql
DECLARE @Load_Note VARCHAR(24) = '' -- Enter a Load Note
DECLARE @Ref_No VARCHAR(24) = '' -- Enter an Order Number

SELECT
			RTRIM([TCAR].[TCAR_Ref]) as [TCAR Reference] 
			,[TCAR].[DT_Created] as [Date Created] -- Shipment Record Creation Date
			,RTRIM([TCAR].[Load_Note]) as [Load Note] -- Elucid Load note
			,RTRIM([TCAR].[Ref_No]) as [Order Number] -- Elucid Order Number
			,RTRIM([TCAR_BOXES].[Box_Ref]) as [Box Reference] -- Reference for the box within the shipment
			,[TCAR_BOXES].[Weight] as [Box Weight] -- Weight of the Box & it's contents
			,[TCAR_BOXES].[Value] as [Box Value] -- Value of the Box contents
			,CAST([TCAR_BOXES].[Qty] as INT) as [Units in Box] -- Total Number of Units in the box
			,[TCAR_BOXES].[Dim_H] as [Height] -- Box's Height
			,[TCAR_BOXES].[Dim_W] as [Width] -- Box's Width
			,[TCAR_BOXES].[Dim_L] as [Length] -- Box's Length
			,RTRIM([TCAR_BOXES].[Tracking]) as [Tracking Number] -- Tracking Number for the box (If Applicable)
FROM 
			[TCAR]
			LEFT JOIN [TCAR_BOXES] on [TCAR].[TCAR_Ref] = [TCAR_BOXES].[TCAR_Ref]
WHERE 
			[TCAR].[Load_Note] = @Load_Note
			OR [TCAR].[Ref_No] = @Ref_No
```

---

### Box Contents

This query will give you a breakdown of the boxes and thier contents for a shipment based on either the Elucid load note or order number.

```sql
DECLARE @Load_Note VARCHAR(24) = '' -- Enter a Load Note
DECLARE @Ref_No VARCHAR(24) = '' -- Enter an Order Number

SELECT
			RTRIM([TCAR].[TCAR_Ref]) as [TCAR Reference] 
			,[TCAR].[DT_Created] as [Date Created] -- Shipment Record Creation Date
			,RTRIM([TCAR].[Load_Note]) as [Load Note] -- Elucid Load note
			,RTRIM([TCAR].[Ref_No]) as [Order Number] -- Elucid Order Number
			,RTRIM([TCAR_BOXES].[Box_Ref]) as [Box Reference] -- Reference for the box within the shipment
			,RTRIM([TCAR_BCON].[Part]) as [Part] -- SKU for the part in the box
			,[TCAR_BCON].[Qty] -- Quantity of that part in the box
			,RTRIM([TCAR_BOXES].[Tracking]) as [Tracking Number] -- Tracking Number for the box (If Applicable)
FROM 
			[TCAR]
			LEFT JOIN [TCAR_BOXES] on [TCAR].[TCAR_Ref] = [TCAR_BOXES].[TCAR_Ref]
			LEFT JOIN [TCAR_BCON] on [TCAR].[TCAR_Ref] = [TCAR_BCON].[TCAR_Ref] AND [TCAR_BCON].[Box_Ref] = [TCAR_BOXES].[Box_Ref]
WHERE 
			[TCAR].[Load_Note] = @Load_Note
			OR [TCAR].[Ref_No] = @Ref_No
```

---

### Filters

If you wish to limit your results set further, you can add the following conditions to the `WHERE` clause of the query.

Filter by status.

```sql
-- Show Completed shipment records
[TCAR].[Status] = 220

-- Show Cancelled shipment records
[TCAR].[Status] = 200

-- Show Open shipment records
[TCAR].[Status] = 30

-- Show Re-printed shipment records
[TCAR].[Status] = 225
```

Filter By Carrier.

```sql
-- Replace 'RMT' & 'RMTSTD' with the carrier and delivery method you wish to filter by
[TCAR].[Carrier] = 'RMT'
AND [TCAR].[Del_Method] = 'RMTSTD'
```

Filter by date and/or time.

```sql
-- All records on a specific date (Inclusive)
CAST([TCAR].[DT_Created] as date) = '2025-04-17'

-- All records between a specif date range (Inclusive)
CAST([TCAR].[DT_Created] as date) >= '2025-04-01'
AND CAST([TCAR].[DT_Created] as date) <= '2025-04-30'

-- All records between a specif date and time
[TCAR].[DT_Created] >= '2025-04-01 08:00:00'
AND [TCAR].[DT_Created] <= '2025-04-01 08:30:00'
```

--- 

### Advanced

Here is a more advance query that will give you all the successful shipment labels created using the solution within a given time frame. This includes totals for each client and a grand total.

```sql
DECLARE @Start VARCHAR(24) = '' -- Enter Start Date
DECLARE @End VARCHAR(24) = '' -- Enter End Date

SELECT
			RTRIM([TCAR].[API_Acc]) + ' - ' + RTRIM([TCAR].[Client]) as [API Account] -- API Account & Client the shipment is under
			,RTRIM([TCAR].[Ref_No]) as [Order Number] -- Elucid Order number the for the shipment
			,RTRIM([TCAR].[Load_Note]) as [Load Note] -- Elucid Load note for the shipment
			,RTRIM([TCAR].[Carrier]) + ' - ' + RTRIM([TCAR].[Del_Method]) as [Service] -- Carrier method for the shipment
			,RTRIM([TCAR].[Service_Descr]) as [Description] -- Service description
			,RTRIM([TCAR].[Tracking]) as [Tracking] -- Shipment Tracking
			,CASE 
					WHEN [TCAR].[Status] = 220 THEN 'PRINTED'
					WHEN [TCAR].[Status] = 225 THEN 'RE-PRINTED'
					ELSE RTRIM([TCAR].[Status])
			END AS [Status] -- Status of the shipment
			,RTRIM(ISNULL([TCAR].[Last_Upd_User], [TCAR].[User_Created])) as [Last Updated User] -- User who last interacted with the shipment
			,[TCAR].[Last_Updated] as [Last_Updated] -- Date time the shipment was last interacted with
			,(
				SELECT 
							COUNT(*)
				FROM 
							[TCAR] as [T]
				WHERE 
							[T].[Status] IN (220,225)
							AND CAST([T].[Last_Updated] as date) >= @Start
							AND CAST([T].[Last_Updated] as date) <= @End
							AND (RTRIM([TCAR].[API_Acc])+RTRIM([TCAR].[Client])) = (RTRIM([T].[API_Acc])+RTRIM([T].[Client]))
			) as [Account Total] -- Total for API account & Client
			,(
				SELECT 
							COUNT(*)
				FROM 
							[TCAR]
				WHERE 
							[TCAR].[status] IN (220,225)
							AND CAST([TCAR].[Last_Updated] as date) >= @Start
							AND CAST([TCAR].[Last_Updated] as date) <= @End
			) as [Total] -- Grand Total
FROM 
			[TCAR] 
WHERE 
			[TCAR].[status] IN (220,225)
			AND CAST([TCAR].[Last_Updated] as date) >= @Start
			AND CAST([TCAR].[Last_Updated] as date) <= @End
```

