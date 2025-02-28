using Microsoft.EntityFrameworkCore;
using PGPARS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PGPARS.Data
{
    public class AuditRepository : IAuditRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogActionAsync(string action, string user, string details, string category)
        {
            var log = new AuditLog
            {
                Action = action,
                Actor = user ?? "System",
                TimeStamp = DateTime.Now,
                Details = details,
                Category = category
            };
            await _context.AuditLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetLogsAsync()
        {
            return await _context.AuditLogs
                .OrderByDescending(a => a.TimeStamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetLogsByCategoryAsync(string category)
        {
            return await _context.AuditLogs
                .Where(a => a.Category == category)
                .OrderByDescending(a => a.TimeStamp)
                .ToListAsync();
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            return await _context.AuditLogs
                .Select(a => a.Category)
                .Distinct()
                .ToListAsync();
        }

     

    }
}
