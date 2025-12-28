using FleetManagement.Domain.Primitives;

namespace FleetManagement.Domain.Entities;

public class User : Entity<Guid>
{
    public string ExternalUserId { get; private set; }
    public string Email { get; private set; }
    public string? FullName { get; private set; }
    public UserRole Role { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid? ExternalDriverId { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    public Tenant? Tenant { get; private set; }
    public ExternalDriver? ExternalDriver { get; private set; }

    public User(
        Guid id,
        string externalUserId,
        string email,
        string? fullName,
        UserRole role,
        Guid tenantId,
        Guid? externalDriverId = null) : base(id)
    {
        if (string.IsNullOrWhiteSpace(externalUserId))
            throw new ArgumentException("ExternalUserId cannot be empty", nameof(externalUserId));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (email.Length > 256)
            throw new ArgumentException("Email cannot exceed 256 characters", nameof(email));

        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty", nameof(tenantId));

        if (role == UserRole.Driver && externalDriverId == null)
            throw new ArgumentException("Driver users must have an associated ExternalDriver", nameof(externalDriverId));

        if (role == UserRole.OrgAdmin && externalDriverId != null)
            throw new ArgumentException("OrgAdmin users cannot have an associated ExternalDriver", nameof(externalDriverId));

        ExternalUserId = externalUserId;
        Email = email;
        FullName = fullName;
        Role = role;
        TenantId = tenantId;
        ExternalDriverId = externalDriverId;
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    // Private constructor for EF Core
    private User() : base(Guid.NewGuid())
    {
        ExternalUserId = string.Empty;
        Email = string.Empty;
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

    public void UpdateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (email.Length > 256)
            throw new ArgumentException("Email cannot exceed 256 characters", nameof(email));

        Email = email;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void UpdateFullName(string? fullName)
    {
        FullName = fullName;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
