using AutoMapper;
using Azure.Core;
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
        public OrderService(AppDbContext context, IMapper mapper, IEventPublisher eventPublisher)
        : base(context, mapper)
        {
            _eventPublisher = eventPublisher;
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
            var response = await CreateAsync(order);

            var orderEvent = new OrderPlacedEvent
            {
                OrderId = response.OrderId,
                UserId = order.UserId,
                ProductId = order.ProductId,
                Quantity = order.Quantity,
                PaymentMethod = order.PaymentMethod,
                CreatedAt = DateTime.UtcNow
            };

            await _eventPublisher.PublishOrderPlacedAsync(orderEvent);

            return response;

        }
    }
}
