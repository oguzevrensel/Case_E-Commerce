using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MiniEcommerceCase.Application.DTOs.Requests.Order;
using MiniEcommerceCase.Application.DTOs.Responses.Order;
using MiniEcommerceCase.Application.Interfaces;
using MiniEcommerceCase.Domain.Entities;
using MiniEcommerceCase.Infrastructure.Context;

namespace MiniEcommerceCase.Infrastructure.Services
{
    public class OrderService : BaseService<Order, CreateOrderRequestDto, CreateOrderResponseDto>, IOrderService
    {
        public OrderService(AppDbContext context, IMapper mapper)
        : base(context, mapper)
        {
        }

        public async Task<List<OrderListItemDto>> GetOrdersByUserIdAsync(Guid userId)
        {
            var orders = await _dbSet
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return _mapper.Map<List<OrderListItemDto>>(orders);
        }

        public async Task<CreateOrderResponseDto> CreateOrderAsync(CreateOrderRequestDto order)
        {
            return await CreateAsync(order); 
        }
    }
}
