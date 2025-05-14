namespace MiniEcommerceCase.Application.DTOs.Responses.Order
{
    public class OrderListItemDto
    {
        public Guid OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
