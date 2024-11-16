using ContainerIntegrationTestsDemo.Data;
using ContainerIntegrationTestsDemo.IntegrationTests.Mocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ContainerIntegrationTestsDemo.IntegrationTests.Helpers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMsSqlTestContainer(this IServiceCollection services, string connectioString)
    {
        return services.AddDbContext<CustomerContext>(options =>
                options.UseSqlServer(connectioString))
            .AddScoped<TestDataRepository>();
    }
}