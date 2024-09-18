using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Practice5_Model.Models;

namespace Practice5_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult DataAccess([FromBody] string dataAccess)
        {
            HttpContext.Session.SetString("DataAccess", dataAccess);

            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult DataAccess()
        {
            var dataAccess = HttpContext.Session.GetString("DataAccess") ?? "EF";

            return Json(new { dataAccess });
        }
    }
}
