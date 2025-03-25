using Contracts.Events;
using MassTransit;

namespace PaymentService.Infrastructure.MassTransit;

public class StockReservedConsumer : IConsumer<StockReservedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public StockReservedConsumer(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<StockReservedEvent> context)
    {
        Console.WriteLine($"[PaymentService] Processing payment for Order {context.Message.OrderId}");

        var paymentSuccess = new Random().Next(0, 2) == 1;

        if (paymentSuccess)
        {
            Console.WriteLine($"[PaymentService] Payment successful for Order {context.Message.OrderId}");
            await _publishEndpoint.Publish(new PaymentProcessedEvent(context.Message.OrderId));
        }
        else
        {
            Console.WriteLine($"[PaymentService] Payment failed for Order {context.Message.OrderId}");
            await _publishEndpoint.Publish(new PaymentFailedEvent(context.Message.OrderId));
        }
    }
}