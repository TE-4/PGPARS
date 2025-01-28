using System.ComponentModel;

namespace PGPARS.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Action { get; set; }
        public string? User { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Details { get; set; }
    }
}
