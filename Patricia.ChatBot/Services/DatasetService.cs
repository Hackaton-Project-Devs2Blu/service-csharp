
namespace Patricia.ChatBot.Services;
public class DatasetService
{
    private readonly string _folder = Path.Combine("Data");

    public DatasetService()
    {
        if (!Directory.Exists(_folder))
            Directory.CreateDirectory(_folder);
    }

    public async Task AddAsync(string content)
    {
        string file = Path.Combine(_folder, $"{Guid.NewGuid()}.txt");
        await File.WriteAllTextAsync(file, content);
    }

    public async Task<string> CombineAllAsync()
    {
        var files = Directory.GetFiles(_folder, "*.txt");
        var texts = new List<string>();

        foreach (var f in files)
            texts.Add(await File.ReadAllTextAsync(f));

        return string.Join("\n---\n", texts);
    }
}