using Microsoft.AspNetCore.Mvc;

namespace HW6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController(ILogger<HealthController> logger) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealth()
        {
            logger.LogInformation($"'{nameof(GetHealth)}' was called");
            return Ok(new { status = "OK" });
        }
    }
}
