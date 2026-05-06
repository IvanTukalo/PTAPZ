namespace Lab4.DTOs
{
    public class CreateOrderDto { public int UserId { get; set; } public int BookId { get; set; } public int DurationDays { get; set; } }
    public class UpdateOrderStatusDto { public string Status { get; set; } }
}