using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GSalvi.EventSourcing.MongoDb.Tests.Configurations;

[ExcludeFromCodeCoverage]
public class TestHost<TStartup> where TStartup : class
{
    public IConfiguration Configuration { get; }
    public IServiceProvider ServiceProvider { get; }

    public TestHost()
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configurations"))
            .AddJsonFile("appsettings.json", true)
            .Build();

        Configuration = configurationBuilder;

        var services = new ServiceCollection();
        services.AddSingleton(Configuration);

        var startup = Activator.CreateInstance<TStartup>();

        startup.GetType().GetMethod("ConfigureServices")!.Invoke(startup, new object[] {services, Configuration});

        ServiceProvider = services.BuildServiceProvider();
    }
}