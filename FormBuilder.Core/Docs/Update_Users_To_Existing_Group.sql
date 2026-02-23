-- ============================================
-- تحديث المستخدمين لمجموعة موجودة
-- ============================================
-- استخدم هذا إذا كنت تريد نقل المستخدمين لمجموعة موجودة
-- ============================================

DECLARE @SourceUserGroupId INT = 2; -- المجموعة الحالية (غير موجودة)
DECLARE @TargetUserGroupId INT; -- المجموعة الهدف (يجب تحديدها)

PRINT '========================================';
PRINT 'Available UserGroups:';
PRINT '========================================';
SELECT ID, Name, IsActive FROM Tbl_UserGroup ORDER BY ID;
PRINT '';

-- تحديد المجموعة الهدف (يمكن تغييرها)
-- الخيارات:
-- 1 = Administration (Admin)
-- 3 = Manage-IT
-- 6 = User
SET @TargetUserGroupId = 6; -- User group (مثل anas)

PRINT '========================================';
PRINT 'Users to update:';
PRINT '========================================';
SELECT 
    Username,
    IdUserType AS CurrentUserGroupId,
    IsActive
FROM Tbl_User
WHERE IdUserType = @SourceUserGroupId AND IsActive = 1;
PRINT '';

-- التحقق من وجود المجموعة الهدف
IF NOT EXISTS (SELECT 1 FROM Tbl_UserGroup WHERE ID = @TargetUserGroupId)
BEGIN
    PRINT 'ERROR: Target UserGroup ID = ' + CAST(@TargetUserGroupId AS VARCHAR(10)) + ' does not exist!';
    PRINT 'Please set @TargetUserGroupId to an existing group ID.';
    RETURN;
END

DECLARE @TargetGroupName NVARCHAR(255);
SELECT @TargetGroupName = Name FROM Tbl_UserGroup WHERE ID = @TargetUserGroupId;

PRINT '========================================';
PRINT 'Updating users from Group ' + CAST(@SourceUserGroupId AS VARCHAR(10)) + ' to Group ' + CAST(@TargetUserGroupId AS VARCHAR(10)) + ' (' + @TargetGroupName + ')';
PRINT '========================================';

-- تحديث المستخدمين
UPDATE Tbl_User
SET IdUserType = @TargetUserGroupId,
    UpdatedDate = GETDATE()
WHERE IdUserType = @SourceUserGroupId 
  AND IsActive = 1;

DECLARE @UsersUpdated INT = @@ROWCOUNT;
PRINT 'Users updated: ' + CAST(@UsersUpdated AS VARCHAR(10));
PRINT '';

-- التحقق من النتيجة
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
PRINT 'SUCCESS: Users updated!';
PRINT '========================================';
PRINT 'All users now have Document permissions.';


