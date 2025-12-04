using Microsoft.AspNetCore.Mvc;

namespace Patricia.ChatBot.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() =>
        Ok(new
        {
            status = "OK",
            timestamp = DateTime.UtcNow
        });
}