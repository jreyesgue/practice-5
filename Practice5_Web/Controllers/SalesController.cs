using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practice5_Model.Models;

namespace Practice5_Web.Controllers
{
    public class SalesController : Controller
    {
        private readonly ServiceFactory _service;

        public SalesController(ServiceFactory service)
        {
            _service = service;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            return View(await _service
                .CreateSaleService(HttpContext)
                .GetSales());
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _service
                .CreateSaleService(HttpContext)
                .GetSaleById(id);
            if (sale == null)
            {
                return NotFound();
            }

            CreateProductSelect(sale.ProductID);
            return View(sale);
        }

        // GET: Sales/Create
        public IActionResult Create()
        {
            CreateProductSelect();
            return View();
        }

        // POST: Sales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SaleID,ProductID,Quantity,SalePrice,SaleDate")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateSaleService(HttpContext)
                    .AddSale(sale);
                return RedirectToAction(nameof(Index));
            }
            CreateProductSelect(sale.ProductID);
            return View(sale);
        }

        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id);
        }

        // POST: Sales/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SaleID,ProductID,Quantity,SalePrice,SaleDate")] Sale sale)
        {
            if (id != sale.SaleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _service.CreateSaleService(HttpContext).UpdateSale(sale);
                return RedirectToAction(nameof(Index));
            }
            CreateProductSelect(sale.ProductID);
            return View(sale);
        }

        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.CreateSaleService(HttpContext)
                .DeleteSale(id);
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
