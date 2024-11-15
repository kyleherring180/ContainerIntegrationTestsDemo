using ContainerIntegrationTestsDemo.Model;

namespace ContainerIntegrationTestsDemo.Application.Abstraction.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetCustomer(string firstName);
    
    void Add(Customer customer);
    
    Task SaveChanges();
}