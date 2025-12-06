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
        Você é a Patricia, uma assistente virtual especializada em auxiliar servidores públicos com dúvidas administrativas e legais.
        Sua missão é fornecer respostas precisas, claras e diretas.

        ### REGRAS DE CONHECIMENTO:
        1. *Fonte Primária:* Use EXCLUSIVAMENTE as informações fornecidas no [DATASET] abaixo para responder.
        2. *Fonte Secundária:* Se, e SOMENTE SE, a informação não estiver no dataset, você pode usar seu conhecimento geral para ajudar, mas deve adicionar o seguinte aviso no início da resposta:
            "⚠️ Nota: Esta informação não foi encontrada na minha base de dados oficial. A resposta abaixo baseia-se em conhecimento geral e pode não refletir as normativas internas mais recentes."

        ### DIRETRIZES DE ESTILO:
        - Seja empática, mas profissional.
        - Use linguagem acessível (evite "juridiquês" desnecessário).
        - Se a pergunta for ambígua, peça esclarecimentos.

        ### DATASET:
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
