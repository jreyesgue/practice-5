using Practice5_Model.Models;

namespace Practice5_DataAccess.Service
{
    public interface ISaleService
    {
        Task<List<Sale>> GetSales();
        Task<Sale?> GetSaleById(int? id);
        Task AddSale(Sale sale);
        Task UpdateSale(Sale sale);
        Task DeleteSale(int id);
    }
}
