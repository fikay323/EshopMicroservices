namespace Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderHandler(IApplicationDbContext dbContext): ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = CreateOrder(command.Order);
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateOrderResult(order.Id.Value);
    }

    private static Order CreateOrder(OrderDto orderDto)
    {
        var orderDtoShippingAddress = orderDto.ShippingAddress;
        var orderDtoBillingAddress = orderDto.BillingAddress;
        var orderDtoPayment = orderDto.Payment;

        var shippingAddress = Address.Of(orderDtoShippingAddress.FirstName, orderDtoShippingAddress.LastName, orderDtoShippingAddress.EmailAddress, orderDtoShippingAddress.AddressLine, orderDtoShippingAddress.Country, orderDtoShippingAddress.State, orderDtoShippingAddress.ZipCode);
        var billingAddress = Address.Of(orderDtoBillingAddress.FirstName, orderDtoBillingAddress.LastName, orderDtoBillingAddress.EmailAddress, orderDtoBillingAddress.AddressLine, orderDtoBillingAddress.Country, orderDtoBillingAddress.State, orderDtoBillingAddress.ZipCode);
        var payment = Payment.Of(orderDtoPayment.CardName, orderDtoPayment.CardNumber, orderDtoPayment.Expiration, orderDtoPayment.Cvv, orderDtoPayment.PaymentMethod);
        var orderId = OrderId.Of(Guid.NewGuid());
        var customerId = CustomerId.Of(orderDto.CustomerId);
        var orderName = OrderName.Of(orderDto.OrderName);

        var newOrder = Order.Create(orderId, customerId, orderName, shippingAddress, billingAddress, payment);
        foreach (var orderItemDto in orderDto.OrderItems)
        {
            var newProductId = ProductId.Of(orderItemDto.ProductId);
            newOrder.Add(newProductId, orderItemDto.Quantity, orderItemDto.Price);
        }

        return newOrder;
    }
}