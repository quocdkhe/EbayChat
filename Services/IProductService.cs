using EbayChat.Entities;

namespace EbayChat.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
    }
}
