namespace FleetManagement.Api.Authorization;

public static class AuthorizationPolicies
{
    public const string RequireSuperAdminRole = "RequireSuperAdminRole";
    public const string RequireOrgAdminRole = "RequireOrgAdminRole";
    public const string RequireDriverRole = "RequireDriverRole";
}
