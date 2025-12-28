using FleetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetManagement.Infrastructure.Persistence.Configurations;

public class PayoutRunConfiguration : IEntityTypeConfiguration<PayoutRun>
{
    public void Configure(EntityTypeBuilder<PayoutRun> builder)
    {
        builder.ToTable("payout_runs");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder.Property(r => r.TenantId)
            .IsRequired()
            .HasColumnName("tenant_id");

        builder.Property(r => r.RuleSetId)
            .IsRequired()
            .HasColumnName("rule_set_id");

        builder.Property(r => r.StartPeriodUtc)
            .IsRequired()
            .HasColumnName("start_period_utc");

        builder.Property(r => r.EndPeriodUtc)
            .IsRequired()
            .HasColumnName("end_period_utc");

        builder.Property(r => r.Status)
            .IsRequired()
            .HasColumnName("status")
            .HasConversion<int>();

        builder.Property(r => r.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(r => r.UpdatedAtUtc)
            .HasColumnName("updated_at_utc")
            .IsRequired();

        builder.HasMany(r => r.PayoutLines)
            .WithOne()
            .HasForeignKey(l => l.PayoutRunId);

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(r => r.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<RuleSet>()
            .WithMany()
            .HasForeignKey(r => r.RuleSetId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
