using Xunit;

namespace GSalvi.EventSourcing.MongoDb.Tests.Configurations;

[CollectionDefinition(nameof(TestsFixtureCollection))]
public class TestsFixtureCollection :
    ICollectionFixture<TestHost<CustomEventDataTestStartup>>,
    ICollectionFixture<TestHost<DefaultEventDataTestStartup>>
{
}