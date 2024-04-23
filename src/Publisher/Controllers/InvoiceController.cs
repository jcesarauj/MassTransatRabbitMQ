using Domain;
using Microsoft.AspNetCore.Mvc;
using Publisher.EventBus;

namespace Publisher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly IEventBus _eventBus;

        public InvoiceController(ILogger<InvoiceController> logger, IEventBus eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoiceAsync([FromBody] InvoiceRequestEvent InvoiceRequest)
        {
            await _eventBus.PublishAsync(InvoiceRequest);

            return Ok();
        }
    }
}
