using Microsoft.AspNetCore.Mvc;
using Practice5_Model.Models;

namespace Practice5_Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ServiceFactory _service;

        public ProductsController(ServiceFactory service)
        {
            _service = service;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _service
                .CreateProductService(HttpContext)
                .GetProducts());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _service
                .CreateProductService(HttpContext)
                .GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Price,Category,DateAdded")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateProductService(HttpContext)
                    .AddProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Price,Category,DateAdded")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _service.CreateProductService(HttpContext)
                    .UpdateProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.CreateProductService(HttpContext)
                .DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
