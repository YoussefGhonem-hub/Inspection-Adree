using Inspection.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inspection.Infrastructure.Persistence;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Inspector> Inspectors { get; set; }
    public DbSet<EntityToInspect> EntityToInspects { get; set; }
    public DbSet<InspectionVisit> InspectionVisits { get; set; }
    public DbSet<Violation> Violations { get; set; }
    public DbSet<ExceptionLog> ExceptionLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}