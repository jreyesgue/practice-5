using Microsoft.Data.SqlClient;
using Practice5_Model.Models;

namespace Practice5_DataAccess.Service.ADO
{
    public class SaleServiceADO : ISaleService
    {
        private readonly string _connectionString;

        public SaleServiceADO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Sale>> GetSales()
        {
            string query = "SELECT * FROM Sale s" +
                " JOIN Product p ON s.ProductID = p.ProductId";
            var sales = new List<Sale>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                await connection.OpenAsync();
                using var reader = command.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    sales.Add(new Sale
                    {
                        SaleID = reader.GetInt32(0),
                        ProductID = reader.GetInt32(1),
                        Quantity = reader.GetInt32(2),
                        SalePrice = reader.GetDecimal(3),
                        SaleDate = reader.GetDateTime(4),
                        Product = new Product
                        {
                            ProductId = reader.GetInt32(5),
                            Name = reader.GetString(6),
                            Price = reader.GetDecimal(7),
                            Category = reader.GetString(8),
                            DateAdded = reader.GetDateTime(9)
                        }
                    });
                }
            }
            return sales;
        }

        public async Task<Sale?> GetSaleById(int? id)
        {
            string query = "SELECT * FROM Sale s" +
                " JOIN Product p ON s.ProductID = p.ProductId WHERE SaleID = @ID";
            Sale? sale = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", id);
                await connection.OpenAsync();
                using var reader = command.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    sale = new Sale
                    {
                        SaleID = reader.GetInt32(0),
                        ProductID = reader.GetInt32(1),
                        Quantity = reader.GetInt32(2),
                        SalePrice = reader.GetDecimal(3),
                        SaleDate = reader.GetDateTime(4),
                        Product = new Product
                        {
                            ProductId = reader.GetInt32(5),
                            Name = reader.GetString(6),
                            Price = reader.GetDecimal(7),
                            Category = reader.GetString(8),
                            DateAdded = reader.GetDateTime(9)
                        }
                    };
                }
            }
            return sale;
        }

        public async Task AddSale(Sale sale)
        {
            string query = "INSERT INTO Sale(ProductID, Quantity, SalePrice, SaleDate)" +
                "VALUES(@ProductID, @Quantity, @SalePrice, @SaleDate)";
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductID", sale.ProductID);
            command.Parameters.AddWithValue("@Quantity", sale.Quantity);
            command.Parameters.AddWithValue("@SalePrice", sale.SalePrice);
            command.Parameters.AddWithValue("@SaleDate", sale.SaleDate);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateSale(Sale sale)
        {
            string query = "UPDATE Sale SET ProductID = @ProductID," +
                " Quantity = @Quantity, SalePrice = @SalePrice," +
                " SaleDate = @SaleDate WHERE SaleID = @ID";
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", sale.SaleID);
            command.Parameters.AddWithValue("@ProductID", sale.ProductID);
            command.Parameters.AddWithValue("@Quantity", sale.Quantity);
            command.Parameters.AddWithValue("@SalePrice", sale.SalePrice);
            command.Parameters.AddWithValue("@SaleDate", sale.SaleDate);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteSale(int id)
        {
            string query = "DELETE FROM Sale WHERE SaleID = @ID";
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
