using formBuilder.Domian.Interfaces;
using FormBuilder.Domian.Entitys.FormBuilder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Repositories
{
    public interface IApprovalDelegationRepository : IBaseRepository<APPROVAL_DELEGATIONS>
    {
        Task<IEnumerable<APPROVAL_DELEGATIONS>> GetActiveDelegationsByFromUserIdAsync(string fromUserId);
        Task<IEnumerable<APPROVAL_DELEGATIONS>> GetActiveDelegationsByToUserIdAsync(string toUserId);
        Task<IEnumerable<APPROVAL_DELEGATIONS>> GetAllDelegationsByToUserIdAsync(string toUserId);
        Task<APPROVAL_DELEGATIONS?> GetActiveDelegationAsync(string fromUserId, DateTime checkDate);
        Task<bool> HasActiveDelegationAsync(string fromUserId, DateTime checkDate);
        
        // Methods for Scope-based resolution
        Task<APPROVAL_DELEGATIONS?> GetActiveDelegationByScopeAsync(
            string fromUserId, 
            string scopeType, 
            int? scopeId, 
            DateTime checkDate);
        
        Task<bool> HasActiveDelegationForScopeAsync(
            string fromUserId, 
            string scopeType, 
            int? scopeId, 
            DateTime checkDate);
    }
}

