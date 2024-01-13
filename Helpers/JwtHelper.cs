using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MedinaApi.Helpers
{
    public static class JwtHelper
    {

        /// <summary>
        /// Use the below code to generate symmetric Secret Key
        ///     var hmac = new HMACSHA256();
        ///     var key = Convert.ToBase64String(hmac.Key);
        /// </summary>

        public static string GenerateToken(Guid id, string role, string key,bool IsSuperUser,bool IsnormalUser)
        {

            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
           var lifetime =IsnormalUser?TimeSpan.FromDays(300): TimeSpan.FromHours(24);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, id.ToString() ),
                            new Claim(ClaimTypes.Role, role),
                            new ("isSuperUser", IsSuperUser.ToString())
                        }),
                Expires = DateTime.Now.Add(lifetime),
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        public static string GenerateToken(int id, string role, string key, bool IsSuperUser, bool IsnormalUser)
        {

            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
            var lifetime = IsnormalUser ? TimeSpan.FromDays(300) : TimeSpan.FromHours(24);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, id.ToString() ),
                            new Claim(ClaimTypes.Role, role),
                            new ("isSuperUser", IsSuperUser.ToString())
                        }),
                Expires = DateTime.Now.Add(lifetime),
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public static Guid? GetUserId(this HttpContext httpContext)
        {
            string? a = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (a is null or "" or " ")
            {
                return null;
            }
            return Guid.Parse(a);
        }
         public static bool IsSuperUser(this HttpContext httpContext)
        {
            string? a = httpContext.User.Claims.FirstOrDefault(c => c.Type == "isSuperUser")?.Value;
            if (a is null or "" or " ")
            {
                return false;
            }
            return bool.Parse(a);
        }
      

    }
}
