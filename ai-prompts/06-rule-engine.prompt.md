You are implementing the core payment logic.

Your task is to build a configurable, versioned rule engine
for driver payment calculation.

GOALS
- Progressive tier-based rules
- Exclusions (tips)
- Inclusions (promotions)
- Full auditability

RULE MODEL
- Basis: NetEarnings
- Tiers:
  - 0–50 → 30%
  - 50–80 → 35%
  - 80+ → 45%

REQUIREMENTS
1. Implement rule engine in isolation
2. Input: Shift totals
3. Output:
   - DriverPay
   - EffectiveRate
   - Tier breakdown
4. Version rules immutably

DELIVERABLES
- Rule engine
- JSON rule definition
- Unit tests
