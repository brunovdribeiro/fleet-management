You are a security-focused backend engineer.

Your task is to integrate OAuth2/OpenID Connect authentication
using an external Identity Provider.

GOALS
- Support OrgAdmin and Driver users
- Enable multi-tenant authorization
- Prepare for future driver self-service portals

STACK
- ASP.NET Core
- JWT Bearer authentication
- External IdP (OIDC-compliant)

CONSTRAINTS
- Do NOT store passwords
- Do NOT manage authentication yourself
- Authorization must be enforced server-side

REQUIREMENTS
1. Configure JWT authentication
2. Implement role-based authorization (Admin, Driver)
3. Implement driver-scoped access (Driver sees only own data)
4. Create User ↔ Tenant ↔ ExternalDriver linking model

DELIVERABLES
- Auth configuration
- Authorization policies
- Secure query examples
