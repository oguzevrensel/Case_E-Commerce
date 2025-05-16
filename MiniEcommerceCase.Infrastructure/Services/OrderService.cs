using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<OrderService> _logger;
        public OrderService(AppDbContext context, IMapper mapper, IEventPublisher eventPublisher, IRedisCacheService cacheService, ILogger<OrderService> logger)
        : base(context, mapper, logger)
        {
            _eventPublisher = eventPublisher;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<List<OrderListItemDto>> GetOrdersByUserIdAsync(Guid userId)
        {
            _logger.LogInformation("GetOrdersByUserIdAsync called for UserId: {UserId}", userId);

            string cacheKey = $"orders:user:{userId}";
            var cached = await _cacheService.GetAsync<List<OrderListItemDto>>(cacheKey);

            if (cached is not null)
            {
                _logger.LogInformation("Cache hit for key: {CacheKey}", cacheKey);
                return cached;
            }
            _logger.LogInformation("Cache miss for key: {CacheKey}", cacheKey);


            var orders = await _dbSet.Where(o => o.UserId == userId).ToListAsync();
            var mapped = _mapper.Map<List<OrderListItemDto>>(orders);

            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(2));

            _logger.LogInformation("Cached fresh result for key: {CacheKey}", cacheKey);

            return mapped;
        }

        public async Task<CreateOrderResponseDto> CreateOrderAsync(CreateOrderRequestDto order)
        {

            var response = await CreateAsync(order);

            _logger.LogInformation("Order created with ID: {OrderId}", response.Id);


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
            _logger.LogInformation("Published OrderPlacedEvent for OrderId: {OrderId}", response.Id);


            await _cacheService.RemoveAsync($"orders:user:{order.UserId}");
            _logger.LogInformation("Invalidated cache for userId: {UserId}", order.UserId);


            return response;

        }
    }
}
