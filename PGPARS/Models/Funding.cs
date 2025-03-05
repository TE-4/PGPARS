using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGPARS.Models
{
    public class Funding
    {
        [Key]
        public int Id { get; set; }  // Primary Key
        public string FundType { get; set; }  // Type of Funding (GRA, GTA, GAA, etc.)
        public string Source { get; set; }  // Source of Funding (COAS, JZAG, etc.)
        public string Cohort { get; set; }  // Cohort Year (2021, 2022, etc.)
        public decimal? Stipends { get; set; }  // Stipend Amount (6.5, 0, 2.4, 5, etc.)
        public int? NumberOfAwards { get; set; }  // Number of Awards (I am not sure if this is different than Stipends)
        public int? Scholarship { get; set; }  // Scholarship Eligibility (true/false)
        public decimal? Amount { get; set; }  // Total Amount of Funding
        public decimal? Remaining { get; set; }  // Remaining Amount of Funding
        public DateTime? DateAdded { get; set; }  // Date Added to the System
        public DateTime? DateModified { get; set; }  // Date Modified in the System
        public string? Comment { get; set; }  // Additional Comments

        public ICollection<FundingAllocation>? FundingAllocations { get; set; } = new List<FundingAllocation>();

        public decimal? RemainingAmount
        {
            get
            {
                decimal allocatedAmount = FundingAllocations?.Sum(a => a.AllocatedAmount) ?? 0;
                return Math.Max(0, (decimal)(Amount - allocatedAmount));

            }

        }

    }

}
