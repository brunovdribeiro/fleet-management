using FleetManagement.Domain.Primitives;

namespace FleetManagement.Domain.Entities;

public class Tenant : Entity<Guid>
{
    public string Name { get; private set; }
    public bool IsActive { get; private set; }

    public Tenant(Guid id, string name) : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tenant name cannot be empty", nameof(name));

        if (name.Length > 200)
            throw new ArgumentException("Tenant name cannot exceed 200 characters", nameof(name));

        Name = name;
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    // Private constructor for EF Core
    private Tenant() : base(Guid.NewGuid())
    {
        Name = string.Empty;
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tenant name cannot be empty", nameof(name));

        if (name.Length > 200)
            throw new ArgumentException("Tenant name cannot exceed 200 characters", nameof(name));

        Name = name;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
