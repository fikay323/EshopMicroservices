namespace Ordering.Domain.Models;

public class Customer: Entity<Guid> {
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    public static Customer Create(CustomerId id, string name, string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
    
        return new Customer
        {
            Id = id.Value,
            Name = name,
            Email = email
        };
    }
}