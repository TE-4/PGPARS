﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGPARS.Models
{
    public class Funding
    {
        [Key]
        public int FundingID { get; set; } // Unique identifier for funding
      
        public string? Cohort { get; set; } // Cohort for which funding is allocated
        public string? Source { get; set; } // Name/Source of the funds
        public string? FundingType { get; set; } // Type of funding (e.g., grant, scholarship)
        public double? Stipends { get; set; } // Amount allocated for stipends
        public double? Scholarships { get; set; } // Amount allocated for scholarships

        [DataType(DataType.Currency)]
        public decimal? Amount { get; set; } // Total fundable amount
        public DateTime DateAdded { get; set; } = DateTime.UtcNow; // Timestamp when the funding was added
        public DateTime DateModified { get; set; } = DateTime.UtcNow; // Timestamp when the funding was last modified
        public string? Comment { get; set; } // Additional notes or comments


        // Foreign key for Applicant
        [ForeignKey("Applicant")]
        public string? Nnumber { get; set; } // Foreign key property (must match Applicant primary key type)

        // Navigation property for Applicant
        public Applicant? Applicant { get; set; }
        public decimal RemainingAmount { get; internal set; }
    }
}
