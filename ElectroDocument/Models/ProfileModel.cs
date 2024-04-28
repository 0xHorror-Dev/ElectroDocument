using ElectroDocument.Controllers.AppContext;

namespace ElectroDocument.Models
{
    public class ProfileModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Patronymic { get; set; }
        public string ImageUrl { get; set; }

        public string FullName {
            get
            {
                return $"{Surname} {Name} {Patronymic ?? ""}";
            } 
        }

        public long? Id { get; set; }
        public string? PasswordError { get; set; }

        public Role Position { get; set; }

        public IEnumerable<Doc> docs { get; set; }
        public IEnumerable<Role> Roles { get; set; }

    }

    public class ProfilePasswordChange
    {
        public string NewPassword { get; set; }
        public string CurrentPassword { get; set; }
    }

    public class ProfilePasswordChangeAdmin : ProfilePasswordChange
    {
        public long Id { get; set; }
    }



    public class ProfileRoleUpdate
    {
        public long Id { get; set; }
        public long Position { get; set; }
    }

}
