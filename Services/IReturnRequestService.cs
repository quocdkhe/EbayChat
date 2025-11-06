using EbayChat.Entities;

namespace EbayChat.Services
{
    public interface IReturnRequestService
    {
        Task<List<ReturnRequest>> GetSentReturnRequest(int buyerId);
        Task<List<ReturnRequest>> GetReceivedReturnRequest(int sellerId);
        Task<ReturnRequest?> GetReturnRequestById(int id);
        Task UpdateStatusReturnRequestAsync(int id, string status);
        Task UpdateReasonReturnRequestAsync(int id, string reason);

    }
}
