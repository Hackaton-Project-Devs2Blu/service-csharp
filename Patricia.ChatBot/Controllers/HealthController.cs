using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            await _db.Database.ExecuteSqlRawAsync("SELECT 1");

            return Ok(new
            {
                status = "OK",
                database = "connected",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "ERROR",
                database = "disconnected",
                error = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }
}