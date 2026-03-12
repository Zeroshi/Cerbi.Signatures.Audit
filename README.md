# Cerbi.Signatures.Audit

[![NuGet](https://img.shields.io/nuget/v/Cerbi.Signatures.Audit)](https://www.nuget.org/packages/Cerbi.Signatures.Audit)
[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

**Typed event factories for audit-related governed log events.** Provides strongly-typed methods for creating `GovernedEvent` instances for entity lifecycle operations — creation, update, and deletion.

## Why This Package?

Audit trails are critical for compliance (SOC2, HIPAA, GDPR). `Cerbi.Signatures.Audit` ensures every audit event in your application has **consistent field names**, **compile-time type safety**, and **automatic governance validation** through the Cerbi pipeline. No more ad-hoc property bags with typos or missing fields.

## Installation

```shell
dotnet add package Cerbi.Signatures.Audit
```

This package depends on [`Cerbi.Governance.Schemas`](https://github.com/Zeroshi/Cerbi.Governance.Schemas), which is installed automatically as a transitive dependency.

## API Reference

All methods are on the static `AuditEvents` class and return a `GovernedEvent` with `Category = "Audit"`.

### `AuditEvents.EntityCreated`

Records the creation of a new entity.

```csharp
using Cerbi.Signatures.Audit;

var evt = AuditEvents.EntityCreated(
    actorId: "usr-admin-01",
    entityType: "Order",
    entityId: "ord-98765",
    tenantId: "tenant-abc",
    correlationId: "corr-100");
```

**Properties dictionary:**
| Key | Value | Description |
|---|---|---|
| `actorId` | `"usr-admin-01"` | User/service that performed the action |
| `entityType` | `"Order"` | Type of entity created |
| `entityId` | `"ord-98765"` | Unique identifier of the new entity |
| `tenantId` | `"tenant-abc"` | Tenant context |
| `correlationId` | `"corr-100"` | Distributed trace ID |

### `AuditEvents.EntityUpdated`

Records a modification to an existing entity.

```csharp
var evt = AuditEvents.EntityUpdated(
    actorId: "usr-editor-02",
    entityType: "Order",
    entityId: "ord-98765",
    tenantId: "tenant-abc",
    correlationId: "corr-101",
    changeType: "StatusChange");
```

**Properties dictionary:**
| Key | Value | Description |
|---|---|---|
| `actorId` | `"usr-editor-02"` | User/service that performed the update |
| `entityType` | `"Order"` | Type of entity modified |
| `entityId` | `"ord-98765"` | Unique identifier of the entity |
| `tenantId` | `"tenant-abc"` | Tenant context |
| `correlationId` | `"corr-101"` | Distributed trace ID |
| `changeType` | `"StatusChange"` | Classification of the change |

### `AuditEvents.EntityDeleted`

Records the deletion (or soft-delete) of an entity.

```csharp
var evt = AuditEvents.EntityDeleted(
    actorId: "usr-admin-01",
    entityType: "Order",
    entityId: "ord-98765",
    tenantId: "tenant-abc",
    correlationId: "corr-102");
```

**Properties dictionary:**
| Key | Value | Description |
|---|---|---|
| `actorId` | `"usr-admin-01"` | User/service that performed the deletion |
| `entityType` | `"Order"` | Type of entity deleted |
| `entityId` | `"ord-98765"` | Unique identifier of the entity |
| `tenantId` | `"tenant-abc"` | Tenant context |
| `correlationId` | `"corr-102"` | Distributed trace ID |

## Supported Loggers

`AuditEvents` produces `GovernedEvent` objects compatible with **every** Cerbi logger governance plugin:

| Logger | Cerbi Plugin | Extraction Method |
|---|---|---|
| **CerbiStream** | [`Cerbi-CerbiStream`](https://github.com/Zeroshi/Cerbi-CerbiStream) | Native — `GovernedEvent` is the primary type |
| **Serilog** | [`Cerbi.Serilog.GovernanceAnalyzer`](https://github.com/Zeroshi/Cerbi.Serilog.GovernanceAnalyzer) | `SerilogEventAdapter.ToDictionary()` extracts `LogEvent.Properties` |
| **MEL** | [`Cerbi.MEL.Governance`](https://github.com/Zeroshi/Cerbi.MEL.Governance) | `CerbiGovernanceLogger.ExtractFields<TState>()` extracts key-value pairs |
| **NLog** | [`Cerbi.NLog.GovernanceAnalyzer`](https://github.com/Zeroshi/Cerbi.NLog.GovernanceAnalyzer) | `GovernanceConfigLoader` maps event properties |

### Example: Using with CerbiStream

```csharp
using CerbiStream;
using Cerbi.Signatures.Audit;

var evt = AuditEvents.EntityCreated("usr-1", "Customer", "cust-500", "tenant-1", "corr-1");
logger.LogGovernedEvent(evt);
```

### Example: Using with Serilog

```csharp
using Serilog;
using Cerbi.Signatures.Audit;

var evt = AuditEvents.EntityUpdated("usr-1", "Customer", "cust-500", "tenant-1", "corr-2", "AddressChange");
Log.ForContext("GovernedEvent", evt, destructureObjects: true)
   .Information("Entity updated: {EntityType} {EntityId}", "Customer", "cust-500");
```

### Example: Using with MEL

```csharp
using Microsoft.Extensions.Logging;
using Cerbi.Signatures.Audit;

var evt = AuditEvents.EntityDeleted("usr-1", "Customer", "cust-500", "tenant-1", "corr-3");
logger.LogInformation("Entity deleted: {@AuditEvent}", evt.Properties);
```

## CerbiShield Dashboard Integration

Audit events are a cornerstone of compliance reporting on the CerbiShield Dashboard:

```
AuditEvents.EntityCreated()  ──▶  GovernedEvent  ──▶  Logger Plugin
                                                        │
                                                  Validate + Score
                                                        │
                                                  ScoreShipper
                                                        │
                                              Azure Service Bus
                                                        │
                                             ScoringApi → Aggregator
                                                        │
                                         ┌──────────────▼──────────────┐
                                         │   CerbiShield Dashboard     │
                                         │                             │
                                         │  📋 Audit trail history     │
                                         │  👤 Actor-based filtering   │
                                         │  🏢 Entity type breakdown   │
                                         │  📊 CRUD operation trends   │
                                         │  🔍 Compliance audit views  │
                                         └─────────────────────────────┘
```

### Dashboard Views for Audit Events

| Dashboard Feature | What You See |
|---|---|
| **Overview → Governance Score** | Audit event compliance contributes to overall governance score |
| **Violations → By App** | Which applications have audit events missing required fields |
| **Violations → Top Rules** | Audit rules ranked by violation frequency (e.g., `AUDIT-001: Missing actorId`) |
| **Reporting → Compliance** | SOC2/HIPAA audit trail completeness reporting |
| **Audit Page** | Full audit log with actor, entity, timestamp, and compliance status |

### Governance Profile Example

```json
{
  "name": "audit-trail",
  "appName": "my-service",
  "version": "1.0.0",
  "requiredFields": ["actorId", "entityType", "entityId", "tenantId", "correlationId"],
  "disallowedFields": ["password", "ssn", "apiKey"],
  "fieldSeverities": {
    "actorId": "Error",
    "entityType": "Error",
    "entityId": "Error",
    "tenantId": "Error",
    "correlationId": "Warn"
  },
  "fieldTypes": {
    "actorId": "String",
    "entityType": "String",
    "entityId": "String",
    "tenantId": "String",
    "changeType": "String"
  }
}
```

## Related Packages

| Package | Purpose |
|---|---|
| [`Cerbi.Governance.Schemas`](https://github.com/Zeroshi/Cerbi.Governance.Schemas) | Core types — `GovernedEvent`, `GovernedEventBuilder`, profile definitions |
| [`Cerbi.Signatures.Security`](https://github.com/Zeroshi/Cerbi.Signatures.Security) | Typed factories for security events (login failures, permission denied) |
| [`Cerbi.Signatures.Api`](https://github.com/Zeroshi/Cerbi.Signatures.Api) | Typed factories for API lifecycle events |

## License

MIT
