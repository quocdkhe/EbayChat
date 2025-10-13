using EbayChat.Services;
using Microsoft.AspNetCore.Mvc;

namespace EbayChat.Controllers
{
    [Route("Product")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index(int? categoryId)
        {
            var products = categoryId.HasValue
                ? _productService.GetProductsByCategory(categoryId.Value).Result
                : _productService.GetAllProducts().Result;

            var categories = _categoryService.GetAllCategories().Result;

            var selectedCategory = categoryId.HasValue
                ? _categoryService.GetCategoryById(categoryId.Value).Result
                : null;

            ViewBag.Categories = categories;
            ViewBag.Products = products;
            ViewBag.SelectedCategory = selectedCategory;

            return View();
        }

        [HttpGet("{id}")]
        public IActionResult Detail(int id)
        {
            var product = _productService.GetProductById(id).Result;
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Product = product;
            return View();
        }
    }
}
