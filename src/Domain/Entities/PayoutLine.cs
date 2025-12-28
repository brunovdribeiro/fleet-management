using FleetManagement.Domain.Primitives;
using FleetManagement.Domain.ValueObjects;

namespace FleetManagement.Domain.Entities;

public class PayoutLine : Entity<Guid>
{
    public Guid PayoutRunId { get; private set; }
    public Guid ExternalDriverId { get; private set; }
    public Money GrossAmount { get; private set; }
    public Money Deductions { get; private set; }
    public Money NetAmount { get; private set; }

    public PayoutLine(Guid id, Guid payoutRunId, Guid externalDriverId, Money grossAmount, Money deductions) : base(id)
    {
        if (payoutRunId == Guid.Empty)
            throw new ArgumentException("PayoutRunId cannot be empty", nameof(payoutRunId));

        if (externalDriverId == Guid.Empty)
            throw new ArgumentException("ExternalDriverId cannot be empty", nameof(externalDriverId));

        if (grossAmount == null)
            throw new ArgumentNullException(nameof(grossAmount));

        if (deductions == null)
            throw new ArgumentNullException(nameof(deductions));

        if (grossAmount.Currency != deductions.Currency)
            throw new ArgumentException("Gross amount and deductions must have the same currency");

        if (deductions.Amount > grossAmount.Amount)
            throw new ArgumentException("Deductions cannot exceed gross amount", nameof(deductions));

        PayoutRunId = payoutRunId;
        ExternalDriverId = externalDriverId;
        GrossAmount = grossAmount;
        Deductions = deductions;
        NetAmount = new Money(grossAmount.Amount - deductions.Amount, grossAmount.Currency);
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    // Private constructor for EF Core
    private PayoutLine() : base(Guid.NewGuid())
    {
        PayoutRunId = Guid.Empty;
        ExternalDriverId = Guid.Empty;
        GrossAmount = new Money(0, string.Empty);
        Deductions = new Money(0, string.Empty);
        NetAmount = new Money(0, string.Empty);
    }

    public void RecalculateNetAmount()
    {
        if (Deductions.Amount > GrossAmount.Amount)
            throw new InvalidOperationException("Deductions cannot exceed gross amount");

        NetAmount = new Money(GrossAmount.Amount - Deductions.Amount, GrossAmount.Currency);
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
