using System;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace GSalvi.EventSourcing.MongoDb.UnitTests
{
    public class EventSourcingMongoDbExtensionsTests
    {
        private readonly Mock<IEventSourcingBuilder> _builder;
        private readonly Mock<IConfiguration> _configuration;

        public EventSourcingMongoDbExtensionsTests()
        {
            _builder = new Mock<IEventSourcingBuilder>();
            _configuration = new Mock<IConfiguration>();
        }

        [Fact]
        public void Constructor_ShouldThrowsAnException_WhenBuilderIsNull()
        {
            // Act - Assert
            Assert.Throws<ArgumentNullException>(() =>
                EventSourcingMongoDbExtensions.UseMongoDb<Snapshot>(default, _configuration.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowsAnException_WhenConfigurationIsNull()
        {
            // Act - Assert
            Assert.Throws<ArgumentNullException>(() =>
                _builder.Object.UseMongoDb<Snapshot>(default));
        }
    }
}