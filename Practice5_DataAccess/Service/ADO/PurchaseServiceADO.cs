using Microsoft.Data.SqlClient;
using Practice5_Model.Models;

namespace Practice5_DataAccess.Service.ADO
{
    public class PurchaseServiceADO : IPurchaseService
    {
        private readonly string _connectionString;

        public PurchaseServiceADO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Purchase>> GetPurchases()
        {
            string query = "SELECT * FROM Purchase pu" +
                " JOIN Product p ON pu.ProductID = p.ProductId";
            var purchases = new List<Purchase>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                await connection.OpenAsync();
                using var reader = command.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    purchases.Add(new Purchase
                    {
                        PurchaseID = reader.GetInt32(0),
                        ProductID = reader.GetInt32(1),
                        Quantity = reader.GetInt32(2),
                        PurchasePrice = reader.GetDecimal(3),
                        PurchaseDate = reader.GetDateTime(4),
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
            return purchases;
        }

        public async Task<Purchase?> GetPurchaseById(int? id)
        {
            string query = "SELECT * FROM Purchase pu" +
                " JOIN Product p ON pu.ProductID = p.ProductId WHERE PurchaseID = @ID";
            Purchase? purchase = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", id);
                await connection.OpenAsync();
                using var reader = command.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    purchase = new Purchase
                    {
                        PurchaseID = reader.GetInt32(0),
                        ProductID = reader.GetInt32(1),
                        Quantity = reader.GetInt32(2),
                        PurchasePrice = reader.GetDecimal(3),
                        PurchaseDate = reader.GetDateTime(4),
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
            return purchase;
        }

        public async Task AddPurchase(Purchase purchase)
        {
            string query = "INSERT INTO Purchase(ProductID, Quantity, PurchasePrice, PurchaseDate)" +
                "VALUES(@ProductID, @Quantity, @PurchasePrice, @PurchaseDate)";
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductID", purchase.ProductID);
            command.Parameters.AddWithValue("@Quantity", purchase.Quantity);
            command.Parameters.AddWithValue("@PurchasePrice", purchase.PurchasePrice);
            command.Parameters.AddWithValue("@PurchaseDate", purchase.PurchaseDate);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdatePurchase(Purchase purchase)
        {
            string query = "UPDATE Purchase SET ProductID = @ProductID," +
                " Quantity = @Quantity, PurchasePrice = @PurchasePrice," +
                " PurchaseDate = @PurchaseDate WHERE PurchaseID = @ID";
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", purchase.PurchaseID);
            command.Parameters.AddWithValue("@ProductID", purchase.ProductID);
            command.Parameters.AddWithValue("@Quantity", purchase.Quantity);
            command.Parameters.AddWithValue("@PurchasePrice", purchase.PurchasePrice);
            command.Parameters.AddWithValue("@PurchaseDate", purchase.PurchaseDate);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeletePurchase(int id)
        {
            string query = "DELETE FROM Purchase WHERE PurchaseID = @ID";
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
