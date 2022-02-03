using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using AutoFixture;
using GSalvi.EventSourcing.MongoDb.Tests.Configurations.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32.SafeHandles;
using Mongo2Go;
using MongoDB.Driver;

namespace GSalvi.EventSourcing.MongoDb.Tests.Configurations;

[ExcludeFromCodeCoverage]
public class CustomEventDataTestStartup : IDisposable
{
    private MongoDbRunner? _runner;

    private const string EventSourcingMongoConnectionStringKey = "EventSourcingDbSettings:ConnectionString";

    private bool _disposedValue;
    private readonly SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

    public void ConfigureServices(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddEventSourcing<CustomEventData>(setup =>
        {
            setup.WithEventDataBuilder<CustomEventDataBuilder>();
            setup.UseMongoDb(configuration);
        });

        _runner = MongoDbRunner.Start();
        configuration.GetSection(EventSourcingMongoConnectionStringKey).Value = _runner.ConnectionString;
        InitializeDatabase(_runner.ConnectionString);
    }

    private static void InitializeDatabase(string connectionString)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("eventSourcing");
        var collection = database.GetCollection<CustomEventData>("eventData");

        var fixture = new Fixture();

        var eventData1 = fixture
            .Build<CustomEventData>()
            .With(x => x.Id, new Guid("013e15a1-c8fc-461d-988e-d0bac666019c"))
            .With(x => x.AggregateId, new Guid("3c8e2dc0-68bd-4a1f-baa9-bbe675d4a63b"))
            .With(x => x.EventType, nameof(CustomerRegistered))
            .With(x => x.Timestamp, DateTime.UtcNow)
            .With(x => x.UserId, new Guid("e872aa96-2c62-4a99-9bb5-fcdcd0fb093e"))
            .With(x => x.Entity,
                fixture.Build<CustomerRegistered>()
                    .With(x => x.CustomerId, new Guid("3c8e2dc0-68bd-4a1f-baa9-bbe675d4a63b"))
                    .Create())
            .Create();
        var eventData2 = fixture
            .Build<CustomEventData>()
            .With(x => x.Id, new Guid("43640e81-6d4f-4b03-ba09-3c176376bcf2"))
            .With(x => x.AggregateId, new Guid("3c8e2dc0-68bd-4a1f-baa9-bbe675d4a63b"))
            .With(x => x.EventType, nameof(CustomerUpdated))
            .With(x => x.Timestamp, DateTime.UtcNow)
            .With(x => x.UserId, new Guid("e872aa96-2c62-4a99-9bb5-fcdcd0fb093e"))
            .With(x => x.Entity,
                fixture.Build<CustomerRegistered>()
                    .With(x => x.CustomerId, new Guid("3c8e2dc0-68bd-4a1f-baa9-bbe675d4a63b"))
                    .Create())
            .Create();
        var eventData3 = fixture
            .Build<CustomEventData>()
            .With(x => x.Id, new Guid("df9bb37c-546f-4522-9d89-c3cf0731ef2b"))
            .With(x => x.AggregateId, new Guid("4e07fc74-77ec-4a17-bf33-239090abefa3"))
            .With(x => x.EventType, nameof(CustomerRegistered))
            .With(x => x.Timestamp, DateTime.UtcNow)
            .With(x => x.UserId, new Guid("e872aa96-2c62-4a99-9bb5-fcdcd0fb093e"))
            .With(x => x.Entity,
                fixture.Build<CustomerRegistered>()
                    .With(x => x.CustomerId, new Guid("4e07fc74-77ec-4a17-bf33-239090abefa3"))
                    .Create())
            .Create();
        collection.InsertMany(new[] {eventData1, eventData2, eventData3});
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue) return;

        if (disposing)
        {
            _safeHandle.Dispose();
            _runner?.Dispose();
        }

        _disposedValue = true;
    }
}