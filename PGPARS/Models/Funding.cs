using System.ComponentModel.DataAnnotations;

namespace PGPARS.Models
{
    public class Funding
    {
        [Key]
        public string? Name { get; set; }  // Name of corporation

        public double? Amount { get; set; }
    }
}
