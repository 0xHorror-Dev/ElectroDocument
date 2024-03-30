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
    }

    public class ProfilePasswordChange
    {
        public string NewPassword { get; set; }
        public string CurrentPassword { get; set; }
    }
}
