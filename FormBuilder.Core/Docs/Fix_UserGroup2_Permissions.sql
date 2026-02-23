-- ============================================
-- Quick Fix: إضافة Document Permissions للمجموعة ID = 2
-- ============================================
-- هذا الـ script السريع يضيف Document permissions مباشرة
-- ============================================

-- إضافة Document Permissions للمجموعة ID = 2
INSERT INTO Tbl_UserGroup_Permission (IdUserGroup, UserPermissionName, IdCreatedBy, CreatedDate, IdLegalEntity)
SELECT 2, PermissionName, 1, GETDATE(), NULL
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
    WHERE IdUserGroup = 2 
    AND UserPermissionName = Permissions.PermissionName
);

PRINT 'Document permissions added to UserGroup ID = 2: ' + CAST(@@ROWCOUNT AS VARCHAR(10));

-- التحقق من النتيجة
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


