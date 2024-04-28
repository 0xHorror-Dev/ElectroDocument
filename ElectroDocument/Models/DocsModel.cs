
using ElectroDocument.Controllers.AppContext;

namespace ElectroDocument.Models
{
    public class DocsModel
    {
        public IEnumerable<Doc> docs { get; set; }
    }

    public class EmployeeContractModel
    {
        public string Fullname { get; set; }
        public long id { get; set; }
        public IEnumerable<Role> Roles { get; set; }    
    }

    public class GenerateEmployeeContractModel
    {
        public string docNumber { get; set; }
        public string salary { get; set; }
        public string position { get; set; }
        public long id { get;set; }

        public DateOnly date { get; set; }

    }

    public class GenerateMoved
    {
        public string docNumber { get; set; }
        public string position { get; set; }
        public string newPosition { get; set; }
        public string Reason { get; set; }
        public long salary { get; set; }
        public long id { get; set; }

        public DateOnly date { get; set; }

    }


    public class GenerateDismissed
    {
        public string docNumber { get; set; }
        public string Reason { get; set; }
        public string Desc { get; set; }
        public long id { get; set; }

        public DateOnly date { get; set; }

    }

    public class GenerateWeekend
    {
        public string docNumber { get; set; }
        public DateOnly End{ get; set; }
        public string? Reason { get; set; }
        public long id { get; set; }

        public DateOnly date { get; set; }

    }

    public class GenerateRoleCreate
    {
        public string docNumber { get; set; }
        public string Role { get; set; }
        public string Salary{ get; set; }
        public long id { get; set; }

        public DateOnly date { get; set; }

    }

    public class GenerateEncourage
    {
        public string docNumber { get; set; }
        public string? Reason { get; set; }
        public string Desc { get; set; }
        public string Role { get; set; }
        public int salary { get; set; }
        public long id { get; set; }

        public DateOnly date { get; set; }

    }
}
