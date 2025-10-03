using EbayChat.Entities;
using Microsoft.EntityFrameworkCore;

namespace EbayChat.Services.ServicesImpl
{
    public class CategoryService : ICategoryService
    {
        private readonly CloneEbayDbContext _context;
        public CategoryService(CloneEbayDbContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
