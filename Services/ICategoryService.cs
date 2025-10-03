using EbayChat.Entities;

namespace EbayChat.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategories();
    }
}
