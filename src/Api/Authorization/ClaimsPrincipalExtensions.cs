using System.Security.Claims;

namespace FleetManagement.Api.Authorization;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? principal.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException("User ID claim not found or invalid");

        return userId;
    }

    public static Guid GetTenantId(this ClaimsPrincipal principal)
    {
        var tenantIdClaim = principal.FindFirst("tenant_id")?.Value;

        if (string.IsNullOrEmpty(tenantIdClaim) || !Guid.TryParse(tenantIdClaim, out var tenantId))
            throw new UnauthorizedAccessException("Tenant ID claim not found or invalid");

        return tenantId;
    }

    public static string GetRole(this ClaimsPrincipal principal)
    {
        var role = principal.FindFirst(ClaimTypes.Role)?.Value
            ?? principal.FindFirst("role")?.Value;

        if (string.IsNullOrEmpty(role))
            throw new UnauthorizedAccessException("Role claim not found");

        return role;
    }

    public static Guid? GetExternalDriverId(this ClaimsPrincipal principal)
    {
        var driverIdClaim = principal.FindFirst("driver_id")?.Value;

        if (string.IsNullOrEmpty(driverIdClaim))
            return null;

        return Guid.TryParse(driverIdClaim, out var driverId) ? driverId : null;
    }
}
