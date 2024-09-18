using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Practice5_Model.Models;

namespace Practice5_Web.Controllers
{
    public class InventoriesController : Controller
    {
        private readonly ServiceFactory _service;

        public InventoriesController(ServiceFactory service)
        {
            _service = service;
        }

        // GET: Inventories
        public async Task<IActionResult> Index()
        {
            return View(await _service
                .CreateInventoryService(HttpContext)
                .GetInventory());
        }

        // GET: Inventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _service
                .CreateInventoryService(HttpContext)
                .GetInventoryById(id);
            if (inventory == null)
            {
                return NotFound();
            }

            CreateProductSelect(inventory.ProductID);
            return View(inventory);
        }

        // GET: Inventories/Create
        public IActionResult Create()
        {
            CreateProductSelect();
            return View();
        }

        // POST: Inventories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InventoryID,ProductID,Stock,DateModified")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateInventoryService(HttpContext)
                    .AddInventory(inventory);
                return RedirectToAction(nameof(Index));
            }
            CreateProductSelect(inventory.ProductID);
            return View(inventory);
        }

        // GET: Inventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id);
        }

        // POST: Inventories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InventoryID,ProductID,Stock,DateModified")] Inventory inventory)
        {
            if (id != inventory.InventoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _service.CreateInventoryService(HttpContext)
                    .UpdateInventory(inventory);
                return RedirectToAction(nameof(Index));
            }
            CreateProductSelect(inventory.ProductID);
            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.CreateInventoryService(HttpContext)
                .DeleteInventory(id);
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
