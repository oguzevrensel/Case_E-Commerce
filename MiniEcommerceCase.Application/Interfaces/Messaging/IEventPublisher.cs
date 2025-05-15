using MiniEcommerceCase.Application.Events;

namespace MiniEcommerceCase.Application.Interfaces.Messaging
{
    public interface IEventPublisher
    {
        Task PublishOrderPlacedAsync(OrderPlacedEvent orderEvent);
    }
}
