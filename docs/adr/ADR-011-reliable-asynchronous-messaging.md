# ADR-011 - Reliable Asynchronous Messaging

## Status

Accepted

---

## Context

NexusERP requires asynchronous communication between application workflows
without coupling business transactions directly to RabbitMQ.

Publishing an Integration Event immediately after committing a database
transaction creates a failure window: business state could be persisted
while the corresponding message is lost.

Message brokers may also deliver messages more than once, and consumer
failures require controlled retry and dead-letter behavior.

NexusERP must address these failure modes while remaining a Modular Monolith.

---

## Decision

NexusERP adopts a reliable asynchronous messaging architecture based on:

- Domain Events
- Integration Events
- Transactional Outbox
- RabbitMQ
- Publisher Confirms
- Transactional Inbox
- Bounded retries
- Dead-letter handling

Business state changes and corresponding Outbox messages are persisted in
the same database transaction.

A background Worker publishes pending Outbox messages to RabbitMQ.

RabbitMQ Publisher Confirms are required before an Outbox message is marked
as successfully processed.

Consumers use manual acknowledgements and persist processed Integration Event
identifiers in the Inbox.

The Integration Event identifier is used as the Inbox primary key to protect
against duplicate deliveries.

Retry and dead-letter forwarding must be confirmed by RabbitMQ before the
original message is acknowledged.

The resulting delivery model is:

**at-least-once delivery with idempotent consumption**

NexusERP does not claim exactly-once distributed delivery.

---

## Message Flow

```text
Business Operation
        |
        v
Domain Event
        |
        v
Integration Event
        |
        v
Transactional Outbox
        |
        v
Background Worker
        |
        v
RabbitMQ
        |
        v
Transactional Inbox
        |
        v
Integration Event Handler
```

The Outbox protects the producer side of the integration boundary.

The Inbox protects the consumer side against duplicate delivery.

RabbitMQ provides transport between both boundaries.

---

## Failure Handling

If publication fails, the Outbox message remains pending and can be retried
by a later Worker iteration.

Transient consumer failures are forwarded through the retry infrastructure.

After the configured retry limit is exhausted, messages are forwarded to the
dead-letter queue.

`MessageId` is preserved across publication, retry and dead-letter flows for
correlation and diagnostics.

---

## Consequences

### Benefits

- Business state and Integration Event persistence remain atomic.
- Temporary broker failures do not require business transactions to publish
  directly to RabbitMQ.
- Duplicate message delivery can be handled idempotently.
- Retry behavior is bounded.
- Permanent consumer failures remain observable through dead-letter handling.
- Messaging remains compatible with the Modular Monolith architecture.
- Integration boundaries can support future service extraction if required.

### Tradeoffs

- Messaging infrastructure introduces additional operational complexity.
- Asynchronous workflows are eventually consistent.
- Duplicate delivery remains possible and consumers must remain idempotent.
- SQL Server and RabbitMQ must both be operated for messaging workflows.
- Outbox polling introduces a small delay between transaction commit and
  message publication.
- The current implementation favors simplicity over RabbitMQ connection and
  channel reuse.

These tradeoffs are accepted because they provide reliable asynchronous
integration without requiring premature microservice decomposition.