-- ============================================
-- Safe Fix: إضافة Document Permissions للمجموعة ID = 2
-- ============================================
-- هذا الـ script يتحقق من وجود المجموعة أولاً
-- ============================================

DECLARE @UserGroupId INT = 2;
DECLARE @CurrentUserId INT = 1;

PRINT '========================================';
PRINT 'Checking UserGroup ID = 2...';
PRINT '========================================';

-- التحقق من وجود المجموعة
IF NOT EXISTS (SELECT 1 FROM Tbl_UserGroup WHERE ID = @UserGroupId)
BEGIN
    PRINT 'ERROR: UserGroup ID = 2 does NOT exist!';
    PRINT '';
    PRINT 'Available UserGroups:';
    SELECT ID, Name, IsActive FROM Tbl_UserGroup ORDER BY ID;
    PRINT '';
    PRINT 'Users with UserGroupId = 2:';
    SELECT 
        Username,
        IdUserType AS UserGroupId,
        IsActive
    FROM Tbl_User
    WHERE IdUserType = 2 AND IsActive = 1;
    PRINT '';
    PRINT '========================================';
    PRINT 'SOLUTION OPTIONS:';
    PRINT '========================================';
    PRINT 'Option 1: Create UserGroup ID = 2';
    PRINT 'Option 2: Update users to existing UserGroup';
    PRINT '========================================';
    RETURN;
END

-- التحقق من اسم المجموعة
DECLARE @GroupName NVARCHAR(255);
SELECT @GroupName = Name FROM Tbl_UserGroup WHERE ID = @UserGroupId;

PRINT 'UserGroup ID = 2 exists: ' + ISNULL(@GroupName, 'NULL');
PRINT '';

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

DECLARE @RowsAdded INT = @@ROWCOUNT;
PRINT 'Document permissions added: ' + CAST(@RowsAdded AS VARCHAR(10));
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
IF @RowsAdded > 0
    PRINT 'SUCCESS: Permissions added!';
ELSE
    PRINT 'INFO: Permissions already exist or group not found.';
PRINT '========================================';


