using DataBaseAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBaseAccess;

public class CreditTrackingDbContext(DbContextOptions<CreditTrackingDbContext> options) : DbContext(options)
{
    public DbSet<Organisation> Organisations { get; set; } = null!;
    public DbSet<OrganisationalUser> Users { get; set; } = null!;
    public DbSet<Plan> Plans { get; set; } = null!;
    public DbSet<ReportRequest> ReportRequests { get; set; } = null!;
    public DbSet<TrialUser> TrialUsers { get; set; } = null!;
}