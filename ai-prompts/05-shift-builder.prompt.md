You are responsible for time-based aggregation logic.

Your task is to build shifts from normalized earning data.

GOALS
- Convert earning items into shifts
- Support shift-based payment logic
- Be deterministic and reprocessable

CONSTRAINTS
- No platform-specific logic
- Shifts must be rebuildable

SHIFT DEFINITION (MVP)
- One shift = one calendar day per driver (tenant timezone)

REQUIREMENTS
1. Implement shift builder service
2. Aggregate earnings per driver per day
3. Emit ShiftUpserted events
4. Support reprocessing

DELIVERABLES
- Shift builder service
- Unit tests
