using PGPARS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PGPARS.Data
{
    public interface IAuditRepository
    {
        Task LogActionAsync(string action, string user, string details, string category);
        Task<IEnumerable<AuditLog>> GetLogsAsync();
        Task<List<string>> GetCategoriesAsync();

        Task<IEnumerable<AuditLog>> GetLogsByFiltersAsync(List<string>? filters,
            string? searchTerm, DateTime? startDate, DateTime? endDate);

        Task DeleteLogsAsync(List<int> logIds);
    }
}
