using Microsoft.AspNetCore.Mvc;
using EbayChat.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Controllers
{
    public class ChatController : Controller
    {
        private readonly CloneEbayDbContext _context;

        public ChatController(CloneEbayDbContext context)
        {
            _context = context;
        }

        // Trang chính - danh sách users
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // Trang chat với một user cụ thể
        public async Task<IActionResult> Chat(int senderId, int receiverId)
        {
            var sender = await _context.Users.FindAsync(senderId);
            var receiver = await _context.Users.FindAsync(receiverId);

            if (sender == null || receiver == null)
            {
                return NotFound();
            }

            // Lấy lịch sử tin nhắn
            var messages = await _context.Messages
                .Include(m => m.sender)
                .Include(m => m.receiver)
                .Where(m => (m.senderId == senderId && m.receiverId == receiverId) ||
                           (m.senderId == receiverId && m.receiverId == senderId))
                .OrderBy(m => m.timestamp)
                .ToListAsync();

            ViewBag.SenderId = senderId;
            ViewBag.SenderName = sender.username;
            ViewBag.ReceiverId = receiverId;
            ViewBag.ReceiverName = receiver.username;
            ViewBag.Messages = messages;

            return View();
        }

        // API để lấy lịch sử tin nhắn
        [HttpGet]
        public async Task<IActionResult> GetMessages(int senderId, int receiverId)
        {
            var messages = await _context.Messages
                .Include(m => m.sender)
                .Where(m => (m.senderId == senderId && m.receiverId == receiverId) ||
                           (m.senderId == receiverId && m.receiverId == senderId))
                .OrderBy(m => m.timestamp)
                .Select(m => new
                {
                    id = m.id,
                    senderId = m.senderId,
                    senderName = m.sender.username,
                    receiverId = m.receiverId,
                    content = m.content,
                    sentAt = m.timestamp.Value.ToString("HH:mm dd/MM/yyyy"),
                })
                .ToListAsync();

            return Json(messages);
        }
    }
}