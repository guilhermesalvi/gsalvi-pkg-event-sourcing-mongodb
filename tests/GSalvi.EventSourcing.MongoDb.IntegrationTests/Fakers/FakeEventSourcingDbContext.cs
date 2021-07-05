using System;
using System.Diagnostics.CodeAnalysis;
using GSalvi.EventSourcing.MongoDb.IntegrationTests.Configurations;
using Mongo2Go;
using MongoDB.Driver;

namespace GSalvi.EventSourcing.MongoDb.IntegrationTests.Fakers
{
    [ExcludeFromCodeCoverage]
    public class FakeEventSourcingDbContext : IEventSourcingDbContext<MySnapshot>, IDisposable
    {
        private readonly MongoDbRunner _runner;
        public IMongoCollection<MySnapshot> SnapshotCollection { get; }

        public FakeEventSourcingDbContext(IEventSourcingDatabaseSettings settings)
        {
            _runner = MongoDbRunner.StartForDebugging(port: 27018);
            _runner.Import(
                settings.DatabaseName,
                settings.SnapshotCollectionName,
                @".\Configurations\DbSeed.json",
                true);

            var client = new MongoClient(_runner.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            SnapshotCollection = database.GetCollection<MySnapshot>(settings.SnapshotCollectionName);
        }

        public void Dispose()
        {
            _runner.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}