namespace Contracts.Commands;

public record ProcessPaymentCommand(Guid OrderId, decimal Amount);