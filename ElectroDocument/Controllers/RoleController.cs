using ElectroDocument.Controllers.AppContext;
using ElectroDocument.Controllers.Services;
using ElectroDocument.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Security.Claims;

namespace ElectroDocument.Controllers
{
	public class RoleController : Controller
	{
		private RoleService roleService;

		public RoleController(RoleService roleService) 
		{
			this.roleService = roleService;
		}


		public async Task<IActionResult> Index()
		{
            //List<RoleModelTableRow> rows = new List<RoleModelTableRow>();

            //foreach(Role r in await roleService.GetRolesAsync())
            //{
            //	rows.Add(new RoleModelTableRow { Id = r.Id, AccessLevel = r.AccessLevel, Name = r.Title, EmployeeCount = r.Employees.Count });

            //         }

            return View(new RoleModel { rolesTable = await roleService.GetRolesAsync()});
		}

		public async Task<IActionResult> Create()
		{
            IEnumerable<Claim> claims = User.Claims;
            Claim claim = claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).First();
            string id = claim.Value;

			var model = new
			{
				Id = Convert.ToInt64(id),
			};

            return View(model);
		}

		public async Task<IResult> Add([FromBody] RoleCreateModel model)
		{
			roleService.CreateRole(model);

			return Results.Ok(model);
        }
	}
}
