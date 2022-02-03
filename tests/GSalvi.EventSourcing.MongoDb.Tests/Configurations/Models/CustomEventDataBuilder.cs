using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace GSalvi.EventSourcing.MongoDb.Tests.Configurations.Models;

[ExcludeFromCodeCoverage]
public class CustomEventDataBuilder : IEventDataBuilder<CustomEventData>
{
    public Task<CustomEventData> BuildAsync(Guid aggregateId, string eventType, dynamic entity)
    {
        return Task.FromResult(new CustomEventData
        {
            Id = Guid.NewGuid(),
            AggregateId = aggregateId,
            EventType = eventType,
            Entity = entity,
            Timestamp = DateTime.UtcNow,
            UserId = new Guid("e872aa96-2c62-4a99-9bb5-fcdcd0fb093e")
        });
    }
}