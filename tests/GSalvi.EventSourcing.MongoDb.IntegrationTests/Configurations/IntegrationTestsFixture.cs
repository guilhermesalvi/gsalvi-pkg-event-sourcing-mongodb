using System;

namespace GSalvi.EventSourcing.MongoDb.IntegrationTests.Configurations
{
    public class IntegrationTestsFixture<TStartup> : IDisposable
        where TStartup : TestStartup
    {
        public readonly TestHost<TStartup> TestHost;

        public IntegrationTestsFixture()
        {
            TestHost = new TestHost<TStartup>();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}