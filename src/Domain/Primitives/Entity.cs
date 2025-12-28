namespace FleetManagement.Domain.Primitives;

public abstract class Entity<TId> where TId : notnull
{
    public TId Id { get; protected set; }
    public DateTime CreatedAtUtc { get; protected set; }
    public DateTime UpdatedAtUtc { get; protected set; }

    protected Entity(TId id)
    {
        Id = id;
    }
}
