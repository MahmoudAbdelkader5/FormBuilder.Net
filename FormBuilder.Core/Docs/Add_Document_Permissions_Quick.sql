-- ============================================
-- Quick Fix: إضافة صلاحيات Document Types
-- ============================================
-- استخدم هذا الـ script إذا كانت Document Types غير ظاهرة
-- ============================================

DECLARE @AdminGroupId INT;
DECLARE @UserGroupId INT;
DECLARE @CurrentUserId INT = 1; -- استبدل بـ ID المستخدم الحالي

-- الحصول على Admin Group ID
SELECT @AdminGroupId = ID 
FROM Tbl_UserGroup 
WHERE Name = 'Administration' OR Name = 'Admin' OR ID = 1;

-- الحصول على User Group ID
SELECT @UserGroupId = ID 
FROM Tbl_UserGroup 
WHERE Name = 'User' OR ID = 6;

PRINT 'Admin Group ID: ' + ISNULL(CAST(@AdminGroupId AS VARCHAR(10)), 'NOT FOUND');
PRINT 'User Group ID: ' + ISNULL(CAST(@UserGroupId AS VARCHAR(10)), 'NOT FOUND');
PRINT '';

-- ============================================
-- إضافة صلاحيات Document للـ Admin
-- ============================================
IF @AdminGroupId IS NOT NULL
BEGIN
    INSERT INTO Tbl_UserGroup_Permission (IdUserGroup, UserPermissionName, IdCreatedBy, CreatedDate, IdLegalEntity)
    SELECT @AdminGroupId, PermissionName, @CurrentUserId, GETDATE(), NULL
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
    
    PRINT 'Document permissions added for Admin: ' + CAST(@@ROWCOUNT AS VARCHAR(10));
END
ELSE
BEGIN
    PRINT 'ERROR: Admin Group not found!';
END

-- ============================================
-- إضافة صلاحيات Document للـ User
-- ============================================
IF @UserGroupId IS NOT NULL AND @UserGroupId > 0
BEGIN
    INSERT INTO Tbl_UserGroup_Permission (IdUserGroup, UserPermissionName, IdCreatedBy, CreatedDate, IdLegalEntity)
    SELECT @UserGroupId, PermissionName, @CurrentUserId, GETDATE(), NULL
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
    
    PRINT 'Document permissions added for User: ' + CAST(@@ROWCOUNT AS VARCHAR(10));
END

-- ============================================
-- التحقق من الصلاحيات المضافة
-- ============================================
PRINT '';
PRINT '========================================';
PRINT 'Document Permissions Verification:';
PRINT '========================================';

SELECT 
    ug.Name AS UserGroupName,
    ugp.UserPermissionName,
    ugp.CreatedDate
FROM Tbl_UserGroup_Permission ugp
INNER JOIN Tbl_UserGroup ug ON ugp.IdUserGroup = ug.ID
WHERE ugp.UserPermissionName LIKE 'Document_%'
ORDER BY ug.Name, ugp.UserPermissionName;

-- ============================================
-- التحقق من صلاحيات مستخدم محدد
-- ============================================
PRINT '';
PRINT '========================================';
PRINT 'Checking permissions for all users:';
PRINT '========================================';

SELECT 
    u.Username,
    u.IdUserType AS UserGroupId,
    ug.Name AS UserGroupName,
    COUNT(CASE WHEN ugp.UserPermissionName LIKE 'Document_%' THEN 1 END) AS DocumentPermissionsCount
FROM Tbl_User u
LEFT JOIN Tbl_UserGroup ug ON u.IdUserType = ug.ID
LEFT JOIN Tbl_UserGroup_Permission ugp ON ug.ID = ugp.IdUserGroup AND ugp.UserPermissionName LIKE 'Document_%'
WHERE u.IsActive = 1
GROUP BY u.Username, u.IdUserType, ug.Name
ORDER BY u.Username;

PRINT '';
PRINT '========================================';
PRINT 'Script completed successfully!';
PRINT '========================================';
PRINT 'If Document Types still not visible:';
PRINT '1. Check API logs';
PRINT '2. Verify user has correct UserGroup';
PRINT '3. Check Angular console for errors';
PRINT '4. Test API directly from Postman/Swagger';


