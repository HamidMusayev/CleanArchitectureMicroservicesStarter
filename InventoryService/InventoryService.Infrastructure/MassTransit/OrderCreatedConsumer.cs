using Contracts.Events;
using MassTransit;

namespace InventoryService.Infrastructure.MassTransit;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderCreatedConsumer(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        Console.WriteLine($"[InventoryService] Checking stock for Order {context.Message.OrderId}");

        var stockAvailable = new Random().Next(0, 2) == 1;

        if (stockAvailable)
        {
            Console.WriteLine($"[InventoryService] Stock reserved for Order {context.Message.OrderId}");
            await _publishEndpoint.Publish(new StockReservedEvent(context.Message.OrderId));
        }
        else
        {
            Console.WriteLine($"[InventoryService] Stock unavailable for Order {context.Message.OrderId}");
            await _publishEndpoint.Publish(new StockFailedEvent(context.Message.OrderId));
        }
    }
}