using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;

namespace GSalvi.EventSourcing.MongoDb;

/// <summary>
/// Defines extension methods to registering dependencies and middlewares
/// </summary>
[ExcludeFromCodeCoverage]
public static class EventSourcingMongoDbExtensions
{
    /// <summary>
    /// Adds required services to ASP.NET container to use event sourcing with MongoDb
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static EventSourcingExtensionsBuilder<EventData> UseMongoDb(
        this EventSourcingExtensionsBuilder<EventData> builder,
        IConfiguration configuration)
    {
        return UseMongoDb<EventData>(builder, configuration);
    }

    /// <summary>
    /// Adds required services to ASP.NET container to use event sourcing with MongoDb
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="classMapInitializer"></param>
    /// <typeparam name="TEventData"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static EventSourcingExtensionsBuilder<TEventData> UseMongoDb<TEventData>(
        this EventSourcingExtensionsBuilder<TEventData> builder,
        IConfiguration configuration,
        Action<BsonClassMap<TEventData>>? classMapInitializer = null)
        where TEventData : EventData
    {
        if (builder is null) throw new ArgumentNullException(nameof(builder));
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));

        builder.Services.Configure<EventSourcingDbSettings>(
            configuration.GetSection(nameof(EventSourcingDbSettings)));

        builder.Services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<EventSourcingDbSettings>>().Value);

        builder.Services.AddScoped<EventSourcingDbContext<TEventData>>();
        builder.Services.AddScoped<IEventDataRepository<TEventData>, EventDataRepository<TEventData>>();

        if (typeof(TEventData) == typeof(EventData))
            EventDataMap.ConfigureMap();
        else
            EventDataMap.ConfigureGenericMap(classMapInitializer);

        return builder;
    }
}