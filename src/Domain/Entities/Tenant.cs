using FleetManagement.Domain.Primitives;

namespace FleetManagement.Domain.Entities;

public class Tenant : Entity<Guid>
{
    public string Name { get; private set; }

    public Tenant(Guid id, string name) : base(id)
    {
        Name = name;
    }

    // Private constructor for EF Core
    private Tenant() : base(Guid.NewGuid()) { }
}
