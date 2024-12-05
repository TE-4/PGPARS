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
        public DbSet<Funding> Fundings { get; internal set; }

        // Do not need a DbSet for AppUser because it's built into IdentityDbContext
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define the one-to-many relationship between Applicant and Funding
            modelBuilder.Entity<Funding>()
                .HasOne(f => f.Applicant)          // Funding has one Applicant
                .WithMany(a => a.Fundings)         // Applicant has many Fundings
                .HasForeignKey(f => f.Nnumber) // Specify foreign key
                .OnDelete(DeleteBehavior.Cascade); // Configure delete behavior

            // relationship such that an Applicant can have multiple Funding entries,
            // and deleting an Applicant will also delete their related Funding entries.
        }

    }

}
