using System.ComponentModel.DataAnnotations;

namespace DataBaseAccess.Models
{
    public abstract class User
    {
        [Key]
        public Guid UserId { get; set; }

        public uint CreditsUsed { get; set; }

        public DateTime? TimeOfLastReportRan { get; set; }

        public ICollection<ReportRequest>? ReportRequests { get; set; }
    }
}
