using Microsoft.EntityFrameworkCore;
using Practice5_DataAccess.Data;
using Practice5_DataAccess.Service.EF;
using Practice5_Model.Models;

namespace Practice5
{
    [TestFixture]
    public class ProductServiceEFTest
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
            context.SaveChanges();
        }

        [Test]
        public async Task GetProducts_ReturnsAllProducts()
        {
            using (var context = new DatabaseContext(_options))
            {
                SetUpData(context);
                var service = new ProductServiceEF(context);

                var products = await service.GetProducts();

                Assert.That(products.Count, Is.EqualTo(2));
            }
        }

        [Test]
        public async Task GetProductById_ProductExists_ReturnsCorrectProduct()
        {
            using(var context = new DatabaseContext(_options))
            {
                SetUpData(context);
                var service = new ProductServiceEF(context);

                var product = await service.GetProductById(1);

                Assert.That(product, Is.Not.Null);
                Assert.That(product.Name, Is.EqualTo("Jeans"));
                Assert.That(product.Category, Is.EqualTo("Bottoms"));
                Assert.That(product.Price, Is.EqualTo(100));
                Assert.That(product.DateAdded, Is.LessThan(DateTime.Now));
            }
        }
    }
}
