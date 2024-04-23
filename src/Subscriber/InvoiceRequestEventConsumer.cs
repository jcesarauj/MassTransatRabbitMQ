using Domain;
using MassTransit;

namespace Subscriber
{
    public sealed class InvoiceRequestEventConsumer : IConsumer<InvoiceRequestEvent>
    {
        private readonly ILogger<InvoiceRequestEventConsumer> _logger;

        public InvoiceRequestEventConsumer(ILogger<InvoiceRequestEventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<InvoiceRequestEvent> context)
        {
            if (context.Message.OperatorId == "Retry") throw new Exception("Testing retry ...");

            return Task.CompletedTask;
        }
    }
}
