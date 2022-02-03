using MongoDB.Bson.Serialization;

namespace GSalvi.EventSourcing.MongoDb;

/// <summary>
/// Defines mappings to event sourcing classes
/// </summary>
public static class EventSourcingBsonClassMapper
{
    private static readonly object Lock = new();

    /// <summary>
    /// Registers a mapping to <typeparamref name="T"/>
    /// </summary>
    /// <param name="classMapInitializer"></param>
    /// <typeparam name="T"></typeparam>
    public static void Register<T>(Action<BsonClassMap<T>> classMapInitializer)
    {
        lock (Lock)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap.RegisterClassMap(classMapInitializer);
            }
        }
    }
}