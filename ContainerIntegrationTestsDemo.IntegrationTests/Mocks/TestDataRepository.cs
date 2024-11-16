using ContainerIntegrationTestsDemo.Data;
using ContainerIntegrationTestsDemo.Model;
using Microsoft.EntityFrameworkCore;

namespace ContainerIntegrationTestsDemo.IntegrationTests.Mocks;

public class TestDataRepository(CustomerContext dbContext)
{
    public async Task SetupDatabase()
    {
        await dbContext.Database.MigrateAsync();
    }

    public async Task DeleteDb()
    {
        await dbContext.Database.CloseConnectionAsync();
        await dbContext.Database.EnsureDeletedAsync();
    }

    public string? GetCurrentConnectionString()
    {
        return dbContext.Database.GetConnectionString();
    }

    public Customer? GetCustomer(string firstName)
    {
        return dbContext.Customers.SingleOrDefault(x => x.FirstName == firstName);
    }
}