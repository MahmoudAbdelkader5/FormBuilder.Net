# CopyToDocument - مرجع سريع

## 🎯 ما هي CopyToDocument؟

Action مدمج في Actions Engine لنسخ البيانات تلقائيًا من Document/Form Submission إلى Document/Form آخر بناءً على Configuration.

---

## 📍 أين تعمل؟

### 1. داخل Actions Engine
- **ActionType**: `CopyToDocument`
- **Location**: `FORM_RULE_ACTIONS` table
- **Configuration**: JSON في `action.Value`

### 2. Events المدعومة
- ✅ `OnFormSubmitted` - عند إرسال Form
- ✅ `OnApprovalCompleted` - عند إكمال Approval
- ✅ `OnDocumentApproved` - عند الموافقة على Document

---

## 🔧 Configuration Structure

```json
{
  "sourceDocumentTypeId": 1,      // مطلوب
  "sourceFormId": 10,              // مطلوب
  "targetDocumentTypeId": 2,       // مطلوب
  "targetFormId": 20,              // مطلوب
  "createNewDocument": true,
  "initialStatus": "Draft",
  "fieldMapping": {
    "SOURCE_FIELD": "TARGET_FIELD"
  },
  "gridMapping": {
    "SOURCE_GRID": "TARGET_GRID"
  },
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

---

## 📋 الملفات الأساسية

### Models
- `COPY_TO_DOCUMENT_AUDIT.cs` - Audit Entity
- `FORM_SUBMISSIONS.cs` - يحتوي على `ParentDocumentId`

### DTOs
- `CopyToDocumentActionDto.cs` - Configuration DTO
- `CopyToDocumentResultDto.cs` - Result DTO
- `CopyToDocumentActionByCodesDto.cs` - DTO للاستخدام بـ Codes

### Services
- `ICopyToDocumentService.cs` - Interface
- `CopyToDocumentService.cs` - Service الرئيسي
- `CopyToDocumentActionExecutorService.cs` - Executor للـ Rules

### Controllers
- `CopyToDocumentController.cs` - API Controller

---

## 🚀 الاستخدام السريع

### 1. إضافة Action في Rule

```sql
INSERT INTO FORM_RULE_ACTIONS (
  RuleId, 
  ActionType, 
  ActionOrder, 
  IsActive, 
  Value
) VALUES (
  1,
  'CopyToDocument',
  1,
  1,
  '{
    "sourceDocumentTypeId": 1,
    "sourceFormId": 10,
    "targetDocumentTypeId": 2,
    "targetFormId": 20,
    "createNewDocument": true,
    "fieldMapping": {"TOTAL_AMOUNT": "CONTRACT_VALUE"},
    "gridMapping": {"ITEMS": "CONTRACT_ITEMS"},
    "copyGridRows": true,
    "linkDocuments": true
  }'
);
```

### 2. API Call (يدوي)

```http
POST /api/CopyToDocument/execute
{
  "config": { ... },
  "sourceSubmissionId": 115
}
```

---

## ✅ الميزات المدعومة

- ✅ Field Mapping (باستخدام FieldCode)
- ✅ Grid Mapping
- ✅ Copy Attachments
- ✅ Copy Metadata
- ✅ Link Documents (ParentDocumentId)
- ✅ Start Workflow
- ✅ Validation شامل
- ✅ Transaction Management
- ✅ Audit Logging

---

## 🔍 Validation

### Required Fields
- `SourceDocumentTypeId` > 0
- `SourceFormId` > 0
- `TargetDocumentTypeId` > 0
- `TargetFormId` > 0
- `SourceSubmissionId` > 0

### Field Mapping Validation
- ✅ Source Field موجود في Source Form
- ✅ Target Field موجود في Target Form
- ✅ Data Types متوافقة

---

## 📊 Audit

### Audit Table
```sql
SELECT * FROM COPY_TO_DOCUMENT_AUDIT
WHERE SourceSubmissionId = 115
ORDER BY ExecutionDate DESC
```

### API Endpoints
- `GET /api/CopyToDocument/audit` - جميع السجلات
- `GET /api/CopyToDocument/audit/{id}` - سجل محدد
- `GET /api/CopyToDocument/audit/submission/{submissionId}` - سجلات لمستند مصدر
- `GET /api/CopyToDocument/audit/target/{targetDocumentId}` - سجلات لمستند هدف

---

## ⚠️ ملاحظات مهمة

1. **FieldCode وليس FieldId**: يستخدم FieldCode لضمان الاستقرار عبر Form Versions
2. **Transaction**: جميع العمليات داخل Transaction واحدة - إما نجاح كامل أو فشل كامل
3. **Workflow**: يتم بدؤه بعد Commit Transaction
4. **Document Number**: يتم توليده تلقائيًا من Document Series

---

## 🎯 مثال عملي

### Purchase Request → Purchase Order

```json
{
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
  "copyAttachments": true
}
```

---

## 📚 المزيد من المعلومات

راجع `CopyToDocument-Complete-Guide-Arabic.md` للدليل الشامل.

