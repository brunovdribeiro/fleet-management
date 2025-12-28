using FleetManagement.Domain.Primitives;

namespace FleetManagement.Domain.Entities;

public class ExternalDriver : Entity<Guid>
{
    private static readonly string[] ValidPlatforms = { "Uber", "Bolt" };

    public Guid TenantId { get; private set; }
    public string ExternalId { get; private set; }
    public string Name { get; private set; }
    public string Platform { get; private set; }

    public ExternalDriver(Guid id, Guid tenantId, string externalId, string name, string platform) : base(id)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty", nameof(tenantId));

        if (string.IsNullOrWhiteSpace(externalId))
            throw new ArgumentException("ExternalId cannot be empty", nameof(externalId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Driver name cannot be empty", nameof(name));

        if (name.Length > 200)
            throw new ArgumentException("Driver name cannot exceed 200 characters", nameof(name));

        if (string.IsNullOrWhiteSpace(platform))
            throw new ArgumentException("Platform cannot be empty", nameof(platform));

        if (!ValidPlatforms.Contains(platform))
            throw new ArgumentException($"Platform must be one of: {string.Join(", ", ValidPlatforms)}", nameof(platform));

        TenantId = tenantId;
        ExternalId = externalId;
        Name = name;
        Platform = platform;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    // Private constructor for EF Core
    private ExternalDriver() : base(Guid.NewGuid())
    {
        TenantId = Guid.Empty;
        ExternalId = string.Empty;
        Name = string.Empty;
        Platform = string.Empty;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Driver name cannot be empty", nameof(name));

        if (name.Length > 200)
            throw new ArgumentException("Driver name cannot exceed 200 characters", nameof(name));

        Name = name;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
