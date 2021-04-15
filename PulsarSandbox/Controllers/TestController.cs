using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PulsarSandbox.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("received GET : ok");
            return Ok();
        }
    }
}
