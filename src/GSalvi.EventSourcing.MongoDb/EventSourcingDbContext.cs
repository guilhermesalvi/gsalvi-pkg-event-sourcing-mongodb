using System;
using System.Runtime.CompilerServices;
using MongoDB.Driver;

[assembly: InternalsVisibleTo("GSalvi.EventSourcing.MongoDb.UnitTests")]

namespace GSalvi.EventSourcing.MongoDb
{
    internal class EventSourcingDbContext<T> : IEventSourcingDbContext<T>
        where T : Snapshot
    {
        public IMongoCollection<T> SnapshotCollection { get; }

        public EventSourcingDbContext(IEventSourcingDatabaseSettings settings)
        {
            if (settings is null) throw new ArgumentNullException(nameof(settings));

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            SnapshotCollection = database.GetCollection<T>(settings.SnapshotCollectionName);
        }
    }
}