using EbayChat.Entities;
using EbayChat.Models.ViewModel;

namespace EbayChat.Services
{
    public interface IReviewService
    {
        Task<bool> CanUserReviewProductAsync(int userId, int productId);
        Task<Review> AddReviewAsync(Review review);
        Task<List<ReviewViewModel>> GetReviewViewModelsByProductAsync(int productId);
    }
}
