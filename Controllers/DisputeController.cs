using EbayChat.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EbayChat.Controllers
{
    public class DisputeController : Controller
    {
        private readonly IDisputeService _disputeService;
        public DisputeController(IDisputeService disputeService)
        {
            _disputeService = disputeService;
        }
        [HttpGet]
        public async Task<IActionResult> SentDisputes()
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var disputes = await _disputeService.GetSentDisputes(userId.Value);

            return View(disputes);
        }

        [HttpGet]
        public async Task<IActionResult> ReceivedDisputes()
        {
            var sellerId = HttpContext.Session.GetInt32("userId");
            if (sellerId == null)
                return RedirectToAction("Login", "Auth");

            var disputes = await _disputeService.GetReceivedDisputes(sellerId.Value);
            return View(disputes);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDescription(int id, string description)
        {
            var dispute = await _disputeService.GetDisputeById(id);
            if (dispute == null)
                return NotFound();

            if (dispute.status?.ToLower() == "open" || dispute.status?.ToLower() == "in progress")
            {
                await _disputeService.UpdateDisputeAsync(id, description);
                TempData["SuccessMessage"] = "Description updated successfully.";
            }

            return RedirectToAction("SentDisputes");
        }

        // 🟠 Seller: update status và resolution
        [HttpPost]
        public async Task<IActionResult> UpdateStatusAndResolution(int id, string status, string resolution)
        {
            var dispute = await _disputeService.GetDisputeById(id);
            if (dispute == null)
                return NotFound();

            if (dispute.status?.ToLower() == "open" || dispute.status?.ToLower() == "in progress")
            {
                await _disputeService.UpdateDisputeAsync(id, status, resolution);
                TempData["SuccessMessage"] = "Dispute updated successfully.";
            }
            return RedirectToAction("ReceivedDisputes");
        }
    }
}
