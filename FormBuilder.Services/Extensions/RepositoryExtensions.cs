using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using formBuilder.Domian.Entitys;
using formBuilder.Domian.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FormBuilder.Services.Extensions
{
    /// <summary>
    /// Extension methods for repositories to reduce code duplication
    /// </summary>
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Gets all active entities
        /// </summary>
        public static async Task<System.Collections.Generic.ICollection<T>> GetActiveAsync<T>(
            this IBaseRepository<T> repository)
            where T : BaseEntity
        {
            return await repository.GetAllAsync(e => e.IsActive);
        }

        /// <summary>
        /// Checks if an entity is active
        /// </summary>
        public static async Task<bool> IsActiveAsync<T>(
            this IBaseRepository<T> repository,
            int id)
            where T : BaseEntity
        {
            return await repository.AnyAsync(e => e.Id == id && e.IsActive);
        }
    }
}
