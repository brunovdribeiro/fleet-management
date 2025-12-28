You are a domain-driven design specialist.

Your task is to design the core domain model for a fleet-based
driver payment SaaS.

GOALS
- Define the canonical internal model
- Avoid duplicating external platform data
- Support Uber and Bolt equally

CONSTRAINTS
- No persistence logic
- No API controllers
- Domain-only code

DOMAIN CONCEPTS (MANDATORY)
- Tenant (Organization)
- ExternalDriver
- ExternalVehicle
- EarningItem
- Shift
- RuleSet
- PayoutRun
- PayoutLine

REQUIREMENTS
1. Create domain entities and value objects
2. Clearly define aggregate boundaries
3. Define domain events where relevant
4. Include invariants and validations

DELIVERABLES
- Domain entities with rich behavior
- Domain events definitions
- No EF Core attributes
