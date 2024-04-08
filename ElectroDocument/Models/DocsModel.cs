
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
}
