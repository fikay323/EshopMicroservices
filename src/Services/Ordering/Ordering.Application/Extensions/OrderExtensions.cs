namespace Ordering.Application.Extensions;

public static class OrderExtensions
{
    public static IEnumerable<OrderDto> ToOrderDtoList(this IEnumerable<Order> orders)
    {
        return orders.Select(o => o.ToOrderDto());
    }

    private static OrderDto ToOrderDto(this Order order)
    {
        var orderShippingAddress = order.ShippingAddress;
        var orderBillingAddress = order.BillingAddress;
        var orderPayment = order.Payment;

        var shippingAddress = new AddressDto(orderShippingAddress.FirstName, orderShippingAddress.LastName, orderShippingAddress.EmailAddress!, orderShippingAddress.AddressLine, orderShippingAddress.Country, orderShippingAddress.State, orderShippingAddress.ZipCode);
        var billingAddress = new AddressDto(orderBillingAddress.FirstName, orderBillingAddress.LastName, orderBillingAddress.EmailAddress!, orderBillingAddress.AddressLine, orderBillingAddress.Country, orderBillingAddress.State, orderBillingAddress.ZipCode);
        var payment = new PaymentDto(orderPayment.CardName!, orderPayment.CardNumber, orderPayment.Expiration, orderPayment.CVV, orderPayment.PaymentMethod);
        var orderId = order.Id.Value;
        var customerId = order.CustomerId.Value;
        var orderName = order.OrderName.Value;
        var status = order.Status;

        var orderItemsDto = order.OrderItems.ToOrderItemsDto();

        return new OrderDto(orderId, customerId, orderName, shippingAddress, billingAddress,payment, status, orderItemsDto);
    }

    private static List<OrderItemDto> ToOrderItemsDto(this IEnumerable<OrderItem> orderItems)
    {
        return orderItems.Select(orderItem => new OrderItemDto(orderItem.OrderId.Value, orderItem.ProductId.Value, orderItem.Quantity, orderItem.Price)).ToList();
    }
}
