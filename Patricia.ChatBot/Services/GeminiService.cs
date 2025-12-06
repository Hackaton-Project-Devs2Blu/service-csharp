using System.Text;
using System.Text.Json;

namespace Patricia.ChatBot.Services;

public class GeminiService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public GeminiService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["GEMINI_API_KEY"] ?? throw new Exception("GEMINI_API_KEY não configurada.");
    }

    public async Task<string> GenerateAsync(string prompt)
    {
        var url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";

        var body = new
        {
            contents = new[]
            {
                new {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(body);

        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("x-goog-api-key", _apiKey);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<GeminiResponse>();
        return data?.candidates?[0].content.parts[0].text ?? "";
    }
}

public class GeminiResponse
{
    public Candidate[] candidates { get; set; }
}

public class Candidate
{
    public Content content { get; set; }
}

public class Content
{
    public Part[] parts { get; set; }
}

public class Part
{
    public string text { get; set; }
}
