using Microsoft.AspNetCore.Mvc;
using ElectroDocument.Models;

namespace ElectroDocument.Controllers
{
    public class TabelController : Controller
    {
        public IActionResult Index()
        {


            return View(new TabelModel { DT = new DateTime(2024, 02, 02, 0, 0, 0, 0, 0) });
        }
    }
}
