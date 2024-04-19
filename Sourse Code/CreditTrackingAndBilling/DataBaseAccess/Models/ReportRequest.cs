using System.ComponentModel.DataAnnotations;

namespace DataBaseAccess.Models;

public class ReportRequest
{
    public Guid ReportRequestId { get; set; }

    [MaxLength(25)]
    public required string ReportId { get; set; }

    public bool ReportRequestSuccessful { get; set; }

    public required User User { get; set; }

    public DateTime? RequestTime { get; set; } = DateTime.Now;
}