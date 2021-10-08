using System;
using MongoDB.Bson.Serialization;

namespace GSalvi.EventSourcing.MongoDb
{
    internal sealed class BsonClassMapper
    {
        private static BsonClassMapper _instance;

        private static readonly object Lock = new();

        public static BsonClassMapper Instance => _instance ??= new BsonClassMapper();

        public BsonClassMapper Register<T>(Action<BsonClassMap<T>> classMapInitializer)
        {
            lock (Lock)
            {
                if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
                {
                    BsonClassMap.RegisterClassMap<T>(classMapInitializer);
                }
            }

            return this;
        }
    }
}