# CopyToDocument Configuration – Source to Target Mapping Specification

## 1. Objective
This document describes the final design of the CopyToDocument action. The action enables
automatic data transfer from a source document to a target document using configurable field
mapping and execution options.

## 2. Core Concept
The CopyToDocument action operates on two documents:
- Source Document: the document or form submission where data originates
- Target Document: the document that will be created or updated
The action is executed automatically by the system when a configured event occurs.

## 3. Source Document Definition
The source document must be explicitly defined:
- SourceDocumentTypeId
- SourceFormId
The source document represents the completed or approved submission whose data will be
copied.

## 4. Target Document Definition
The target document must be explicitly defined:
- TargetDocumentTypeId
- TargetFormId
Additional target options:
- CreateNewDocument (true / false)
- InitialStatus (Draft / Submitted)
- StartWorkflow (true / false)

## 5. Field Mapping Configuration
Field mapping defines how data is transferred from the source document to the target
document.
Each mapping entry must include:
- SourceFieldCode
- TargetFieldCode
FieldCode must be used instead of FieldId to ensure stability across versions.
Example:
TOTAL_AMOUNT → CONTRACT_VALUE

## 6. Supported Data Types
The CopyToDocument action must support copying the following:
- Simple field values
- Calculated fields
- Grid rows and grid totals
- System metadata (submission date, document number, created by)

## 7. Additional Action Options
The action may include the following configuration options:
- CopyCalculatedFields (Yes / No)
- CopyGridData (Yes / No)
- CopyAttachments (Yes / No)
- LinkDocuments (ParentDocumentId)
- OverrideTargetDefaults (Yes / No)

## 8. Execution Trigger
The CopyToDocument action is executed based on system events, such as:
- OnFormSubmitted
- OnApprovalCompleted
- OnDocumentApproved
The event definition is external to the action and managed by the Actions Engine.

## 9. Execution Flow
1. Event is triggered
2. CopyToDocument configuration is resolved
3. Target document is created or loaded
4. Field mapping is applied
5. Additional options are executed
6. Target workflow is optionally started
7. Execution result is logged

## 10. Validation Rules
Before execution, the system must validate:
- Source and target document compatibility
- Existence of mapped fields
- Data type compatibility
If validation fails, the action must not partially execute.

## 11. Error Handling
If execution fails:
- The operation must be rolled back
- The error must be logged
- A system error message must be returned

## 12. Audit & Traceability
Each CopyToDocument execution must be logged, including:
- SourceDocumentId
- TargetDocumentId
- ActionId
- Execution timestamp

## 13. Example Configuration
Example:
Source Document: Purchase Request
Target Document: Purchase Order
Mapped Fields:
- REQUEST_AMOUNT → ORDER_AMOUNT
- REQUEST_DATE → ORDER_DATE
Options:
- CreateNewDocument = true
- StartWorkflow = true

## 14. Summary
The CopyToDocument action provides a standardized and configurable mechanism for
document-to-document data propagation. It is a core capability of the Built-in Actions Engine
and enables advanced workflow automation.

---

## API Reference

### Execute CopyToDocument
`POST /api/CopyToDocument/execute`

Executes the action manually (same path used by the Actions Engine).

### Audit Records
- `GET /api/CopyToDocument/audit`
- `GET /api/CopyToDocument/audit/{id}`
- `GET /api/CopyToDocument/audit/submission/{submissionId}`
- `GET /api/CopyToDocument/audit/target/{targetDocumentId}`

## DTO Reference

### ExecuteCopyToDocumentRequestDto
```json
{
  "config": { ...CopyToDocumentActionDto... },
  "sourceSubmissionId": 115,
  "actionId": null,
  "ruleId": null
}
```

### CopyToDocumentActionDto
```json
{
  "sourceDocumentTypeId": 1,
  "sourceFormId": 1,
  "sourceSubmissionId": null,
  "targetDocumentTypeId": 1,
  "targetFormId": 1,
  "createNewDocument": true,
  "targetDocumentId": null,
  "initialStatus": "Draft",
  "fieldMapping": { "TOTAL_AMOUNT": "CONTRACT_VALUE" },
  "gridMapping": { "ITEMS": "CONTRACT_ITEMS" },
  "copyCalculatedFields": true,
  "copyGridRows": true,
  "startWorkflow": false,
  "linkDocuments": true,
  "copyAttachments": false,
  "copyMetadata": false,
  "overrideTargetDefaults": false,
  "metadataFields": []
}
```

### CopyToDocumentResultDto
```json
{
  "success": true,
  "targetDocumentId": 116,
  "targetDocumentNumber": "ser-000113",
  "errorMessage": null,
  "fieldsCopied": 1,
  "gridRowsCopied": 1,
  "actionId": null,
  "sourceSubmissionId": 115
}
```
