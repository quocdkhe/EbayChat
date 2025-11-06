using EbayChat.Entities;

namespace EbayChat.Services
{
    public interface IOrderService
    {
        OrderTable? CreateOrder(int buyerId, int productId, int quantity, string fullName, string phone, string street, string city, string state, string country);
        OrderTable? GetOrderById(int id);
        List<OrderTable> GetOrdersByUserId(int userId);
        void CreateReturnRequest(ReturnRequest request);
        void CreateDispute(Dispute dispute);

    }
}
