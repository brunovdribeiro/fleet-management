using FleetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetManagement.Infrastructure.Persistence.Configurations;

public class PayScheduleConfiguration : IEntityTypeConfiguration<PaySchedule>
{
    public void Configure(EntityTypeBuilder<PaySchedule> builder)
    {
        builder.ToTable("pay_schedules");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder.Property(p => p.TenantId)
            .IsRequired()
            .HasColumnName("tenant_id");

        builder.Property(p => p.Frequency)
            .IsRequired()
            .HasColumnName("frequency")
            .HasConversion<int>();

        builder.Property(p => p.DayOfWeek)
            .HasColumnName("day_of_week")
            .HasConversion<int?>();

        builder.Property(p => p.DayOfMonth)
            .HasColumnName("day_of_month");

        builder.Property(p => p.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(p => p.UpdatedAtUtc)
            .HasColumnName("updated_at_utc")
            .IsRequired();

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(p => p.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
