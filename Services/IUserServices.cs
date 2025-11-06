using EbayChat.Entities;
using EbayChat.Models.ViewModel;

namespace EbayChat.Services
{
    public interface IUserServices
    {
        Task<User> GetUserByUsernameAndPassword(String username, String password);
        Task<SellerInfoViewModel?> GetSellerInfoViewModelAsync(int sellerId);
    }
}
