using Ordering.Domain.Enums;

namespace Ordering.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderHandler(IApplicationDbContext dbContext): ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.Order.Id);
        var order = await dbContext.Orders
            .FindAsync([orderId], cancellationToken: cancellationToken);

        if (order is null) throw new OrderNotFoundException(command.Order.Id);

        UpdateOrderWithNewValues(order, command.Order);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateOrderResult(true);    
    }
    
    private static void UpdateOrderWithNewValues(Order order, OrderDto orderDto)
    {
        var orderName = OrderName.Of(orderDto.OrderName);
        var newShippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
        var newBillingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);
        var newPayment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);
        var newStatus = orderDto.Status;
        
        // 1. Check OrderName
        if (!order.OrderName.Equals(orderName)) 
        {
            order.UpdateOrderName(orderName);
        }

        // 2. Check Shipping
        if (!order.ShippingAddress.Equals(newShippingAddress))
        {
            order.UpdateShippingAddress(newShippingAddress);
        }
        
        // 2. Check Billing
        if (!order.BillingAddress.Equals(newBillingAddress))
        {
            order.UpdateBillingAddress(newBillingAddress);
        }

        // 3. Check Payment
        if (!order.Payment.Equals(newPayment))
        {
            order.UpdatePayment(newPayment);
        }
        
        // 4. Check Status
        if (!order.Status.Equals(newStatus))
        {
            order.UpdateStatus(newStatus);
        }
    }
}