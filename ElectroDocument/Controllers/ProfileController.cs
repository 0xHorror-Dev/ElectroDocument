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
using Pomelo.EntityFrameworkCore.MySql.Query.ExpressionTranslators.Internal;
using System.Reflection.Metadata;

namespace ElectroDocument.Controllers
{
    public record IndividualUpdateRequest(string Id, string? Surname, string? Name, string? Patronymic, string? Address, string? Phone);

    [Authorize(Policy= "AdminOrUser")]
    public class ProfileController : Controller
    {
        private UserService userService;
        private ILogger<ProfileController> logger;
        private DocsService service;
        private RoleService roleService;
        private readonly IWebHostEnvironment _env;

        public ProfileController(UserService userService, RoleService roleService, ILogger<ProfileController> logger, IWebHostEnvironment env, DocsService service)
        {
            this.userService = userService;
            this.logger = logger;
            this.service = service;
            this.roleService = roleService;
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


            if (emp.Role.AccessLevel == "Admin" || emp.Role.AccessLevel == "Editor") return Redirect($"/Profile/Edit/{id}");

            ProfileModel model = new ProfileModel
            {
                Name = emp.Individual.Name,
                Surname = emp.Individual.Surname,
                Patronymic = emp.Individual.Patronymic,
                ImageUrl = $"/Profile/Image/{id}",
                PasswordError = PasswordError is not null ? "Неверный пароль" : null,
                Position = emp.Role
            };

            model.docs = service.GetFullDocsByUserId(emp.Id);

            return View(model) ;
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


        [Authorize(Policy = "Editing")]
        [HttpPost]
        public async Task<IResult> Individual(IndividualUpdateRequest updateRequest)
        {
            await userService.UpdateEmployee(updateRequest.Id, updateRequest.Surname, updateRequest.Name, updateRequest.Patronymic, updateRequest.Address, updateRequest.Phone);
            return Results.Ok(updateRequest);
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
        [HttpPost]
        public async Task<ActionResult> AdminChangeProfilePassword([FromForm] AdminProfilePasswordChange model)
        {
            model.NewPassword = Utils.sha256(model.NewPassword);
            await userService.ForceUpdatePassword(Convert.ToInt64(model.Id), model.NewPassword);
            return Redirect("Home");
        }

        [Authorize(Policy = "Editing")]
        public async Task<ActionResult> Edit(long id)
        {
            Employee? emp = await userService.GetEmployeeAsync(id.ToString());
            IEnumerable<Role> roles = await roleService.GetRolesAsync();

            var model = new ProfileModel
            {
                Name = emp.Individual.Name,
                Surname = emp.Individual.Surname,
                Patronymic = emp.Individual.Patronymic,
                ImageUrl = $"/Profile/Image/{id}",
                Id = id,
                Position = emp.Role,
                Roles = roles
            };

            model.docs = service.GetFullDocsByUserId(emp.Id);
            return View(model);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IResult RoleUpdate([FromBody] ProfileRoleUpdate model)
        {
            service.UpdateRole(model.Id, model.Position);
            return Results.Ok(model);
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
