using System.Collections.Generic;
using PGPARS.Models;

namespace PGPARS.Models.ViewModels
{
    public class FundingAssignmentViewModel
    {
        public Funding Funding { get; set; }
        public IEnumerable<Applicant> Applicants { get; set; }
    }
}
