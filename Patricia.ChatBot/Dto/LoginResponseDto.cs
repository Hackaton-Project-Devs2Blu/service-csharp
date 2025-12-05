namespace Patricia.ChatBot.Dto
{
    public class LoginResponseDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
    }
}
