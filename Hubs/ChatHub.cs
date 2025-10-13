using EbayChat.Entities;
using Microsoft.AspNetCore.SignalR;

namespace EbayChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly CloneEbayDbContext _context;
        public ChatHub(CloneEbayDbContext context) => _context = context;

        // Tạo group duy nhất cho 1-1 conversation (đảm bảo user nhỏ trước)
        private static string ConversationGroup(int user1, int user2)
        {
            var min = Math.Min(user1, user2);
            var max = Math.Max(user1, user2);
            return $"chat-{min}-{max}";
        }

        // Khi mở box chat => client gọi hàm này để join vào group hội thoại
        public async Task JoinConversation(int userId, int otherId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ConversationGroup(userId, otherId));
        }

        // Gửi tin nhắn 1-1
        public async Task SendMessage(int senderId, int receiverId, string message)
        {
            // Lưu DB
            var newMessage = new Message
            {
                senderId = senderId,
                receiverId = receiverId,
                content = message,
                timestamp = DateTime.UtcNow,
            };

            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            var sender = await _context.Users.FindAsync(senderId);

            var payload = new
            {
                id = newMessage.id,
                senderId = senderId,
                senderName = sender?.username ?? $"User {senderId}",
                receiverId = receiverId,
                content = message,
                sentAt = newMessage.timestamp.Value.ToString("HH:mm dd/MM/yyyy")
            };

            // Gửi chỉ vào group hội thoại 1-1
            await Clients.Group(ConversationGroup(senderId, receiverId))
                         .SendAsync("ReceiveMessage", payload);
        }

        // Đánh dấu đã đọc
        public async Task MarkAsRead(int messageId)
        {
            var msg = await _context.Messages.FindAsync(messageId);
            if (msg is null) return;

            // ví dụ: set isRead = true
            // msg.IsRead = true;

            await _context.SaveChangesAsync();
        }

        // Sự kiện "đang gõ"
        public async Task UserTyping(int senderId, int receiverId)
        {
            await Clients.Group(ConversationGroup(senderId, receiverId))
                         .SendAsync("UserIsTyping", senderId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
