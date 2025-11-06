using EbayChat.Entities;
using EbayChat.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EbayChat.Services.ServicesImpl
{
    public class UserServices : IUserServices
    {
        private readonly CloneEbayDbContext _context;

        public UserServices(CloneEbayDbContext context)
        {
            _context = context;
        }

        public async Task<SellerInfoViewModel?> GetSellerInfoViewModelAsync(int sellerId)
        {
            var seller = await _context.Users
                .Include(s => s.Reviews)
                .FirstOrDefaultAsync(s => s.id == sellerId);
            if (seller == null)
            {
                return null;
            }
            var reviews = await _context.Reviews
                .Include(r => r.product)
                .Include(r => r.reviewer)
                .Where(r => r.product.sellerId == sellerId)
                .OrderByDescending(r => r.createdAt)
                .ToListAsync();
            var sellerInfoViewModel = new SellerInfoViewModel
            {
                SellerId = seller.id,
                SellerName = seller.username,
                TotalReviews = reviews.Count,
                AverageRate = reviews.Count > 0 ? reviews.Average(r => r.rating) : 0.0,
                PositiveRate = reviews.Count > 0 ? (double)reviews.Count(r => r.rating >= 4) / reviews.Count * 100 : 0.0,
                SellerAvatar = seller.avatarURL,
                SellerUsername = seller.username,
                Reviews = reviews.Select(r => new ReviewViewModel
                {
                    ReviewId = r.id,
                    ProductId = r.productId.Value,
                    ReviewerId = r.reviewerId.Value,
                    ReviewerName = r.reviewer != null ? HideReviewerName(r.reviewer.username) : "Unknown",
                    Rating = r.rating.Value,
                    Comment = r.comment,
                    CreatedAt = r.createdAt
                }).ToList()
            };
            return sellerInfoViewModel;
        }

        public async Task<User> GetUserByUsernameAndPassword(string username, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.username == username && u.password == password);
        }
        private string HideReviewerName(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length < 2)
            {
                return name;
            }
            return name[0] + "******" + name[name.Length - 1];
        }
    }
}
