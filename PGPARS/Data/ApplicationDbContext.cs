﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

        // Do not need a DbSet for AppUser because it's built into IdentityDbContext
    }
}
