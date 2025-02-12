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
        public DbSet<FundingAllocations> FundingAllocations { get; set; }


        public DbSet<Review> Reviews { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<ApplicantReviewer> ApplicantReviewers { get; set; } // Register join table

        // fluent API to configure the relationship between committee(AppUser) and applicant

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define Many-to-Many Relationship
            modelBuilder.Entity<ApplicantReviewer>()
                .HasOne(ar => ar.Applicant)
                .WithMany(a => a.ApplicantReviewers)
                .HasForeignKey(ar => ar.Nnumber);

            modelBuilder.Entity<ApplicantReviewer>()
                .HasOne(ar => ar.AppUser)
                .WithMany(au => au.ApplicantReviewers)
                .HasForeignKey(ar => ar.AppUserId);
        }

    }
}
