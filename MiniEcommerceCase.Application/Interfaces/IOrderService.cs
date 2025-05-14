using MiniEcommerceCase.Application.DTOs.Requests.Order;
using MiniEcommerceCase.Application.DTOs.Responses.Order;

namespace MiniEcommerceCase.Application.Interfaces
{
    public interface IOrderService
    {
        Task<CreateOrderResponseDto> CreateOrderAsync(CreateOrderRequestDto request);
        Task<List<OrderListItemDto>> GetOrdersByUserIdAsync(Guid userId);

    }
}
