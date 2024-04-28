using ElectroDocument.Controllers.AppContext;
using ElectroDocument.Models;
using ElectroDocument.Models.Cached;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Caching.Distributed;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ElectroDocument.Controllers.Services
{

    public class UserService
    {
        private IDistributedCache cache;
        private ElectroDocumentContext db;

        public UserService(ElectroDocumentContext context, IDistributedCache distributedCache)
        {
            cache = distributedCache;
            db = context;
        }

        public async Task<Employee?> GetEmployeeAsync(string id)
        {
            db.EmployeeCredentials.Load();
            db.Individuals.Load();
            db.Roles.Load();
            return await db.Employees.FindAsync(Convert.ToInt64(id));
        }

        public async Task<long> GetEmployeeId(string Username)
        {
            await db.Employees.LoadAsync();
            await db.EmployeeCredentials.LoadAsync();
            return db.Employees.Where(emp => emp.Credentials.UserName == Username).First().Id;
        }

        public async Task<bool> UpdatePassword(long id, string currentPassword, string newPassword)
        {
            await db.Employees.LoadAsync();
            await db.EmployeeCredentials.LoadAsync();
            Employee emp = await db.Employees.FindAsync(id);

            if (emp.Credentials.Password == currentPassword)
            {
                emp.Credentials.Password = newPassword;
                db.SaveChanges();
                return true;
            }
            else return false;
        }

        public async Task<Employee?> GetEmployeeAsync(LoginData data)
        {
            Employee? employee = null;

            string? cacheResult = await cache.GetStringAsync(data.Username);
            if (cacheResult is null)
            {
                await db.Employees.LoadAsync();
                await db.EmployeeCredentials.LoadAsync();
                await db.Roles.LoadAsync();

                employee = await db.Employees.Where(employee => employee.Credentials.UserName == data.Username && employee.Credentials.Password == data.Password).FirstAsync();

                CachedEmployee cached = new CachedEmployee { Id = employee.Id, Password = employee.Credentials.Password };

                await cache.SetStringAsync(data.Username, JsonSerializer.Serialize(cached), new
                DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
            else
            {
                CachedEmployee cachedEmployee = JsonSerializer.Deserialize<CachedEmployee?>(cacheResult);
                if(cachedEmployee.Password == data.Password)
                {
                    await db.Employees.LoadAsync();
                    await db.EmployeeCredentials.LoadAsync();
                    await db.Roles.LoadAsync();
                    employee = await db.Employees.FindAsync(cachedEmployee.Id);
                }
            }

            return employee;
        }

        public string GetRoleTitleById(long id)
        {
            return db.Roles.Find(id).Title;
        }

        public async Task<long> RegisterUser(UsersUserModel usersModel)
        {
            try
            {
                Employee emp = new Employee() { RoleId = usersModel.Role};
                emp.Individual = new Individual { Address = usersModel.Address, Name = usersModel.Name, Patronymic = usersModel.Patronymic, PhoneNumber = usersModel.PhoneNumber, Surname = usersModel.Surname };
                emp.Credentials = new EmployeeCredential { Password = usersModel.Password, UserName = usersModel.UserName };
                db.Employees.Add(emp);
                var task = db.SaveChangesAsync();
                task.Wait();
                db.Employees.Load();
                db.EmployeeCredentials.Load();
                long empId = db.Employees.Where(emp=>emp.Credentials.UserName ==  usersModel.UserName).First().Id;
                await db.SaveChangesAsync();
                return empId;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return 0;
        }

        public async Task<IEnumerable<Employee?>> GetEmployees()
        {
            await db.Employees.LoadAsync();
            await db.Individuals.LoadAsync();
            await db.Roles.LoadAsync();

            return db.Employees;
        }

    }
}
