namespace Patricia.ChatBot.Dto;

public class UserRequestDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
}

