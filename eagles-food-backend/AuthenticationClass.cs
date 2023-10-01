using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using eagles_food_backend.Domains.Models;

using Microsoft.IdentityModel.Tokens;

using BC = BCrypt.Net.BCrypt;

namespace eagles_food_backend
{
    public class AuthenticationClass
    {
        public readonly IConfiguration config;

        public AuthenticationClass(IConfiguration config)
        {
            this.config = config;
        }
        public void CreatePasswordHash(string password, out string password_hash)
        {
            // BCrypt maintains internal salt
            password_hash = BC.HashPassword(password);
        }

        //Verifying password
        public bool verifyPasswordHash(string password, string hash)
        {
            return BC.Verify(password, hash);
        }

        //Creating Token
        public string createToken(string id, string role)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, id),
                new Claim(ClaimTypes.Role, role),
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config.GetSection("JwtSettings:secretKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),//Expires 24 hours after creation time
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public string createResetToken(string user_id)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user_id),
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config.GetSection("JwtSettings:secretKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),//Expires 24 hours after creation time
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }


        public Response<string> ProcessPayment()
        {
            Response<string> res = new Response<string>();

            return res;
        }
    }


}