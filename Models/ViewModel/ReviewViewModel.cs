namespace EbayChat.Models.ViewModel
{
    public class ReviewViewModel
    {
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public int ProductId { get; set; }
        public int ReviewerId { get; set; }
        public string? ReviewerName { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
