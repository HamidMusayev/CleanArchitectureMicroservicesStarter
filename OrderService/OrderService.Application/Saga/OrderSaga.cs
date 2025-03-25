using Contracts.Events;
using MassTransit;

namespace OrderService.Application.Saga;

public class OrderSaga : MassTransitStateMachine<OrderState>
{
    public OrderSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderCreated, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => StockReserved, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => PaymentProcessed, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => StockFailed, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => PaymentFailed, x => x.CorrelateById(m => m.Message.OrderId));

        Initially(
            When(OrderCreated)
                .Then(context =>
                {
                    Console.WriteLine($"[Saga] OrderCreated received. OrderId: {context.Data.OrderId}");
                })
                .TransitionTo(AwaitingStock));

        During(AwaitingStock,
            When(StockReserved)
                .TransitionTo(AwaitingPayment),
            When(StockFailed)
                .ThenAsync(context => context.Publish(new OrderCancelledEvent(context.Data.OrderId)))
                .Finalize());

        During(AwaitingPayment,
            When(PaymentProcessed)
                .Then(context => { Console.WriteLine($"[Saga] Order confirmed. OrderId: {context.Data.OrderId}"); })
                .Finalize(),
            When(PaymentFailed)
                .ThenAsync(context => context.Publish(new OrderCancelledEvent(context.Data.OrderId)))
                .Finalize());
    }

    public State AwaitingStock { get; private set; }
    public State AwaitingPayment { get; private set; }

    public Event<OrderCreatedEvent> OrderCreated { get; private set; }
    public Event<StockReservedEvent> StockReserved { get; private set; }
    public Event<PaymentProcessedEvent> PaymentProcessed { get; private set; }
    public Event<StockFailedEvent> StockFailed { get; private set; }
    public Event<PaymentFailedEvent> PaymentFailed { get; private set; }
}