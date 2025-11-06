using EbayChat.Entities;
using Microsoft.EntityFrameworkCore;

namespace EbayChat.Services.ServicesImpl
{
    public class DisputeService : IDisputeService
    {
        private readonly CloneEbayDbContext _context;
        public DisputeService(CloneEbayDbContext context)
        {
            _context = context;
        }

        public async Task<Dispute?> GetDisputeById(int id)
        {
            return await _context.Disputes
                .FirstOrDefaultAsync(d => d.id == id);
        }

        public async Task<List<Dispute>> GetReceivedDisputes(int sellerId)
        {
            var disputes = await _context.Disputes
                .Include(d => d.order)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(ot => ot.product)
                .Include(d => d.raisedByNavigation)
                .Where(d => d.order.OrderItems.Any(ot => ot.product.sellerId == sellerId))
                .ToListAsync();
            return disputes;
        }

        public async Task<List<Dispute>> GetSentDisputes(int buyerId)
        {
            var disputes = await _context.Disputes
                .Include(d => d.raisedByNavigation)
                .Where(d => d.raisedBy == buyerId)
                .ToListAsync();
            return disputes;
        }

        public async Task UpdateDisputeAsync(int id, string status, string resolution)
        {
            var dispute = await _context.Disputes.FindAsync(id);
            if (dispute != null)
            {
                dispute.status = status;
                dispute.resolution = resolution;
                _context.Disputes.Update(dispute);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateDisputeAsync(int id, string description)
        {
            var dispute = await _context.Disputes.FindAsync(id);
            if (dispute != null)
            {
                dispute.description = description;
                _context.Disputes.Update(dispute);
                await _context.SaveChangesAsync();
            }
        }
    }
}
