using MassTransit;

namespace OrderService.Application.Saga;

public class OrderState : SagaStateMachineInstance
{
    public string CurrentState { get; set; } // Stores the current saga state

    public Guid OrderId { get; set; } // The ID of the order being processed
    public bool IsStockReserved { get; set; } // Tracks if stock has been reserved
    public bool IsPaymentProcessed { get; set; } // Tracks if payment has been processed

    public DateTime CreatedAt { get; set; } // Timestamp for when the saga started
    public Guid CorrelationId { get; set; } // Required by MassTransit for Saga correlation
}