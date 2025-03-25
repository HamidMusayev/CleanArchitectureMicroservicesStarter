namespace OrderService.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public string Product { get; set; }
    public int Quantity { get; set; }
    public string Status { get; set; } = "Pending";
}