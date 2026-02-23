# CopyToDocument Test Files - دليل الاستخدام

## 📁 الملفات المتوفرة

### 1. `CopyToDocument-Test-Scenario-PR-to-PO.md`
**الوصف:** دليل شامل لاختبار سيناريو Purchase Request → Purchase Order  
**المحتوى:**
- ✅ جميع Test Cases مع شرح مفصل
- ✅ API URLs كاملة
- ✅ JSON Examples جاهزة
- ✅ cURL Commands
- ✅ Expected Responses
- ✅ Validation Tests
- ✅ Troubleshooting Guide

**الاستخدام:** اقرأ هذا الملف للحصول على فهم كامل للاختبارات

---

### 2. `CopyToDocument-Test-Cases.json`
**الوصف:** ملف JSON يحتوي على جميع Test Cases منظمة  
**المحتوى:**
- ✅ جميع Test Scenarios (7 سيناريوهات)
- ✅ Audit Tests (4 اختبارات)
- ✅ Validation Tests (3 اختبارات)
- ✅ cURL Examples
- ✅ Notes و Important Information

**الاستخدام:** 
- استيراد في Postman
- استخدام في Automation Tests
- مرجع سريع للـ JSON Structure

---

### 3. `CopyToDocument-Test-API.html`
**الوصف:** صفحة HTML تفاعلية لاختبار API  
**المحتوى:**
- ✅ واجهة مستخدم بسيطة
- ✅ جميع Test Cases معروضة
- ✅ أزرار نسخ سريعة
- ✅ JSON Formatting
- ✅ cURL Examples

**الاستخدام:**
1. افتح الملف في المتصفح
2. انقر على "📋 نسخ JSON" لنسخ أي Test Case
3. الصق في Postman أو أي API Client

---

## 🚀 البدء السريع

### الطريقة 1: استخدام HTML File
1. افتح `CopyToDocument-Test-API.html` في المتصفح
2. اختر Test Case
3. انقر على "📋 نسخ JSON"
4. الصق في Postman أو API Client

### الطريقة 2: استخدام JSON File
1. افتح `CopyToDocument-Test-Cases.json`
2. ابحث عن Test Case المطلوب
3. انسخ `requestBody`
4. الصق في Postman

### الطريقة 3: استخدام Markdown File
1. افتح `CopyToDocument-Test-Scenario-PR-to-PO.md`
2. اتبع التعليمات خطوة بخطوة
3. استخدم cURL Commands أو JSON Examples

---

## 📋 Test Cases المتوفرة

### Main Scenarios
1. **Full Copy** - نسخ كامل مع جميع الخيارات
2. **Using Codes** - نسخ باستخدام Codes بدلاً من IDs
3. **Without Attachments** - نسخ بدون Attachments
4. **Without Workflow** - نسخ بدون بدء Workflow
5. **With Metadata** - نسخ مع Metadata
6. **Minimal Fields** - نسخ مع حقول قليلة فقط
7. **Update Existing** - تحديث مستند موجود

### Audit Tests
1. Get All Audit Records
2. Get Audit by Source Submission ID
3. Get Audit by Target Document ID
4. Get Audit Record by ID

### Validation Tests
1. Missing sourceDocumentTypeId
2. Missing sourceFormId
3. Invalid initialStatus

---

## ⚙️ الإعدادات المطلوبة

### قبل الاختبار
```json
{
  "baseUrl": "http://localhost:5000",
  "token": "YOUR_ACCESS_TOKEN",
  "sourceDocumentTypeId": 1,
  "sourceFormId": 10,
  "targetDocumentTypeId": 2,
  "targetFormId": 20,
  "sourceSubmissionId": 115
}
```

### Field Mapping
```json
{
  "REQUEST_AMOUNT": "ORDER_AMOUNT",
  "REQUEST_DATE": "ORDER_DATE",
  "SUPPLIER_NAME": "VENDOR_NAME",
  "REQUEST_DESCRIPTION": "ORDER_DESCRIPTION"
}
```

### Grid Mapping
```json
{
  "REQUEST_ITEMS": "ORDER_ITEMS"
}
```

---

## 🔧 Postman Setup

### 1. Import Collection
- استورد `CopyToDocument.postman_collection.json` الموجود في:
  ```
  FormBuilder.Core/Docs/CopyToDocument.postman_collection.json
  ```

### 2. Set Variables
```
baseUrl: http://localhost:5000
token: YOUR_ACCESS_TOKEN
submissionId: 115
targetDocumentId: 116
```

### 3. Run Tests
- اختر Test Case
- اضغط Send
- تحقق من Response

---

## 📊 Expected Results

### Success Response
```json
{
  "statusCode": 200,
  "message": "CopyToDocument executed successfully",
  "data": {
    "success": true,
    "targetDocumentId": 116,
    "targetDocumentNumber": "PO-000116",
    "fieldsCopied": 4,
    "gridRowsCopied": 5
  }
}
```

### Error Response
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

---

## ✅ Checklist

### قبل الاختبار
- [ ] تأكد من وجود Purchase Request Submission
- [ ] تأكد من إكمال Approval
- [ ] تأكد من وجود Document Types و Forms
- [ ] احصل على Access Token
- [ ] استبدل IDs و Codes بالقيم الفعلية

### أثناء الاختبار
- [ ] Test 1: Full Copy
- [ ] Test 2: Using Codes
- [ ] Test 3: Without Attachments
- [ ] Test 4: Without Workflow
- [ ] Test 5: With Metadata
- [ ] Audit Tests
- [ ] Validation Tests

### بعد الاختبار
- [ ] تحقق من إنشاء Purchase Order
- [ ] تحقق من نسخ الحقول
- [ ] تحقق من نسخ Grid
- [ ] تحقق من نسخ Attachments
- [ ] تحقق من ParentDocumentId
- [ ] تحقق من بدء Workflow
- [ ] تحقق من Audit Records

---

## 📚 ملفات إضافية

- `CopyToDocument-Complete-Guide-Arabic.md` - الدليل الشامل
- `CopyToDocument-Quick-Reference-Arabic.md` - المرجع السريع
- `CopyToDocument.postman_collection.json` - Postman Collection

---

## 🆘 المساعدة

إذا واجهت مشاكل:
1. راجع `CopyToDocument-Test-Scenario-PR-to-PO.md` - قسم Troubleshooting
2. تحقق من Validation Errors
3. تأكد من صحة IDs و Codes
4. تحقق من Access Token

---

## 🎯 الخلاصة

جميع الملفات جاهزة للاستخدام:
- ✅ HTML File للاستخدام السريع
- ✅ JSON File للاستيراد في Postman
- ✅ Markdown File للدليل الشامل

اختر الطريقة التي تناسبك وابدأ الاختبار! 🚀

