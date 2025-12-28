You are a senior backend engineer and software architect.

Your task is to bootstrap the foundation of a SaaS platform for ride-hailing
fleet payment automation.

GOALS
- Create a .NET solution structured for long-term SaaS development
- Prepare the system for event-driven architecture
- Support multi-tenancy from day one

TECH STACK (MANDATORY)
- .NET 8+
- ASP.NET Core Web API
- PostgreSQL
- EF Core
- Docker-ready
- Clean Architecture / Modular Monolith

CONSTRAINTS
- No external integrations yet
- No frontend required
- No authentication logic yet (stubs only)

REQUIREMENTS
1. Create a solution with projects:
   - Api
   - Domain
   - Application
   - Infrastructure
   - Workers
   - Tests

2. Configure EF Core with PostgreSQL (docker using PostgreSQL)
3. Implement Tenant entity and TenantId propagation
4. Add database migrations
5. Provide a README with instructions to run locally

DELIVERABLES
- Compilable solution
- Database schema with Tenant table
- Clear folder structure
- No business logic yet
