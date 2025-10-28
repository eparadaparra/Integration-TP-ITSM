using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using tasksAction.Models;

namespace tasksAction.Custom
{
    public class Utilities
    {
        private readonly IConfiguration _config;
        public Utilities(IConfiguration config)
        {
            _config = config;
        }

        public string EncryptSHA256(string txt)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                //Codifica Texto y lo convierte en arreglo de bytes
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(txt));

                //Crea el arreglo de bytes en una cadena de Texto
                StringBuilder builder = new StringBuilder();
                for(int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string triggerJWT(AuthUsr objeto)
        {
            // Crear Información de Usuario
            var userClaims = new[]
            {
                new Claim(ClaimTypes.Email, objeto.email)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // Creamos detalle de Token
            var jwtConfig = new JwtSecurityToken(
                    claims: userClaims,
                    // expires: DateTime.UtcNow.AddMinutes(10), //Token expira en 10 minutos
                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}
