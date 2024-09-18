using Microsoft.EntityFrameworkCore;
using Practice5_DataAccess.Data;
using Practice5_Model.Models;

namespace Practice5_DataAccess.Service.EF
{
    public class InventoryServiceEF : IInventoryService
    {
        private readonly DatabaseContext _context;

        public InventoryServiceEF(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Inventory>> GetInventory()
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .ToListAsync();
        }

        public async Task<Inventory?> GetInventoryById(int? id)
        {
            return await _context.Inventory
                .Include(i => i.Product)
                .FirstAsync(i => i.InventoryID == id);
        }

        public async Task AddInventory(Inventory inventory)
        {
            _context.Inventory.Add(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateInventory(Inventory inventory)
        {
            _context.Inventory.Update(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInventory(int id)
        {
            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventory.Remove(inventory);
            }

            await _context.SaveChangesAsync();
        }
    }
}
