using EbayChat.Entities;
using EbayChat.Models.DTOs;

namespace EbayChat.Services
{
    public interface IChatServices
    {
        Task<IEnumerable<BoxChatDTO>> GetBoxChats(int userId);
        Task<IEnumerable<Message>> GetAllMessagesBySenderAndReceiver(int senderId, int receiverId);
    }
}
