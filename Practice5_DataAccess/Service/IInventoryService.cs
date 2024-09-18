using Practice5_Model.Models;

namespace Practice5_DataAccess.Service
{
    public interface IInventoryService
    {
        Task<List<Inventory>> GetInventory();
        Task<Inventory?> GetInventoryById(int? id);
        Task AddInventory(Inventory inventory);
        Task UpdateInventory(Inventory inventory);
        Task DeleteInventory(int id);
    }
}
