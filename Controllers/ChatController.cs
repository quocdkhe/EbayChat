using EbayChat.Entities;
using EbayChat.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Controllers
{
    public class ChatController : Controller
    {
        private readonly CloneEbayDbContext _context;
        private readonly IChatServices _chatServices;

        public ChatController(CloneEbayDbContext context, IChatServices chatServices)
        {
            _context = context;
            _chatServices = chatServices;
        }

        public async Task<IActionResult> Index()
{
    int? currentUserId = HttpContext.Session.GetInt32("userId");
    if (currentUserId == null)
    {
        return RedirectToAction("Login", "Auth");
    }

    // Lấy danh sách tất cả tin nhắn có liên quan tới user hiện tại
    var relatedMessages = await _context.Messages
        .Include(m => m.sender)
        .Include(m => m.receiver)
        .Where(m => m.senderId == currentUserId || m.receiverId == currentUserId)
        .ToListAsync();

    // Group theo user đối phương (người còn lại trong cuộc trò chuyện)
    var groupedChats = relatedMessages
        .GroupBy(m => m.senderId == currentUserId ? m.receiverId : m.senderId)
        .Select(g => new
        {
            PartnerId = g.Key,
            PartnerName = g.First().senderId == currentUserId
                ? g.First().receiver.username
                : g.First().sender.username,
            LastMessage = g.OrderByDescending(x => x.timestamp).First().content,
            LastMessageTime = g.OrderByDescending(x => x.timestamp).First().timestamp
        })
        .OrderByDescending(x => x.LastMessageTime)
        .ToList();

    ViewBag.Chats = groupedChats;
    ViewBag.CurrentUserId = currentUserId;

    return View();
}



        // Trang chat với một user cụ thể
        public async Task<IActionResult> Chat(int receiverId)
        {
            // Lấy user hiện tại từ session
            int? senderId = HttpContext.Session.GetInt32("userId");
            if (senderId == null)
            {
                return RedirectToAction("Login", "Auth");
            }
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