using System.Linq;
using GSalvi.EventSourcing.MongoDb.IntegrationTests.Fakers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GSalvi.EventSourcing.MongoDb.IntegrationTests.Configurations
{
    public class TestStartup
    {
        public void ConfigureServices(
            IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddEventSourcing<MySnapshot>(setupAction =>
            {
                setupAction.WithSnapshotBuilder<MySnapshotBuilder, MySnapshot>();
                setupAction.UseMongoDb<MySnapshot>(configuration);
            });

            var descriptor = services.SingleOrDefault(sd =>
                sd.ServiceType == typeof(IEventSourcingDbContext<MySnapshot>));
            services.Remove(descriptor);

            services.AddScoped<IEventSourcingDbContext<MySnapshot>, FakeEventSourcingDbContext>();
        }
    }
}