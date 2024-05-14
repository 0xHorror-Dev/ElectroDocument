using ElectroDocument.Controllers.AppContext;
using ElectroDocument.Controllers.Services;
using ElectroDocument.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spire.Doc;
using System.Reflection.Metadata;
using System.Security.Claims;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using System.IO.Packaging;
using System.Xml.Linq;
using Document = Spire.Doc.Document;
using System.Text;
using System.IO;
using System.Xml.Serialization;

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

        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> Index(long? id)
        {

            DocsModel model;
            model = new DocsModel();
            if (id is not null)
            {
                model.docs = service.GetFullDocsByUserId(id.Value);
            }
            else
            {
                IEnumerable<Claim> claims = User.Claims;
                Claim claim = claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First();
                string dbid = claim.Value;
                Employee? emp = await userService.GetEmployeeAsync(dbid);
                model.docs = service.GetFullDocsByUserId(emp.Id);
            }
            return View(model);
        }


        public async Task<IActionResult> View(long? id)
        {
            var model = new
            {
                Id = id.Value
            };
            return base.View(model);
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
        public async Task<FileContentResult> GenerateDocumentPDF(string id)
        {

            Document doc = await service.GenerateDocument(Convert.ToInt64(id));
            
            using (MemoryStream docxStream = new MemoryStream())
            {
                try
                {
                    doc.SaveToStream(docxStream, FileFormat.Docx);
                    var source = Package.Open(docxStream);
                    var document = WordprocessingDocument.Open(source);
                    HtmlConverterSettings settings = new HtmlConverterSettings();
                    XElement html = HtmlConverter.ConvertToHtml(document, settings);
                    byte[] result = Encoding.UTF8.GetBytes(html.ToString());
                    var contentType = "text/html;charset=utf-8";
                    return File(result, contentType);
                }
                catch 
                {
                    doc.SaveToStream(docxStream, FileFormat.PDF);
                    byte[] byteArray = docxStream.ToArray();
                    var contentType = "application/pdf";
                    return File(byteArray, contentType);
                }

            }
        }

        [Authorize(Policy = "Editing")]
        [HttpGet]
        public async Task<ActionResult> EmployeeContract(string? id, string? edit)
        {
            Employee emp = await userService.GetEmployeeAsync(id);

            EmployeeContractModel model = new EmployeeContractModel();
            model.Fullname = $"{emp.Individual.Name} {emp.Individual.Surname} {emp.Individual.Patronymic}";
            model.id = Convert.ToInt64(id);
            model.Roles = await roleService.GetRolesAsync();

            if (edit is not null)
            {
                model.DocId = Convert.ToInt64(edit);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> EditDoc(string id)
        {
            Doc targetDoc = service.GetDocById(Convert.ToInt64(id));
            switch (targetDoc.DocType)
            {
                case (sbyte?)DocumentTypes.Moved: return Redirect($"/Docs/Moved?id={targetDoc.EmployeeId}&Edit={id}");
                case (sbyte?)DocumentTypes.Weekend: return Redirect($"/Docs/Weekend?id={targetDoc.EmployeeId}&Edit={id}");
                case (sbyte?)DocumentTypes.Dismissed: return Redirect($"/Docs/Dismissed?id={targetDoc.EmployeeId}&Edit={id}");
                case (sbyte?)DocumentTypes.EmploymentContract: return Redirect($"/Docs/EmployeeContract?id={targetDoc.EmployeeId}&Edit={id}");
                case (sbyte?)DocumentTypes.Encouragement: return Redirect($"/Docs/Encourage?id={targetDoc.EmployeeId}&Edit={id}");
            }

            return Redirect("/Home");
        }


        [Authorize(Policy = "Editing")]
        [HttpGet]
        public async Task<ActionResult> Moved(string? id, string? edit)
        {
            Employee emp = await userService.GetEmployeeAsync(id);

            EmployeeContractModel model = new EmployeeContractModel();
            model.Fullname = $"{emp.Individual.Name} {emp.Individual.Surname} {emp.Individual.Patronymic}";
            model.id = Convert.ToInt64(id);
            model.Roles = await roleService.GetRolesAsync();

            if (edit is not null)
            {
                model.DocId = Convert.ToInt64(edit);
            }

            return View(model);
        }

        [Authorize(Policy = "Editing")]
        [HttpGet]
        public async Task<ActionResult> Dismissed(string? id, string? edit)
        {
            Employee emp = await userService.GetEmployeeAsync(id);

            EmployeeContractModel model = new EmployeeContractModel();
            model.Fullname = $"{emp.Individual.Name} {emp.Individual.Surname} {emp.Individual.Patronymic}";
            model.id = Convert.ToInt64(id);
            model.Roles = await roleService.GetRolesAsync();

            if (edit is not null)
            {
                model.DocId = Convert.ToInt64(edit);
            }

            return View(model);
        }

        [Authorize(Policy = "Editing")]
        [HttpGet]
        public async Task<ActionResult> Weekend(string? id, string? edit)
        {
            Employee emp = await userService.GetEmployeeAsync(id);

            EmployeeContractModel model = new EmployeeContractModel();
            model.Fullname = $"{emp.Individual.Name} {emp.Individual.Surname} {emp.Individual.Patronymic}";
            model.id = Convert.ToInt64(id);
            model.Roles = await roleService.GetRolesAsync();

            if (edit is not null)
            {
                model.DocId = Convert.ToInt64(edit);
            }

            return View(model);
        }

        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> RoleCreate(string? id, string? edit)
        {
            Employee emp = await userService.GetEmployeeAsync(id);

            EmployeeContractModel model = new EmployeeContractModel();
            model.Fullname = $"{emp.Individual.Name} {emp.Individual.Surname} {emp.Individual.Patronymic}";
            model.id = Convert.ToInt64(id);
            model.Roles = await roleService.GetRolesAsync();


            if (edit is not null)
            {
                model.DocId = Convert.ToInt64(edit);
            }

            return View(model);
        }

        [Authorize(Policy = "Editing")]
        public async Task<ActionResult> Encourage(string? id, string? edit)
        {
            Employee emp = await userService.GetEmployeeAsync(id);

            EmployeeContractModel model = new EmployeeContractModel();
            model.Fullname = $"{emp.Individual.Name} {emp.Individual.Surname} {emp.Individual.Patronymic}";
            model.id = Convert.ToInt64(id);
            model.Roles = await roleService.GetRolesAsync();


            if (edit is not null)
            {
                model.DocId = Convert.ToInt64(edit);
            }

            return View(model);
        }

        [Authorize(Policy = "Editing")]
        [HttpPost]
        public async Task<IResult> GenerateEmployeeContract([FromForm] GenerateEmployeeContractModel model)
        {
            Employee emp = await userService.GetEmployeeAsync(model.id.ToString());

            EmployeeContactData data = new EmployeeContactData();
            data.Salary = Convert.ToInt32(model.salary);
            data.Number = Convert.ToInt32(model.docNumber);
            data.Date = model.date;
            data.Role = model.position;

            if (model.docId is not null)
            {
                if (model.editorId is null) return Results.StatusCode(StatusCodes.Status400BadRequest);

                service.EditDocument(model.id, model.editorId.Value, model.docId.Value, data);
            }
            else
            {
                service.CreateDocument(model.id, data);
            }

            return Results.StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Policy = "Editing")]
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
            if(model.docId is not null)
            {
                if(model.editorId is null) return Results.StatusCode(StatusCodes.Status400BadRequest);

                service.EditDocument(model.id, model.editorId.Value, model.docId.Value, data);
            }
            else
            {
                service.CreateDocument(model.id, data);
            }
            return Results.StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Policy = "Editing")]
        [HttpPost]
        public async Task<IResult> GenerateDismissed([FromForm] GenerateDismissed model)
        {
            Employee emp = await userService.GetEmployeeAsync(model.id.ToString());

            DismissedData data = new DismissedData();
            data.Number = Convert.ToInt32(model.docNumber);
            data.Date = model.date;
            data.Reason = model.Reason;
            data.Desc = model.Desc;

            if (model.docId is not null)
            {
                if (model.editorId is null) return Results.StatusCode(StatusCodes.Status400BadRequest);

                service.EditDocument(model.id, model.editorId.Value, model.docId.Value, data);
            }
            else
            {
                service.CreateDocument(model.id, data);
            }

            return Results.StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Policy = "Editing")]
        [HttpPost]
        public async Task<IResult> GenerateWeekend([FromForm] GenerateWeekend model)
        {
            Employee emp = await userService.GetEmployeeAsync(model.id.ToString());

            WeekendData data = new WeekendData();
            data.Number = Convert.ToInt32(model.docNumber);
            data.Date = model.date;
            data.Reason = model.Reason;
            data.End = model.End;

            if (model.docId is not null)
            {
                if (model.editorId is null) return Results.StatusCode(StatusCodes.Status400BadRequest);

                service.EditDocument(model.id, model.editorId.Value, model.docId.Value, data);
            }
            else
            {
                service.CreateDocument(model.id, data);
            }
            return Results.StatusCode(StatusCodes.Status200OK);
        }
        [Authorize(Policy = "Editing")]
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


            return Results.StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Policy = "Editing")]
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
            if (model.docId is not null)
            {
                if (model.editorId is null) return Results.StatusCode(StatusCodes.Status400BadRequest);

                service.EditDocument(model.id, model.editorId.Value, model.docId.Value, data);
            }
            else
            {
                service.CreateDocument(model.id, data);
            }
            return Results.StatusCode(StatusCodes.Status200OK);
        }

    }
}
