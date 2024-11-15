using System.ComponentModel.DataAnnotations;

namespace ContainerIntegrationTestsDemo.Model;

public class Customer
{
    [Key]
    public Guid Id { get; private init; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }

    public Customer(string firstName, string lastName, string email)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
    
    //Empty Constructor for EF
    private Customer(){}
}