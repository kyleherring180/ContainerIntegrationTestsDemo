using ContainerIntegrationTestsDemo.Application.Abstraction.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ContainerIntegrationTestsDemo.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddScoped<ICustomerService, CustomerService>();
    }
    
}