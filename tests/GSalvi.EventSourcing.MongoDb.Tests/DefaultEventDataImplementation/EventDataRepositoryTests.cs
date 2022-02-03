using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using GSalvi.EventSourcing.MongoDb.Tests.Configurations;
using GSalvi.EventSourcing.MongoDb.Tests.Configurations.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

namespace GSalvi.EventSourcing.MongoDb.Tests.DefaultEventDataImplementation;

[Collection(nameof(TestsFixtureCollection))]
public class EventDataRepositoryTests
{
    private readonly EventSourcingDbContext<EventData> _context;
    private readonly IEventDataRepository<EventData> _repository;
    private readonly Fixture _fixture;

    public EventDataRepositoryTests(TestHost<DefaultEventDataTestStartup> testHost)
    {
        _repository = testHost.ServiceProvider.GetRequiredService<IEventDataRepository<EventData>>();
        _context = testHost.ServiceProvider.GetRequiredService<EventSourcingDbContext<EventData>>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task AddAsync_ShouldAddOneItemInDatabase()
    {
        // Arrange
        var customerRegistered = _fixture.Create<CustomerRegistered>();
        var eventData = _fixture
            .Build<CustomEventData>()
            .With(x => x.Entity, customerRegistered)
            .Create();

        // Act
        await _repository.AddAsync(eventData).ConfigureAwait(false);

        // Assert
        var result = _context.EventDataCollection.AsQueryable().FirstOrDefault(x => x.Id == eventData.Id);
        result.Should().NotBeNull();
        result!.Id.Should().Be(eventData.Id);
        result.AggregateId.Should().Be(eventData.AggregateId);
        result.EventType.Should().Be(eventData.EventType);
        result.Timestamp.ToString("yyyy-MM-dd HH:mm:ss").Should()
            .Be(eventData.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"));
        var parsedCustomerRegistered = (CustomerRegistered) result.Entity!;
        parsedCustomerRegistered.CustomerId.Should().Be(customerRegistered.CustomerId);
        parsedCustomerRegistered.CustomerName.Should().Be(customerRegistered.CustomerName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldGetItemFromDatabase()
    {
        // Arrange
        const string eventType = nameof(CustomerRegistered);
        var id = new Guid("013e15a1-c8fc-461d-988e-d0bac666019c");
        var aggregateId = new Guid("3c8e2dc0-68bd-4a1f-baa9-bbe675d4a63b");
        var customerId = new Guid("3c8e2dc0-68bd-4a1f-baa9-bbe675d4a63b");

        // Act
        var result = await _repository.GetByIdAsync(id).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.AggregateId.Should().Be(aggregateId);
        result.EventType.Should().Be(eventType);
        result.Timestamp.Should().NotBe(default);
        var customerRegistered = (CustomerRegistered) result.Entity!;
        customerRegistered.CustomerId.Should().Be(customerId);
    }

    [Fact]
    public async Task GetByAggregateIdAsync_ShouldGetItemsFromDatabase()
    {
        // Arrange
        var id1 = new Guid("013e15a1-c8fc-461d-988e-d0bac666019c");
        var id2 = new Guid("43640e81-6d4f-4b03-ba09-3c176376bcf2");
        const string eventType1 = nameof(CustomerRegistered);
        const string eventType2 = nameof(CustomerUpdated);
        var aggregateId = new Guid("3c8e2dc0-68bd-4a1f-baa9-bbe675d4a63b");

        // Act
        var result = (await _repository
            .GetByAggregateIdAsync(aggregateId)
            .ConfigureAwait(false)).ToList();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.ElementAt(0).Id.Should().Be(id1);
        result.ElementAt(1).Id.Should().Be(id2);
        result.ElementAt(0).EventType.Should().Be(eventType1);
        result.ElementAt(1).EventType.Should().Be(eventType2);
    }

    [Fact]
    public async Task GetByEventTypeAsync_ShouldGetItemsFromDatabase()
    {
        // Arrange
        var id1 = new Guid("013e15a1-c8fc-461d-988e-d0bac666019c");
        var id2 = new Guid("df9bb37c-546f-4522-9d89-c3cf0731ef2b");
        const string eventType = nameof(CustomerRegistered);

        // Act
        var result = (await _repository
            .GetByEventTypeAsync(eventType)
            .ConfigureAwait(false)).ToList();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.ElementAt(0).Id.Should().Be(id1);
        result.ElementAt(1).Id.Should().Be(id2);
    }

    [Fact]
    public void GetAll_ShouldGetItemsFromQuery()
    {
        // Arrange
        var id1 = new Guid("43640e81-6d4f-4b03-ba09-3c176376bcf2");
        const string eventType = nameof(CustomerUpdated);

        // Act
        var result = _repository
            .GetAll()
            .Where(x => x.EventType == eventType)
            .ToList();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.ElementAt(0).Id.Should().Be(id1);
    }
}