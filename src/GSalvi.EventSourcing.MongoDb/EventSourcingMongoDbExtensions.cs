using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GSalvi.EventSourcing.MongoDb
{
    /// <summary>
    /// Define extension methods to registering dependencies.
    /// </summary>
    public static class EventSourcingMongoDbExtensions
    {
        /// <summary>
        /// Add required services to ASP.NET container.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IEventSourcingBuilder UseMongoDb(
            this IEventSourcingBuilder builder,
            IConfiguration configuration)
        {
            return UseMongoDb<Snapshot>(builder, configuration);
        }

        /// <summary>
        /// Add required services to ASP.NET container.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEventSourcingBuilder UseMongoDb<T>(
            this IEventSourcingBuilder builder,
            IConfiguration configuration)
            where T : Snapshot
        {
            if (builder is null) throw new ArgumentNullException(nameof(builder));
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            builder.Services.Configure<EventSourcingDatabaseSettings>(
                configuration.GetSection(nameof(EventSourcingDatabaseSettings)));

            builder.Services.AddSingleton<IEventSourcingDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<EventSourcingDatabaseSettings>>().Value);

            builder.Services.AddScoped<IEventSourcingDbContext<T>, EventSourcingDbContext<T>>();

            if (typeof(T) == typeof(Snapshot))
            {
                builder.Services.AddScoped<ISnapshotRepository, SnapshotRepository>();
                SnapshotMap.ConfigureMap();
            }
            else
            {
                builder.Services.AddScoped<ISnapshotRepository<T>, SnapshotRepository<T>>();
                SnapshotMap.ConfigureGenericMap<T>();
            }

            return builder;
        }
    }
}