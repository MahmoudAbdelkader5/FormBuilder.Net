# CopyToDocument Test Scenario: Purchase Request → Purchase Order

## 🎯 السيناريو

**Purchase Request** يتعمله Approval → عند إكمال Approval (`OnApprovalCompleted`) → **CopyToDocument** يعمل:
- ✅ إنشاء **Purchase Order** جديد
- ✅ نقل حقول محددة + Grid + Attachments
- ✅ وضع `ParentDocumentId = SourceDocumentId` (LinkDocuments)
- ✅ بدء Workflow للـ Purchase Order (StartWorkflow)

---

## 📋 المتطلبات قبل الاختبار

### 1. البيانات المطلوبة
```sql
-- تأكد من وجود:
-- 1. Document Type: Purchase Request (ID = 1 أو احصل على ID الفعلي)
-- 2. Form: Purchase Request Form (ID = 10 أو احصل على ID الفعلي)
-- 3. Document Type: Purchase Order (ID = 2 أو احصل على ID الفعلي)
-- 4. Form: Purchase Order Form (ID = 20 أو احصل على ID الفعلي)
-- 5. Purchase Request Submission موجود ومكتمل Approval (ID = 115 أو استخدم ID فعلي)
```

### 2. الحقول المطلوبة في Forms
**Purchase Request Form (Source):**
- `REQUEST_AMOUNT` (Number/Currency)
- `REQUEST_DATE` (Date)
- `SUPPLIER_NAME` (Text)
- `REQUEST_DESCRIPTION` (Text)
- Grid: `REQUEST_ITEMS` (Grid Code)

**Purchase Order Form (Target):**
- `ORDER_AMOUNT` (Number/Currency)
- `ORDER_DATE` (Date)
- `VENDOR_NAME` (Text)
- `ORDER_DESCRIPTION` (Text)
- Grid: `ORDER_ITEMS` (Grid Code)

---

## 🚀 Test API URLs & JSON

### Base URL
```
http://localhost:5000
```
أو
```
https://your-api-domain.com
```

---

## 1️⃣ Test 1: CopyToDocument كامل (Purchase Request → Purchase Order)

### API URL
```
POST http://localhost:5000/api/CopyToDocument/execute
```

### Headers
```json
{
  "Content-Type": "application/json",
  "Authorization": "Bearer YOUR_ACCESS_TOKEN"
}
```

### Request Body (JSON)
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
      "REQUEST_AMOUNT": "ORDER_AMOUNT",
      "REQUEST_DATE": "ORDER_DATE",
      "SUPPLIER_NAME": "VENDOR_NAME",
      "REQUEST_DESCRIPTION": "ORDER_DESCRIPTION"
    },
    "gridMapping": {
      "REQUEST_ITEMS": "ORDER_ITEMS"
    },
    "copyCalculatedFields": true,
    "copyGridRows": true,
    "startWorkflow": true,
    "linkDocuments": true,
    "copyAttachments": true,
    "copyMetadata": false,
    "overrideTargetDefaults": false,
    "metadataFields": []
  },
  "sourceSubmissionId": 115,
  "actionId": null,
  "ruleId": null
}
```

### cURL Command
```bash
curl -X POST "http://localhost:5000/api/CopyToDocument/execute" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN" \
  -d '{
    "config": {
      "sourceDocumentTypeId": 1,
      "sourceFormId": 10,
      "targetDocumentTypeId": 2,
      "targetFormId": 20,
      "createNewDocument": true,
      "initialStatus": "Draft",
      "fieldMapping": {
        "REQUEST_AMOUNT": "ORDER_AMOUNT",
        "REQUEST_DATE": "ORDER_DATE",
        "SUPPLIER_NAME": "VENDOR_NAME",
        "REQUEST_DESCRIPTION": "ORDER_DESCRIPTION"
      },
      "gridMapping": {
        "REQUEST_ITEMS": "ORDER_ITEMS"
      },
      "copyCalculatedFields": true,
      "copyGridRows": true,
      "startWorkflow": true,
      "linkDocuments": true,
      "copyAttachments": true,
      "copyMetadata": false,
      "overrideTargetDefaults": false,
      "metadataFields": []
    },
    "sourceSubmissionId": 115,
    "actionId": null,
    "ruleId": null
  }'
```

### Expected Response (Success)
```json
{
  "statusCode": 200,
  "message": "CopyToDocument executed successfully",
  "data": {
    "success": true,
    "targetDocumentId": 116,
    "targetDocumentNumber": "PO-000116",
    "errorMessage": null,
    "fieldsCopied": 4,
    "gridRowsCopied": 5,
    "actionId": null,
    "sourceSubmissionId": 115
  }
}
```

---

## 2️⃣ Test 2: CopyToDocument باستخدام Codes (بدلاً من IDs)

### API URL
```
POST http://localhost:5000/api/CopyToDocument/execute-by-codes
```

### Request Body (JSON)
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
      "REQUEST_AMOUNT": "ORDER_AMOUNT",
      "REQUEST_DATE": "ORDER_DATE",
      "SUPPLIER_NAME": "VENDOR_NAME"
    },
    "gridMapping": {
      "REQUEST_ITEMS": "ORDER_ITEMS"
    },
    "copyCalculatedFields": true,
    "copyGridRows": true,
    "startWorkflow": true,
    "linkDocuments": true,
    "copyAttachments": true,
    "copyMetadata": false,
    "overrideTargetDefaults": false,
    "metadataFields": []
  },
  "sourceSubmissionId": 115,
  "actionId": null,
  "ruleId": null
}
```

### cURL Command
```bash
curl -X POST "http://localhost:5000/api/CopyToDocument/execute-by-codes" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN" \
  -d '{
    "config": {
      "sourceDocumentTypeCode": "PURCHASE_REQUEST",
      "sourceFormCode": "PR_FORM",
      "targetDocumentTypeCode": "PURCHASE_ORDER",
      "targetFormCode": "PO_FORM",
      "createNewDocument": true,
      "initialStatus": "Draft",
      "fieldMapping": {
        "REQUEST_AMOUNT": "ORDER_AMOUNT",
        "REQUEST_DATE": "ORDER_DATE",
        "SUPPLIER_NAME": "VENDOR_NAME"
      },
      "gridMapping": {
        "REQUEST_ITEMS": "ORDER_ITEMS"
      },
      "copyCalculatedFields": true,
      "copyGridRows": true,
      "startWorkflow": true,
      "linkDocuments": true,
      "copyAttachments": true
    },
    "sourceSubmissionId": 115
  }'
```

---

## 3️⃣ Test 3: CopyToDocument بدون Attachments (Attachments = false)

### Request Body (JSON)
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
      "REQUEST_AMOUNT": "ORDER_AMOUNT",
      "REQUEST_DATE": "ORDER_DATE",
      "SUPPLIER_NAME": "VENDOR_NAME"
    },
    "gridMapping": {
      "REQUEST_ITEMS": "ORDER_ITEMS"
    },
    "copyCalculatedFields": true,
    "copyGridRows": true,
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

---

## 4️⃣ Test 4: CopyToDocument بدون Workflow (StartWorkflow = false)

### Request Body (JSON)
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
      "REQUEST_AMOUNT": "ORDER_AMOUNT",
      "REQUEST_DATE": "ORDER_DATE",
      "SUPPLIER_NAME": "VENDOR_NAME"
    },
    "gridMapping": {
      "REQUEST_ITEMS": "ORDER_ITEMS"
    },
    "copyCalculatedFields": true,
    "copyGridRows": true,
    "startWorkflow": false,
    "linkDocuments": true,
    "copyAttachments": true,
    "copyMetadata": false,
    "overrideTargetDefaults": false,
    "metadataFields": []
  },
  "sourceSubmissionId": 115
}
```

---

## 5️⃣ Test 5: CopyToDocument مع Metadata

### Request Body (JSON)
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
      "REQUEST_AMOUNT": "ORDER_AMOUNT",
      "REQUEST_DATE": "ORDER_DATE"
    },
    "gridMapping": {
      "REQUEST_ITEMS": "ORDER_ITEMS"
    },
    "copyCalculatedFields": true,
    "copyGridRows": true,
    "startWorkflow": true,
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

---

## 📊 Audit Tests

### 6️⃣ Get All Audit Records
```
GET http://localhost:5000/api/CopyToDocument/audit?page=1&pageSize=50
```

### cURL
```bash
curl -X GET "http://localhost:5000/api/CopyToDocument/audit?page=1&pageSize=50" \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN"
```

### 7️⃣ Get Audit by Source Submission ID
```
GET http://localhost:5000/api/CopyToDocument/audit/submission/115
```

### cURL
```bash
curl -X GET "http://localhost:5000/api/CopyToDocument/audit/submission/115" \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN"
```

### 8️⃣ Get Audit by Target Document ID
```
GET http://localhost:5000/api/CopyToDocument/audit/target/116
```

### cURL
```bash
curl -X GET "http://localhost:5000/api/CopyToDocument/audit/target/116" \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN"
```

### Expected Audit Response
```json
{
  "statusCode": 200,
  "message": "Audit records retrieved successfully",
  "data": [
    {
      "id": 1,
      "sourceSubmissionId": 115,
      "targetDocumentId": 116,
      "actionId": null,
      "ruleId": null,
      "sourceFormId": 10,
      "targetFormId": 20,
      "targetDocumentTypeId": 2,
      "success": true,
      "errorMessage": null,
      "fieldsCopied": 4,
      "gridRowsCopied": 5,
      "targetDocumentNumber": "PO-000116",
      "executionDate": "2024-02-08T10:30:00Z",
      "createdDate": "2024-02-08T10:30:00Z",
      "createdByUserId": "user123"
    }
  ]
}
```

---

## ✅ Checklist للاختبار

### قبل الاختبار
- [ ] تأكد من وجود Purchase Request Submission (ID = 115 أو ID فعلي)
- [ ] تأكد من أن Purchase Request مكتمل Approval
- [ ] تأكد من وجود Document Types و Forms المطلوبة
- [ ] تأكد من وجود الحقول المطلوبة في Forms
- [ ] تأكد من وجود Grids المطلوبة
- [ ] احصل على Access Token

### أثناء الاختبار
- [ ] Test 1: CopyToDocument كامل (مع جميع الخيارات)
- [ ] Test 2: CopyToDocument باستخدام Codes
- [ ] Test 3: CopyToDocument بدون Attachments
- [ ] Test 4: CopyToDocument بدون Workflow
- [ ] Test 5: CopyToDocument مع Metadata
- [ ] Test 6: Get All Audit Records
- [ ] Test 7: Get Audit by Source Submission
- [ ] Test 8: Get Audit by Target Document

### التحقق من النتائج
- [ ] تم إنشاء Purchase Order جديد
- [ ] تم نسخ الحقول المطلوبة (FieldsCopied > 0)
- [ ] تم نسخ Grid Rows (GridRowsCopied > 0)
- [ ] تم نسخ Attachments (إذا كان copyAttachments = true)
- [ ] تم وضع ParentDocumentId في Target Document
- [ ] تم بدء Workflow (إذا كان startWorkflow = true)
- [ ] تم تسجيل Audit Record
- [ ] Response يحتوي على TargetDocumentId و TargetDocumentNumber

---

## 🔍 Validation Tests

### 9️⃣ Test: Missing sourceDocumentTypeId (يجب أن يفشل)
```json
{
  "config": {
    "sourceFormId": 10,
    "targetDocumentTypeId": 2,
    "targetFormId": 20,
    "createNewDocument": true,
    "fieldMapping": {}
  },
  "sourceSubmissionId": 115
}
```

### Expected Error Response
```json
{
  "statusCode": 400,
  "message": "Validation failed",
  "data": {
    "errors": {
      "config.sourceDocumentTypeId": ["SourceDocumentTypeId must be greater than 0"]
    }
  }
}
```

### 🔟 Test: Invalid Field Mapping (Field غير موجود)
```json
{
  "config": {
    "sourceDocumentTypeId": 1,
    "sourceFormId": 10,
    "targetDocumentTypeId": 2,
    "targetFormId": 20,
    "createNewDocument": true,
    "fieldMapping": {
      "NON_EXISTENT_FIELD": "TARGET_FIELD"
    }
  },
  "sourceSubmissionId": 115
}
```

### Expected Error Response
```json
{
  "statusCode": 500,
  "message": "Error executing CopyToDocument: Source field 'NON_EXISTENT_FIELD' not found in source form 10",
  "data": {
    "success": false,
    "errorMessage": "Source field 'NON_EXISTENT_FIELD' not found in source form 10"
  }
}
```

---

## 📝 Postman Collection

يمكنك استيراد ملف `CopyToDocument.postman_collection.json` الموجود في:
```
FormBuilder.Core/Docs/CopyToDocument.postman_collection.json
```

### Postman Variables
```
baseUrl: http://localhost:5000
token: YOUR_ACCESS_TOKEN
submissionId: 115
targetDocumentId: 116
```

---

## 🎯 سيناريو الاختبار الكامل

### الخطوة 1: إنشاء Purchase Request Submission
1. قم بإنشاء Purchase Request جديد
2. املأ الحقول المطلوبة
3. أضف Items في Grid
4. أرفق ملفات (Attachments)
5. Submit للـ Approval

### الخطوة 2: إكمال Approval
1. قم بالموافقة على Purchase Request
2. عند `OnApprovalCompleted`، سيتم استدعاء CopyToDocument تلقائيًا

### الخطوة 3: التحقق من النتائج
1. تحقق من إنشاء Purchase Order جديد
2. تحقق من نسخ الحقول
3. تحقق من نسخ Grid Items
4. تحقق من نسخ Attachments
5. تحقق من ParentDocumentId
6. تحقق من بدء Workflow
7. تحقق من Audit Record

---

## 🔧 Troubleshooting

### Error: "Source submission not found"
- ✅ تأكد من أن `sourceSubmissionId` صحيح
- ✅ تأكد من أن Submission موجود في Database

### Error: "Source field not found"
- ✅ تأكد من أن FieldCode صحيح في Source Form
- ✅ تأكد من أن FieldCode موجود وغير محذوف

### Error: "Target field not found"
- ✅ تأكد من أن FieldCode صحيح في Target Form
- ✅ تأكد من أن FieldCode موجود وغير محذوف

### Error: "No active document series found"
- ✅ تأكد من وجود Document Series للـ Target Document Type
- ✅ تأكد من أن Series نشط (IsActive = true)

### Error: "Data type mismatch"
- ✅ تأكد من أن Data Types متوافقة بين Source و Target Fields
- ✅ راجع قائمة الأنواع المتوافقة في Documentation

---

## 📚 مراجع إضافية

- `CopyToDocument-Complete-Guide-Arabic.md` - الدليل الشامل
- `CopyToDocument-Quick-Reference-Arabic.md` - المرجع السريع
- `CopyToDocument.postman_collection.json` - Postman Collection

---

## ✅ الخلاصة

هذا الملف يحتوي على جميع Test Cases المطلوبة لاختبار سيناريو **Purchase Request → Purchase Order** مع:
- ✅ نسخ الحقول
- ✅ نسخ Grid
- ✅ نسخ Attachments
- ✅ ربط المستندات (ParentDocumentId)
- ✅ بدء Workflow

جميع الـ API URLs و JSON Examples جاهزة للاستخدام! 🚀

