using FleetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetManagement.Infrastructure.Persistence.Configurations;

public class RuleSetConfiguration : IEntityTypeConfiguration<RuleSet>
{
    public void Configure(EntityTypeBuilder<RuleSet> builder)
    {
        builder.ToTable("rule_sets");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder.Property(r => r.TenantId)
            .IsRequired()
            .HasColumnName("tenant_id");

        builder.Property(r => r.Name)
            .HasMaxLength(200)
            .IsRequired()
            .HasColumnName("name");

        builder.Property(r => r.Description)
            .HasMaxLength(1000)
            .IsRequired()
            .HasColumnName("description");

        builder.Property(r => r.IsActive)
            .IsRequired()
            .HasColumnName("is_active");

        builder.Property(r => r.CommissionPercentage)
            .HasPrecision(18, 2)
            .IsRequired()
            .HasColumnName("commission_percentage");

        builder.OwnsOne(r => r.FixedFee, fee =>
        {
            fee.Property(m => m.Amount)
                .HasPrecision(18, 2)
                .HasColumnName("fixed_fee_amount");
            fee.Property(m => m.Currency)
                .HasMaxLength(3)
                .HasColumnName("fixed_fee_currency");
        });

        builder.Property(r => r.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(r => r.UpdatedAtUtc)
            .HasColumnName("updated_at_utc")
            .IsRequired();

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(r => r.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
