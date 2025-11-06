using EbayChat.Entities;

namespace EbayChat.Services
{
    public interface IDisputeService
    {
        Task<List<Dispute>> GetSentDisputes(int buyerId);
        Task<List<Dispute>> GetReceivedDisputes(int sellerId);
        Task<Dispute?> GetDisputeById(int id);
        Task UpdateDisputeAsync(int id, string status, string resolution);
        Task UpdateDisputeAsync(int id, string description);

    }
}
