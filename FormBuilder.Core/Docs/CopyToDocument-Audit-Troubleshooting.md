# CopyToDocument Audit Troubleshooting

## 🔍 المشكلة

Audit records لا تُحفظ في جدول `COPY_TO_DOCUMENT_AUDIT` بعد تنفيذ CopyToDocument بنجاح.

## 📋 خطوات التحقق

### 1. فحص Application Logs

ابحث في Logs عن الرسائل التالية:

```
[Information] Starting CopyToDocument audit logging. SourceSubmissionId: 1, TargetDocumentId: 4, Success: True
[Information] Created audit entity. SourceSubmissionId: 1, TargetDocumentId: 4, SourceFormId: 1, TargetFormId: 1
[Information] Added audit entity to DbContext
[Information] Audit entity state before SaveChanges: Added
[Information] SaveChangesAsync completed. Returned count: 1
[Information] Audit entity state after SaveChanges: Unchanged, Id: X
[Information] Audit record saved successfully with ID: X
```

**إذا لم تجد هذه الرسائل:**
- `LogAuditAsync` لا يتم استدعاؤه
- تحقق من أن `ExecuteCopyToDocumentAsync` يتم استدعاؤه بنجاح

**إذا وجدت رسائل خطأ:**
- ابحث عن `Error during SaveChangesAsync for audit record`
- ابحث عن `Critical: Failed to log CopyToDocument audit`

### 2. فحص Database مباشرة

```sql
-- فحص جميع Audit Records
SELECT TOP 10 * 
FROM COPY_TO_DOCUMENT_AUDIT 
ORDER BY ExecutionDate DESC

-- فحص Audit Records لـ Submission محدد
SELECT * 
FROM COPY_TO_DOCUMENT_AUDIT 
WHERE SourceSubmissionId = 1
ORDER BY ExecutionDate DESC

-- فحص Audit Records لـ Target Document محدد
SELECT * 
FROM COPY_TO_DOCUMENT_AUDIT 
WHERE TargetDocumentId = 4
ORDER BY ExecutionDate DESC
```

### 3. فحص API Response

```http
GET http://localhost:5203/api/CopyToDocument/audit/submission/1
```

**إذا كان Response فارغ `[]`:**
- Audit records لم تُحفظ
- تحقق من Logs لمعرفة السبب

### 4. فحص Entity State

في Logs، تحقق من:
- `Audit entity state before SaveChanges: Added` ✅
- `SaveChangesAsync completed. Returned count: 1` ✅
- `Audit entity state after SaveChanges: Unchanged, Id: X` ✅

**إذا كان State مختلف:**
- `Detached` = Entity غير متصل بالـ DbContext
- `Modified` = Entity تم تعديله لكن لم يُحفظ
- `Deleted` = Entity تم حذفه

## 🔧 الحلول المحتملة

### الحل 1: التحقق من DbContext State

إذا كان `DbContext` في حالة غير صحيحة بعد commit الـ transaction الرئيسية، قد تحتاج إلى:

```csharp
// في LogAuditAsync
if (_unitOfWork.AppDbContext.Database.CurrentTransaction != null)
{
    // Transaction موجود - استخدمه
    await _unitOfWork.CompleteAsyn();
}
else
{
    // لا يوجد transaction - أنشئ واحد جديد
    using var transaction = await _unitOfWork.AppDbContext.Database.BeginTransactionAsync();
    try
    {
        await _unitOfWork.CompleteAsyn();
        await transaction.CommitAsync();
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}
```

### الحل 2: استخدام Repository بدلاً من DbContext مباشرة

إذا كان `DbContext` لا يعمل بشكل صحيح، يمكن استخدام Repository:

```csharp
// إنشاء Repository للـ COPY_TO_DOCUMENT_AUDIT
var auditRepository = _unitOfWork.Repositary<COPY_TO_DOCUMENT_AUDIT>();
await auditRepository.AddAsync(audit);
await _unitOfWork.CompleteAsyn();
```

### الحل 3: استخدام Background Service

إذا كان المشكلة في Timing، يمكن استخدام Background Service لحفظ Audit:

```csharp
// في LogAuditAsync
await _backgroundJobClient.Enqueue(() => SaveAuditRecordAsync(audit));
```

## 🐛 Common Issues

### Issue 1: DbContext Disposed

**الأعراض:**
- `ObjectDisposedException` في Logs
- `DbContext is null`

**الحل:**
- تأكد من أن `UnitOfWork` لا يتم dispose قبل `LogAuditAsync`
- استخدم `IServiceScope` لإنشاء DbContext جديد

### Issue 2: Transaction Conflict

**الأعراض:**
- `SaveChangesAsync` لا يعمل
- `savedCount = 0`

**الحل:**
- استخدم transaction منفصلة للـ Audit
- تأكد من commit الـ transaction الرئيسية قبل حفظ Audit

### Issue 3: Entity Not Tracked

**الأعراض:**
- `Entity State: Detached`
- `Id = 0` بعد SaveChanges

**الحل:**
- استخدم `AddAsync` بدلاً من `Add`
- تأكد من أن Entity في حالة `Added` قبل SaveChanges

## 📊 Debugging Steps

### Step 1: Enable Detailed Logging

في `appsettings.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "FormBuilder.Services.Services.FormBuilder.CopyToDocumentService": "Debug"
    }
  }
}
```

### Step 2: Add Breakpoints

ضع breakpoints في:
- بداية `LogAuditAsync`
- بعد `AddAsync`
- بعد `CompleteAsyn`
- في catch block

### Step 3: Check Database Connection

```sql
-- تحقق من أن Table موجود
SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = 'COPY_TO_DOCUMENT_AUDIT'

-- تحقق من Permissions
EXEC sp_helprotect 'COPY_TO_DOCUMENT_AUDIT'
```

## ✅ Checklist

- [ ] فحص Application Logs
- [ ] فحص Database مباشرة
- [ ] فحص API Response
- [ ] فحص Entity State
- [ ] تحقق من DbContext State
- [ ] تحقق من Transaction State
- [ ] تحقق من Database Permissions
- [ ] تحقق من Table Existence

## 🎯 Next Steps

1. **فحص Logs** - ابحث عن رسائل `LogAuditAsync`
2. **فحص Database** - تحقق من وجود Records
3. **فحص API** - استخدم `/api/CopyToDocument/audit/submission/{id}`
4. **إذا لم تجد Records** - راجع Logs للأخطاء
5. **إذا وجدت أخطاء** - طبق الحلول المقترحة أعلاه

## 📝 ملاحظات

- Audit Logging يجب أن يحدث **بعد** commit الـ transaction الرئيسية
- Audit Logging يجب أن يكون **مستقل** عن نجاح/فشل العملية الرئيسية
- Audit Logging يجب أن **لا يوقف** العملية الرئيسية في حالة الفشل

