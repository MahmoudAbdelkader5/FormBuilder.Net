# CopyToDocument API Testing Guide

## Base URL
```
http://localhost:5000/api/CopyToDocument
```

---

## 1. Execute CopyToDocument (Using IDs)

### Endpoint
```
POST /api/CopyToDocument/execute
```

### Headers
```json
{
  "Content-Type": "application/json",
  "Authorization": "Bearer YOUR_TOKEN"
}
```

### Request Body Example 1: Create New Document (Draft)

```json
{
  "config": {
    "sourceDocumentTypeId": 1,
    "sourceFormId": 10,
    "targetDocumentTypeId": 2,
    "targetFormId": 20,
    "createNewDocument": true,
    "initialStatus": "Draft",
    "fieldMapping": {
      "TOTAL_AMOUNT": "CONTRACT_VALUE",
      "REQUEST_DATE": "ORDER_DATE",
      "CUSTOMER_NAME": "PARTY_NAME"
    },
    "gridMapping": {
      "ITEMS": "CONTRACT_ITEMS"
    },
    "copyCalculatedFields": true,
    "copyGridRows": true,
    "startWorkflow": false,
    "linkDocuments": true,
    "copyAttachments": false,
    "copyMetadata": false,
    "overrideTargetDefaults": false,
    "metadataFields": []
  },
  "sourceSubmissionId": 115,
  "actionId": null,
  "ruleId": null
}
```

### Request Body Example 2: Create New Document (Submitted)

```json
{
  "config": {
    "sourceDocumentTypeId": 1,
    "sourceFormId": 10,
    "targetDocumentTypeId": 2,
    "targetFormId": 20,
    "createNewDocument": true,
    "initialStatus": "Submitted",
    "fieldMapping": {
      "TOTAL_AMOUNT": "CONTRACT_VALUE"
    },
    "gridMapping": {},
    "copyCalculatedFields": true,
    "copyGridRows": true,
    "startWorkflow": false,
    "linkDocuments": true,
    "copyAttachments": false,
    "copyMetadata": false,
    "overrideTargetDefaults": false,
    "metadataFields": []
  },
  "sourceSubmissionId": 115
}
```

### Request Body Example 3: Update Existing Document

```json
{
  "config": {
    "sourceDocumentTypeId": 1,
    "sourceFormId": 10,
    "targetDocumentTypeId": 2,
    "targetFormId": 20,
    "createNewDocument": false,
    "targetDocumentId": 116,
    "initialStatus": "Draft",
    "fieldMapping": {
      "TOTAL_AMOUNT": "CONTRACT_VALUE",
      "REQUEST_DATE": "ORDER_DATE"
    },
    "gridMapping": {
      "ITEMS": "CONTRACT_ITEMS"
    },
    "copyCalculatedFields": true,
    "copyGridRows": true,
    "startWorkflow": false,
    "linkDocuments": false,
    "copyAttachments": false,
    "copyMetadata": false,
    "overrideTargetDefaults": true,
    "metadataFields": []
  },
  "sourceSubmissionId": 115
}
```

### Request Body Example 4: Copy with Attachments and Metadata

```json
{
  "config": {
    "sourceDocumentTypeId": 1,
    "sourceFormId": 10,
    "targetDocumentTypeId": 2,
    "targetFormId": 20,
    "createNewDocument": true,
    "initialStatus": "Draft",
    "fieldMapping": {
      "TOTAL_AMOUNT": "CONTRACT_VALUE"
    },
    "gridMapping": {},
    "copyCalculatedFields": true,
    "copyGridRows": false,
    "startWorkflow": false,
    "linkDocuments": true,
    "copyAttachments": true,
    "copyMetadata": true,
    "overrideTargetDefaults": false,
    "metadataFields": [
      "SubmittedDate",
      "SubmittedByUserId",
      "Status"
    ]
  },
  "sourceSubmissionId": 115
}
```

### Request Body Example 5: Start Workflow

```json
{
  "config": {
    "sourceDocumentTypeId": 1,
    "sourceFormId": 10,
    "targetDocumentTypeId": 2,
    "targetFormId": 20,
    "createNewDocument": true,
    "initialStatus": "Draft",
    "fieldMapping": {
      "TOTAL_AMOUNT": "CONTRACT_VALUE"
    },
    "gridMapping": {},
    "copyCalculatedFields": true,
    "copyGridRows": false,
    "startWorkflow": true,
    "linkDocuments": true,
    "copyAttachments": false,
    "copyMetadata": false,
    "overrideTargetDefaults": false,
    "metadataFields": []
  },
  "sourceSubmissionId": 115
}
```

### Success Response (200 OK)

```json
{
  "statusCode": 200,
  "message": "CopyToDocument executed successfully",
  "data": {
    "success": true,
    "targetDocumentId": 116,
    "targetDocumentNumber": "ser-000113",
    "errorMessage": null,
    "fieldsCopied": 2,
    "gridRowsCopied": 5,
    "actionId": null,
    "sourceSubmissionId": 115
  }
}
```

### Error Response (400 Bad Request - Validation)

```json
{
  "statusCode": 400,
  "message": "Validation failed",
  "data": {
    "errors": {
      "config.sourceDocumentTypeId": ["SourceDocumentTypeId must be greater than 0"],
      "config.sourceFormId": ["SourceFormId must be greater than 0"]
    },
    "message": "One or more validation errors occurred."
  }
}
```

### Error Response (500 Internal Server Error)

```json
{
  "statusCode": 500,
  "message": "Error executing CopyToDocument: Source submission 115 document type 3 does not match configured SourceDocumentTypeId 1",
  "data": {
    "success": false,
    "targetDocumentId": null,
    "targetDocumentNumber": null,
    "errorMessage": "Source submission 115 document type 3 does not match configured SourceDocumentTypeId 1",
    "fieldsCopied": 0,
    "gridRowsCopied": 0,
    "actionId": null,
    "sourceSubmissionId": 115
  }
}
```

---

## 2. Execute CopyToDocument (Using Codes)

### Endpoint
```
POST /api/CopyToDocument/execute-by-codes
```

### Request Body Example

```json
{
  "config": {
    "sourceDocumentTypeCode": "PURCHASE_REQUEST",
    "sourceFormCode": "PR_FORM",
    "targetDocumentTypeCode": "PURCHASE_ORDER",
    "targetFormCode": "PO_FORM",
    "createNewDocument": true,
    "initialStatus": "Draft",
    "fieldMapping": {
      "TOTAL_AMOUNT": "CONTRACT_VALUE",
      "REQUEST_DATE": "ORDER_DATE"
    },
    "gridMapping": {
      "ITEMS": "CONTRACT_ITEMS"
    },
    "copyCalculatedFields": true,
    "copyGridRows": true,
    "startWorkflow": false,
    "linkDocuments": true,
    "copyAttachments": false,
    "copyMetadata": false,
    "overrideTargetDefaults": false,
    "metadataFields": []
  },
  "sourceSubmissionId": 115,
  "actionId": null,
  "ruleId": null
}
```

### Success Response

```json
{
  "statusCode": 200,
  "message": "CopyToDocument executed successfully",
  "data": {
    "success": true,
    "targetDocumentId": 116,
    "targetDocumentNumber": "ser-000113",
    "errorMessage": null,
    "fieldsCopied": 2,
    "gridRowsCopied": 5,
    "actionId": null,
    "sourceSubmissionId": 115
  }
}
```

---

## 3. Get All Audit Records

### Endpoint
```
GET /api/CopyToDocument/audit
```

### Query Parameters
- `sourceSubmissionId` (optional): Filter by source submission ID
- `targetDocumentId` (optional): Filter by target document ID
- `ruleId` (optional): Filter by rule ID
- `success` (optional): Filter by success status (true/false)
- `page` (optional): Page number (default: 1)
- `pageSize` (optional): Page size (default: 50)

### Example Request
```
GET /api/CopyToDocument/audit?sourceSubmissionId=115&page=1&pageSize=10
```

### Success Response (200 OK)

```json
{
  "statusCode": 200,
  "message": "Audit records retrieved successfully",
  "data": {
    "data": [
      {
        "id": 1,
        "sourceSubmissionId": 115,
        "targetDocumentId": 116,
        "actionId": 5,
        "ruleId": 2,
        "sourceFormId": 10,
        "targetFormId": 20,
        "targetDocumentTypeId": 2,
        "success": true,
        "errorMessage": null,
        "fieldsCopied": 2,
        "gridRowsCopied": 5,
        "targetDocumentNumber": "ser-000113",
        "executionDate": "2024-02-08T10:30:00Z",
        "createdDate": "2024-02-08T10:30:00Z",
        "createdByUserId": "user123"
      }
    ],
    "totalCount": 1,
    "page": 1,
    "pageSize": 10,
    "totalPages": 1
  }
}
```

---

## 4. Get Audit Record By ID

### Endpoint
```
GET /api/CopyToDocument/audit/{id}
```

### Example Request
```
GET /api/CopyToDocument/audit/1
```

### Success Response (200 OK)

```json
{
  "statusCode": 200,
  "message": "Audit record retrieved successfully",
  "data": {
    "id": 1,
    "sourceSubmissionId": 115,
    "targetDocumentId": 116,
    "actionId": 5,
    "ruleId": 2,
    "sourceFormId": 10,
    "targetFormId": 20,
    "targetDocumentTypeId": 2,
    "success": true,
    "errorMessage": null,
    "fieldsCopied": 2,
    "gridRowsCopied": 5,
    "targetDocumentNumber": "ser-000113",
    "executionDate": "2024-02-08T10:30:00Z",
    "createdDate": "2024-02-08T10:30:00Z",
    "createdByUserId": "user123"
  }
}
```

### Error Response (404 Not Found)

```json
{
  "statusCode": 404,
  "message": "Audit record with ID 999 not found"
}
```

---

## 5. Get Audit Records By Submission ID

### Endpoint
```
GET /api/CopyToDocument/audit/submission/{submissionId}
```

### Example Request
```
GET /api/CopyToDocument/audit/submission/115
```

### Success Response (200 OK)

```json
{
  "statusCode": 200,
  "message": "Audit records retrieved successfully",
  "data": [
    {
      "id": 1,
      "sourceSubmissionId": 115,
      "targetDocumentId": 116,
      "actionId": 5,
      "ruleId": 2,
      "sourceFormId": 10,
      "targetFormId": 20,
      "targetDocumentTypeId": 2,
      "success": true,
      "errorMessage": null,
      "fieldsCopied": 2,
      "gridRowsCopied": 5,
      "targetDocumentNumber": "ser-000113",
      "executionDate": "2024-02-08T10:30:00Z",
      "createdDate": "2024-02-08T10:30:00Z",
      "createdByUserId": "user123"
    }
  ]
}
```

---

## 6. Get Audit Records By Target Document ID

### Endpoint
```
GET /api/CopyToDocument/audit/target/{targetDocumentId}
```

### Example Request
```
GET /api/CopyToDocument/audit/target/116
```

### Success Response (200 OK)

```json
{
  "statusCode": 200,
  "message": "Audit records retrieved successfully",
  "data": [
    {
      "id": 1,
      "sourceSubmissionId": 115,
      "targetDocumentId": 116,
      "actionId": 5,
      "ruleId": 2,
      "sourceFormId": 10,
      "targetFormId": 20,
      "targetDocumentTypeId": 2,
      "success": true,
      "errorMessage": null,
      "fieldsCopied": 2,
      "gridRowsCopied": 5,
      "targetDocumentNumber": "ser-000113",
      "executionDate": "2024-02-08T10:30:00Z",
      "createdDate": "2024-02-08T10:30:00Z",
      "createdByUserId": "user123"
  }
  ]
}
```

---

## Postman Collection JSON

```json
{
  "info": {
    "name": "CopyToDocument API",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "item": [
    {
      "name": "Execute CopyToDocument (IDs)",
      "request": {
        "method": "POST",
        "header": [
          {
            "key": "Content-Type",
            "value": "application/json"
          },
          {
            "key": "Authorization",
            "value": "Bearer {{token}}"
          }
        ],
        "body": {
          "mode": "raw",
          "raw": "{\n  \"config\": {\n    \"sourceDocumentTypeId\": 1,\n    \"sourceFormId\": 10,\n    \"targetDocumentTypeId\": 2,\n    \"targetFormId\": 20,\n    \"createNewDocument\": true,\n    \"initialStatus\": \"Draft\",\n    \"fieldMapping\": {\n      \"TOTAL_AMOUNT\": \"CONTRACT_VALUE\",\n      \"REQUEST_DATE\": \"ORDER_DATE\"\n    },\n    \"gridMapping\": {\n      \"ITEMS\": \"CONTRACT_ITEMS\"\n    },\n    \"copyCalculatedFields\": true,\n    \"copyGridRows\": true,\n    \"startWorkflow\": false,\n    \"linkDocuments\": true,\n    \"copyAttachments\": false,\n    \"copyMetadata\": false,\n    \"overrideTargetDefaults\": false,\n    \"metadataFields\": []\n  },\n  \"sourceSubmissionId\": 115\n}"
        },
        "url": {
          "raw": "{{baseUrl}}/api/CopyToDocument/execute",
          "host": ["{{baseUrl}}"],
          "path": ["api", "CopyToDocument", "execute"]
        }
      }
    },
    {
      "name": "Execute CopyToDocument (Codes)",
      "request": {
        "method": "POST",
        "header": [
          {
            "key": "Content-Type",
            "value": "application/json"
          }
        ],
        "body": {
          "mode": "raw",
          "raw": "{\n  \"config\": {\n    \"sourceDocumentTypeCode\": \"PURCHASE_REQUEST\",\n    \"sourceFormCode\": \"PR_FORM\",\n    \"targetDocumentTypeCode\": \"PURCHASE_ORDER\",\n    \"targetFormCode\": \"PO_FORM\",\n    \"createNewDocument\": true,\n    \"initialStatus\": \"Draft\",\n    \"fieldMapping\": {\n      \"TOTAL_AMOUNT\": \"CONTRACT_VALUE\"\n    },\n    \"copyCalculatedFields\": true,\n    \"copyGridRows\": true,\n    \"startWorkflow\": false,\n    \"linkDocuments\": true\n  },\n  \"sourceSubmissionId\": 115\n}"
        },
        "url": {
          "raw": "{{baseUrl}}/api/CopyToDocument/execute-by-codes",
          "host": ["{{baseUrl}}"],
          "path": ["api", "CopyToDocument", "execute-by-codes"]
        }
      }
    },
    {
      "name": "Get All Audit Records",
      "request": {
        "method": "GET",
        "header": [],
        "url": {
          "raw": "{{baseUrl}}/api/CopyToDocument/audit?page=1&pageSize=10",
          "host": ["{{baseUrl}}"],
          "path": ["api", "CopyToDocument", "audit"],
          "query": [
            {
              "key": "page",
              "value": "1"
            },
            {
              "key": "pageSize",
              "value": "10"
            }
          ]
        }
      }
    },
    {
      "name": "Get Audit Record By ID",
      "request": {
        "method": "GET",
        "header": [],
        "url": {
          "raw": "{{baseUrl}}/api/CopyToDocument/audit/1",
          "host": ["{{baseUrl}}"],
          "path": ["api", "CopyToDocument", "audit", "1"]
        }
      }
    },
    {
      "name": "Get Audit By Submission ID",
      "request": {
        "method": "GET",
        "header": [],
        "url": {
          "raw": "{{baseUrl}}/api/CopyToDocument/audit/submission/115",
          "host": ["{{baseUrl}}"],
          "path": ["api", "CopyToDocument", "audit", "submission", "115"]
        }
      }
    },
    {
      "name": "Get Audit By Target Document ID",
      "request": {
        "method": "GET",
        "header": [],
        "url": {
          "raw": "{{baseUrl}}/api/CopyToDocument/audit/target/116",
          "host": ["{{baseUrl}}"],
          "path": ["api", "CopyToDocument", "audit", "target", "116"]
        }
      }
    }
  ],
  "variable": [
    {
      "key": "baseUrl",
      "value": "http://localhost:5000"
    },
    {
      "key": "token",
      "value": "YOUR_TOKEN_HERE"
    }
  ]
}
```

---

## cURL Examples

### 1. Execute CopyToDocument (IDs)

```bash
curl -X POST "http://localhost:5000/api/CopyToDocument/execute" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "config": {
      "sourceDocumentTypeId": 1,
      "sourceFormId": 10,
      "targetDocumentTypeId": 2,
      "targetFormId": 20,
      "createNewDocument": true,
      "initialStatus": "Draft",
      "fieldMapping": {
        "TOTAL_AMOUNT": "CONTRACT_VALUE",
        "REQUEST_DATE": "ORDER_DATE"
      },
      "gridMapping": {
        "ITEMS": "CONTRACT_ITEMS"
      },
      "copyCalculatedFields": true,
      "copyGridRows": true,
      "startWorkflow": false,
      "linkDocuments": true,
      "copyAttachments": false,
      "copyMetadata": false,
      "overrideTargetDefaults": false,
      "metadataFields": []
    },
    "sourceSubmissionId": 115
  }'
```

### 2. Execute CopyToDocument (Codes)

```bash
curl -X POST "http://localhost:5000/api/CopyToDocument/execute-by-codes" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "config": {
      "sourceDocumentTypeCode": "PURCHASE_REQUEST",
      "sourceFormCode": "PR_FORM",
      "targetDocumentTypeCode": "PURCHASE_ORDER",
      "targetFormCode": "PO_FORM",
      "createNewDocument": true,
      "initialStatus": "Draft",
      "fieldMapping": {
        "TOTAL_AMOUNT": "CONTRACT_VALUE"
      },
      "copyCalculatedFields": true,
      "copyGridRows": true,
      "startWorkflow": false,
      "linkDocuments": true
    },
    "sourceSubmissionId": 115
  }'
```

### 3. Get All Audit Records

```bash
curl -X GET "http://localhost:5000/api/CopyToDocument/audit?page=1&pageSize=10" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### 4. Get Audit Record By ID

```bash
curl -X GET "http://localhost:5000/api/CopyToDocument/audit/1" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### 5. Get Audit By Submission ID

```bash
curl -X GET "http://localhost:5000/api/CopyToDocument/audit/submission/115" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### 6. Get Audit By Target Document ID

```bash
curl -X GET "http://localhost:5000/api/CopyToDocument/audit/target/116" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

---

## Test Scenarios

### Scenario 1: Create New Document with Draft Status
```json
{
  "config": {
    "sourceDocumentTypeId": 1,
    "sourceFormId": 10,
    "targetDocumentTypeId": 2,
    "targetFormId": 20,
    "createNewDocument": true,
    "initialStatus": "Draft",
    "fieldMapping": {
      "AMOUNT": "TOTAL"
    },
    "copyCalculatedFields": true,
    "copyGridRows": false,
    "startWorkflow": false,
    "linkDocuments": true,
    "copyAttachments": false,
    "copyMetadata": false,
    "overrideTargetDefaults": false,
    "metadataFields": []
  },
  "sourceSubmissionId": 115
}
```

### Scenario 2: Create New Document with Submitted Status
```json
{
  "config": {
    "sourceDocumentTypeId": 1,
    "sourceFormId": 10,
    "targetDocumentTypeId": 2,
    "targetFormId": 20,
    "createNewDocument": true,
    "initialStatus": "Submitted",
    "fieldMapping": {
      "AMOUNT": "TOTAL"
    },
    "copyCalculatedFields": true,
    "copyGridRows": false,
    "startWorkflow": false,
    "linkDocuments": true,
    "copyAttachments": false,
    "copyMetadata": false,
    "overrideTargetDefaults": false,
    "metadataFields": []
  },
  "sourceSubmissionId": 115
}
```

### Scenario 3: Update Existing Document
```json
{
  "config": {
    "sourceDocumentTypeId": 1,
    "sourceFormId": 10,
    "targetDocumentTypeId": 2,
    "targetFormId": 20,
    "createNewDocument": false,
    "targetDocumentId": 116,
    "initialStatus": "Draft",
    "fieldMapping": {
      "AMOUNT": "TOTAL"
    },
    "copyCalculatedFields": true,
    "copyGridRows": false,
    "startWorkflow": false,
    "linkDocuments": false,
    "copyAttachments": false,
    "copyMetadata": false,
    "overrideTargetDefaults": true,
    "metadataFields": []
  },
  "sourceSubmissionId": 115
}
```

### Scenario 4: Validation Error - Missing Required Fields
```json
{
  "config": {
    "targetDocumentTypeId": 2,
    "targetFormId": 20,
    "createNewDocument": true,
    "fieldMapping": {}
  },
  "sourceSubmissionId": 115
}
```
**Expected Error:**
```json
{
  "statusCode": 400,
  "message": "Validation failed",
  "data": {
    "errors": {
      "config.sourceDocumentTypeId": ["SourceDocumentTypeId must be greater than 0"],
      "config.sourceFormId": ["SourceFormId must be greater than 0"]
    }
  }
}
```

### Scenario 5: Compatibility Error - Document Type Mismatch
```json
{
  "config": {
    "sourceDocumentTypeId": 1,
    "sourceFormId": 10,
    "targetDocumentTypeId": 2,
    "targetFormId": 20,
    "createNewDocument": true,
    "initialStatus": "Draft",
    "fieldMapping": {},
    "copyCalculatedFields": true,
    "copyGridRows": false,
    "startWorkflow": false,
    "linkDocuments": true,
    "copyAttachments": false,
    "copyMetadata": false,
    "overrideTargetDefaults": false,
    "metadataFields": []
  },
  "sourceSubmissionId": 115
}
```
**Expected Error (if submission doesn't match):**
```json
{
  "statusCode": 500,
  "message": "Source submission 115 document type 3 does not match configured SourceDocumentTypeId 1"
}
```

---

## Quick Test Checklist

- [ ] Test with valid sourceDocumentTypeId and sourceFormId
- [ ] Test with missing sourceDocumentTypeId (should fail)
- [ ] Test with missing sourceFormId (should fail)
- [ ] Test with initialStatus = "Draft"
- [ ] Test with initialStatus = "Submitted"
- [ ] Test with invalid initialStatus (should fail)
- [ ] Test createNewDocument = true
- [ ] Test createNewDocument = false with targetDocumentId
- [ ] Test field mapping
- [ ] Test grid mapping
- [ ] Test copyAttachments = true
- [ ] Test copyMetadata = true
- [ ] Test startWorkflow = true
- [ ] Test document type compatibility validation
- [ ] Test field type compatibility validation
- [ ] Test audit endpoints

---

## Notes

1. **Required Fields (NEW):**
   - `sourceDocumentTypeId` - **Required**
   - `sourceFormId` - **Required** (was optional before)
   - `initialStatus` - Optional (default: "Draft")

2. **Validation:**
   - Source and target document compatibility is validated
   - Field existence is validated
   - Data type compatibility is validated

3. **Error Handling:**
   - All validation errors return 400 Bad Request
   - Execution errors return 500 Internal Server Error
   - Errors include detailed messages

4. **Audit:**
   - All executions are logged in COPY_TO_DOCUMENT_AUDIT table
   - Audit records include success/failure status
   - Audit records include execution details

