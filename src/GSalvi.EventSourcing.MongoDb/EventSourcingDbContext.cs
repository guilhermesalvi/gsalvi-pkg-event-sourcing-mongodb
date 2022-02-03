using MongoDB.Driver;

namespace GSalvi.EventSourcing.MongoDb;

internal class EventSourcingDbContext<TEventData> where TEventData : EventData
{
    public IMongoCollection<TEventData> EventDataCollection { get; }

    public EventSourcingDbContext(EventSourcingDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        EventDataCollection = database.GetCollection<TEventData>(settings.EventDataCollectionName);
    }
}