You are an integration engineer.

Your task is to design a platform-agnostic ingestion pipeline
for external ride-hailing platforms.

GOALS
- Support Uber via API
- Support Bolt via non-API ingestion
- Normalize all data into a canonical model

CONSTRAINTS
- Do NOT hardcode platform logic in domain
- Assume integrations may fail or retry
- Idempotency is mandatory

REQUIREMENTS
1. Define a Connector interface
2. Implement UberConnector (API stub)
3. Implement BoltConnector (file-based ingestion)
4. Normalize all inputs into EarningItem
5. Enforce deduplication rules

DELIVERABLES
- Connector abstractions
- Ingestion pipeline
- Normalization logic
