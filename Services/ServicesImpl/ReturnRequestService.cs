using EbayChat.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EbayChat.Services.ServicesImpl
{
    public class ReturnRequestService : IReturnRequestService
    {
        private readonly CloneEbayDbContext _context;
        public ReturnRequestService(CloneEbayDbContext context)
        {
            _context = context;
        }
        public async Task<List<ReturnRequest>> GetReceivedReturnRequest(int sellerId)
        {
            var requests = await _context.ReturnRequests
                .Include(r => r.order)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(ot => ot.product)
                .Include(r => r.user)
                .Where(r => r.order.OrderItems.Any(ot => ot.product.sellerId == sellerId))
                .ToListAsync();
            return requests;
        }

        public async Task<ReturnRequest?> GetReturnRequestById(int id)
        {
            return await _context.ReturnRequests
                .FirstOrDefaultAsync(r => r.id == id);
        }
        public Task<List<ReturnRequest>> GetSentReturnRequest(int buyerId)
        {
            var requests = _context.ReturnRequests
                .Include(r => r.order)
                .Include(r => r.user)
                .Where(r => r.userId == buyerId)
                .ToListAsync();
            return requests;
        }

        public async Task UpdateStatusReturnRequestAsync(int id, string status)
        {
            var request = await _context.ReturnRequests.FindAsync(id);
            if (request != null)
            {
                request.status = status;
                _context.ReturnRequests.Update(request);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateReasonReturnRequestAsync(int id, string reason)
        {
            var request = await _context.ReturnRequests.FindAsync(id);
            if (request != null)
            {
                request.reason = reason;
                _context.ReturnRequests.Update(request);
                await _context.SaveChangesAsync();
            }
        }
    }
}
