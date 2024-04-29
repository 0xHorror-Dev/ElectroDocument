using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;

using StackExchange.Redis;

using ElectroDocument.Models;
using ElectroDocument.Controllers.Services;
using ElectroDocument.Controllers.AppContext;
using ElectroDocument.Middleware;
using System.Net;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ElectroDocumentContext>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<RoleService>();
builder.Services.AddTransient<DocsService>();
builder.Services.AddTransient<NotificationService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "redis_serv";
    options.ConfigurationOptions = new ConfigurationOptions()
    {
        EndPoints = { { "192.168.0.225:6379" },
        },
        DefaultDatabase = 0
    };
});

builder.Services.AddSession();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
            
            RoleClaimType="RolePolicy"
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("RolePolicy", new[] { "Admin" }));
    options.AddPolicy("User", policy => policy.RequireClaim("RolePolicy", new[] { "User" }));
    options.AddPolicy("AdminOrUser", policy => policy.RequireClaim("RolePolicy", new[] { "User", "Admin" }));
});

var app = builder.Build();

app.UseSession();
app.UseMiddleware<JwtMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePages(async context =>
{
    var request = context.HttpContext.Request;
    var response = context.HttpContext.Response;

    if(response.StatusCode == (int)HttpStatusCode.Unauthorized)
    {
        response.Redirect("/Auth");
    }
    else if (response.StatusCode == (int)HttpStatusCode.NotFound)
    {
        response.Redirect("/");
    }
    else if (response.StatusCode == (int)HttpStatusCode.Forbidden)
    {
        response.Redirect("/");
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
