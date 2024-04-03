using ElectroDocument.Controllers.AppContext;

namespace ElectroDocument.Models
{
    public class RoleModelTableRow
    { 
        public long Id { get; set; } 
        public string Name { get; set; }
        public int EmployeeCount{ get; set; }
        public string AccessLevel { get; set; }
    }

    public class RoleModel
    {
        public IEnumerable<Role> rolesTable;
    }

    public class RoleCreateModel
    {
        public string Title { get; set; }   
        public string AccessLevel { get; set; }
    }
}
