using Microsoft.EntityFrameworkCore;
using Practice5_DataAccess.Data;
using Practice5_DataAccess.Service.EF;
using Practice5_Model.Models;

namespace Practice5_Test
{
    [TestFixture]
    public class InventoryServiceEFTest
    {
        private DbContextOptions<DatabaseContext> _options;

        [SetUp]
        public void SetUp()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        private void SetUpData(DatabaseContext context)
        {
            context.Database.EnsureDeleted();
            context.Product.AddRange(
                new Product
                {
                    ProductId = 1,
                    Name = "Jeans",
                    Category = "Bottoms",
                    Price = 100,
                    DateAdded = DateTime.Now
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Shirt",
                    Category = "Tops",
                    Price = 80,
                    DateAdded = DateTime.Now
                });
            context.Inventory.AddRange(
                new Inventory
                {
                    InventoryID = 1,
                    ProductID = 1,
                    Stock = 10,
                    DateModified = DateTime.Now
                },
                new Inventory
                {
                    InventoryID = 2,
                    ProductID = 2,
                    Stock = 15,
                    DateModified = DateTime.Now
                });
            context.SaveChanges();
        }

        [Test]
        public async Task GetInventory_ReturnsAllInventory()
        {
            using var context = new DatabaseContext(_options);
            SetUpData(context);
            var service = new InventoryServiceEF(context);

            var inventory = await service.GetInventory();

            Assert.That(inventory.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetInventoryById_InventoryExists_ReturnsCorrectInventory()
        {
            using var context = new DatabaseContext(_options);
            SetUpData(context);
            var service = new InventoryServiceEF(context);

            var inventory = await service.GetInventoryById(1);

            Assert.That(inventory, Is.Not.Null);
            Assert.That(inventory.ProductID, Is.EqualTo(1));
            Assert.That(inventory.Stock, Is.EqualTo(10));
            Assert.That(inventory.DateModified, Is.LessThan(DateTime.Now));
        }

        [Test]
        public async Task AddInventory_ValidInventory_InventoryAdded()
        {
            using var context = new DatabaseContext(_options);
            var service = new InventoryServiceEF(context);
            var newProduct = new Product
            {
                ProductId = 3,
                Name = "Jacket",
                Category = "Tops",
                Price = 100,
                DateAdded = DateTime.Now
            };
            var newInventory = new Inventory
            {
                InventoryID = 3,
                ProductID = 3,
                Stock = 20,
                DateModified = DateTime.Now
            };

            context.Product.Add(newProduct);
            await service.AddInventory(newInventory);

            var inventory = await context.Inventory.FindAsync(3);

            Assert.That(inventory, Is.Not.Null);
            Assert.That(inventory.ProductID, Is.EqualTo(3));
            Assert.That(inventory.Stock, Is.EqualTo(20));
            Assert.That(inventory.DateModified, Is.LessThan(DateTime.Now));
        }

        [Test]
        public async Task UpdateInventory_ValidInventory_InventoryUpdated()
        {
            using var context = new DatabaseContext(_options);
            SetUpData(context);
            var service = new InventoryServiceEF(context);

            var inventory = await context.Inventory.FindAsync(1);
            inventory.Stock = 30;

            await service.UpdateInventory(inventory);

            var updatedInventory = await context.Inventory.FindAsync(1);

            Assert.That(updatedInventory, Is.Not.Null);
            Assert.That(updatedInventory.Stock, Is.EqualTo(30));
        }

        [Test]
        public async Task DeleteInventory_InventoryExists_InventoryDeleted()
        {
            using var context = new DatabaseContext(_options);
            SetUpData(context);
            var service = new InventoryServiceEF(context);

            await service.DeleteInventory(1);

            var inventory = await context.Inventory.FindAsync(1);

            Assert.That(inventory, Is.Null);
        }
    }
}
