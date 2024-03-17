using ElectroDocument.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ElectroDocument.Controllers
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
            return View(new LayoutModel { IsAuthenticated = false });
        }

        
        public IActionResult Privacy()
        {
            return View(new LayoutModel { IsAuthenticated = false });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { IsAuthenticated = false, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
