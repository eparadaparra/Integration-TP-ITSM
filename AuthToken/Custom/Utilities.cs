using AuthToken.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthToken.Custom
{
    public class Utilities
    {
        private readonly string _key;

        public Utilities(IConfiguration config)
        {
            _key = config.GetSection("Settings").GetSection("key").ToString();
        }

        public string encriptarSHA256(string txt)
        {
            using (SHA256 sha256Hash = SHA256.Create()){
            
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(txt));

                StringBuilder builder = new StringBuilder();
                for(int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public AuthResult triggerJWT(AuthUsr auth)
        {
            var userClaims = new[]
            {
                new Claim(ClaimTypes.Email, auth.email)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken(
                    claims: userClaims,
                    signingCredentials: credentials
                );

            return new AuthResult { token = new JwtSecurityTokenHandler().WriteToken(jwtConfig) };
        }
    }
}
