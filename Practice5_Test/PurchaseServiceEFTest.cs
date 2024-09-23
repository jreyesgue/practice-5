using Microsoft.EntityFrameworkCore;
using Practice5_DataAccess.Data;
using Practice5_DataAccess.Service.EF;
using Practice5_Model.Models;

namespace Practice5_Test
{
    [TestFixture]
    public class PurchaseServiceEFTest
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
            context.Purchase.AddRange(
                new Purchase
                {
                    PurchaseID = 1,
                    ProductID = 1,
                    PurchasePrice = 100,
                    PurchaseDate = DateTime.Now
                },
                new Purchase
                {
                    PurchaseID = 2,
                    ProductID = 2,
                    PurchasePrice = 80,
                    PurchaseDate = DateTime.Now
                });
            context.SaveChanges();
        }

        [Test]
        public async Task GetPurchase_ReturnsAllPurchases()
        {
            using var context = new DatabaseContext(_options);
            SetUpData(context);
            var service = new PurchaseServiceEF(context);

            var purchases = await service.GetPurchases();

            Assert.That(purchases.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetPurchaseById_PurchaseExists_ReturnsCorrectPurchase()
        {
            using var context = new DatabaseContext(_options);
            SetUpData(context);
            var service = new PurchaseServiceEF(context);

            var purchase = await service.GetPurchaseById(1);

            Assert.That(purchase, Is.Not.Null);
            Assert.That(purchase.ProductID, Is.EqualTo(1));
            Assert.That(purchase.PurchasePrice, Is.EqualTo(100));
            Assert.That(purchase.PurchaseDate, Is.LessThan(DateTime.Now));
        }

        [Test]
        public async Task AddPurchase_ValidPurchase_PurchaseAdded()
        {
            using var context = new DatabaseContext(_options);
            var service = new PurchaseServiceEF(context);
            var newPurchase = new Purchase
            {
                PurchaseID = 3,
                ProductID = 1,
                PurchasePrice = 110,
                PurchaseDate = DateTime.Now
            };

            await service.AddPurchase(newPurchase);

            var purchase = await context.Purchase.FindAsync(3);

            Assert.That(purchase, Is.Not.Null);
            Assert.That(purchase.ProductID, Is.EqualTo(1));
            Assert.That(purchase.PurchasePrice, Is.EqualTo(110));
            Assert.That(purchase.PurchaseDate, Is.LessThan(DateTime.Now));
        }

        [Test]
        public async Task UpdatePurchase_ValidPurchase_PurchaseUpdated()
        {
            using var context = new DatabaseContext(_options);
            SetUpData(context);
            var service = new PurchaseServiceEF(context);

            var purchase = await context.Purchase.FindAsync(1);
            purchase.PurchasePrice = 120;

            await service.UpdatePurchase(purchase);

            var updatedPurchase = await context.Purchase.FindAsync(1);

            Assert.That(updatedPurchase, Is.Not.Null);
            Assert.That(updatedPurchase.PurchasePrice, Is.EqualTo(120));
        }

        [Test]
        public async Task DeletePurchase_PurchaseExists_PurchaseDeleted()
        {
            using var context = new DatabaseContext(_options);
            SetUpData(context);
            var service = new PurchaseServiceEF(context);

            await service.DeletePurchase(1);

            var purchase = await context.Purchase.FindAsync(1);

            Assert.That(purchase, Is.Null);
        }
    }
}
