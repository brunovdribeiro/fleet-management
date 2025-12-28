namespace FleetManagement.Application.Tenants.DTOs;

public record TenantDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);
