namespace GSalvi.EventSourcing.MongoDb
{
    internal class EventSourcingDatabaseSettings : IEventSourcingDatabaseSettings
    {
        public string SnapshotCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}