using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GSalvi.EventSourcing.MongoDb
{
    internal static class SnapshotMap
    {
        public static void ConfigureMap()
        {
            BsonClassMap.RegisterClassMap<Snapshot>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(x => x.Id).SetSerializer(new GuidSerializer(BsonType.String));
                cm.MapMember(x => x.AggregateId).SetSerializer(new GuidSerializer(BsonType.String));
                cm.MapMember(x => x.Timestamp).SetSerializer(new DateTimeSerializer(BsonType.DateTime));
            });
        }

        public static void ConfigureGenericMap<T>() where T : Snapshot
        {
            BsonClassMap.RegisterClassMap<Snapshot>(cm =>
            {
                cm.AutoMap();
                cm.SetIsRootClass(true);
                cm.MapIdMember(x => x.Id).SetSerializer(new GuidSerializer(BsonType.String));
                cm.MapMember(x => x.AggregateId).SetSerializer(new GuidSerializer(BsonType.String));
            });

            BsonClassMap.RegisterClassMap<T>(cm => cm.AutoMap());
        }
    }
}