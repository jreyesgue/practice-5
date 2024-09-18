using Microsoft.EntityFrameworkCore;
using Practice5_DataAccess.Data;
using Practice5_Model.Models;

namespace Practice5_DataAccess.Service.EF
{
    public class PurchaseServiceEF : IPurchaseService
    {
        private readonly DatabaseContext _context;

        public PurchaseServiceEF(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Purchase>> GetPurchases()
        {
            return await _context.Purchase
                .Include(i => i.Product)
                .ToListAsync();
        }

        public async Task<Purchase?> GetPurchaseById(int? id)
        {
            return await _context.Purchase
                .Include(i => i.Product)
                .FirstAsync(i => i.PurchaseID == id);
        }

        public async Task AddPurchase(Purchase purchase)
        {
            _context.Purchase.Add(purchase);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePurchase(Purchase purchase)
        {
            _context.Purchase.Update(purchase);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePurchase(int id)
        {
            var purchase = await _context.Purchase.FindAsync(id);
            if (purchase != null)
            {
                _context.Purchase.Remove(purchase);
            }

            await _context.SaveChangesAsync();
        }
    }
}
