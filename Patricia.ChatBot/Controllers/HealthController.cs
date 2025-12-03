using Microsoft.AspNetCore.Mvc;

namespace Patricia.ChatBot.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _db;

    public HealthController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            await _db.Database.CanConnectAsync();

            return Ok(new
            {
                status = "OK",
                database = "Connected",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "ERROR",
                database = "Not Connected",
                error = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }
}