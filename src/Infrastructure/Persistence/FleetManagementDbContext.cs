using System.Reflection;
using FleetManagement.Application.Common.Interfaces;
using FleetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Infrastructure.Persistence;

public class FleetManagementDbContext : DbContext, IApplicationDbContext
{
    public FleetManagementDbContext(DbContextOptions<FleetManagementDbContext> options) : base(options)
    {
    }

    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ExternalDriver> ExternalDrivers { get; set; }
    public DbSet<RuleSet> RuleSets { get; set; }
    public DbSet<PaySchedule> PaySchedules { get; set; }
    public DbSet<PayoutRun> PayoutRuns { get; set; }
    public DbSet<PayoutLine> PayoutLines { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}