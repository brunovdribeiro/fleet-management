using FleetManagement.Domain.Primitives;

namespace FleetManagement.Domain.DomainEvents;

public sealed class PayoutLineCreated : IDomainEvent
{
    public Guid PayoutLineId { get; }
    public Guid PayoutRunId { get; }
    public Guid ExternalDriverId { get; }

    public PayoutLineCreated(Guid payoutLineId, Guid payoutRunId, Guid externalDriverId)
    {
        Id = Guid.NewGuid();
        OccurredOnUtc = DateTime.UtcNow;
        PayoutLineId = payoutLineId;
        PayoutRunId = payoutRunId;
        ExternalDriverId = externalDriverId;
    }

    public Guid Id { get; }
    public DateTime OccurredOnUtc { get; }
}
