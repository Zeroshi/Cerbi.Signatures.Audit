# Cerbi.Signatures.Audit

Developer helper methods for audit events in the Cerbi governance ecosystem.

## Purpose

Provides strongly-typed signature methods that produce `GovernedEvent` objects for audit scenarios. These events are **not** written to any logger directly — they are structured event objects ready for consumption by any logging adapter.

## Signatures

| Method | Event Name | Description |
|--------|-----------|-------------|
| `AuditEvents.EntityCreated(...)` | `Audit.EntityCreated` | Entity created |
| `AuditEvents.EntityUpdated(...)` | `Audit.EntityUpdated` | Entity updated |
| `AuditEvents.EntityDeleted(...)` | `Audit.EntityDeleted` | Entity deleted |

## Usage

```csharp
using Cerbi.Signatures.Audit;

var evt = AuditEvents.EntityCreated(
    actorId: "user-123",
    entityType: "Order",
    entityId: "order-456",
    tenantId: "tenant-abc",
    correlationId: "corr-789"
);

// evt is a GovernedEvent — pass it to your logger adapter
```

## Dependencies

- [Cerbi.Governance.Schemas](https://github.com/Zeroshi/Cerbi.Governance.Schemas)

## Build

```bash
dotnet build
dotnet test
```

## License

MIT
