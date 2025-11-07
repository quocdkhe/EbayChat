using EbayChat.Services;
using Microsoft.AspNetCore.Mvc;

namespace EbayChat.Controllers
{
    [Route("Product")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IUserServices _userService;
        private readonly IReviewService _reviewService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductController(IProductService productService, ICategoryService categoryService, IUserServices userService, IReviewService reviewService,
            IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _categoryService = categoryService;
            _userService = userService;
            _reviewService = reviewService;
            _httpContextAccessor = httpContextAccessor;
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
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null) return NotFound();

            var sellerInfo = await _userService.GetSellerInfoViewModelAsync(product.sellerId == null ? 0 : product.sellerId.Value);


            var reviews = await _reviewService.GetReviewViewModelsByProductAsync(product.id);

            // Lấy userId từ session
            var userId = _httpContextAccessor.HttpContext?.Session.GetInt32("userId") ?? 0;
            var canReview = userId > 0 ? await _reviewService.CanUserReviewProductAsync(userId, product.id) : false;

            ViewBag.Product = product;
            ViewBag.SellerInfo = sellerInfo;
            ViewBag.Reviews = reviews;
            ViewBag.CanReview = canReview;

            return View();
        }

        [HttpPost("SubmitReview")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReview(int productId, int rating, string comment)
        {
            var userId = _httpContextAccessor.HttpContext?.Session.GetInt32("userId") ?? 0;
            if (userId <= 0)
            {
                TempData["ErrorMessage"] = "You are required to login to write a review";
                return RedirectToAction("Detail", new { id = productId });
            }

            // Kiểm tra user có quyền review sản phẩm này không
            var canReview = await _reviewService.CanUserReviewProductAsync(userId, productId);
            if (!canReview)
            {
                TempData["ErrorMessage"] = "You can not write a review without purchasing this product.";
                return RedirectToAction("Detail", new { id = productId });
            }

            // Kiểm tra rating hợp lệ
            if (rating < 1 || rating > 5)
            {
                TempData["ErrorMessage"] = "Invalid rate.";
                return RedirectToAction("Detail", new { id = productId });
            }

            // Tạo Review entity
            var review = new Entities.Review
            {
                productId = productId,
                reviewerId = userId,
                rating = rating,
                comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim(),
                createdAt = System.DateTime.UtcNow
            };

            await _reviewService.AddReviewAsync(review);

            TempData["SuccessMessage"] = "Thanks for your rating!";
            return RedirectToAction("Detail", new { id = productId });
        }
    }
}
