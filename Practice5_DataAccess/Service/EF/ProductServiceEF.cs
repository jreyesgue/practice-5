using Microsoft.EntityFrameworkCore;
using Practice5_DataAccess.Data;
using Practice5_Model.Models;

namespace Practice5_DataAccess.Service.EF
{
    public class ProductServiceEF : IProductService
    {
        private readonly DatabaseContext _context;

        public ProductServiceEF(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Product.ToListAsync();
        }

        public async Task<Product?> GetProductById(int? id)
        {
            return await _context.Product.FindAsync(id);
        }

        public async Task AddProduct(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            _context.Product.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
