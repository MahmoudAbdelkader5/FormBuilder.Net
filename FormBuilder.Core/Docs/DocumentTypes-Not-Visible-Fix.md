# حل مشكلة عدم ظهور Document Types

## المشكلة
Document Types غير ظاهرة في الواجهة (DOCUMENTS SETUP)

## الأسباب المحتملة

### 1. مشكلة في الـ Permissions
الـ Controller يتطلب permission `Document_Allow_View` لعرض Document Types.

### 2. الـ API لا يعمل بشكل صحيح
قد تكون هناك مشكلة في استدعاء الـ API من الواجهة الأمامية.

---

## الحلول

### الحل 1: التحقق من الـ Permissions

#### الخطوة 1: التحقق من وجود الـ Permissions في قاعدة البيانات

```sql
-- التحقق من صلاحيات Document
SELECT 
    ugp.IdUserGroup,
    ug.Name AS UserGroupName,
    ugp.UserPermissionName
FROM Tbl_UserGroup_Permission ugp
INNER JOIN Tbl_UserGroup ug ON ugp.IdUserGroup = ug.ID
WHERE ugp.UserPermissionName LIKE 'Document_%'
ORDER BY ug.Name, ugp.UserPermissionName;
```

#### الخطوة 2: إضافة الـ Permissions إذا لم تكن موجودة

قم بتشغيل ملف `Add_Admin_Permissions.sql`:

```sql
-- أو قم بتشغيل هذا الكود مباشرة
DECLARE @AdminGroupId INT;
SELECT @AdminGroupId = ID FROM Tbl_UserGroup WHERE Name = 'Administration' OR Name = 'Admin' OR ID = 1;

-- إضافة صلاحيات Document
INSERT INTO Tbl_UserGroup_Permission (IdUserGroup, UserPermissionName, IdCreatedBy, CreatedDate, IdLegalEntity)
SELECT @AdminGroupId, PermissionName, 1, GETDATE(), NULL
FROM (
    VALUES 
        ('Document_Allow_View'),
        ('Document_Allow_Create'),
        ('Document_Allow_Edit'),
        ('Document_Allow_Delete'),
        ('Document_Allow_Manage'),
        ('Document_Allow_Configure'),
        ('Document_Allow_ViewAll'),
        ('Document_Allow_Export'),
        ('Document_Allow_Import')
) AS Permissions(PermissionName)
WHERE NOT EXISTS (
    SELECT 1 FROM Tbl_UserGroup_Permission 
    WHERE IdUserGroup = @AdminGroupId 
    AND UserPermissionName = Permissions.PermissionName
);
```

#### الخطوة 3: التحقق من صلاحيات المستخدم الحالي

```sql
-- التحقق من صلاحيات المستخدم الحالي
SELECT 
    u.Username,
    ug.Name AS UserGroupName,
    ugp.UserPermissionName
FROM Tbl_User u
INNER JOIN Tbl_UserGroup ug ON u.IdUserType = ug.ID
INNER JOIN Tbl_UserGroup_Permission ugp ON ug.ID = ugp.IdUserGroup
WHERE u.Username = 'admin' -- استبدل بـ username الخاص بك
  AND ugp.UserPermissionName LIKE 'Document_%'
ORDER BY ugp.UserPermissionName;
```

---

### الحل 2: اختبار الـ API مباشرة

#### اختبار باستخدام Postman/cURL

```bash
# GET جميع Document Types
curl -X GET "http://localhost:5000/api/DocumentTypes" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json"
```

#### اختبار باستخدام Swagger

1. افتح Swagger UI: `http://localhost:5000/swagger`
2. ابحث عن `DocumentTypes` controller
3. جرب endpoint `GET /api/DocumentTypes`
4. تحقق من الـ Response

---

### الحل 3: التحقق من الـ Logs

تحقق من الـ logs في الـ API لمعرفة الخطأ:

```csharp
// في DocumentTypesController.cs
// الـ logs ستظهر إذا كان هناك مشكلة في الـ permission
```

---

### الحل 4: إزالة الـ Permission Requirement مؤقتاً (للاختبار فقط)

⚠️ **تحذير: هذا للاختبار فقط، لا تستخدمه في Production**

```csharp
// في DocumentTypesController.cs
// قم بتعليق [RequirePermission] مؤقتاً للاختبار

[HttpGet]
// [RequirePermission("Document_Allow_View")] // مؤقتاً معطل
public async Task<IActionResult> GetAll()
{
    var result = await _documentTypeService.GetAllAsync();
    return result.ToActionResult();
}
```

---

## اختبار سريع

### 1. اختبار الـ API مباشرة

```bash
# بدون authentication (إذا كان مسموح)
curl -X GET "http://localhost:5000/api/DocumentTypes"

# مع authentication
curl -X GET "http://localhost:5000/api/DocumentTypes" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### 2. اختبار من Angular

```typescript
// في Angular service
getDocumentTypes(): Observable<any> {
  return this.http.get('/api/DocumentTypes', {
    headers: {
      'Authorization': `Bearer ${this.token}`
    }
  });
}
```

### 3. التحقق من الـ Console في المتصفح

افتح Developer Tools (F12) وتحقق من:
- Network tab: هل الـ request يصل للـ API؟
- Console tab: هل هناك أخطاء JavaScript؟
- Response: ما هو الـ response من الـ API؟

---

## SQL Script كامل لإضافة Permissions

```sql
-- ============================================
-- إضافة صلاحيات Document Types
-- ============================================

DECLARE @AdminGroupId INT;
DECLARE @UserGroupId INT;

-- الحصول على Admin Group ID
SELECT @AdminGroupId = ID 
FROM Tbl_UserGroup 
WHERE Name = 'Administration' OR Name = 'Admin' OR ID = 1;

-- الحصول على User Group ID (اختياري)
SELECT @UserGroupId = ID 
FROM Tbl_UserGroup 
WHERE Name = 'User' OR ID = 6;

-- إضافة صلاحيات للـ Admin
IF @AdminGroupId IS NOT NULL
BEGIN
    INSERT INTO Tbl_UserGroup_Permission (IdUserGroup, UserPermissionName, IdCreatedBy, CreatedDate, IdLegalEntity)
    SELECT @AdminGroupId, PermissionName, 1, GETDATE(), NULL
    FROM (
        VALUES 
            ('Document_Allow_View'),
            ('Document_Allow_Create'),
            ('Document_Allow_Edit'),
            ('Document_Allow_Delete'),
            ('Document_Allow_Manage'),
            ('Document_Allow_Configure'),
            ('Document_Allow_ViewAll'),
            ('Document_Allow_Export'),
            ('Document_Allow_Import')
    ) AS Permissions(PermissionName)
    WHERE NOT EXISTS (
        SELECT 1 FROM Tbl_UserGroup_Permission 
        WHERE IdUserGroup = @AdminGroupId 
        AND UserPermissionName = Permissions.PermissionName
    );
    
    PRINT 'Document permissions added for Admin Group: ' + CAST(@@ROWCOUNT AS VARCHAR(10));
END

-- إضافة صلاحيات للـ User (إذا كان موجود)
IF @UserGroupId IS NOT NULL AND @UserGroupId > 0
BEGIN
    INSERT INTO Tbl_UserGroup_Permission (IdUserGroup, UserPermissionName, IdCreatedBy, CreatedDate, IdLegalEntity)
    SELECT @UserGroupId, PermissionName, 1, GETDATE(), NULL
    FROM (
        VALUES 
            ('Document_Allow_View'),
            ('Document_Allow_Create'),
            ('Document_Allow_Edit'),
            ('Document_Allow_Delete'),
            ('Document_Allow_Manage'),
            ('Document_Allow_Configure'),
            ('Document_Allow_ViewAll'),
            ('Document_Allow_Export'),
            ('Document_Allow_Import')
    ) AS Permissions(PermissionName)
    WHERE NOT EXISTS (
        SELECT 1 FROM Tbl_UserGroup_Permission 
        WHERE IdUserGroup = @UserGroupId 
        AND UserPermissionName = Permissions.PermissionName
    );
    
    PRINT 'Document permissions added for User Group: ' + CAST(@@ROWCOUNT AS VARCHAR(10));
END

-- التحقق من الصلاحيات المضافة
PRINT '';
PRINT '========================================';
PRINT 'Document Permissions Summary:';
PRINT '========================================';

SELECT 
    ug.Name AS UserGroupName,
    ugp.UserPermissionName,
    ugp.CreatedDate
FROM Tbl_UserGroup_Permission ugp
INNER JOIN Tbl_UserGroup ug ON ugp.IdUserGroup = ug.ID
WHERE ugp.UserPermissionName LIKE 'Document_%'
ORDER BY ug.Name, ugp.UserPermissionName;
```

---

## Endpoints المتاحة

### 1. Get All Document Types
```
GET /api/DocumentTypes
Requires: Document_Allow_View
```

### 2. Get Document Type By ID
```
GET /api/DocumentTypes/{id}
Requires: Document_Allow_View
```

### 3. Get Document Type By Code
```
GET /api/DocumentTypes/code/{code}
Requires: Document_Allow_View
```

### 4. Get Active Document Types
```
GET /api/DocumentTypes/active
Requires: Document_Allow_View
```

### 5. Get Root Menu Items
```
GET /api/DocumentTypes/parent-menu/null
Requires: Document_Allow_View
```

### 6. Create Document Type
```
POST /api/DocumentTypes
Requires: Document_Allow_Create
```

### 7. Update Document Type
```
PUT /api/DocumentTypes/{id}
Requires: Document_Allow_Edit
```

### 8. Delete Document Type
```
DELETE /api/DocumentTypes/{id}
Requires: Document_Allow_Delete
```

---

## مثال JSON Response

```json
{
  "statusCode": 200,
  "message": "Success",
  "data": [
    {
      "id": 1,
      "name": "Purchase Request",
      "code": "PURCHASE_REQUEST",
      "formBuilderId": 10,
      "menuCaption": "Purchase Request",
      "menuOrder": 1,
      "parentMenuId": null,
      "isActive": true,
      "approvalWorkflowId": 1,
      "approvalWorkflowName": "Purchase Approval",
      "formBuilderName": "Purchase Form",
      "parentMenuName": null
    },
    {
      "id": 2,
      "name": "Purchase Order",
      "code": "PURCHASE_ORDER",
      "formBuilderId": 20,
      "menuCaption": "Purchase Order",
      "menuOrder": 2,
      "parentMenuId": null,
      "isActive": true,
      "approvalWorkflowId": 2,
      "approvalWorkflowName": "Order Approval",
      "formBuilderName": "Order Form",
      "parentMenuName": null
    }
  ]
}
```

---

## Checklist للتحقق

- [ ] تم تشغيل `Add_Admin_Permissions.sql`
- [ ] المستخدم يملك `Document_Allow_View` permission
- [ ] الـ API يعمل (تم اختباره من Postman/Swagger)
- [ ] الـ Token صحيح ومفعل
- [ ] لا توجد أخطاء في الـ Console
- [ ] الـ Network requests تصل للـ API بنجاح
- [ ] الـ Response يحتوي على data

---

## ملاحظات مهمة

1. **Permissions مطلوبة**: جميع endpoints تحتاج permissions
2. **Authentication مطلوب**: جميع endpoints تحتاج Bearer token
3. **التحقق من User Group**: تأكد أن المستخدم في User Group صحيح
4. **التحقق من الـ Logs**: راجع logs الـ API لمعرفة الأخطاء

---

## إذا استمرت المشكلة

1. تحقق من الـ logs في الـ API
2. تحقق من الـ Network tab في المتصفح
3. تحقق من الـ Console للأخطاء
4. تأكد من أن الـ API يعمل بشكل صحيح
5. تأكد من أن الـ permissions موجودة في قاعدة البيانات


