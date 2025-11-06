using EbayChat.Entities;
using Microsoft.EntityFrameworkCore;

namespace EbayChat.Services.ServicesImpl
{
    public class OrderService : IOrderService
    {
        private readonly CloneEbayDbContext _context;
        public OrderService(CloneEbayDbContext context)
        {
            _context = context;
        }

        public void CreateDispute(Dispute dispute)
        {
            if(dispute != null)
            {
                _context.Disputes.Add(dispute);
                _context.SaveChanges();
            }
        }

        public OrderTable? CreateOrder(int buyerId, int productId, int quantity, string fullName, string phone, string street, string city, string state, string country)
        {
            var product = _context.Products
                .Include(p => p.seller)
                .Include(p => p.OrderItems)
                .Include(p => p.Reviews)
                .Include(p => p.category)
                .FirstOrDefault(p => p.id == productId);
            if (product == null || product.price == null || product.seller?.id == buyerId)
                return null;

            // Tạo Address
            var address = new Address
            {
                userId = buyerId,
                fullName = fullName,
                phone = phone,
                street = street,
                city = city,
                state = state,  
                country = country,
                isDefault = false
            };
            _context.Addresses.Add(address);
            _context.SaveChanges();

            // Tạo Order
            var order = new OrderTable
            {
                buyerId = buyerId,
                addressId = address.id,
                orderDate = DateTime.Now,
                totalPrice = product.price * quantity,
                status = "Completed"
            };
            _context.OrderTables.Add(order);
            _context.SaveChanges();

            // Tạo OrderItem
            var orderItem = new OrderItem
            {
                orderId = order.id,
                productId = product.id,
                quantity = quantity,
                unitPrice = product.price
            };
            _context.OrderItems.Add(orderItem);
            _context.SaveChanges();

            return order;
        }

        public void CreateReturnRequest(ReturnRequest request)
        {
            if (request != null)
            {
                _context.ReturnRequests.Add(request);
                _context.SaveChanges();
            }
        }

        public OrderTable? GetOrderById(int id)
        {
            return _context.OrderTables
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.product)
                .Include(o => o.Disputes)
                .Include(o => o.Payments)
                .Include(o => o.ReturnRequests)
                .Include(o => o.ShippingInfos)
                .Include(o => o.address)
                .Include(o => o.buyer)
                .Where(o => o.id == id)
                .FirstOrDefault();
        }

        public List<OrderTable> GetOrdersByUserId(int userId)
        {
            return _context
                .OrderTables
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.product)
                .Include(o => o.Disputes)
                .Include(o => o.Payments)
                .Include(o => o.ReturnRequests)
                .Include(o => o.ShippingInfos)
                .Include(o => o.address)
                .Include(o => o.buyer)
                .Where(o => o.buyerId == userId)
                .OrderByDescending(o => o.orderDate)
                .ToList();
        }
    }
}
