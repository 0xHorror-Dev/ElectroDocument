using ElectroDocument.Controllers.AppContext;
using ElectroDocument.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace ElectroDocument.Controllers.Services
{
    public class RoleService
    {

        private IDistributedCache cache;
        private ElectroDocumentContext db;

        public RoleService(ElectroDocumentContext context, IDistributedCache distributedCache)
        {
            cache = distributedCache;
            db = context;
        }


        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            await db.Roles.LoadAsync();
            await db.Employees.LoadAsync();
            return db.Roles;
        }
        
        public async Task CreateRole(RoleCreateModel model)
        {
            Role newRole = new Role() { Title = model.Title, AccessLevel = model.AccessLevel};
            db.Roles.Add(newRole);
            db.SaveChanges();
        }

    }
}
