-- ============================================
-- إنشاء UserGroup ID = 2 وإضافة Document Permissions
-- ============================================
-- استخدم هذا إذا كانت المجموعة ID = 2 غير موجودة
-- ============================================

DECLARE @UserGroupId INT = 2;
DECLARE @CurrentUserId INT = 1;

PRINT '========================================';
PRINT 'Creating UserGroup ID = 2...';
PRINT '========================================';

-- التحقق من وجود المجموعة
IF EXISTS (SELECT 1 FROM Tbl_UserGroup WHERE ID = @UserGroupId)
BEGIN
    PRINT 'UserGroup ID = 2 already exists!';
    SELECT ID, Name, IsActive FROM Tbl_UserGroup WHERE ID = @UserGroupId;
    PRINT '';
    PRINT 'Skipping creation. Adding permissions only...';
END
ELSE
BEGIN
    PRINT 'UserGroup ID = 2 does not exist. Creating...';
    
    -- إنشاء المجموعة
    INSERT INTO Tbl_UserGroup (ID, Name, IsActive, CreatedDate, IdCreatedBy)
    VALUES (@UserGroupId, 'Default Users', 1, GETDATE(), @CurrentUserId);
    
    PRINT 'UserGroup created successfully!';
    PRINT '';
END

-- إضافة Document Permissions
PRINT 'Adding Document permissions...';

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

PRINT 'Document permissions added: ' + CAST(@@ROWCOUNT AS VARCHAR(10));
PRINT '';

-- التحقق من النتيجة
PRINT '========================================';
PRINT 'Verification:';
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
PRINT 'SUCCESS: UserGroup created and permissions added!';
PRINT '========================================';


