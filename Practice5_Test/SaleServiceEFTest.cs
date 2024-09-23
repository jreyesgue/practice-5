using Microsoft.EntityFrameworkCore;
using Practice5_DataAccess.Data;
using Practice5_DataAccess.Service.EF;
using Practice5_Model.Models;

namespace Practice5_Test
{
    [TestFixture]
    public class SaleServiceEFTest
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
            context.Sale.AddRange(
                new Sale
                {
                    SaleID = 1,
                    ProductID = 1,
                    SalePrice = 100,
                    SaleDate = DateTime.Now
                },
                new Sale
                {
                    SaleID = 2,
                    ProductID = 2,
                    SalePrice = 80,
                    SaleDate = DateTime.Now
                });
            context.SaveChanges();
        }

        [Test]
        public async Task GetSale_ReturnsAllSales()
        {
            using var context = new DatabaseContext(_options);
            SetUpData(context);
            var service = new SaleServiceEF(context);

            var sales = await service.GetSales();

            Assert.That(sales.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetSaleById_SaleExists_ReturnsCorrectSale()
        {
            using var context = new DatabaseContext(_options);
            SetUpData(context);
            var service = new SaleServiceEF(context);

            var sale = await service.GetSaleById(1);

            Assert.That(sale, Is.Not.Null);
            Assert.That(sale.ProductID, Is.EqualTo(1));
            Assert.That(sale.SalePrice, Is.EqualTo(100));
            Assert.That(sale.SaleDate, Is.LessThan(DateTime.Now));
        }

        [Test]
        public async Task AddSale_ValidSale_SaleAdded()
        {
            using var context = new DatabaseContext(_options);
            var service = new SaleServiceEF(context);
            var newSale = new Sale
            {
                SaleID = 3,
                ProductID = 1,
                SalePrice = 110,
                SaleDate = DateTime.Now
            };

            await service.AddSale(newSale);

            var sale = await context.Sale.FindAsync(3);

            Assert.That(sale, Is.Not.Null);
            Assert.That(sale.ProductID, Is.EqualTo(1));
            Assert.That(sale.SalePrice, Is.EqualTo(110));
            Assert.That(sale.SaleDate, Is.LessThan(DateTime.Now));
        }

        [Test]
        public async Task UpdateSale_ValidSale_SaleUpdated()
        {
            using var context = new DatabaseContext(_options);
            SetUpData(context);
            var service = new SaleServiceEF(context);

            var sale = await context.Sale.FindAsync(1);
            sale.SalePrice = 120;

            await service.UpdateSale(sale);

            var updatedSale = await context.Sale.FindAsync(1);

            Assert.That(updatedSale, Is.Not.Null);
            Assert.That(updatedSale.SalePrice, Is.EqualTo(120));
        }

        [Test]
        public async Task DeleteSale_SaleExists_SaleDeleted()
        {
            using var context = new DatabaseContext(_options);
            SetUpData(context);
            var service = new SaleServiceEF(context);

            await service.DeleteSale(1);

            var sale = await context.Sale.FindAsync(1);

            Assert.That(sale, Is.Null);
        }
    }
}
