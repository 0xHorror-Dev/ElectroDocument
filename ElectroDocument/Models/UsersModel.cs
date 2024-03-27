using ElectroDocument.Controllers.AppContext;

namespace ElectroDocument.Models
{
    public class UsersUserModel : ProfileModel
    {
        public string Address{ get; set; }
        public string PhoneNumber{ get; set; }

        public string Policy { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

    }

    public class UsersModel
    {
        public IEnumerable<Employee?> profiles = new List<Employee>();
    }

}
