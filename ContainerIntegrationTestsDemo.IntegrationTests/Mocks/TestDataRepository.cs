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

    public async Task ClearDatabase()
    {
        foreach (var entityType in dbContext.Model.GetEntityTypes().Where(x => !x.IsOwned()))
        {
            var clrType = entityType.ClrType;
            
            //Use reflection to call the generic Set<TEntity>() method on DbContext
            var method = typeof(DbContext)
                .GetMethods()
                .First(m => m.Name == "Set" && m.IsGenericMethod && m.GetParameters().Length == 0)
                .MakeGenericMethod(clrType);

            var dbSet = method.Invoke(dbContext, null) as IQueryable;

            if (dbSet != null)
            {
                //dbSet is now an IQueryable which can be used to get all the entities.
                var entities = dbSet.Cast<object>().ToList();
                
                //Use RemoveRange to remove all entities of this type
                dbContext.RemoveRange(entities);
            }
        }

        await dbContext.SaveChangesAsync();
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