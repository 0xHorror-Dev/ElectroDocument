using ElectroDocument.Controllers.AppContext;
using ElectroDocument.Controllers.Services;
using ElectroDocument.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Web;
using System.Security.Claims;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;

namespace ElectroDocument.Controllers
{
    [Authorize(Policy="User")]
    public class ProfileController : Controller
    {
        private UserService userService;
        private ILogger<ProfileController> logger;
        private readonly IWebHostEnvironment _env;

        public ProfileController(UserService userService, ILogger<ProfileController> logger, IWebHostEnvironment env)
        {
            this.userService = userService;
            this.logger = logger;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult> Image(string id)
        {
            var dir = $"/UsersImages";
            var path = Path.Combine(dir, id + ".jpg");
            logger.LogDebug("loading image by path {0}", path);
            return base.File(path, "image/jpeg");
        }

        //[Authorize(Policy="User")]
        [ResponseCache(Duration=2)]
        public async Task<ActionResult> Index()
        {
            IEnumerable<Claim> claims = User.Claims;
            Claim claim = claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First();
            string id = claim.Value;
            Employee? emp = await userService.GetEmployeeAsync(id);

            return View(new ProfileModel
            {
                Name = emp.Individual.Name,
                Surname = emp.Individual.Surname,
                Patronymic = emp.Individual.Patronymic,
                ImageUrl = $"/Profile/Image/{id}"
            });
        }

        // GET: ProfileController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProfileController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProfileController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProfileController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProfileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProfileController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProfileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
