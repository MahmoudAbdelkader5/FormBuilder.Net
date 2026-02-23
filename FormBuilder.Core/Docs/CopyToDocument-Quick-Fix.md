# CopyToDocument - حل سريع لمشكلة Field Not Found

## 🔴 المشكلة

```
Target field 'ORDER_AMOUNT' (FieldCode: ORDER_AMOUNT) not found in target form 1
```

## ✅ الحل السريع

### الخطوة 1: جلب الحقول الموجودة في Target Form

```http
GET http://localhost:5203/api/FormFields/form/1
```

**Expected Response:**
```json
{
  "statusCode": 200,
  "message": "Form fields retrieved successfully",
  "data": [
    {
      "id": 1,
      "fieldCode": "F",
      "fieldName": "f1",
      "fieldTypeId": 1,
      "fieldTypeName": "Number"
    },
    {
      "id": 2,
      "fieldCode": "ANOTHER_FIELD",
      "fieldName": "Another Field",
      "fieldTypeId": 2,
      "fieldTypeName": "Text"
    }
  ]
}
```

### الخطوة 2: استخدام FieldCode موجود

استخدم FieldCode موجود في Target Form. على سبيل المثال، إذا كان Target Form يحتوي على `F`:

```json
{
  "config": {
    "sourceDocumentTypeId": 1,
    "sourceFormId": 1,
    "targetDocumentTypeId": 1,
    "targetFormId": 1,
    "createNewDocument": true,
    "initialStatus": "Draft",
    "fieldMapping": {
      "F": "F"
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
  "sourceSubmissionId": 1,
  "actionId": null,
  "ruleId": null
}
```

### الخطوة 3: إذا أردت استخدام FieldCode مختلف

#### Option A: إضافة Field جديد في Target Form

1. استخدم API لإضافة Field:
```http
POST http://localhost:5203/api/FormFields
```

```json
{
  "tabId": 1,
  "fieldTypeId": 1,
  "fieldName": "Order Amount",
  "fieldCode": "ORDER_AMOUNT",
  "fieldOrder": 1,
  "isMandatory": false,
  "isEditable": true,
  "isVisible": true
}
```

2. ثم استخدم FieldMapping:
```json
{
  "fieldMapping": {
    "F": "ORDER_AMOUNT"
  }
}
```

#### Option B: استخدام FieldCode موجود

ابحث في Response من `GET /api/FormFields/form/1` عن FieldCode موجود واستخدمه.

## 📋 Checklist

- [ ] جلب الحقول من Source Form: `GET /api/FormFields/form/1`
- [ ] جلب الحقول من Target Form: `GET /api/FormFields/form/1`
- [ ] اختيار FieldCodes موجودة من كلا Form
- [ ] استخدام FieldMapping صحيح
- [ ] التأكد من أن أنواع البيانات متوافقة
- [ ] إعادة المحاولة

## 🎯 مثال عملي

إذا كان:
- Source Form يحتوي على: `F` (Number)
- Target Form يحتوي على: `F` (Number)

استخدم:
```json
{
  "fieldMapping": {
    "F": "F"
  }
}
```

## ⚠️ ملاحظات مهمة

1. **FieldCode Case-Sensitive**: `ORDER_AMOUNT` ≠ `order_amount`
2. **FieldCode يجب أن يكون موجود**: في Source Form و Target Form
3. **أنواع البيانات يجب أن تكون متوافقة**: Number → Number, Text → Text
4. **FieldCode وليس FieldName**: استخدم FieldCode دائماً

