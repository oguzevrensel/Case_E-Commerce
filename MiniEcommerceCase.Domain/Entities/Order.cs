using MiniEcommerceCase.Domain.Common;
using MiniEcommerceCase.Domain.Enums;

namespace MiniEcommerceCase.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }
}
