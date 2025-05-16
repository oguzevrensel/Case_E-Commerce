using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniEcommerceCase.Application.DTOs.Requests.Order;
using MiniEcommerceCase.Application.Interfaces.Messaging;
using MiniEcommerceCase.Application.Interfaces;
using MiniEcommerceCase.Infrastructure.Context;
using MiniEcommerceCase.Infrastructure.Services;
using Moq;
using MiniEcommerceCase.Application.DTOs.Responses.Order;
using MiniEcommerceCase.Domain.Entities;
using MiniEcommerceCase.Domain.Enums;

namespace MiniEcommerceCase.Test
{
    public class OrderServiceTests
    {
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IEventPublisher> publisherMock;
        private readonly Mock<IRedisCacheService> cacheMock;
        private readonly Mock<ILogger<OrderService>> loggerMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            mapperMock = new Mock<IMapper>();
            publisherMock = new Mock<IEventPublisher>();
            cacheMock = new Mock<IRedisCacheService>();
            loggerMock = new Mock<ILogger<OrderService>>();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderTestDb")
                .Options;

            var context = new AppDbContext(options);

            _orderService = new OrderService(context, mapperMock.Object, publisherMock.Object, cacheMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task CreateOrderAsync_Should_Create_Order()
        {
            // Arrange
            var request = new CreateOrderRequestDto
            {
                UserId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 1,
                PaymentMethod = "CreditCard"
            };

            mapperMock.Setup(x => x.Map<Order>(It.IsAny<CreateOrderRequestDto>()))
                .Returns((CreateOrderRequestDto dto) => new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = dto.UserId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    PaymentMethod = Enum.Parse<PaymentMethod>(dto.PaymentMethod),
                    Status = OrderStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                });

            mapperMock.Setup(x => x.Map<CreateOrderResponseDto>(It.IsAny<Order>()))
                .Returns((Order order) => new CreateOrderResponseDto
                {
                    Id = order.Id,
                    Status = order.Status.ToString()
                });

            // Act
            var result = await _orderService.CreateOrderAsync(request);

            // Assert
            Assert.NotNull(result);

        }
    }
}