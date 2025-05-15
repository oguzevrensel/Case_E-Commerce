using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using MiniEcommerceCase.Application.Events;
using MiniEcommerceCase.Application.Interfaces.Messaging;
using RabbitMQ.Client;

namespace MiniEcommerceCase.Infrastructure.Messaging
{
    public class RabbitMqPublisher : IEventPublisher
    {
        private readonly IConfiguration _configuration;

        public RabbitMqPublisher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task PublishOrderPlacedAsync(OrderPlacedEvent orderEvent)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _configuration["RabbitMq:Host"],
                    UserName = _configuration["RabbitMq:Username"],
                    Password = _configuration["RabbitMq:Password"]
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(
                    queue: "order-placed",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var messageBody = JsonSerializer.Serialize(orderEvent);
                var body = Encoding.UTF8.GetBytes(messageBody);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: "order-placed",
                    basicProperties: null,
                    body: body);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to publish OrderPlacedEvent to RabbitMQ", ex);
            }
        }

    }
}
