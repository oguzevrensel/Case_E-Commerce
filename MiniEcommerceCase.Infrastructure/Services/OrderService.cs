using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MiniEcommerceCase.Application.DTOs.Requests.Order;
using MiniEcommerceCase.Application.DTOs.Responses.Order;
using MiniEcommerceCase.Application.Events;
using MiniEcommerceCase.Application.Interfaces;
using MiniEcommerceCase.Application.Interfaces.Messaging;
using MiniEcommerceCase.Domain.Entities;
using MiniEcommerceCase.Infrastructure.Context;

namespace MiniEcommerceCase.Infrastructure.Services
{
    public class OrderService : BaseService<Order, CreateOrderRequestDto, CreateOrderResponseDto>, IOrderService
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IRedisCacheService _cacheService;
        public OrderService(AppDbContext context, IMapper mapper, IEventPublisher eventPublisher, IRedisCacheService cacheService)
        : base(context, mapper)
        {
            _eventPublisher = eventPublisher;
            _cacheService = cacheService;
        }

        public async Task<List<OrderListItemDto>> GetOrdersByUserIdAsync(Guid userId)
        {
            string cacheKey = $"orders:user:{userId}";
            var cached = await _cacheService.GetAsync<List<OrderListItemDto>>(cacheKey);

            if (cached is not null)
                return cached;

            var orders = await _dbSet.Where(o => o.UserId == userId).ToListAsync();
            var mapped = _mapper.Map<List<OrderListItemDto>>(orders);

            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(2)); 
            return mapped;
        }

        public async Task<CreateOrderResponseDto> CreateOrderAsync(CreateOrderRequestDto order)
        {
            var response = await CreateAsync(order);

            var orderEvent = new OrderPlacedEvent
            {
                OrderId = response.Id,
                UserId = order.UserId,
                ProductId = order.ProductId,
                Quantity = order.Quantity,
                PaymentMethod = order.PaymentMethod,
                CreatedAt = DateTime.UtcNow
            };

            await _eventPublisher.PublishOrderPlacedAsync(orderEvent);

            await _cacheService.RemoveAsync($"orders:user:{order.UserId}");

            return response;

        }
    }
}
