namespace GSalvi.EventSourcing.MongoDb
{
    /// <summary>
    /// Define settings for access database.
    /// </summary>
    public interface IEventSourcingDatabaseSettings
    {
        /// <summary>
        /// Represents the name of collection.
        /// </summary>
        string SnapshotCollectionName { get; set; }
        
        /// <summary>
        /// Represents the connection string.
        /// </summary>
        string ConnectionString { get; set; }
        
        /// <summary>
        /// Represents the database name.
        /// </summary>
        string DatabaseName { get; set; }
    }
}