using ContainerIntegrationTestsDemo.Application.Abstraction.Repositories;
using ContainerIntegrationTestsDemo.Model;
using Microsoft.EntityFrameworkCore;

namespace ContainerIntegrationTestsDemo.Data.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerContext _dbContext;

    public CustomerRepository(CustomerContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Customer?> GetCustomer(string firstName)
    {
        return await _dbContext.Customers.SingleAsync(x => x.FirstName == firstName);
    }

    public void Add(Customer customer)
    {
        _dbContext.Customers.Add(customer);
    }

    public async Task SaveChanges()
    {
        await _dbContext.SaveChangesAsync();
    }
}