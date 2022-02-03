using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GSalvi.EventSourcing.MongoDb;

[ExcludeFromCodeCoverage]
internal static class EventDataMap
{
    public static void ConfigureMap()
    {
        EventSourcingBsonClassMapper.Register<EventData>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(x => x.Id).SetSerializer(new GuidSerializer(BsonType.String));
            cm.MapMember(x => x.AggregateId).SetSerializer(new GuidSerializer(BsonType.String));
            cm.MapMember(x => x.Timestamp).SetSerializer(new DateTimeSerializer(BsonType.DateTime));
        });
    }

    public static void ConfigureGenericMap<TEventData>(Action<BsonClassMap<TEventData>>? classMapInitializer = null)
        where TEventData : EventData
    {
        EventSourcingBsonClassMapper.Register<EventData>(cm =>
        {
            cm.AutoMap();
            cm.SetIsRootClass(true);
            cm.MapIdMember(x => x.Id).SetSerializer(new GuidSerializer(BsonType.String));
            cm.MapMember(x => x.AggregateId).SetSerializer(new GuidSerializer(BsonType.String));
            cm.MapMember(x => x.Timestamp).SetSerializer(DateTimeSerializer.LocalInstance);
        });

        if (classMapInitializer is null)
            BsonClassMap.RegisterClassMap<TEventData>(cm => cm.AutoMap());
        else
            BsonClassMap.RegisterClassMap(classMapInitializer);
    }
}