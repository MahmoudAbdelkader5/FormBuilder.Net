using FormBuilder.Domian.Interfaces;
using FormBuilder.Infrastructure.Data;
using FormBuilder.Domian.Entitys.FormBuilder;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormBuilder.core;

namespace FormBuilder.Services.Repository
{
    public class UserQueriesRepository : BaseRepository<USER_QUERIES>, IUserQueriesRepository
    {
        private readonly FormBuilderDbContext _context;

        public UserQueriesRepository(FormBuilderDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<USER_QUERIES>> GetByUserIdAsync(string userId)
        {
            return await _context.USER_QUERIES
                .Where(q => q.UserId == userId && !q.IsDeleted)
                .OrderByDescending(q => q.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<USER_QUERIES>> GetByUserIdAndDatabaseAsync(string userId, string databaseName)
        {
            return await _context.USER_QUERIES
                .Where(q => q.UserId == userId && 
                           q.DatabaseName == databaseName && 
                           !q.IsDeleted)
                .OrderByDescending(q => q.CreatedDate)
                .ToListAsync();
        }
    }
}

