using ContainerIntegrationTestsDemo.Model;
using Microsoft.EntityFrameworkCore;

namespace ContainerIntegrationTestsDemo.Data;

public class CustomerContext : DbContext
{
    public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
    {
    }

    public CustomerContext(string connectionString) : base(new DbContextOptionsBuilder<CustomerContext>().UseSqlServer(connectionString).Options)
    {
    }
    
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}