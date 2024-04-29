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
    [Authorize]
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

        [Authorize(Policy = "Admin")]
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

        [Authorize(Policy = "Admin")]
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

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<ActionResult> Moved(string? id)
        {
            Employee emp = await userService.GetEmployeeAsync(id);

            EmployeeContractModel model = new EmployeeContractModel();
            model.Fullname = $"{emp.Individual.Name} {emp.Individual.Surname} {emp.Individual.Patronymic}";
            model.id = Convert.ToInt64(id);
            model.Roles = await roleService.GetRolesAsync();

            return View(model);
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<ActionResult> Dismissed(string? id)
        {
            Employee emp = await userService.GetEmployeeAsync(id);

            EmployeeContractModel model = new EmployeeContractModel();
            model.Fullname = $"{emp.Individual.Name} {emp.Individual.Surname} {emp.Individual.Patronymic}";
            model.id = Convert.ToInt64(id);
            model.Roles = await roleService.GetRolesAsync();

            return View(model);
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<ActionResult> Weekend(string? id)
        {
            Employee emp = await userService.GetEmployeeAsync(id);

            EmployeeContractModel model = new EmployeeContractModel();
            model.Fullname = $"{emp.Individual.Name} {emp.Individual.Surname} {emp.Individual.Patronymic}";
            model.id = Convert.ToInt64(id);
            model.Roles = await roleService.GetRolesAsync();

            return View(model);
        }

        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> RoleCreate(string? id)
        {
            Employee emp = await userService.GetEmployeeAsync(id);

            EmployeeContractModel model = new EmployeeContractModel();
            model.Fullname = $"{emp.Individual.Name} {emp.Individual.Surname} {emp.Individual.Patronymic}";
            model.id = Convert.ToInt64(id);
            model.Roles = await roleService.GetRolesAsync();

            return View(model);
        }

        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> Encourage(string? id)
        {
            Employee emp = await userService.GetEmployeeAsync(id);

            EmployeeContractModel model = new EmployeeContractModel();
            model.Fullname = $"{emp.Individual.Name} {emp.Individual.Surname} {emp.Individual.Patronymic}";
            model.id = Convert.ToInt64(id);
            model.Roles = await roleService.GetRolesAsync();

            return View(model);
        }

        [Authorize(Policy = "Admin")]
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

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IResult> GenerateMoved([FromForm] GenerateMoved model)
        {
            Employee emp = await userService.GetEmployeeAsync(model.id.ToString());

            RoleMoveData data = new RoleMoveData();
            data.Number = Convert.ToInt32(model.docNumber);
            data.Date = model.date;
            data.OldRole = model.position;
            data.NewRole = model.newPosition;
            data.Reason = model.Reason;
            data.Salary = model.salary;

            service.CreateDocument(model.id, data);
            return Results.StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IResult> GenerateDismissed([FromForm] GenerateDismissed model)
        {
            Employee emp = await userService.GetEmployeeAsync(model.id.ToString());

            DismissedData data = new DismissedData();
            data.Number = Convert.ToInt32(model.docNumber);
            data.Date = model.date;
            data.Reason = model.Reason;
            data.Desc = model.Desc;

            service.CreateDocument(model.id, data);
            return Results.StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IResult> GenerateWeekend([FromForm] GenerateWeekend model)
        {
            Employee emp = await userService.GetEmployeeAsync(model.id.ToString());

            WeekendData data = new WeekendData();
            data.Number = Convert.ToInt32(model.docNumber);
            data.Date = model.date;
            data.Reason = model.Reason;
            data.End = model.End;

            service.CreateDocument(model.id, data);
            return Results.StatusCode(StatusCodes.Status200OK);
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IResult> GenerateRoleCreation([FromForm] GenerateRoleCreate model)
        {
            Employee emp = await userService.GetEmployeeAsync(model.id.ToString());


            RoleCreationData data = new RoleCreationData();
            data.Number = Convert.ToInt32(model.docNumber);
            data.Date = model.date;
            data.NewRole = model.Role;
            data.Salary = Convert.ToInt32(model.Salary);
            data.Resposible = model.responsible;

            service.CreateDocument(model.id, data);
            return Results.StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IResult> GenerateEncourage([FromForm] GenerateEncourage model)
        {
            Employee emp = await userService.GetEmployeeAsync(model.id.ToString());


            EncourageData data = new EncourageData();
            data.Number = Convert.ToInt32(model.docNumber);
            data.Date = model.date;
            data.Role = model.Role;
            data.Reason = model.Reason;
            data.Desc = model.Desc;
            data.Salary = Convert.ToInt32(model.salary);

            service.CreateDocument(model.id, data);
            return Results.StatusCode(StatusCodes.Status200OK);
        }

    }
}
