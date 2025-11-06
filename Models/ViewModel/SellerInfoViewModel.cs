    namespace EbayChat.Models.ViewModel
{
    public class SellerInfoViewModel
    {
        public int SellerId { get; set; }
        public string? SellerName { get; set; }
        public double? AverageRate { get; set; }
        public int? TotalReviews { get; set; }
        public double? PositiveRate { get; set; }
        public string? SellerAvatar { get; set; }
        public string? SellerUsername { get; set; }
        public virtual ICollection<ReviewViewModel> Reviews { get; set; } = new List<ReviewViewModel>();
    }
}
