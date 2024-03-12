# Dynamics 365 Sales CRM Plugins

This repository contains two plugins developed in C# .NET for Dynamics 365 Sales CRM. These plugins enhance the functionality of the CRM system related to lead conversion and lead creation by Sales Agents.

## Plugins Logic
### Plugin 1: Lead to Opportunity Conversion 
**Description:** 
- When a lead is converted into an opportunity, the contact information present on the lead is transformed into a contact record.

### Plugin 2: Lead Creation with Contact Information
**Description:**
- When a Sales Agent creates a lead with contact information, the system verifies if an existing contact with the same email address already exists in the system.
- If an existing contact with the same email address is found, the lead record is associated with the existing contact.
