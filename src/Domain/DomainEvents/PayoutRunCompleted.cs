using FleetManagement.Domain.Primitives;

namespace FleetManagement.Domain.DomainEvents;

public sealed class PayoutRunCompleted : IDomainEvent
{
    public Guid PayoutRunId { get; }

    public PayoutRunCompleted(Guid payoutRunId)
    {
        Id = Guid.NewGuid();
        OccurredOnUtc = DateTime.UtcNow;
        PayoutRunId = payoutRunId;
    }

    public Guid Id { get; }
    public DateTime OccurredOnUtc { get; }
}
