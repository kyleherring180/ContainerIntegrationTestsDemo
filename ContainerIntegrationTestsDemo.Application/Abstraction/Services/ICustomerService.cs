using ContainerIntegrationTestsDemo.Model;

namespace ContainerIntegrationTestsDemo.Application.Abstraction.Services;

public interface ICustomerService
{
    Task CustomerEvent(Customer customer);
}