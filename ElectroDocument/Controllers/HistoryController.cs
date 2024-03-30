using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectroDocument.Controllers
{
    [Authorize(Policy = "Admin")]
    public class HistoryController : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }
    }
}
