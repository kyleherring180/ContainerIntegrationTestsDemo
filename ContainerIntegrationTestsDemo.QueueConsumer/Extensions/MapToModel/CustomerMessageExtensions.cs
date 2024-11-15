using ContainerIntegrationTestsDemo.Contracts.Queue;
using ContainerIntegrationTestsDemo.Model;

namespace ContainerIntegrationTestsDemo.QueueConsumer.Extensions.MapToModel;

internal static class CustomerMessageExtensions
{
    public static Customer ToModel(this CustomerMessage value)
    {
        return new Customer(value.FirstName, value.LastName, value.Email);
    }
    
}