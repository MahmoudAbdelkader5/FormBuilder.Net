using FormBuilder.Core.Models;

public interface IRoleService
{
    Task<IEnumerable<TblUserGroup>> GetAllRolesAsync();
}
