using Microsoft.AspNetCore.Mvc;
using Patricia.ChatBot.Entity;
using Patricia.ChatBot.Services;
using System.Text.Json;

namespace Patricia.ChatBot.Controllers;

public class PagedResponse<T>
{
    public List<T> Content { get; set; }
    public int Number { get; set; }
    public int Size { get; set; }
    public int TotalElements { get; set; }
    public int TotalPages { get; set; }
}

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

        try
        {
            using var client = new HttpClient();

            var url = "http://hackathon-project-alb-hackathon-1539304958.us-west-2.elb.amazonaws.com/api/java/knowledgebase";

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return StatusCode(500, new { error = "Enfim o java deu problema." });

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var listKnowledge = JsonSerializer.Deserialize<PagedResponse<KnowledgeBaseEntity>>(json, options);

            var dataset = string.Join("\n",
                listKnowledge.Content.Select(k =>
                    $"Id: {k.Id}; Titulo: {k.Titulo}; Pergunta: {k.Pergunta}; Resposta: {k.Resposta}; Categoria: {k.Categoria}"
                )
            );

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
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}

public record ChatRequest(string Message);
