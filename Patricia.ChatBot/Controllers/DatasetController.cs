using Patricia.ChatBot.Services;
using Microsoft.AspNetCore.Mvc;

namespace Patricia.ChatBot.Controllers;

[ApiController]
[Route("dataset")]
public class DatasetController : ControllerBase
{
    private readonly DatasetService _dataset;

    public DatasetController(DatasetService dataset)
    {
        _dataset = dataset;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] DatasetRequest req)
    {
        await _dataset.AddAsync(req.Content);
        return Ok(new { status = "added" });
    }
}

public record DatasetRequest(string Content);
