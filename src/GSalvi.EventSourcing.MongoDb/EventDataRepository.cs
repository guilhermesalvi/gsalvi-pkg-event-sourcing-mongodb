using MongoDB.Driver;

namespace GSalvi.EventSourcing.MongoDb;

internal class EventDataRepository<TEventData> : IEventDataRepository<TEventData>
    where TEventData : EventData
{
    private readonly EventSourcingDbContext<TEventData> _context;

    public EventDataRepository(EventSourcingDbContext<TEventData> context)
    {
        _context = context;
    }

    public Task AddAsync(TEventData eventData) => _context.EventDataCollection.InsertOneAsync(eventData);

    public Task<TEventData> GetByIdAsync(Guid id) => _context.EventDataCollection
        .Find(x => x.Id == id)
        .FirstOrDefaultAsync();

    public async Task<IEnumerable<TEventData>> GetByAggregateIdAsync(Guid aggregateId)
    {
        var filter = Builders<TEventData>.Filter.Eq(nameof(EventData.AggregateId), aggregateId);
        return await _context.EventDataCollection
            .Find(filter)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<TEventData>> GetByEventTypeAsync(string eventType)
    {
        var filter = Builders<TEventData>.Filter.Eq(nameof(EventData.EventType), eventType);
        return await _context.EventDataCollection
            .Find(filter)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public IQueryable<TEventData> GetAll() => _context.EventDataCollection.AsQueryable();
}