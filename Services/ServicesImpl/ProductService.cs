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
            return await _context.Products
                .Include(p => p.category)
                .Include(p => p.seller)
                .Include(p => p.Reviews)
                .Include(p => p.OrderItems)
                .Include(p => p.category)
                .ToListAsync();
        }
        public async Task<List<Product>> GetProductsByCategory(int categoryId)
        {
            return await _context.Products
                .Include(p => p.category)
                .Include(p => p.seller)
                .Include(p => p.Reviews)
                .Include(p => p.OrderItems)
                .Include(p => p.category)
                .Where(p => p.categoryId == categoryId)
                .ToListAsync();
        }
        public async Task<Product?> GetProductById(int productId)
        {
            return await _context.Products
                .Include(p => p.category)
                .Include(p => p.seller)
                .Include(p => p.Reviews)
                .Include(p => p.OrderItems)
                .Include(p => p.category)
                .FirstOrDefaultAsync(p => p.id == productId);
        }
    }
}
