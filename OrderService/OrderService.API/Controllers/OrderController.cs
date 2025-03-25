using Contracts.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTOs;

namespace OrderService.API.Controllers;

[Route("api/orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        if (string.IsNullOrEmpty(request.Product) || request.Quantity <= 0) return BadRequest("Invalid order request.");

        // Generate OrderId and prepare event data
        var orderId = Guid.NewGuid();
        var orderCreatedEvent = new OrderCreatedEvent(orderId, request.Product, request.Quantity);

        // Publish OrderCreated event to initiate saga
        await _publishEndpoint.Publish(orderCreatedEvent);

        return Ok(new { OrderId = orderId, Message = "Order placed successfully." });
    }
}