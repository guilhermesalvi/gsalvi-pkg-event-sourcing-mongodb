using System;

namespace GSalvi.EventSourcing.MongoDb.IntegrationTests.Configurations
{
    public class MySnapshotBuilder : ISnapshotBuilder<MySnapshot>
    {
        public MySnapshot Create(Guid aggregateId, string eventType, string serializedData)
        {
            return new MySnapshot
            {
                Id = Guid.NewGuid(),
                AggregateId = aggregateId,
                EventType = eventType,
                SerializedData = serializedData,
                Timestamp = DateTime.UtcNow,
                UserId = string.Empty
            };
        }
    }
}