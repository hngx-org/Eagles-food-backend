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

        public UserService(LunchDbContext db_context, IMapper mapper, AuthenticationClass authentication)
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
                        var token = authentication.createToken((user_login.id).ToString(), "user");
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

        public async Task<Response<List<UserReadDTO>>> GetAllUsersForOrganization(int user_id)
        {
            try
            {
                long? org_id = (await db_context.users.FirstOrDefaultAsync(x => x.id == user_id))?.org_id;
                if (org_id == null)
                {
                    return new Response<List<UserReadDTO>>() { message = "User not found", success = false, status_code = "404" };
                }
                var users = await db_context.users.Where(x => x.org_id == org_id).Select(x => new UserReadDTO(
                $"{x.first_name} {x.last_name}",
                x.email,
                x.profile_pic,
                x.id.ToString())).ToListAsync();

                return new Response<List<UserReadDTO>>() { data = users, message = "Users fetched successfully" };
            }
            catch (Exception)
            {
                return new Response<List<UserReadDTO>>() { message = "Internal Server Error", status_code = "500" };
            }
        }

        public async Task<Response<UserProfileReadDTO>> GetUserProfile(int id)
        {
            try
            {
                var user = await db_context.users.FirstOrDefaultAsync(x => x.id == id);
                if (user == null)
                {
                    return new Response<UserProfileReadDTO>() { message = "User not found", success = false, status_code = "404" };
                }
                var userprofile = new UserProfileReadDTO
                (
                    name: $"{user.first_name} {user.last_name}",
                    email: user.email,
                    profile_picture: user.profile_pic,
                    phonenumber: "",
                    bank_number: user.bank_number,
                    bank_code: user.bank_code,
                    bank_name: user.bank_name,
                    is_admin: user.is_admin
                );

                return new Response<UserProfileReadDTO>() { data = userprofile, message = "User data fetched successfully" };
            }
            catch (Exception)
            {
                return new Response<UserProfileReadDTO>() { message = "Internal Server Error", status_code = "500" };
            }
        }

        public async Task<Response<UserBankUpdateDTO>> UpdateUserBank(UserBankUpdateDTO userbank, int user_id)
        {
            try
            {
                var user = await db_context.users.FirstOrDefaultAsync(x => x.id == user_id);
                if (user == null)
                {
                    return new Response<UserBankUpdateDTO>() { message = "User not found", success = false, status_code = "404" };
                }
                user.bank_number = userbank.bank_number;
                user.bank_name = userbank.bank_name;
                user.bank_code = userbank.bank_code;
                await db_context.SaveChangesAsync();

                return new Response<UserBankUpdateDTO>() { data = userbank, message = "Successfully created bank account" };
            }
            catch (Exception)
            {
                return new Response<UserBankUpdateDTO>() { message = "Internal Server Error", status_code = "500" };
            }

        }

        public async Task<Response<UserReadDTO>> SearchForUser(string email)
        {
            try
            {
                var user = await db_context.users.FirstOrDefaultAsync(x => x.email == email);
                if (user == null)
                {
                    return new Response<UserReadDTO>() { message = "User not found", success = false, status_code = "404" };
                }

                var userReadDto = new UserReadDTO(
                    name: $"{user.first_name} {user.last_name}",
                    email: user.email,
                    profile_picture: user.profile_pic,
                    user_id: user.id.ToString());

                return new Response<UserReadDTO>() { data = userReadDto, message = "User found" };
            }
            catch (Exception)
            {
                return new Response<UserReadDTO>() { message = "Internal Server Error", status_code = "500" };
            }
        }
    }
}
