using eagles_food_backend.Data;
using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;
using Microsoft.EntityFrameworkCore;

namespace eagles_food_backend.Services.UserServices
{
    public class UserService : IUserRepository
    {
        private readonly LunchDbContext db_context;
        private readonly IMapper mapper;
        private readonly AuthenticationClass authentication;

        public UserService(LunchDbContext db_context,IMapper mapper,AuthenticationClass authentication)
        {
            this.db_context = db_context;
            this.mapper = mapper;
            this.authentication = authentication;
        }
        public async Task<Response<User>> CreateUser(CreateUserDTO user)
        {
            Response<User> response = new Response<User>();
            User? newUser = mapper.Map<User>(user);

            try
            {
                authentication.CreatePasswordHash(user.password, out byte[] password_hash, out byte[] password_salt);
                newUser.password_salt = password_salt;
                newUser.password_hash = password_hash;
                await db_context.users.AddAsync(newUser);
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
            User? user_login = await db_context.users.Where(u => u.username == user.username).FirstOrDefaultAsync();
            if (user_login is not null)
            {
                try
                {
                    if (!authentication.verifyPasswordHash(user.password, user_login.password_hash, user_login.password_salt))
                    {
                        response.success = false;
                        response.message = "Incorrect password";
                    }
                    else
                    {
                        var token = authentication.createToken((user_login.UserId).ToString(), "user");
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
