using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ElectroDocument.Models
{
    public static class AuthOptions
    {
        public const string ISSUER = "ElectroDocument"; // издатель токена
        public const string AUDIENCE = "ElectroDocumentTokenClient"; // потребитель токена
        const string KEY = "mysupersecret_secretsecretsecretkey!123";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
