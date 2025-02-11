using System.Collections.Generic;
using PGPARS.Models;

namespace PGPARS.Models.ViewModels
{
    public class FundingAssignmentViewModel
    {
            public FundingSource Funding { get; set; }  // The selected funding source
            public List<Applicant> Applicants { get; set; } // List of applicants eligible for funding
            public List<FundingAllocations> Allocations { get; set; } // Existing allocations for this funding
    }
