﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<AuditLog>> GetLogsByFiltersAsync(
                                                List<string>? filters,
                                                string? searchTerm,
                                                DateTime? startDate,
                                                DateTime? endDate)
        {
            var query = _context.AuditLogs.AsQueryable();

            if (filters != null && filters.Any())
            {           
                query = query.Where(a => filters.Contains(a.Category));
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(a =>
                    a.Action.Contains(searchTerm) ||
                    a.Details.Contains(searchTerm) ||
                    a.Actor.Contains(searchTerm));
            }

            if (startDate.HasValue)
            {
                query = query.Where(a => a.TimeStamp >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(a => a.TimeStamp <= endDate.Value.AddDays(1).AddTicks(-1));
            }

            return await query.OrderByDescending(a => a.TimeStamp).ToListAsync();
        }


        public async Task<List<string>> GetCategoriesAsync()
        {
            return await _context.AuditLogs
                .Select(a => a.Category)
                .Distinct()
                .ToListAsync();
        }

        public async Task DeleteLogsAsync(List<int> logIds)
        {
            if (logIds == null || !logIds.Any())
                return;

            var logsToDelete = await _context.AuditLogs
                .Where(log => logIds.Contains(log.Id))
                .ToListAsync();

            if (logsToDelete.Any())
            {
                _context.AuditLogs.RemoveRange(logsToDelete);
                await _context.SaveChangesAsync();
            }
        }





    }
}
