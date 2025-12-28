using FleetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetManagement.Infrastructure.Persistence.Configurations;

public class PayoutLineConfiguration : IEntityTypeConfiguration<PayoutLine>
{
    public void Configure(EntityTypeBuilder<PayoutLine> builder)
    {
        builder.ToTable("payout_lines");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder.Property(l => l.PayoutRunId)
            .IsRequired()
            .HasColumnName("payout_run_id");

        builder.Property(l => l.ExternalDriverId)
            .IsRequired()
            .HasColumnName("external_driver_id");

        builder.OwnsOne(l => l.GrossAmount, amount =>
        {
            amount.Property(m => m.Amount)
                .HasPrecision(18, 2)
                .HasColumnName("gross_amount_amount");
            amount.Property(m => m.Currency)
                .HasMaxLength(3)
                .HasColumnName("gross_amount_currency");
        });

        builder.OwnsOne(l => l.Deductions, amount =>
        {
            amount.Property(m => m.Amount)
                .HasPrecision(18, 2)
                .HasColumnName("deductions_amount");
            amount.Property(m => m.Currency)
                .HasMaxLength(3)
                .HasColumnName("deductions_currency");
        });

        builder.OwnsOne(l => l.NetAmount, amount =>
        {
            amount.Property(m => m.Amount)
                .HasPrecision(18, 2)
                .HasColumnName("net_amount_amount");
            amount.Property(m => m.Currency)
                .HasMaxLength(3)
                .HasColumnName("net_amount_currency");
        });

        builder.Property(l => l.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(l => l.UpdatedAtUtc)
            .HasColumnName("updated_at_utc")
            .IsRequired();

        builder.HasOne<PayoutRun>()
            .WithMany(r => r.PayoutLines)
            .HasForeignKey(l => l.PayoutRunId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
