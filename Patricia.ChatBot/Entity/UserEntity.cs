using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Patricia.ChatBot.Entity;

[Table("users")]
public class UserEntity
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("nome")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [Column("email")]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column("senha_hash")]
    public string SenhaHash { get; set; } = string.Empty;

    [Required]
    [Column("is_admin")]
    public bool IsAdmin { get; set; } = false;

    [Required]
    [Column("criado_em")]
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    // Relacionamento 1:N com KnowledgeBase
    public List<KnowledgeBaseEntity>? KnowledgeBaseAtualizados { get; set; }
}

