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
using Microsoft.Extensions.Hosting;

namespace ElectroDocument.Controllers
{
    [Authorize(Policy= "AdminOrUser")]
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

        [HttpPost]
        public async Task<ActionResult> ChangeProfilePicture()
        {
            long id = 0;
            Claim c = User.Claims.Where(c=> c.Type == ClaimTypes.Name).First();
            string name = c.Value;
            id = await userService.GetEmployeeId(name);

            if (Request.Form.Files.Count > 0)
            {
                var dir = $"{_env.WebRootPath}/UsersImages";
                var path = Path.Combine(dir, id.ToString() + ".jpg");
                var uploadedFile = Request.Form.Files[0];

                // Check if the file is an image
                if (uploadedFile.ContentType.StartsWith("image/"))
                {
                    // Process the image
                    using (var memoryStream = new MemoryStream())
                    {
                        await uploadedFile.CopyToAsync(memoryStream);
                        byte[] imageData = memoryStream.ToArray();

                        // Here you can save the image, process it further, or return it as needed
                        Console.WriteLine();
                        System.IO.File.WriteAllBytes(path, imageData);

                        //return File(imageData, uploadedFile.ContentType);
                    }
                }
            }

            return Redirect("Home");
        }

        //[Authorize(Policy="User")]
        [HttpGet()]
        public async Task<ActionResult> Index(string? PasswordError)
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
                ImageUrl = $"/Profile/Image/{id}",
                PasswordError = PasswordError
            });
        }

        public async Task<ActionResult> ChangePassword([FromForm]ProfilePasswordChange model)
        {
            IEnumerable<Claim> claims = User.Claims;
            Claim claim = claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First();
            string id = claim.Value;
            model.CurrentPassword = Utils.sha256(model.CurrentPassword);
            model.NewPassword = Utils.sha256(model.NewPassword);
            if (await userService.UpdatePassword(Convert.ToInt64(id), model.CurrentPassword, model.NewPassword))
            {
                return Redirect("Home");
            }
            return Redirect("/Profile?PasswordError=Error");
        }

       [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AdminChangeProfilePicture([FromForm]long id)
        {
            if (Request.Form.Files.Count > 0)
            {
                var dir = $"{_env.WebRootPath}/UsersImages";
                var path = Path.Combine(dir, id.ToString() + ".jpg");
                var uploadedFile = Request.Form.Files[0];

                // Check if the file is an image
                if (uploadedFile.ContentType.StartsWith("image/"))
                {
                    // Process the image
                    using (var memoryStream = new MemoryStream())
                    {
                        await uploadedFile.CopyToAsync(memoryStream);
                        byte[] imageData = memoryStream.ToArray();

                        // Here you can save the image, process it further, or return it as needed
                        Console.WriteLine();
                        System.IO.File.WriteAllBytes(path, imageData);

                        //return File(imageData, uploadedFile.ContentType);
                    }
                }
            }

            return Redirect("Home");
        }

        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> Edit(long id)
        {
            Employee? emp = await userService.GetEmployeeAsync(id.ToString());

            return View(new ProfileModel
            {
                Name = emp.Individual.Name,
                Surname = emp.Individual.Surname,
                Patronymic = emp.Individual.Patronymic,
                ImageUrl = $"/Profile/Image/{id}",
                Id = id
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
