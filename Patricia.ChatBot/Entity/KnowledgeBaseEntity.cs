using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patricia.ChatBot.Entity
{
    [Table("knowledgebase")]
    public class KnowledgeBaseEntity
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("titulo")]
        [MaxLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [Column("pergunta")]
        public string Pergunta { get; set; } = string.Empty;

        [Required]
        [Column("resposta")]
        public string Resposta { get; set; } = string.Empty;

        [Column("categoria")]
        [MaxLength(100)]
        public string? Categoria { get; set; }

        [Required]
        [Column("atualizado_em")]
        public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;

        [Column("atualizado_por")]
        public long? AtualizadoPor { get; set; }

        // FK → users.id
        public UserEntity? AtualizadoPorUsuario { get; set; }
    }
}
