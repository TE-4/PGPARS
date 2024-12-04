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

        // DbSet for Applicant, which EF Core will manage
        public DbSet<Applicant> Applicants { get; set; }

        // DbSet for Funding, which EF Core will manage
        public DbSet<Funding> Fundings { get; set; }

        public DbSet<Review> Reviews { get; set; }

        // Do not need a DbSet for AppUser because it's built into IdentityDbContext
    }
}
