using ContainerIntegrationTestsDemo.Application.Abstraction.Repositories;
using ContainerIntegrationTestsDemo.Application.Abstraction.Services;
using ContainerIntegrationTestsDemo.Model;

namespace ContainerIntegrationTestsDemo.Application;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task CustomerEvent(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        
        _customerRepository.Add(customer);
        await _customerRepository.SaveChanges();
    }
}