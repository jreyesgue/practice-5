using Microsoft.Data.SqlClient;
using Practice5_Model.Models;

namespace Practice5_DataAccess.Service.ADO
{
    public class ProductServiceADO : IProductService
    {
        private readonly string _connectionString;

        public ProductServiceADO(string connectionString = "")
        {
            _connectionString = connectionString;
        }

        public async Task<List<Product>> GetProducts()
        {
            string query = "SELECT * FROM Product";
            var products = new List<Product>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                await connection.OpenAsync();
                using var reader = command.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    products.Add(new Product
                    {
                        ProductId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Price = reader.GetDecimal(2),
                        Category = reader.GetString(3),
                        DateAdded = reader.GetDateTime(4)
                    });
                }
            }
            return products;
        }

        public async Task<Product?> GetProductById(int? id)
        {
            string query = "SELECT * FROM Product WHERE ProductId = @ID";
            Product? product = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", id);
                await connection.OpenAsync();
                using var reader = command.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    product = new Product
                    {
                        ProductId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Price = reader.GetDecimal(2),
                        Category = reader.GetString(3),
                        DateAdded = reader.GetDateTime(4)
                    };
                }
            }
            return product;
        }

        public async Task AddProduct(Product product)
        {
            string query = "INSERT INTO Product(Name, Price, Category, DateAdded)" +
                "VALUES(@Name, @Price, @Category, @DateAdded)";
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@Category", product.Category);
            command.Parameters.AddWithValue("@DateAdded", product.DateAdded);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            string query = "UPDATE Product SET Name = @Name, Price = @Price," +
                " Category = @Category, DateAdded = @DateAdded WHERE ProductId = @ID";
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", product.ProductId);
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@Category", product.Category);
            command.Parameters.AddWithValue("@DateAdded", product.DateAdded);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteProduct(int id)
        {
            string query = "DELETE FROM Product WHERE ProductId = @ID";
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
