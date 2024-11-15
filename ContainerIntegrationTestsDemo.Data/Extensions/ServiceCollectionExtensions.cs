using ContainerIntegrationTestsDemo.Application.Abstraction.Repositories;
using ContainerIntegrationTestsDemo.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ContainerIntegrationTestsDemo.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataWithoutContext(this IServiceCollection services)
    {
        return services.AddScoped<ICustomerRepository, CustomerRepository>();
    }
}