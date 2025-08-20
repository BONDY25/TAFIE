**TAFIE** began as a diagnostic tool and has evolved into a critical application for streamlining shipping and fulfillment processes. It was developed to address limitations within a legacy ERP system, providing a robust and flexible solution for managing carrier label generation and API integrations.

---

### **Project Context**

The initial version of this application was a simple tool for viewing database-stored error messages from failed carrier label attempts. The goal was to provide a clear, actionable diagnosis to users, as the ERP system's native error handling was insufficient. This first step empowered users to quickly identify and resolve shipping issues, ensuring parcels could be shipped without delay.

As carrier services evolved, the limitations and high costs of the ERP's third-party integrations became a significant bottleneck. This led to a strategic decision to expand TAFIE's capabilities, transforming it from a diagnostic viewer into a full-fledged carrier label generation engine.

---

### **Key Features and Functionality**

* **API-Based Carrier Label Generation**: TAFIE now handles API requests to multiple carrier label providers, processes the responses, and prints labels directly. This gives the business full control over its shipping processes and eliminates reliance on expensive ERP modules.  
* **Multi-Box Shipment Support**: The application supports creating labels for multi-box shipments, accommodating complex orders with ease.  
* **ERP Integration**: TAFIE seamlessly integrates with the existing ERP system, retrieving necessary data and saving shipment information to the database.  
* **Advanced Error Handling**: Beyond its initial function, the application now provides detailed, user-friendly error messages that help resolve shipping failures.

---

### **Technical Details**

* **Technology Stack**: The application was built using **C\# WinForms**.  
* **Database**: It interacts with **multiple SQL databases** to retrieve order data, make API requests, and save shipment records.  
* **API Integration**: The application uses APIs to communicate with various carrier services for label creation.

This project demonstrates my ability to identify business inefficiencies and develop a comprehensive solution that not only resolves the immediate problem but also lays the groundwork for future system architecture. TAFIE is positioned to potentially replace the ERP's shipping functionality, which would significantly reduce licensing costs and provide greater control over operations.

In this repository are database dictionaries, technical reports and class indexes which help explain the operation of the application

---
