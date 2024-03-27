﻿using ElectroDocument.Controllers.AppContext;
using ElectroDocument.Models;
using ElectroDocument.Models.Cached;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            return await db.Employees.FindAsync(Convert.ToInt64(id));
        }

        public async Task<Employee?> GetEmployeeAsync(LoginData data)
        {
            Employee? employee = null;

            string? cacheResult = await cache.GetStringAsync(data.Username);
            if (cacheResult is null)
            {
                await db.Employees.LoadAsync();
                await db.EmployeeCredentials.LoadAsync();

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
                    employee = await db.Employees.FindAsync(cachedEmployee.Id);
                }
            }

            return employee;
        }

        public void RegisterUser(UsersUserModel usersModel)
        {
            try
            {
                Employee emp = new Employee() { Policy = usersModel.Policy };
                emp.Individual = new Individual { Address = usersModel.Address, Name = usersModel.Name, Patronymic = usersModel.Patronymic, PhoneNumber = usersModel.PhoneNumber };
                emp.Credentials = new EmployeeCredential { Password = usersModel.Password, UserName = usersModel.UserName };
                db.Employees.Add(emp);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        public async Task<IEnumerable<Employee?>> GetEmployees()
        {
            await db.Employees.LoadAsync();
            await db.Individuals.LoadAsync();

            return db.Employees;
        }

    }
}
