namespace ElectroDocument.Models
{
    public class LoginData
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsValid { 
            get
            {
                return Username != null && Password != null;
            } 
        }

    }
}
