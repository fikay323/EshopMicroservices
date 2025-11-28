namespace Ordering.Domain.Models;

public class Order : Aggregate<OrderId>
{
    private readonly List<OrderItem> _orderItems = new List<OrderItem>();
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public CustomerId CustomerId { get; private set; } = null!;
    public OrderName OrderName { get; private set; } = null!;
    public Address ShippingAddress { get; private set; } = null!;
    public Address BillingAddress { get; private set; } = null!;
    public Payment Payment { get; private set; } = null!;
    public OrderStatus Status { get; private set; } = OrderStatus.Draft;
    
    public decimal TotalPrice {
        get => OrderItems.Sum(x => x.Price * x.Quantity);
        private set { }
    }
    
    public static Order Create(OrderId id, CustomerId customerId, OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment)
    {
        var order = new Order
        {
            Id = id,
            CustomerId = customerId,
            OrderName = orderName,
            ShippingAddress = shippingAddress,
            BillingAddress = billingAddress,
            Payment = payment,
            Status = OrderStatus.Pending
        };

        order.AddDomainEvent(new OrderCreatedEvent(order));

        return order;
    }

    public void Add(ProductId productId, int quantity, decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var orderItem = new OrderItem(Id, productId, price, quantity);
        _orderItems.Add(orderItem);
    }

    public void UpdateOrderName(OrderName orderName)
    {
        OrderName = orderName;
        AddOrderUpdatedEvent();
    }

    public void UpdateShippingAddress(Address newAddress)
    {
        if (Status != OrderStatus.Draft && Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException("Cannot change shipping address once order is processed.");
        }

        ShippingAddress = newAddress;
        AddOrderUpdatedEvent();
    }

    public void UpdateBillingAddress(Address newAddress)
    {
        BillingAddress = newAddress;
        AddOrderUpdatedEvent();
    }

    public void UpdatePayment(Payment newPayment)
    {
        Payment = newPayment;
        AddOrderUpdatedEvent();
    }
    public void UpdateStatus(OrderStatus status) 
    {
        Status = status;
        AddOrderUpdatedEvent();
    }

    public void Remove(ProductId productId)
    {
        var orderItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
        if (orderItem is not null)
        {
            _orderItems.Remove(orderItem);
        }
    }
    
    private void AddOrderUpdatedEvent()
    {
        var hasEvent = DomainEvents.OfType<OrderUpdatedEvent>().Any();

        if (!hasEvent)
        {
            AddDomainEvent(new OrderUpdatedEvent(this));
        }
    }
}