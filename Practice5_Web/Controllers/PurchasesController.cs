using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Practice5_Model.Models;

namespace Practice5_Web.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly ServiceFactory _service;

        public PurchasesController(ServiceFactory service)
        {
            _service = service;
        }

        // GET: Purchases
        public async Task<IActionResult> Index()
        {
            return View(await _service
                .CreatePurchaseService(HttpContext)
                .GetPurchases());
        }

        // GET: Purchases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _service
                .CreatePurchaseService(HttpContext)
                .GetPurchaseById(id);
            if (purchase == null)
            {
                return NotFound();
            }

            CreateProductSelect(purchase.ProductID);
            return View(purchase);
        }

        // GET: Purchases/Create
        public IActionResult Create()
        {
            CreateProductSelect();
            return View();
        }

        // POST: Purchases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PurchaseID,ProductID,Quantity,PurchasePrice,PurchaseDate")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                await _service.CreatePurchaseService(HttpContext)
                    .AddPurchase(purchase);
                return RedirectToAction(nameof(Index));
            }
            CreateProductSelect(purchase.ProductID);
            return View(purchase);
        }

        // GET: Purchases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id);
        }

        // POST: Purchases/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PurchaseID,ProductID,Quantity,PurchasePrice,PurchaseDate")] Purchase purchase)
        {
            if (id != purchase.PurchaseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _service.CreatePurchaseService(HttpContext)
                    .UpdatePurchase(purchase);
                return RedirectToAction(nameof(Index));
            }
            CreateProductSelect(purchase.ProductID);
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.CreatePurchaseService(HttpContext)
                .DeletePurchase(id);
            return RedirectToAction(nameof(Index));
        }

        private void CreateProductSelect()
        {
            var products = _service.CreateProductService(HttpContext).GetProducts().Result;
            ViewData["ProductID"] = new SelectList(products, "ProductId", "Name");
        }

        private void CreateProductSelect(int selected)
        {
            var products = _service.CreateProductService(HttpContext).GetProducts().Result;
            ViewData["ProductID"] = new SelectList(products, "ProductId", "Name", selected);
        }
    }
}
