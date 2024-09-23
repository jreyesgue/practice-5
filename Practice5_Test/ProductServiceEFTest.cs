using Microsoft.EntityFrameworkCore;
using Practice5_DataAccess.Data;
using Practice5_DataAccess.Service.EF;
using Practice5_Model.Models;

namespace Practice5_Test
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
            using var context = new DatabaseContext(_options);
            SetUpData(context);
            var service = new ProductServiceEF(context);

            var products = await service.GetProducts();

            Assert.That(products.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetProductById_ProductExists_ReturnsCorrectProduct()
        {
            using var context = new DatabaseContext(_options);
            SetUpData(context);
            var service = new ProductServiceEF(context);

            var product = await service.GetProductById(1);

            Assert.That(product, Is.Not.Null);
            Assert.That(product.Name, Is.EqualTo("Jeans"));
            Assert.That(product.Category, Is.EqualTo("Bottoms"));
            Assert.That(product.Price, Is.EqualTo(100));
            Assert.That(product.DateAdded, Is.LessThan(DateTime.Now));
        }

        [Test]
        public async Task AddProduct_ValidProduct_ProductAdded()
        {
            using var context = new DatabaseContext(_options);
            var service = new ProductServiceEF(context);
            var newProduct = new Product
            {
                ProductId = 3,
                Name = "Jacket",
                Category = "Tops",
                Price = 100,
                DateAdded = DateTime.Now
            };

            await service.AddProduct(newProduct);

            var product = await context.Product.FindAsync(3);

            Assert.That(product, Is.Not.Null);
            Assert.That(product.Name, Is.EqualTo("Jacket"));
            Assert.That(product.Category, Is.EqualTo("Tops"));
            Assert.That(product.Price, Is.EqualTo(100));
            Assert.That(product.DateAdded, Is.LessThan(DateTime.Now));
        }

        [Test]
        public async Task UpdateProduct_ValidProduct_ProductUpdated()
        {
            using var context = new DatabaseContext(_options);
            SetUpData(context);
            var service = new ProductServiceEF(context);

            var product = await context.Product.FindAsync(1);
            product.Name = "Trousers";
            product.Price = 120;

            await service.UpdateProduct(product);

            var updatedProduct = await context.Product.FindAsync(1);

            Assert.That(updatedProduct, Is.Not.Null);
            Assert.That(updatedProduct.Name, Is.EqualTo("Trousers"));
            Assert.That(updatedProduct.Price, Is.EqualTo(120));
        }

        [Test]
        public async Task DeleteProduct_ProductExists_ProductDeleted()
        {
            using var context = new DatabaseContext(_options);
            SetUpData(context);
            var service = new ProductServiceEF(context);

            await service.DeleteProduct(1);

            var product = await context.Product.FindAsync(1);

            Assert.That(product, Is.Null);
        }
    }
}
