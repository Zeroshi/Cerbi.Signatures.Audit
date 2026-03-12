using Xunit;
using Cerbi.Signatures.Audit;

namespace Cerbi.Signatures.Audit.Tests;

public class AuditEventsTests
{
    [Fact]
    public void EntityCreated_SetsEventName()
    {
        var evt = AuditEvents.EntityCreated("actor-1", "User", "entity-1", "tenant-1", "corr-1");

        Assert.Equal("Audit.EntityCreated", evt.EventName);
    }

    [Fact]
    public void EntityCreated_SetsCategoryToAudit()
    {
        var evt = AuditEvents.EntityCreated("actor-1", "User", "entity-1", "tenant-1", "corr-1");

        Assert.Equal("Audit", evt.Category);
    }

    [Fact]
    public void EntityCreated_SetsMessage()
    {
        var evt = AuditEvents.EntityCreated("actor-1", "User", "entity-1", "tenant-1", "corr-1");

        Assert.Equal("User entity created", evt.Message);
    }

    [Fact]
    public void EntityCreated_ContainsAllRequiredProperties()
    {
        var evt = AuditEvents.EntityCreated("actor-1", "User", "entity-1", "tenant-1", "corr-1");

        Assert.Equal("actor-1", evt.Properties["actorId"]);
        Assert.Equal("User", evt.Properties["entityType"]);
        Assert.Equal("entity-1", evt.Properties["entityId"]);
        Assert.Equal("tenant-1", evt.Properties["tenantId"]);
        Assert.Equal("corr-1", evt.Properties["correlationId"]);
    }

    [Fact]
    public void EntityUpdated_SetsEventName()
    {
        var evt = AuditEvents.EntityUpdated("actor-1", "Order", "entity-2", "Modify", "tenant-1", "corr-2");

        Assert.Equal("Audit.EntityUpdated", evt.EventName);
    }

    [Fact]
    public void EntityUpdated_ContainsChangeType()
    {
        var evt = AuditEvents.EntityUpdated("actor-1", "Order", "entity-2", "Modify", "tenant-1", "corr-2");

        Assert.Equal("Modify", evt.Properties["changeType"]);
    }

    [Fact]
    public void EntityDeleted_SetsEventName()
    {
        var evt = AuditEvents.EntityDeleted("actor-1", "Product", "entity-3", "tenant-1", "corr-3");

        Assert.Equal("Audit.EntityDeleted", evt.EventName);
    }

    [Fact]
    public void EntityDeleted_ContainsAllRequiredProperties()
    {
        var evt = AuditEvents.EntityDeleted("actor-1", "Product", "entity-3", "tenant-1", "corr-3");

        Assert.Equal("actor-1", evt.Properties["actorId"]);
        Assert.Equal("Product", evt.Properties["entityType"]);
        Assert.Equal("entity-3", evt.Properties["entityId"]);
        Assert.Equal("tenant-1", evt.Properties["tenantId"]);
        Assert.Equal("corr-3", evt.Properties["correlationId"]);
    }
}
