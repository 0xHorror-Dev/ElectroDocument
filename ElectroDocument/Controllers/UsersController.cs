﻿using ElectroDocument.Controllers.AppContext;
using ElectroDocument.Controllers.Services;
using ElectroDocument.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ElectroDocument.Controllers
{
    [Authorize(Policy = "Admin")]
    public class UsersController : Controller
    {
        UserService service;
        IWebHostEnvironment hostingEnvironment;
        RoleService roleService;

        public UsersController(UserService service, IWebHostEnvironment hostingEnvironment, RoleService roleService)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.service = service;
            this.roleService=roleService;
        }


        async public Task<ActionResult> Register()
        {
            return View(new { roles = await roleService.GetRolesAsync()});
        }

        async public Task<ActionResult> Index(string? searchby)
        {
            IEnumerable<Employee?> emps = await service.GetEmployees();
            UsersModel model = new UsersModel();

            emps = emps.OrderBy(emp => emp.Individual.Surname);

            if(searchby is not null)
            {
                searchby = searchby.ToLower();
                emps = emps.Where(emp =>
                {
                    string fullname = $"{emp.Individual.Surname} {emp.Individual.Name} {emp.Individual.Patronymic}".ToLower();

                    return fullname.IndexOf(searchby, 0) != -1;
                });
            }

            model.profiles = emps;

            return View(model);
        }

        [HttpPost]
        async public Task<ActionResult> Add([FromForm] UsersUserModel model)
        {
            model.Password= Utils.sha256(model.Password);
            long id = await service.RegisterUser(model);

            if (Request.Form.Files.Count > 0)
            {
                var dir = $"{hostingEnvironment.WebRootPath}/UsersImages";
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


            return RedirectToAction("Users");
        }

    }
}

