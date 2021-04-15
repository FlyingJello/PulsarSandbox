using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PulsarConnector.Services;

namespace PulsarConnector.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProduceController : ControllerBase
    {
        private readonly ILogger<ProduceController> _logger;
        private readonly IProducerService _producerService;

        public ProduceController(ILogger<ProduceController> logger, IProducerService producerService)
        {
            _logger = logger;
            _producerService = producerService;
        }

        [HttpPost("{persistence}/{tenant}/{ns}/{topic}")]
        public async Task<IActionResult> Produce(string persistence, string tenant, string ns, string topic)
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var payload = await reader.ReadToEndAsync();

            var topicPath = $"{persistence}://{tenant}/{ns}/{topic}";

            _logger.LogInformation("Producing in '{topicPath}'", topicPath);
            _logger.LogTrace(payload);

            await _producerService.ProduceAsync(topicPath, payload);

            return Ok();
        }
    }
}