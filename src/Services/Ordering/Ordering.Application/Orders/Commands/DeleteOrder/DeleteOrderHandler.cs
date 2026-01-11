namespace Ordering.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderHandler(IApplicationDbContext dbContext): ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var order = new Order { Id = OrderId.Of(command.OrderId)};
        dbContext.Orders.Remove(order);
        var affectedRows = await dbContext.SaveChangesAsync(cancellationToken);
        
        Console.WriteLine("affected: {affectedRows}", affectedRows);

        return affectedRows == 0 
            ? throw new OrderNotFoundException(command.OrderId)
            : new DeleteOrderResult(true);
    }
}