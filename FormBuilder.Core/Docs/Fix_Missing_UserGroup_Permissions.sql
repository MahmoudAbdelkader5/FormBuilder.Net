-- ============================================
-- إصلاح مشكلة المستخدمين بدون Document Permissions
-- ============================================
-- هذا الـ script يضيف Document permissions للمجموعات المفقودة
-- ============================================

DECLARE @UserGroupId2 INT = 2; -- المجموعة التي تحتوي على مستخدمين بدون permissions
DECLARE @CurrentUserId INT = 1;

PRINT '========================================';
PRINT 'Fixing Missing Document Permissions';
PRINT '========================================';
PRINT '';

-- ============================================
-- التحقق من المجموعة ID = 2
-- ============================================
PRINT 'Checking UserGroup ID = 2:';
SELECT 
    ID,
    Name,
    IsActive
FROM Tbl_UserGroup
WHERE ID = 2;

PRINT '';

-- ============================================
-- إضافة Document Permissions للمجموعة ID = 2
-- ============================================
IF EXISTS (SELECT 1 FROM Tbl_UserGroup WHERE ID = 2)
BEGIN
    PRINT 'Adding Document permissions to UserGroup ID = 2...';
    
    INSERT INTO Tbl_UserGroup_Permission (IdUserGroup, UserPermissionName, IdCreatedBy, CreatedDate, IdLegalEntity)
    SELECT @UserGroupId2, PermissionName, @CurrentUserId, GETDATE(), NULL
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
        WHERE IdUserGroup = @UserGroupId2 
        AND UserPermissionName = Permissions.PermissionName
    );
    
    PRINT 'Document permissions added: ' + CAST(@@ROWCOUNT AS VARCHAR(10));
    PRINT '';
END
ELSE
BEGIN
    PRINT 'WARNING: UserGroup ID = 2 does not exist!';
    PRINT 'Creating new UserGroup...';
    
    -- إنشاء مجموعة جديدة إذا لم تكن موجودة
    INSERT INTO Tbl_UserGroup (ID, Name, IsActive, CreatedDate, IdCreatedBy)
    VALUES (2, 'Default Users', 1, GETDATE(), @CurrentUserId);
    
    PRINT 'UserGroup created. Now adding permissions...';
    
    INSERT INTO Tbl_UserGroup_Permission (IdUserGroup, UserPermissionName, IdCreatedBy, CreatedDate, IdLegalEntity)
    SELECT 2, PermissionName, @CurrentUserId, GETDATE(), NULL
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
    ) AS Permissions(PermissionName);
    
    PRINT 'Document permissions added: ' + CAST(@@ROWCOUNT AS VARCHAR(10));
    PRINT '';
END

-- ============================================
-- إضافة Document Permissions لجميع المجموعات النشطة
-- ============================================
PRINT 'Adding Document permissions to all active UserGroups...';

DECLARE @GroupId INT;
DECLARE group_cursor CURSOR FOR
    SELECT ID FROM Tbl_UserGroup WHERE IsActive = 1;

OPEN group_cursor;
FETCH NEXT FROM group_cursor INTO @GroupId;

WHILE @@FETCH_STATUS = 0
BEGIN
    INSERT INTO Tbl_UserGroup_Permission (IdUserGroup, UserPermissionName, IdCreatedBy, CreatedDate, IdLegalEntity)
    SELECT @GroupId, PermissionName, @CurrentUserId, GETDATE(), NULL
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
        WHERE IdUserGroup = @GroupId 
        AND UserPermissionName = Permissions.PermissionName
    );
    
    FETCH NEXT FROM group_cursor INTO @GroupId;
END

CLOSE group_cursor;
DEALLOCATE group_cursor;

PRINT 'Permissions added to all active groups.';
PRINT '';

-- ============================================
-- التحقق من النتيجة
-- ============================================
PRINT '========================================';
PRINT 'Verification - Document Permissions:';
PRINT '========================================';

SELECT 
    ug.ID AS UserGroupId,
    ug.Name AS UserGroupName,
    COUNT(ugp.UserPermissionName) AS DocumentPermissionsCount
FROM Tbl_UserGroup ug
LEFT JOIN Tbl_UserGroup_Permission ugp ON ug.ID = ugp.IdUserGroup 
    AND ugp.UserPermissionName LIKE 'Document_%'
WHERE ug.IsActive = 1
GROUP BY ug.ID, ug.Name
ORDER BY ug.ID;

PRINT '';

-- ============================================
-- التحقق من صلاحيات جميع المستخدمين
-- ============================================
PRINT '========================================';
PRINT 'Verification - User Permissions:';
PRINT '========================================';

SELECT 
    u.Username,
    u.IdUserType AS UserGroupId,
    ug.Name AS UserGroupName,
    COUNT(CASE WHEN ugp.UserPermissionName LIKE 'Document_%' THEN 1 END) AS DocumentPermissionsCount
FROM Tbl_User u
LEFT JOIN Tbl_UserGroup ug ON u.IdUserType = ug.ID
LEFT JOIN Tbl_UserGroup_Permission ugp ON ug.ID = ugp.IdUserGroup 
    AND ugp.UserPermissionName LIKE 'Document_%'
WHERE u.IsActive = 1
GROUP BY u.Username, u.IdUserType, ug.Name
ORDER BY u.Username;

PRINT '';
PRINT '========================================';
PRINT 'Fix completed!';
PRINT '========================================';
PRINT 'All users should now have Document permissions.';
PRINT 'Please refresh the Angular application.';


