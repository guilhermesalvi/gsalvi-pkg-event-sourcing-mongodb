using MongoDB.Driver;

namespace GSalvi.EventSourcing.MongoDb
{
    /// <summary>
    /// Defines a db context of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEventSourcingDbContext<T> where T : Snapshot
    {
        /// <summary>
        /// Represents the db collection of <typeparamref name="T"/>.
        /// </summary>
        IMongoCollection<T> SnapshotCollection { get; }
    }
}