using System.Net.Http.Json;
using System.Text.Json;

namespace Patricia.ChatBot.Services;

public class GeminiService
{
    private readonly HttpClient _http = new();
    private readonly string _apiKey;

    public GeminiService(IConfiguration config)
    {
        _apiKey = "AIzaSyDEBPk6JCSsggSGBLzhcSxSL7UwpUxt9ms"; //  config["GEMINI_API_KEY"]
            // ?? throw new Exception("GEMINI_API_KEY não configurada.");
    }

    public async Task<string> GenerateAsync(string prompt)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey);

        var url =
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";

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
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await _http.PostAsync(url, content);
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
