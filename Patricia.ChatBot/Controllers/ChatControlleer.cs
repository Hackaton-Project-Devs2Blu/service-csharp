using Microsoft.AspNetCore.Mvc;
using Patricia.ChatBot.Services;

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
        string systemPrompt = """
        Você é um assistente especializado que só pode responder usando exclusivamente
        as informações fornecidas no dataset abaixo.
        Regras obrigatórias:
        1. Responda somente com base no dataset.
        2. Se a pergunta do usuário não tiver resposta no dataset, responda:
           "Não encontrei essa informação no dataset."
        3. Não invente, não adivinhe, não use conhecimento externo.
        4. Não faça suposições fora do dataset.
        5. Se o dataset não possuir detalhes suficientes, diga isso.
        Abaixo está o dataset. Use somente ele para responder.
        """;

        string dataset = await _dataset.CombineAllAsync();

        string finalPrompt = $"""
        {systemPrompt}

        [D A T A S E T]
        {dataset}

        [U S U Á R I O]
        {req.Message}
        """;

        string result = await _gemini.GenerateAsync(finalPrompt);

        return Ok(new { response = result });
    }
}

public record ChatRequest(string Message);
