using eagles_food_backend.Domains.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;


namespace eagles_food_backend
{
    public class AuthenticationClass
    {


            public readonly IConfiguration config;

            public AuthenticationClass(IConfiguration config)
            {
                this.config = config;
            }
            public void CreatePasswordHash(string password, out byte[] password_hash, out byte[] password_salt)
            {
                using (var hmac = new HMACSHA512())
                {
                    password_salt = hmac.Key;
                    password_hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }
            }

            //Verifying password
            public bool verifyPasswordHash(string password, byte[] hash, byte[] salt)
            {
                using (var hmac = new HMACSHA512(salt))
                {
                    var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                    return computeHash.SequenceEqual(hash);
                }

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


            public Response<string> ProcessPayment()
            {
                Response<string> res = new Response<string>();


                return res;
            }
        }
    

}
