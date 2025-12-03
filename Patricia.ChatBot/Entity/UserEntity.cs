using System;
namespace Patricia.ChatBot.Entity;

public class UserEntity
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}

