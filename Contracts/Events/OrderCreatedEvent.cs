namespace Contracts.Events;

public record OrderCreatedEvent(Guid OrderId, string Product, int Quantity);