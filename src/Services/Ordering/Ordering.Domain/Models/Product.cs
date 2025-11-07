namespace Ordering.Domain.Models;

public class Product : Entity<string>
{
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; } = 0;

    public static Product Create(string id, string name, decimal price)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var product = new Product
        {
            Id = id,
            Name = name,
            Price = price
        };

        return product;
    }
}