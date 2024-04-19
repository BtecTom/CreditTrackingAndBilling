namespace DataBaseAccess.Models
{
    public class ReportRequest
    {
        public Guid ReportRequestId { get; set; }

        public required string ReportId { get; set; }

        public bool ReportRequestSuccessful { get; set; }

        public required User User { get; set; }

        public DateTime? RequestTime { get; set; } = DateTime.Now;
    }
}
