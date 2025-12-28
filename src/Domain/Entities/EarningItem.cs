using FleetManagement.Domain.Primitives;
using FleetManagement.Domain.ValueObjects;

namespace FleetManagement.Domain.Entities;

public class EarningItem : Entity<Guid>
{
    public Guid ShiftId { get; private set; }
    public string Description { get; private set; }
    public Money Amount { get; private set; }
    public DateTime TimestampUtc { get; private set; }

    public EarningItem(Guid id, Guid shiftId, string description, Money amount, DateTime timestampUtc) : base(id)
    {
        if (shiftId == Guid.Empty)
            throw new ArgumentException("ShiftId cannot be empty", nameof(shiftId));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));

        if (description.Length > 500)
            throw new ArgumentException("Description cannot exceed 500 characters", nameof(description));

        if (amount == null)
            throw new ArgumentNullException(nameof(amount));

        if (timestampUtc > DateTime.UtcNow)
            throw new ArgumentException("Timestamp cannot be in the future", nameof(timestampUtc));

        ShiftId = shiftId;
        Description = description;
        Amount = amount;
        TimestampUtc = timestampUtc;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    // Private constructor for EF Core
    private EarningItem() : base(Guid.NewGuid())
    {
        ShiftId = Guid.Empty;
        Description = string.Empty;
        Amount = new Money(0, string.Empty);
    }
}
