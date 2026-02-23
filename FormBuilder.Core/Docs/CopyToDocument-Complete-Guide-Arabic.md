# دليل شامل: ميزة CopyToDocument في FormBuilder

## 📋 نظرة عامة

ميزة **CopyToDocument** هي Action مدمج في Actions Engine تسمح بنسخ البيانات تلقائيًا من Document/Form Submission إلى Document/Form آخر بناءً على Configuration (إعدادات) من غير الحاجة لكتابة كود لكل حالة.

---

## ✅ ما تم تنفيذه بالفعل

### 1. البنية الأساسية

#### ✅ Models & Entities
- `COPY_TO_DOCUMENT_AUDIT` - جدول Audit لتسجيل كل عملية نسخ
- `FORM_SUBMISSIONS.ParentDocumentId` - حقل لربط المستندات (تم إضافته في Migration)

#### ✅ DTOs
- `CopyToDocumentActionDto` - Configuration DTO كامل مع جميع الخيارات
- `CopyToDocumentResultDto` - Result DTO يحتوي على نتائج التنفيذ
- `CopyToDocumentActionByCodesDto` - DTO للاستخدام بـ Codes بدلاً من IDs
- `CopyToDocumentAuditDto` - DTO لسجلات Audit

#### ✅ Services
- `ICopyToDocumentService` - Interface للخدمة
- `CopyToDocumentService` - Service كامل للتنفيذ مع:
  - ✅ Validation شامل
  - ✅ Field Mapping
  - ✅ Grid Mapping
  - ✅ Attachments Copying
  - ✅ Metadata Copying
  - ✅ Transaction Management
  - ✅ Error Handling
  - ✅ Audit Logging

- `CopyToDocumentActionExecutorService` - Service لتنفيذ Actions من Rules تلقائيًا

#### ✅ Controllers
- `CopyToDocumentController` - Controller كامل مع:
  - ✅ `POST /api/CopyToDocument/execute` - تنفيذ يدوي (باستخدام IDs)
  - ✅ `POST /api/CopyToDocument/execute-by-codes` - تنفيذ يدوي (باستخدام Codes)
  - ✅ `GET /api/CopyToDocument/audit` - جلب جميع سجلات Audit
  - ✅ `GET /api/CopyToDocument/audit/{id}` - جلب سجل Audit محدد
  - ✅ `GET /api/CopyToDocument/audit/submission/{submissionId}` - جلب سجلات Audit لمستند مصدر
  - ✅ `GET /api/CopyToDocument/audit/target/{targetDocumentId}` - جلب سجلات Audit لمستند هدف

#### ✅ Integration
- ✅ Integration مع `FormSubmissionTriggersService` - يتم استدعاء CopyToDocument تلقائيًا عند:
  - `OnFormSubmitted` - عند إرسال Form
  - `OnApprovalCompleted` - عند إكمال Approval
- ✅ Integration مع `FORM_RULESService` - دعم CopyToDocument كـ Action Type في Rules

---

## 🏗️ البنية المعمارية

### 1. Execution Flow

```
Event Trigger (OnFormSubmitted / OnApprovalCompleted)
    ↓
FormSubmissionTriggersService
    ↓
CopyToDocumentActionExecutorService
    ↓
    ├─ Load Active Rules
    ├─ Filter CopyToDocument Actions
    ├─ Parse Configuration (JSON)
    └─ Execute for each Action
        ↓
CopyToDocumentService.ExecuteCopyToDocumentAsync()
    ↓
    ├─ Validation
    │   ├─ Source Document Type & Form
    │   ├─ Target Document Type & Form
    │   ├─ Field Mappings
    │   └─ Data Type Compatibility
    ├─ Create/Get Target Document
    ├─ Copy Field Values
    ├─ Copy Grid Data (optional)
    ├─ Copy Attachments (optional)
    ├─ Copy Metadata (optional)
    ├─ Link Documents (optional)
    ├─ Save Changes (Transaction)
    ├─ Start Workflow (optional)
    └─ Log Audit
```

### 2. Configuration Structure

```json
{
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
}
```

---

## 📝 كيفية الاستخدام

### 1. إعداد CopyToDocument Action في Form Rule

#### الخطوة 1: إنشاء Rule
```csharp
// في FORM_RULES
{
  "FormBuilderId": 10,
  "RuleName": "Copy PR to PO on Approval",
  "IsActive": true,
  "TriggerEvent": "OnApprovalCompleted"
}
```

#### الخطوة 2: إضافة CopyToDocument Action
```csharp
// في FORM_RULE_ACTIONS
{
  "RuleId": 1,
  "ActionType": "CopyToDocument",
  "ActionOrder": 1,
  "IsActive": true,
  "Value": "{
    \"sourceDocumentTypeId\": 1,
    \"sourceFormId\": 10,
    \"targetDocumentTypeId\": 2,
    \"targetFormId\": 20,
    \"createNewDocument\": true,
    \"initialStatus\": \"Draft\",
    \"fieldMapping\": {
      \"TOTAL_AMOUNT\": \"CONTRACT_VALUE\",
      \"REQUEST_DATE\": \"ORDER_DATE\"
    },
    \"gridMapping\": {
      \"ITEMS\": \"CONTRACT_ITEMS\"
    },
    \"copyCalculatedFields\": true,
    \"copyGridRows\": true,
    \"startWorkflow\": false,
    \"linkDocuments\": true,
    \"copyAttachments\": false
  }"
}
```

### 2. التنفيذ اليدوي عبر API

#### استخدام IDs
```http
POST /api/CopyToDocument/execute
Content-Type: application/json
Authorization: Bearer {token}

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
    "gridMapping": {
      "ITEMS": "CONTRACT_ITEMS"
    },
    "copyCalculatedFields": true,
    "copyGridRows": true,
    "startWorkflow": false,
    "linkDocuments": true,
    "copyAttachments": false
  },
  "sourceSubmissionId": 115,
  "actionId": null,
  "ruleId": null
}
```

#### استخدام Codes
```http
POST /api/CopyToDocument/execute-by-codes
Content-Type: application/json
Authorization: Bearer {token}

{
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
    "gridMapping": {
      "ITEMS": "CONTRACT_ITEMS"
    },
    "copyCalculatedFields": true,
    "copyGridRows": true,
    "startWorkflow": false,
    "linkDocuments": true
  },
  "sourceSubmissionId": 115
}
```

### 3. جلب سجلات Audit

```http
GET /api/CopyToDocument/audit?sourceSubmissionId=115&page=1&pageSize=50
Authorization: Bearer {token}
```

---

## 🔧 Configuration Options

### A) تعريف المصدر (Source)
- ✅ `SourceDocumentTypeId` - **مطلوب** - ID نوع المستند المصدر
- ✅ `SourceFormId` - **مطلوب** - ID الفورم المصدر
- ✅ `SourceSubmissionId` - اختياري - ID الـ Submission (يتم استخدام الـ Submission الحالي إذا لم يتم التحديد)

### B) تعريف الهدف (Target)
- ✅ `TargetDocumentTypeId` - **مطلوب** - ID نوع المستند الهدف
- ✅ `TargetFormId` - **مطلوب** - ID الفورم الهدف
- ✅ `CreateNewDocument` - `true` لإنشاء مستند جديد، `false` لتحديث مستند موجود
- ✅ `TargetDocumentId` - مطلوب إذا كان `CreateNewDocument = false`
- ✅ `InitialStatus` - "Draft" أو "Submitted" (افتراضي: "Draft")

### C) Field Mapping
- ✅ `FieldMapping` - Dictionary: `SourceFieldCode → TargetFieldCode`
  - مثال: `{"TOTAL_AMOUNT": "CONTRACT_VALUE", "CUSTOMER_NAME": "PARTY_NAME"}`
  - **مهم**: يستخدم FieldCode وليس FieldId لضمان الاستقرار عبر Form Versions

### D) Grid Mapping
- ✅ `GridMapping` - Dictionary: `SourceGridCode → TargetGridCode`
  - مثال: `{"ITEMS": "CONTRACT_ITEMS"}`

### E) خيارات التنفيذ (Options)
- ✅ `CopyCalculatedFields` - نسخ الحقول المحسوبة (افتراضي: `true`)
- ✅ `CopyGridRows` - نسخ صفوف الجداول (افتراضي: `true`)
- ✅ `CopyAttachments` - نسخ المرفقات (افتراضي: `false`)
- ✅ `CopyMetadata` - نسخ Metadata (افتراضي: `false`)
- ✅ `MetadataFields` - قائمة حقول Metadata للنسخ (مثل: `["SubmittedDate", "SubmittedByUserId"]`)
- ✅ `OverrideTargetDefaults` - استبدال القيم الافتراضية في الهدف (افتراضي: `false`)
- ✅ `LinkDocuments` - ربط المستندات (ParentDocumentId) (افتراضي: `true`)
- ✅ `StartWorkflow` - بدء Workflow للمستند الهدف (افتراضي: `false`)

---

## 🔍 Validation Rules

### 1. Source Validation
- ✅ Source Document Type موجود
- ✅ Source Form موجود
- ✅ Source Submission موجود
- ✅ Source Submission يطابق SourceDocumentTypeId و SourceFormId

### 2. Target Validation
- ✅ Target Document Type موجود
- ✅ Target Form موجود
- ✅ إذا `CreateNewDocument = false`، يجب أن يكون TargetDocumentId موجود وصالح

### 3. Field Mapping Validation
- ✅ جميع Source Fields موجودة في Source Form
- ✅ جميع Target Fields موجودة في Target Form
- ✅ Data Types متوافقة بين Source و Target

### 4. Data Type Compatibility
الأنواع المتوافقة:
- `TEXT` ↔ `TEXT`, `TEXTAREA`, `RICH_TEXT`, `EMAIL`, `PHONE`, `URL`
- `NUMBER` ↔ `NUMBER`, `DECIMAL`, `CURRENCY`, `PERCENTAGE`
- `DATE` ↔ `DATE`, `DATETIME`
- `BOOLEAN` ↔ `BOOLEAN`, `CHECKBOX`

---

## 📊 Audit & Traceability

### Audit Table Structure
```sql
COPY_TO_DOCUMENT_AUDIT
├─ Id
├─ SourceSubmissionId
├─ TargetDocumentId
├─ ActionId (من FORM_RULE_ACTIONS)
├─ RuleId (من FORM_RULES)
├─ SourceFormId
├─ TargetFormId
├─ TargetDocumentTypeId
├─ Success
├─ ErrorMessage
├─ FieldsCopied
├─ GridRowsCopied
├─ TargetDocumentNumber
├─ ExecutionDate
└─ CreatedByUserId
```

### Audit Endpoints
- ✅ `GET /api/CopyToDocument/audit` - جميع السجلات مع Pagination و Filters
- ✅ `GET /api/CopyToDocument/audit/{id}` - سجل محدد
- ✅ `GET /api/CopyToDocument/audit/submission/{submissionId}` - سجلات لمستند مصدر
- ✅ `GET /api/CopyToDocument/audit/target/{targetDocumentId}` - سجلات لمستند هدف

---

## 🎯 سيناريوهات الاستخدام

### سيناريو 1: Purchase Request → Purchase Order
```json
{
  "sourceDocumentTypeId": 1,  // Purchase Request
  "sourceFormId": 10,
  "targetDocumentTypeId": 2,   // Purchase Order
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
  "copyAttachments": true
}
```

### سيناريو 2: Quotation → Sales Order
```json
{
  "sourceDocumentTypeId": 3,  // Quotation
  "sourceFormId": 30,
  "targetDocumentTypeId": 4,   // Sales Order
  "targetFormId": 40,
  "createNewDocument": true,
  "initialStatus": "Submitted",
  "fieldMapping": {
    "QUOTE_TOTAL": "ORDER_TOTAL",
    "CUSTOMER_ID": "CUSTOMER_ID",
    "VALID_UNTIL": "REQUIRED_DATE"
  },
  "gridMapping": {
    "QUOTE_ITEMS": "ORDER_ITEMS"
  },
  "copyCalculatedFields": true,
  "copyGridRows": true,
  "startWorkflow": true,
  "linkDocuments": true,
  "copyAttachments": false,
  "copyMetadata": true,
  "metadataFields": ["SubmittedDate", "SubmittedByUserId"]
}
```

---

## ⚠️ ملاحظات مهمة

### 1. FieldCode vs FieldId
- ✅ **يستخدم FieldCode دائماً** وليس FieldId
- ✅ هذا يضمن الاستقرار عبر Form Versions
- ✅ FieldCode يجب أن يكون فريد داخل Form

### 2. Transaction Management
- ✅ جميع العمليات تتم داخل Transaction
- ✅ في حالة الفشل، يتم Rollback كامل
- ✅ لا يتم تنفيذ جزئي - إما نجاح كامل أو فشل كامل

### 3. Workflow Execution
- ✅ Workflow يتم بدؤه **بعد** Commit Transaction
- ✅ هذا يمنع مشاكل Nested Transactions
- ✅ إذا فشل Workflow، يتم تسجيل الخطأ في Result

### 4. Document Number Generation
- ✅ يتم توليد Document Number تلقائيًا من Document Series
- ✅ يتم التحقق من التكرار مع Retry Logic (حتى 10 محاولات)
- ✅ يستخدم ProjectId من Source Document

### 5. Attachments Copying
- ✅ يتم نسخ الملفات فعليًا (ليس فقط الروابط)
- ✅ يتم حفظ الملفات في مجلد منفصل للمستند الهدف
- ✅ يتم التحقق من وجود الملف قبل النسخ

---

## 🔄 Integration Points

### 1. Actions Engine
- ✅ CopyToDocument هو Action Type في `FORM_RULE_ACTIONS`
- ✅ يتم تنفيذه تلقائيًا عند حدوث Events:
  - `OnFormSubmitted`
  - `OnApprovalCompleted`
  - `OnDocumentApproved`

### 2. Form Submission Triggers
- ✅ `FormSubmissionTriggersService` يستدعي `CopyToDocumentActionExecutorService`
- ✅ يتم التنفيذ بعد Save Submission
- ✅ يتم التنفيذ بعد Approval Completion

### 3. Rules Engine
- ✅ `FORM_RULESService` يدعم CopyToDocument كـ Valid Action Type
- ✅ Validation يتحقق من وجود Configuration في `action.Value`

---

## 📈 Performance Considerations

### 1. Batch Operations
- ✅ يتم نسخ Grid Rows في Batch
- ✅ يتم نسخ Attachments بشكل متوازي (Parallel)

### 2. Database Queries
- ✅ استخدام `GetByIdWithDetailsAsync` لتقليل Queries
- ✅ استخدام Dictionary Lookups للـ Field Mappings

### 3. Transaction Scope
- ✅ Transaction يتم Commit فقط عند النجاح الكامل
- ✅ Rollback فوري في حالة الفشل

---

## 🐛 Error Handling

### 1. Validation Errors
- ✅ يتم إرجاع Error Message واضح
- ✅ يتم تسجيل الخطأ في Audit
- ✅ Transaction يتم Rollback

### 2. Execution Errors
- ✅ يتم Catch جميع Exceptions
- ✅ يتم تسجيل الخطأ في Log
- ✅ يتم إرجاع Error Message في Result

### 3. Partial Failures
- ✅ **لا يوجد Partial Execution** - إما نجاح كامل أو فشل كامل
- ✅ جميع العمليات داخل Transaction واحدة

---

## 📚 ملفات التوثيق الإضافية

1. `CopyToDocument.md` - المواصفات الكاملة
2. `CopyToDocument-API-Testing.md` - أمثلة API Testing
3. `CopyToDocument-Angular-Integration.md` - Integration مع Angular
4. `CopyToDocument.postman_collection.json` - Postman Collection

---

## ✅ Checklist للتنفيذ

### ✅ تم تنفيذه
- [x] Database Schema (Migration)
- [x] Models & Entities
- [x] DTOs (كاملة)
- [x] Services (كاملة)
- [x] Controllers (كاملة)
- [x] Validation (شاملة)
- [x] Field Mapping
- [x] Grid Mapping
- [x] Attachments Copying
- [x] Metadata Copying
- [x] Audit Logging
- [x] Error Handling
- [x] Transaction Management
- [x] Integration مع Actions Engine
- [x] Integration مع Form Submission Triggers
- [x] API Endpoints (كاملة)
- [x] Documentation (شاملة)

### 🔄 تحسينات مقترحة (اختيارية)
- [ ] UI Screen لتكوين CopyToDocument Mappings (في Admin Dashboard)
- [ ] Visual Field Mapping Tool
- [ ] Preview قبل التنفيذ
- [ ] Scheduled CopyToDocument (Background Jobs)
- [ ] CopyToDocument Templates
- [ ] Advanced Filtering في Audit
- [ ] Export Audit Reports
- [ ] Performance Monitoring & Metrics

---

## 🎓 الخلاصة

ميزة **CopyToDocument** تم تنفيذها بالكامل وتعمل بشكل تلقائي من خلال Actions Engine. يمكن استخدامها:

1. **تلقائياً**: من خلال Form Rules عند حدوث Events
2. **يدوياً**: من خلال API Endpoints

جميع المتطلبات المذكورة في المواصفات تم تنفيذها:
- ✅ Configuration-based (بدون كود)
- ✅ Field Mapping باستخدام FieldCode
- ✅ Grid Mapping
- ✅ Attachments Copying
- ✅ Metadata Copying
- ✅ Validation شامل
- ✅ Transaction Management
- ✅ Audit Logging
- ✅ Error Handling

النظام جاهز للاستخدام في Production! 🚀

