using ElectroDocument.Controllers.AppContext;
using ElectroDocument.Controllers.Services;
using ElectroDocument.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ElectroDocument.Controllers
{
    public class AuthController : Controller
    {
        UserService userService;

        public AuthController(UserService userService)
        {
            this.userService = userService;
        }

        public ActionResult Index()
        {
            return View(new LayoutModel { IsAuthenticated = false });
        }

        [HttpPost(Name = "login")]
        public async Task<IResult> Login([FromBody] LoginData data)
        {
            if (data.IsValid)
            {
                Console.WriteLine($"{data.Username} {data.Password}");

                Employee? employee = await userService.GetEmployeeAsync(data);
                if (employee is null) return Results.Unauthorized();

                List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.Name, data.Username) };
                JwtSecurityToken jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)), // время действия 2 минуты
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                
                var response = new
                { 
                    accessToken = new JwtSecurityTokenHandler().WriteToken(jwt)
                };

                return Results.Json(response);
            }

            return Results.StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}
