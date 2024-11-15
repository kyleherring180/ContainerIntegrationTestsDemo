using ContainerIntegrationTestsDemo.Application.Extensions;
using ContainerIntegrationTestsDemo.Data;
using ContainerIntegrationTestsDemo.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<CustomerContext>(options =>
                options.UseSqlServer("connection"))
            .AddApplication()
            .AddDataWithoutContext();

    }).Build();

await host.RunAsync();
