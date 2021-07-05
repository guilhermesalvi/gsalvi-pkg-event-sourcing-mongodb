using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using GSalvi.EventSourcing.MongoDb.IntegrationTests.Configurations;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

namespace GSalvi.EventSourcing.MongoDb.IntegrationTests
{
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class SnapshotRepositoryTests
    {
        private readonly ISnapshotRepository<MySnapshot> _repository;
        private readonly IEventSourcingDbContext<MySnapshot> _context;

        public SnapshotRepositoryTests(IntegrationTestsFixture<TestStartup> testsFixture)
        {
            _repository = testsFixture.TestHost.ServiceProvider.GetRequiredService<ISnapshotRepository<MySnapshot>>();
            _context = testsFixture.TestHost.ServiceProvider.GetRequiredService<IEventSourcingDbContext<MySnapshot>>();
        }

        [Fact]
        public async Task AddAsync_ShouldAddOneItem()
        {
            // Arrange
            var snapshot = new Fixture().Create<MySnapshot>();

            // Act
            await _repository.AddAsync(snapshot);

            // Assert
            _context.SnapshotCollection
                .Find(x => x.Id == snapshot.Id)
                .FirstOrDefault()
                .Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldGetOneItem()
        {
            // Arrange
            var id = new Guid("5bd18f3d-f4a7-4044-a62e-6215a849ed9c");

            // Act
            var result = await _repository.GetByIdAsync(id).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.AggregateId.Should().Be("998acdf2-0a55-fe45-9641-e9c3d164b4f3");
            result.EventType.Should().Be("UserUpdated");
            result.SerializedData.Should().Be("{'userId': 'fb4ffa0c-0f08-4b68-bd23-316a48aa8ffa'}");
        }

        [Fact]
        public async Task GetByAggregateIdAsync_ShouldGetTwoItem()
        {
            // Arrange
            var aggregateId = new Guid("998acdf2-0a55-fe45-9641-e9c3d164b4f3");

            // Act
            var result = await _repository
                .GetByAggregateIdAsync(aggregateId)
                .ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);

            foreach (var snapshot in result)
            {
                snapshot.Id.Should().NotBe(Guid.Empty);
                snapshot.AggregateId.Should().Be(aggregateId);
                snapshot.EventType.Should().Be("UserUpdated");
                snapshot.SerializedData.Should().Be("{'userId': 'fb4ffa0c-0f08-4b68-bd23-316a48aa8ffa'}");
            }
        }

        [Fact]
        public async Task GetByEventType_ShouldGetTwoItem()
        {
            // Arrange
            var eventType = "UserUpdated";

            // Act
            var result = await _repository
                .GetByEventTypeAsync(eventType)
                .ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(3);

            foreach (var snapshot in result)
            {
                snapshot.Id.Should().NotBe(Guid.Empty);
                snapshot.AggregateId.Should().NotBe(Guid.Empty);
                snapshot.EventType.Should().Be(eventType);
                snapshot.SerializedData.Should().NotBeNull();
            }
        }
    }
}