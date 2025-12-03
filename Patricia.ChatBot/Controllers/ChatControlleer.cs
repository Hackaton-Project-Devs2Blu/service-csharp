using Microsoft.AspNetCore.Mvc;
using Patricia.ChatBot.Services;
using Microsoft.AspNetCore.Mvc;

namespace Patricia.ChatBot.Controllers;
    

[ApiController]
[Route("chat")]
public class ChatController : ControllerBase
{
    private readonly GeminiService _gemini;
    private readonly DatasetService _dataset;

    public ChatController(GeminiService gemini, DatasetService dataset)
    {
        _gemini = gemini;
        _dataset = dataset;
    }

    [HttpPost]
    public async Task<IActionResult> Chat([FromBody] ChatRequest req)
    {
        // Junta dataset
        string context = await _dataset.CombineAllAsync();

        string finalPrompt =
            $"Contexto adicional:\n{context}\n\n" +
            $"Conversa do usuário:\n{req.Message}";

        string result = await _gemini.GenerateAsync(finalPrompt);

        return Ok(new { response = result });
    }
}

public record ChatRequest(string Message);
