using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PGPARS.Models;


namespace PGPARS.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet properties for the models
        public DbSet<Applicant> Applicants { get; set; }

        public DbSet<Funding> Fundings { get;  set; }
        public DbSet<FundingAllocation> FundingAllocations { get; set; }

        // Review is now the Join table for Applicant and AppUser (Committee)
        public DbSet<Review> Reviews { get; set; } 
        public DbSet<AuditLog> AuditLogs { get; set; }


        // I removed OnModelCreating for now, but it can be added back in if needed

    }
}
