using ElectroDocument.Controllers.AppContext;
using ElectroDocument.Controllers.Services;
using ElectroDocument.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Unicode;

namespace ElectroDocument.Controllers
{
    public class AuthController : Controller
    {
        private UserService userService;

        public AuthController(UserService userService)
        {
            this.userService = userService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost(Name = "login")]
        public async Task<IResult> Login([FromBody] LoginData data)
        {
            if (data.IsValid)
            {
                data.Password = Utils.sha256(data.Password);

                Employee? employee = await userService.GetEmployeeAsync(data);
                if (employee is null) return Results.Unauthorized();
                
                /*
                    TO DO:
                    Load role policy from database
                 */

                List<Claim> claims = new List<Claim> { 
                    new Claim(ClaimTypes.Name, data.Username), 
                    new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()), 
                    new Claim("RolePolicy", employee.Role.AccessLevel) 
                };

                JwtSecurityToken jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                
                var response = new
                { 
                    accessToken = new JwtSecurityTokenHandler().WriteToken(jwt)
                };

                HttpContext.Session.SetString("accessToken", response.accessToken);

                return Results.Json(response);
            }

            return Results.StatusCode(StatusCodes.Status400BadRequest);
        }


        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("accessToken");
            return RedirectToAction("Index", "Auth");
        }

    }
}
