namespace EbayChat.Models.DTOs
{
    public class BoxChatDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Avatar { get; set; }
        public string? LastMessage { get; set; }
        public string? Time { get; set; }
        public bool Seen { get; set; }
    }
}
