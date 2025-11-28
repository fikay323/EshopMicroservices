namespace Ordering.Application.Exceptions;

public class OrderNotFoundException : Exception
{
    public OrderNotFoundException(Guid id) : base($"Order with id {id} was not found")
    {
    }
}
