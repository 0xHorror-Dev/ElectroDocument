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
    }
}
