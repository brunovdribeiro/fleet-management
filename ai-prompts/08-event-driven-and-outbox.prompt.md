You are introducing event-driven reliability.

Your task is to implement the Outbox Pattern and messaging.

GOALS
- Reliable event publication
- Async processing
- No lost events

STACK
- RabbitMQ (or abstract broker)
- EF Core Outbox table

REQUIREMENTS
1. Implement OutboxMessage table
2. Publish domain events asynchronously
3. Implement idempotent consumers
4. Add DLQ handling

DELIVERABLES
- Outbox infrastructure
- Message publishers and consumers
