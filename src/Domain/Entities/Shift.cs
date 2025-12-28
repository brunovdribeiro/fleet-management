using FleetManagement.Domain.DomainEvents;
using FleetManagement.Domain.Primitives;
using FleetManagement.Domain.ValueObjects;

namespace FleetManagement.Domain.Entities;

public class Shift : Entity<Guid>
{
    private readonly List<EarningItem> _earningItems = new();

    public Guid ExternalDriverId { get; private set; }
    public Guid? ExternalVehicleId { get; private set; }
    public DateTime StartUtc { get; private set; }
    public DateTime? EndUtc { get; private set; }
    public IReadOnlyCollection<EarningItem> EarningItems => _earningItems.AsReadOnly();

    public bool IsActive => !EndUtc.HasValue;

    public Shift(Guid id, Guid externalDriverId, Guid? externalVehicleId, DateTime startUtc) : base(id)
    {
        if (externalDriverId == Guid.Empty)
            throw new ArgumentException("ExternalDriverId cannot be empty", nameof(externalDriverId));

        if (startUtc > DateTime.UtcNow)
            throw new ArgumentException("Shift start time cannot be in the future", nameof(startUtc));

        ExternalDriverId = externalDriverId;
        ExternalVehicleId = externalVehicleId;
        StartUtc = startUtc;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    // Private constructor for EF Core
    private Shift() : base(Guid.NewGuid())
    {
        ExternalDriverId = Guid.Empty;
    }

    public void EndShift(DateTime endUtc)
    {
        if (EndUtc.HasValue)
            throw new InvalidOperationException("Shift has already ended");

        if (endUtc < StartUtc)
            throw new ArgumentException("End time cannot be before start time", nameof(endUtc));

        if (endUtc > DateTime.UtcNow)
            throw new ArgumentException("End time cannot be in the future", nameof(endUtc));

        EndUtc = endUtc;
        UpdatedAtUtc = DateTime.UtcNow;
        RaiseDomainEvent(new ShiftCompleted(Id, ExternalDriverId));
    }

    public void AddEarningItem(EarningItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (item.ShiftId != Id)
            throw new ArgumentException("EarningItem does not belong to this shift", nameof(item));

        if (EndUtc.HasValue && item.TimestampUtc > EndUtc.Value)
            throw new ArgumentException("Cannot add earning item after shift has ended", nameof(item));

        if (item.TimestampUtc < StartUtc)
            throw new ArgumentException("Earning item timestamp cannot be before shift start", nameof(item));

        _earningItems.Add(item);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public Money GetTotalEarnings()
    {
        if (!_earningItems.Any())
            return new Money(0, "USD");

        var firstCurrency = _earningItems.First().Amount.Currency;

        if (_earningItems.Any(e => e.Amount.Currency != firstCurrency))
            throw new InvalidOperationException("Cannot sum earnings with different currencies");

        var total = _earningItems.Sum(e => e.Amount.Amount);
        return new Money(total, firstCurrency);
    }
}
