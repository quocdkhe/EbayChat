using EbayChat.Entities;
using Microsoft.EntityFrameworkCore;

namespace EbayChat.Services.ServicesImpl
{
    public class ProductService : IProductService
    {
        private readonly CloneEbayDbContext _context;
        public ProductService(CloneEbayDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
