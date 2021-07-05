using AutoFixture;
using FluentAssertions;
using Xunit;

namespace GSalvi.EventSourcing.MongoDb.UnitTests
{
    public class EventSourcingDatabaseSettingsTests
    {
        private readonly string _snapshotCollectionName;
        private readonly string _connectionString;
        private readonly string _databaseName;
        private readonly EventSourcingDatabaseSettings _settings;

        public EventSourcingDatabaseSettingsTests()
        {
            var fixture = new Fixture();
            _snapshotCollectionName = fixture.Create<string>();
            _connectionString = fixture.Create<string>();
            _databaseName = fixture.Create<string>();
            _settings = new EventSourcingDatabaseSettings
            {
                SnapshotCollectionName = _snapshotCollectionName,
                ConnectionString = _connectionString,
                DatabaseName = _databaseName
            };
        }

        [Fact]
        public void SnapshotCollectionName_ShouldNotBeChanged_AfterInitialization()
        {
            // Assert
            _settings.SnapshotCollectionName.Should().Be(_snapshotCollectionName);
        }

        [Fact]
        public void ConnectionString_ShouldNotBeChanged_AfterInitialization()
        {
            // Assert
            _settings.ConnectionString.Should().Be(_connectionString);
        }

        [Fact]
        public void DatabaseName_ShouldNotBeChanged_AfterInitialization()
        {
            // Assert
            _settings.DatabaseName.Should().Be(_databaseName);
        }
    }
}