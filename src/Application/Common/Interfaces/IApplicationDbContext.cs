using FleetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Tenant> Tenants { get; }
    DbSet<RuleSet> RuleSets { get; }
    DbSet<PaySchedule> PaySchedules { get; }
    DbSet<PayoutRun> PayoutRuns { get; }
    DbSet<PayoutLine> PayoutLines { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
