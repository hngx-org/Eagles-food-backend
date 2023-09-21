using AutoMapper;
using eagles_food_backend.Data;
using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace eagles_food_backend.Services.UserServices
{
    public class UserService : IUserRepository
    {
        private readonly LunchDbContext db_context;
        private readonly IMapper mapper;
        private readonly AuthenticationClass authentication;
        private readonly IPasswordHasher<CreateUserDTO> _passwordHasher;

        public UserService(LunchDbContext db_context, IMapper mapper, AuthenticationClass authentication, IPasswordHasher<CreateUserDTO> passwordHasher)
        {
            this.db_context = db_context;
            this.mapper = mapper;
            this.authentication = authentication;
            _passwordHasher = passwordHasher;

        }
        public async Task<Response<User>> CreateUser(CreateUserDTO user)
        {
            Response<User> response = new Response<User>();
            User? newUser = mapper.Map<User>(user);

            try
            {
                var hashed = _passwordHasher.HashPassword(user, user.Password);

                //	authentication.CreatePasswordHash(user.password, out byte[] password_hash, out byte[] password_salt);
                //   newUser.password_salt = password_salt;

                newUser.PasswordHash = "LiterallyNotAHash";

                await db_context.Users.AddAsync(newUser);
                await db_context.SaveChangesAsync();

                response.data = newUser;
                response.message = "Sign up successful";
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                response.message = ex.Message;
            }

            return response;
        }

        public async Task<Response<string>> Login(UserLoginDTO user)
        {
            Response<string> response = new Response<string>();
            User? user_login = await db_context.Users.Where(u => u.Email == user.Email).FirstOrDefaultAsync();
            var userindb = mapper.Map<CreateUserDTO>(user_login);
            if (user_login is not null)
            {
                try
                {
                    var result = _passwordHasher.VerifyHashedPassword(userindb, user_login.PasswordHash, user.Password);
                    if (result == PasswordVerificationResult.Failed)
                    {
                        response.success = false;
                        response.message = "Incorrect password";
                    }

                    //if (!authentication.verifyPasswordHash(user.password, user_login.password_hash, user_login.password_salt))
                    //{
                    //    response.success = false;
                    //    response.message = "Incorrect password";
                    //}

                    else
                    {
                        var token = authentication.createToken((user_login.Id).ToString(), "user");
                        response.data = token;
                        response.message = "Login succesful";
                    }

                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.message = ex.Message;

                }
            }
            else
            {
                response.success = false;
                response.message = "user not found";
            }

            return response;
        }
    }
}
