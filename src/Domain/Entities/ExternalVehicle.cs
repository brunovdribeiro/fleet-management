using FleetManagement.Domain.Primitives;

namespace FleetManagement.Domain.Entities;

public class ExternalVehicle : Entity<Guid>
{
    private static readonly string[] ValidPlatforms = { "Uber", "Bolt" };

    public Guid TenantId { get; private set; }
    public string ExternalId { get; private set; }
    public string LicensePlate { get; private set; }
    public string Platform { get; private set; }

    public ExternalVehicle(Guid id, Guid tenantId, string externalId, string licensePlate, string platform) : base(id)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty", nameof(tenantId));

        if (string.IsNullOrWhiteSpace(externalId))
            throw new ArgumentException("ExternalId cannot be empty", nameof(externalId));

        if (string.IsNullOrWhiteSpace(licensePlate))
            throw new ArgumentException("LicensePlate cannot be empty", nameof(licensePlate));

        if (licensePlate.Length > 20)
            throw new ArgumentException("LicensePlate cannot exceed 20 characters", nameof(licensePlate));

        if (string.IsNullOrWhiteSpace(platform))
            throw new ArgumentException("Platform cannot be empty", nameof(platform));

        if (!ValidPlatforms.Contains(platform))
            throw new ArgumentException($"Platform must be one of: {string.Join(", ", ValidPlatforms)}", nameof(platform));

        TenantId = tenantId;
        ExternalId = externalId;
        LicensePlate = licensePlate;
        Platform = platform;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    // Private constructor for EF Core
    private ExternalVehicle() : base(Guid.NewGuid())
    {
        TenantId = Guid.Empty;
        ExternalId = string.Empty;
        LicensePlate = string.Empty;
        Platform = string.Empty;
    }

    public void UpdateLicensePlate(string licensePlate)
    {
        if (string.IsNullOrWhiteSpace(licensePlate))
            throw new ArgumentException("LicensePlate cannot be empty", nameof(licensePlate));

        if (licensePlate.Length > 20)
            throw new ArgumentException("LicensePlate cannot exceed 20 characters", nameof(licensePlate));

        LicensePlate = licensePlate;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
