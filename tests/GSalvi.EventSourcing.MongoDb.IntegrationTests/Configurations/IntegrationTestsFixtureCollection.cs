using Xunit;

namespace GSalvi.EventSourcing.MongoDb.IntegrationTests.Configurations
{
    [CollectionDefinition(nameof(IntegrationTestsFixtureCollection))]
    public class IntegrationTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<TestStartup>>
    {
    }
}