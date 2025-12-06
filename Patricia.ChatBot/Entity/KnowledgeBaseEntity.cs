namespace Patricia.ChatBot.Entity
{
    public class KnowledgeBaseEntity
    {
        public long Id { get; set; }
        public string Titulo { get; set; }
        public string Pergunta { get; set; }
        public string Resposta { get; set; }
        public string Categoria { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedByName { get; set; }
    }
}
