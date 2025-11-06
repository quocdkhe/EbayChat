using EbayChat.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EbayChat.Controllers
{
    public class ReturnRequestController : Controller
    {
        private readonly IReturnRequestService _returnRequestService;
        public ReturnRequestController(IReturnRequestService returnRequestService)
        {
            _returnRequestService = returnRequestService;
        }
        // 🟢 Buyer: xem các request đã gửi
        [HttpGet]
        public async Task<IActionResult> SentReturnRequests()
        {
            var buyerId = HttpContext.Session.GetInt32("userId");
            if (buyerId == null)
                return RedirectToAction("Login", "Auth");

            var requests = await _returnRequestService.GetSentReturnRequest(buyerId.Value);
            return View(requests);
        }

        // 🟣 Seller: xem các request gửi đến
        [HttpGet]
        public async Task<IActionResult> ReceivedReturnRequests()
        {
            var sellerId = HttpContext.Session.GetInt32("userId");
            if (sellerId == null)
                return RedirectToAction("Login", "Auth");

            var requests = await _returnRequestService.GetReceivedReturnRequest(sellerId.Value);
            return View(requests);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            await _returnRequestService.UpdateStatusReturnRequestAsync(id, status);
            TempData["SuccessMessage"] = "Return request updated successfully.";
            return RedirectToAction("ReceivedReturnRequests");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateReason(int id, string reason)
        {
            await _returnRequestService.UpdateReasonReturnRequestAsync(id, reason);

            TempData["SuccessMessage"] = "Reason updated successfully.";
            return RedirectToAction("SentReturnRequests");
        }
    }
}
