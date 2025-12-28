You are optimizing read performance.

Your task is to build read-only projections for reporting.

GOALS
- Fast dashboards
- No load on write DB
- Eventually consistent

REQUIREMENTS
1. Create read models:
   - DriverWeeklySummary
   - VehicleDailySummary
2. Build consumers to update projections
3. Expose read APIs

DELIVERABLES
- Read DB schema
- Projection consumers
- Reporting endpoints
