using Microsoft.Data.SqlClient;
using Practice5_Model.Models;

namespace Practice5_DataAccess.Service.ADO
{
    public class InventoryServiceADO : IInventoryService
    {
        private readonly string _connectionString;

        public InventoryServiceADO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Inventory>> GetInventory()
        {
            string query = "SELECT * FROM Inventory i" +
                " JOIN Product p ON i.ProductID = p.ProductId";
            var inventory = new List<Inventory>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                await connection.OpenAsync();
                using var reader = command.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    inventory.Add(new Inventory
                    {
                        InventoryID = reader.GetInt32(0),
                        ProductID = reader.GetInt32(1),
                        Stock = reader.GetInt32(2),
                        DateModified = reader.GetDateTime(3),
                        Product = new Product
                        {
                            ProductId = reader.GetInt32(4),
                            Name = reader.GetString(5),
                            Price = reader.GetDecimal(6),
                            Category = reader.GetString(7),
                            DateAdded = reader.GetDateTime(8)
                        }
                    });
                }
            }
            return inventory;
        }

        public async Task<Inventory?> GetInventoryById(int? id)
        {
            string query = "SELECT * FROM Inventory i" +
                " JOIN Product p ON i.ProductID = p.ProductId WHERE InventoryID = @ID";
            Inventory? inventory = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", id);
                await connection.OpenAsync();
                using var reader = command.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    inventory = new Inventory
                    {
                        InventoryID = reader.GetInt32(0),
                        ProductID = reader.GetInt32(1),
                        Stock = reader.GetInt32(2),
                        DateModified = reader.GetDateTime(3),
                        Product = new Product
                        {
                            ProductId = reader.GetInt32(4),
                            Name = reader.GetString(5),
                            Price = reader.GetDecimal(6),
                            Category = reader.GetString(7),
                            DateAdded = reader.GetDateTime(8)
                        }
                    };
                }
            }
            return inventory;
        }
        public async Task AddInventory(Inventory inventory)
        {
            string query = "INSERT INTO Inventory(ProductID, Stock, DateModified)" +
                "VALUES(@ProductID, @Stock, @DateModified)";
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductID", inventory.ProductID);
            command.Parameters.AddWithValue("@Stock", inventory.Stock);
            command.Parameters.AddWithValue("@DateModified", inventory.DateModified);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateInventory(Inventory inventory)
        {
            string query = "UPDATE Inventory SET ProductID = @ProductID, Stock = @Stock," +
                " DateModified = @DateModified WHERE InventoryID = @ID";
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", inventory.InventoryID);
            command.Parameters.AddWithValue("@ProductID", inventory.ProductID);
            command.Parameters.AddWithValue("@Stock", inventory.Stock);
            command.Parameters.AddWithValue("@DateModified", inventory.DateModified);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteInventory(int id)
        {
            string query = "DELETE FROM Inventory WHERE InventoryID = @ID";
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
