using Domain.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Common.JwtToken
{
    public class JwtGenerate
    {

        static public string GenerateJWT(LoginView loginview, IConfiguration _config, string Role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            bool isAlive = true;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,loginview.Email),
                new Claim(ClaimTypes.Role, Role),
                new Claim(ClaimTypes.Email, loginview.Email)
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(5), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}

