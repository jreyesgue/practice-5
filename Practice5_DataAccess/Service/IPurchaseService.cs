using Practice5_Model.Models;

namespace Practice5_DataAccess.Service
{
    public interface IPurchaseService
    {
        Task<List<Purchase>> GetPurchases();
        Task<Purchase?> GetPurchaseById(int? id);
        Task AddPurchase(Purchase purchase);
        Task UpdatePurchase(Purchase purchase);
        Task DeletePurchase(int id);
    }
}
