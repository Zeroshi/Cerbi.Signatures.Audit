using Cerbi.Governance.Schemas.Events;

namespace Cerbi.Signatures.Audit;

public static class AuditEvents
{
    public static GovernedEvent EntityCreated(
        string actorId,
        string entityType,
        string entityId,
        string tenantId,
        string correlationId)
    {
        return GovernedEventBuilder.Create(
            eventName: "Audit.EntityCreated",
            category: "Audit",
            message: $"{entityType} entity created",
            properties: new Dictionary<string, object?>
            {
                ["actorId"] = actorId,
                ["entityType"] = entityType,
                ["entityId"] = entityId,
                ["tenantId"] = tenantId,
                ["correlationId"] = correlationId
            });
    }

    public static GovernedEvent EntityUpdated(
        string actorId,
        string entityType,
        string entityId,
        string changeType,
        string tenantId,
        string correlationId)
    {
        return GovernedEventBuilder.Create(
            eventName: "Audit.EntityUpdated",
            category: "Audit",
            message: $"{entityType} entity updated",
            properties: new Dictionary<string, object?>
            {
                ["actorId"] = actorId,
                ["entityType"] = entityType,
                ["entityId"] = entityId,
                ["changeType"] = changeType,
                ["tenantId"] = tenantId,
                ["correlationId"] = correlationId
            });
    }

    public static GovernedEvent EntityDeleted(
        string actorId,
        string entityType,
        string entityId,
        string tenantId,
        string correlationId)
    {
        return GovernedEventBuilder.Create(
            eventName: "Audit.EntityDeleted",
            category: "Audit",
            message: $"{entityType} entity deleted",
            properties: new Dictionary<string, object?>
            {
                ["actorId"] = actorId,
                ["entityType"] = entityType,
                ["entityId"] = entityId,
                ["tenantId"] = tenantId,
                ["correlationId"] = correlationId
            });
    }
}
