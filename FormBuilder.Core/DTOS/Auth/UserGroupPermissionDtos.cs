namespace FormBuilder.Application.Dtos.Auth
{
    public class CreateUserGroupPermissionDto
    {
        public int UserGroupId { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public int? LegalEntityId { get; set; }
    }

    public class UpdateUserGroupPermissionDto
    {
        public string NewPermissionName { get; set; } = string.Empty;
        public int? LegalEntityId { get; set; }
    }

    public class SyncUserGroupPermissionsDto
    {
        public int? LegalEntityId { get; set; }
        public List<string> PermissionNames { get; set; } = new();
    }
}


