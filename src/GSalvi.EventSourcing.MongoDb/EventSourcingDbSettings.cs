namespace GSalvi.EventSourcing.MongoDb;

internal class EventSourcingDbSettings
{
    public string? EventDataCollectionName { get; set; }
    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
}