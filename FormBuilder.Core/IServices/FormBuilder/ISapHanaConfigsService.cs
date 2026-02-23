using FormBuilder.Application.DTOS;
using FormBuilder.Core.DTOS.FormBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Core.IServices.FormBuilder
{
    public interface ISapHanaConfigsService
    {
        /// <summary>
        /// Returns the active SAP HANA connection string (plaintext) from DB, or null if none configured.
        /// </summary>
        Task<string?> GetActiveConnectionStringAsync();

        /// <summary>
        /// Returns a specific SAP config connection string (plaintext) by config id, or null if not found/inactive.
        /// </summary>
        Task<string?> GetConnectionStringByIdAsync(int id);

        /// <summary>
        /// Creates a new active SAP HANA config (encrypting the connection string) and deactivates others.
        /// </summary>
        Task<bool> SetActiveAsync(string name, string connectionString);

        // CRUD (never returns secrets)
        Task<ServiceResult<IEnumerable<SapHanaConfigDto>>> GetAllAsync(bool includeInactive = true);
        Task<ServiceResult<SapHanaConfigDto>> GetByIdAsync(int id);
        Task<ServiceResult<SapHanaConfigDto>> CreateAsync(CreateSapHanaConfigDto dto);
        Task<ServiceResult<SapHanaConfigDto>> UpdateAsync(int id, UpdateSapHanaConfigDto dto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<bool>> ToggleActiveAsync(int id, bool isActive);
    }
}

