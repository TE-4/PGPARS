using System.ComponentModel.DataAnnotations;

namespace PGPARS.Models
{
    public class Review
    {
        [Key]
        public string? ReviewId { get; set; } //name

        public bool? IsAccepted { get; set; }

    }
}
