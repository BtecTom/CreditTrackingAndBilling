using System.ComponentModel.DataAnnotations;

namespace DataBaseAccess.Models;

public class Plan
{
    public Guid PlanId { get; set; }

    [MaxLength(25)]
    public required string PlanName { get; set; }

    public uint Credits {  get; set; }
}