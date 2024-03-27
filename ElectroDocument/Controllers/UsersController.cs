using ElectroDocument.Controllers.AppContext;
using ElectroDocument.Controllers.Services;
using ElectroDocument.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ElectroDocument.Controllers
{
    [Authorize(Policy = "Admin")]
    public class UsersController : Controller
    {
        UserService service;

        public UsersController(UserService service)
        {
            this.service = service;
        }


        async public Task<ActionResult> Register()
        {
            return View();
        }

        async public Task<ActionResult> Index()
        {
            IEnumerable<Employee?> emps = await service.GetEmployees();
            UsersModel model = new UsersModel();
            model.profiles = emps;

            return View(model);
        }

        [HttpPost]
        async public Task<ActionResult> Add([FromForm] UsersUserModel model)
        {
            model.Password= Utils.sha256(model.Password);
            service.RegisterUser(model);

            return RedirectToAction("Users");
        }

    }
}
