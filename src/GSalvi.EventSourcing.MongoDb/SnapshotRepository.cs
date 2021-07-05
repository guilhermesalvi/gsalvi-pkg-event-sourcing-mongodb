using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace GSalvi.EventSourcing.MongoDb
{
    internal class SnapshotRepository : SnapshotRepository<Snapshot>, ISnapshotRepository
    {
        public SnapshotRepository(IEventSourcingDbContext<Snapshot> context) : base(context)
        {
        }
    }

    internal class SnapshotRepository<T> : ISnapshotRepository<T>
        where T : Snapshot
    {
        private readonly IEventSourcingDbContext<T> _context;

        public SnapshotRepository(IEventSourcingDbContext<T> context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task AddAsync(T obj)
        {
            return _context.SnapshotCollection.InsertOneAsync(obj);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.SnapshotCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetByAggregateIdAsync(Guid aggregateId)
        {
            var filter = Builders<T>.Filter.Eq(nameof(Snapshot.AggregateId), aggregateId);
            return await _context.SnapshotCollection
                .Find(filter)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetByEventTypeAsync(string eventType)
        {
            var filter = Builders<T>.Filter.Eq(nameof(Snapshot.EventType), eventType);
            return await _context.SnapshotCollection
                .Find(filter)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public IQueryable<T> GetAll()
        {
            return _context.SnapshotCollection.AsQueryable();
        }
    }
}