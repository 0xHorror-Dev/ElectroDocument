using ElectroDocument.Controllers.AppContext;
using ElectroDocument.Controllers.Services;
using ElectroDocument.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spire.Doc;
using System.Reflection.Metadata;
using System.Security.Claims;
using Document = Spire.Doc.Document;


namespace ElectroDocument.Controllers
{
    [Authorize(Policy = "Admin")]
    public class DocsController : Controller
    {
        DocsService service;
        UserService userService;
        RoleService roleService;
        public DocsController(DocsService service, UserService userService, RoleService roleService)
        {
            this.service = service;
            this.userService = userService;
            this.roleService=roleService;
        }

        public async Task<IActionResult> Index(long? id)
        {
            DocsModel model;
            model = new DocsModel();
            if (id is not null)
            {
                model.docs = service.GetDocsByUserId(id.Value);
            }
            else
            {
                IEnumerable<Claim> claims = User.Claims;
                Claim claim = claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First();
                string dbid = claim.Value;
                Employee? emp = await userService.GetEmployeeAsync(dbid);
                model.docs = service.GetDocsByUserId(emp.Id);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<FileContentResult> GenerateDocument(string id)
        {
            Document doc = await service.GenerateDocument(Convert.ToInt64(id));

            using (MemoryStream stream = new MemoryStream())
            {
                doc.SaveToStream(stream, FileFormat.Docx);

                // Do something with the MemoryStream if needed
                // For example, you can convert it to a byte array
                byte[] byteArray = stream.ToArray();
                var contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                // Now you can use the byteArray for further processing
                return File(byteArray, contentType);
            }
        }

        [HttpGet]
        public async Task<ActionResult> EmployeeContract(string? id)
        {
            Employee emp = await userService.GetEmployeeAsync(id);

            EmployeeContractModel model = new EmployeeContractModel();
            model.Fullname = $"{emp.Individual.Name} {emp.Individual.Surname} {emp.Individual.Patronymic}";
            model.id = Convert.ToInt64(id);
            model.Roles = await roleService.GetRolesAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IResult> GenerateEmployeeContract([FromForm] GenerateEmployeeContractModel model)
        {
            Employee emp = await userService.GetEmployeeAsync(model.id.ToString());

            EmployeeContactData data = new EmployeeContactData();
            data.Salary = Convert.ToInt32(model.salary);
            data.Number = Convert.ToInt32(model.docNumber);
            data.Date = model.date;
            data.Role = Convert.ToInt64(model.position);

            service.CreateDocument(model.id, data);
            return Results.StatusCode(StatusCodes.Status200OK);
        }

    }
}
