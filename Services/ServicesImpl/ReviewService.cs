using EbayChat.Entities;
using EbayChat.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace EbayChat.Services.ServicesImpl
{
    public class ReviewService : IReviewService
    {
        private readonly CloneEbayDbContext _context;
        public ReviewService(CloneEbayDbContext context)
        {
            _context = context;
        }
        // Kiểm tra user đã từng mua product hay chưa
        public async Task<bool> CanUserReviewProductAsync(int userId, int productId)
        {
            if (userId <= 0 || productId <= 0)
                return false;

            // 1️⃣ Kiểm tra user đã từng mua product này chưa
            var hasPurchased = await _context.OrderTables
                .Include(o => o.OrderItems)
                .Where(o => o.buyerId == userId)
                .AnyAsync(o => o.OrderItems.Any(oi => oi.productId == productId));

            return !hasPurchased;
        }

        public async Task<Review> AddReviewAsync(Review review)
        {
            if (review == null) throw new ArgumentNullException(nameof(review));

            review.createdAt = review.createdAt ?? DateTime.UtcNow;

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            // eager load reviewer to return complete object (optional)
            await _context.Entry(review).Reference(r => r.reviewer).LoadAsync();
            return review;
        }

        public async Task<List<ReviewViewModel>> GetReviewViewModelsByProductAsync(int productId)
        {
            var reviews = await _context.Reviews
                .Include(r => r.reviewer)
                .Where(r => r.productId == productId)
                .OrderByDescending(r => r.createdAt)
                .ToListAsync();
            var reviewViewModels = reviews.Select(r => new ReviewViewModel
            {
                ReviewId = r.id,
                ProductId = r.productId.Value,
                ReviewerId = r.reviewerId.Value,
                ReviewerName = r.reviewer != null ? HideReviewerName(r.reviewer.username) : "Unknown",
                Rating = r.rating.Value,
                Comment = r.comment,
                CreatedAt = r.createdAt
            }).ToList();
            return reviewViewModels;
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
