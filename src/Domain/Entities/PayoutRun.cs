using FleetManagement.Domain.DomainEvents;
using FleetManagement.Domain.Enums;
using FleetManagement.Domain.Primitives;
using FleetManagement.Domain.ValueObjects;

namespace FleetManagement.Domain.Entities;

public class PayoutRun : Entity<Guid>
{
    private readonly List<PayoutLine> _payoutLines = new();

    public Guid TenantId { get; private set; }
    public Guid RuleSetId { get; private set; }
    public DateTime StartPeriodUtc { get; private set; }
    public DateTime EndPeriodUtc { get; private set; }
    public PayoutRunStatus Status { get; private set; }
    public IReadOnlyCollection<PayoutLine> PayoutLines => _payoutLines.AsReadOnly();

    public PayoutRun(Guid id, Guid tenantId, Guid ruleSetId, DateTime startPeriodUtc, DateTime endPeriodUtc) : base(id)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty", nameof(tenantId));

        if (ruleSetId == Guid.Empty)
            throw new ArgumentException("RuleSetId cannot be empty", nameof(ruleSetId));

        if (endPeriodUtc <= startPeriodUtc)
            throw new ArgumentException("End period must be after start period", nameof(endPeriodUtc));

        if (startPeriodUtc > DateTime.UtcNow)
            throw new ArgumentException("Start period cannot be in the future", nameof(startPeriodUtc));

        TenantId = tenantId;
        RuleSetId = ruleSetId;
        StartPeriodUtc = startPeriodUtc;
        EndPeriodUtc = endPeriodUtc;
        Status = PayoutRunStatus.Pending;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    // Private constructor for EF Core
    private PayoutRun() : base(Guid.NewGuid())
    {
        TenantId = Guid.Empty;
        RuleSetId = Guid.Empty;
    }

    public void AddLine(PayoutLine line)
    {
        if (line == null)
            throw new ArgumentNullException(nameof(line));

        if (Status != PayoutRunStatus.Pending)
            throw new InvalidOperationException("Cannot add lines to a completed payout run");

        if (line.PayoutRunId != Id)
            throw new ArgumentException("PayoutLine does not belong to this payout run", nameof(line));

        if (_payoutLines.Any(l => l.ExternalDriverId == line.ExternalDriverId))
            throw new InvalidOperationException("A payout line for this driver already exists in this run");

        _payoutLines.Add(line);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (Status != PayoutRunStatus.Pending)
            throw new InvalidOperationException("Payout run has already been completed");

        if (!_payoutLines.Any())
            throw new InvalidOperationException("Cannot complete payout run with no payout lines");

        Status = PayoutRunStatus.Completed;
        RaiseDomainEvent(new PayoutRunCompleted(Id));
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public Money GetTotalGrossAmount()
    {
        if (!_payoutLines.Any())
            return new Money(0, "USD");

        var firstCurrency = _payoutLines.First().GrossAmount.Currency;

        if (_payoutLines.Any(l => l.GrossAmount.Currency != firstCurrency))
            throw new InvalidOperationException("Cannot sum amounts with different currencies");

        var total = _payoutLines.Sum(l => l.GrossAmount.Amount);
        return new Money(total, firstCurrency);
    }

    public Money GetTotalNetAmount()
    {
        if (!_payoutLines.Any())
            return new Money(0, "USD");

        var firstCurrency = _payoutLines.First().NetAmount.Currency;

        if (_payoutLines.Any(l => l.NetAmount.Currency != firstCurrency))
            throw new InvalidOperationException("Cannot sum amounts with different currencies");

        var total = _payoutLines.Sum(l => l.NetAmount.Amount);
        return new Money(total, firstCurrency);
    }
}
