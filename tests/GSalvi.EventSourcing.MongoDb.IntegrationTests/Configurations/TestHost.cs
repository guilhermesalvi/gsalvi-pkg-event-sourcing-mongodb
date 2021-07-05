using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GSalvi.EventSourcing.MongoDb.IntegrationTests.Configurations
{
    public class TestHost<T> where T : TestStartup
    {
        public IConfiguration Configuration { get; }
        public IServiceProvider ServiceProvider { get; }

        public TestHost()
        {
            var configurationBuilder =
                new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configurations"))
                    .AddJsonFile("appsettings.json", true)
                    .Build();

            Configuration = configurationBuilder;

            var services = new ServiceCollection();
            services.AddSingleton(Configuration);

            var startup = Activator.CreateInstance<T>();

            startup.ConfigureServices(services, Configuration);

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}