namespace MiniEcommerceCase.Application.DTOs.Requests.Order
{
    public class CreateOrderRequestDto
    {

        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public string PaymentMethod { get; set; }
    }
}
