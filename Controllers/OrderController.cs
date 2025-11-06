using EbayChat.Entities;
using EbayChat.Services;
using EbayChat.Services.ServicesImpl;
using Microsoft.AspNetCore.Mvc;

namespace EbayChat.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public IActionResult BuyNow(int productId, int quantity, string fullName, string phone, string street, string city, string state, string country)
        {
            // Lấy user từ session
            var buyerId = HttpContext.Session.GetInt32("userId");
            if (buyerId == null)
            {
                TempData["ErrorMessage"] = "Please login before buying.";
                return RedirectToAction("Index", "Home");
            }

            var order = _orderService.CreateOrder(buyerId.Value, productId, quantity, fullName, phone, street, city, state, country);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Purchase failed. Please try again.";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("OrderDetail", new { id = order.id });
        }

        [HttpGet]
        public IActionResult OrderDetail(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction("Index", "Home");
            }

            return View(order);
        }

        [HttpGet]
        public IActionResult MyOrders()
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Please login to view your orders.";
                return RedirectToAction("Login", "User");
            }

            var orders = _orderService.GetOrdersByUserId(userId.Value);
            return View(orders);
        }
        [HttpPost]
        public IActionResult SubmitReturnRequest(int orderId, string reason)
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var request = new ReturnRequest
            {
                orderId = orderId,
                userId = userId,
                reason = reason,
                status = "Pending",
                createdAt = DateTime.Now
            };
            _orderService.CreateReturnRequest(request);

            TempData["SuccessMessage"] = "Your return request has been submitted successfully.";
            return RedirectToAction("OrderDetail", new { id = orderId });
        }

        [HttpPost]
        public IActionResult SubmitDispute(int orderId, string description)
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var dispute = new Dispute
            {
                orderId = orderId,
                raisedBy = userId,
                description = description,
                status = "Open"
            };
            _orderService.CreateDispute(dispute);

            TempData["SuccessMessage"] = "Your dispute has been submitted for review.";
            return RedirectToAction("OrderDetail", new { id = orderId });
        }

    }
}
