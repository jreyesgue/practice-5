using Microsoft.EntityFrameworkCore;
using Practice5_DataAccess.Data;
using Practice5_Model.Models;

namespace Practice5_DataAccess.Service.EF
{
    public class SaleServiceEF : ISaleService
    {
        private readonly DatabaseContext _context;

        public SaleServiceEF(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Sale>> GetSales()
        {
            return await _context.Sale
                .Include(i => i.Product)
                .ToListAsync();
        }

        public async Task<Sale?> GetSaleById(int? id)
        {
            return await _context.Sale
                .Include(i => i.Product)
                .FirstAsync(i => i.SaleID == id);
        }

        public async Task AddSale(Sale sale)
        {
            _context.Sale.Add(sale);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSale(Sale sale)
        {
            _context.Sale.Update(sale);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSale(int id)
        {
            var sale = await _context.Sale.FindAsync(id);
            if (sale != null)
            {
                _context.Sale.Remove(sale);
            }

            await _context.SaveChangesAsync();
        }
    }
}
