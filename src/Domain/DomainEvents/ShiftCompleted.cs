using FleetManagement.Domain.Primitives;

namespace FleetManagement.Domain.DomainEvents;

public sealed class ShiftCompleted : IDomainEvent
{
    public Guid ShiftId { get; }
    public Guid ExternalDriverId { get; }

    public ShiftCompleted(Guid shiftId, Guid externalDriverId)
    {
        Id = Guid.NewGuid();
        OccurredOnUtc = DateTime.UtcNow;
        ShiftId = shiftId;
        ExternalDriverId = externalDriverId;
    }

    public Guid Id { get; }
    public DateTime OccurredOnUtc { get; }
}
