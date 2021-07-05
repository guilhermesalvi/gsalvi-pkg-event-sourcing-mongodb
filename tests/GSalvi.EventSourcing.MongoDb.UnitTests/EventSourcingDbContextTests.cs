using System;
using Xunit;

namespace GSalvi.EventSourcing.MongoDb.UnitTests
{
    public class EventSourcingDbContextTests
    {
        [Fact]
        public void Constructor_ShouldThrowsAnException_WhenServicesIsNull()
        {
            // Act - Assert
            Assert.Throws<ArgumentNullException>(() =>
                new EventSourcingDbContext<Snapshot>(default));
        }
    }
}