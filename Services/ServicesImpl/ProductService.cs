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
        public async Task<List<Product>> GetProductsByCategory(int categoryId)
        {
            return await _context.Products
                .Where(p => p.categoryId == categoryId)
                .ToListAsync();
        }
        public async Task<Product?> GetProductById(int productId)
        {
            return await _context.Products
                .Include(p => p.category)
                .Include(p => p.seller)
                .FirstOrDefaultAsync(p => p.id == productId);
        }
    }
}
