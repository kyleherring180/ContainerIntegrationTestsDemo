using ContainerIntegrationTestsDemo.QueueConsumer.Consumers;

namespace ContainerIntegrationTestsDemo.QueueConsumer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueueConsumer(this IServiceCollection services)
    {
        return services.AddScoped<CustomerInformationConsumer>();
    }
}