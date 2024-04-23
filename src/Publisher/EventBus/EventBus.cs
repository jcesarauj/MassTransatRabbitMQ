
using MassTransit;

namespace Publisher.EventBus
{
    public sealed class EventBus : IEventBus
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EventBus(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
            => _publishEndpoint.Publish<T>(message, cancellationToken);
    }
}
