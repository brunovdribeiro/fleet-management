using FleetManagement.Domain.Primitives;
using FleetManagement.Domain.ValueObjects;

namespace FleetManagement.Domain.Entities;

public class RuleSet : Entity<Guid>
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public decimal CommissionPercentage { get; private set; }
    public Money? FixedFee { get; private set; }

    public RuleSet(Guid id, Guid tenantId, string name, string description, decimal commissionPercentage, Money? fixedFee = null) : base(id)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty", nameof(tenantId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("RuleSet name cannot be empty", nameof(name));

        if (name.Length > 200)
            throw new ArgumentException("RuleSet name cannot exceed 200 characters", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));

        if (description.Length > 1000)
            throw new ArgumentException("Description cannot exceed 1000 characters", nameof(description));

        if (commissionPercentage < 0 || commissionPercentage > 100)
            throw new ArgumentException("Commission percentage must be between 0 and 100", nameof(commissionPercentage));

        TenantId = tenantId;
        Name = name;
        Description = description;
        CommissionPercentage = commissionPercentage;
        FixedFee = fixedFee;
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    // Private constructor for EF Core
    private RuleSet() : base(Guid.NewGuid())
    {
        TenantId = Guid.Empty;
        Name = string.Empty;
        Description = string.Empty;
    }

    public void UpdateCommissionPercentage(decimal percentage)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentException("Commission percentage must be between 0 and 100", nameof(percentage));

        CommissionPercentage = percentage;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void UpdateFixedFee(Money? fixedFee)
    {
        FixedFee = fixedFee;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public Money CalculateDeductions(Money grossAmount)
    {
        if (grossAmount == null)
            throw new ArgumentNullException(nameof(grossAmount));

        var commissionAmount = grossAmount.Amount * (CommissionPercentage / 100);
        var totalDeductions = commissionAmount;

        if (FixedFee != null)
        {
            if (FixedFee.Currency != grossAmount.Currency)
                throw new InvalidOperationException("Fixed fee currency must match gross amount currency");

            totalDeductions += FixedFee.Amount;
        }

        return new Money(totalDeductions, grossAmount.Currency);
    }
}
