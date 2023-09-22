using eagles_food_backend.Data;
using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

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

        // create a new user
        public async Task<Response<Dictionary<string, string>>> CreateUser(CreateUserDTO user)
        {
            Response<Dictionary<string, string>> response = new();
            User? newUser = mapper.Map<User>(user);

            // ensure it's an actual (plausible) email
            if (!new EmailAddressAttribute().IsValid(user.Email))
            {
                response.success = false;
                response.message = "Invalid email";
                response.data = new Dictionary<string, string>() {
                    { "email", user.Email }
                };
                response.statusCode = HttpStatusCode.BadRequest;

                return response;
            }

            // ensure email is unique
            if (await db_context.Users.AnyAsync(u => u.Email == user.Email))
            {
                response.success = false;
                response.message = "Email already exists";
                response.data = new Dictionary<string, string>() {
                    { "email", user.Email }
                };
                response.statusCode = HttpStatusCode.BadRequest;

                return response;
            }

            // store in db
            try
            {
                authentication.CreatePasswordHash(user.Password, out string password_hash);

                newUser.PasswordHash = password_hash;
                newUser.IsAdmin = false;

                await db_context.Users.AddAsync(newUser);
                await db_context.SaveChangesAsync();

                response.success = true;
                response.data = new Dictionary<string, string>() {
                    { "id", newUser.Id.ToString() },
                    { "email", newUser.Email }
                };
                response.message = "User signed up successfully";
                response.statusCode = HttpStatusCode.Created;
            }

            // catch any errors
            catch (Exception ex)
            {
                response.statusCode = HttpStatusCode.InternalServerError;
                response.success = false;
                response.message = ex.Message;
            }

            return response;
        }

        // login a user
        public async Task<Response<Dictionary<string, string>>> Login(UserLoginDTO user)
        {
            Response<Dictionary<string, string>> response = new();
            User? user_login = await db_context.Users.Where(u => u.Email == user.Email).FirstOrDefaultAsync();
            var userindb = mapper.Map<CreateUserDTO>(user_login);

            // ensure user exists
            if (user_login is null)
            {
                response.success = false;
                response.message = "User not found";
                response.statusCode = HttpStatusCode.Unauthorized;
                response.data = new Dictionary<string, string>() {
                    { "email", user.Email }
                };

                return response;
            }

            // ensure password is correct
            try
            {
                var passwordIsValid = authentication.verifyPasswordHash(user.Password, user_login.PasswordHash);

                if (!passwordIsValid)
                {
                    response.success = false;
                    response.message = "Incorrect password";
                    response.statusCode = HttpStatusCode.Unauthorized;
                }
                else
                {
                    // get role (even if it's null) and create token
                    var role = user_login.IsAdmin == true ? "admin" : "user";
                    var token = authentication.createToken(user_login.Id.ToString(), role);

                    response.success = true;
                    response.data = new Dictionary<string, string>() {
                        { "access_token", token }
                    };
                    response.message = "User authenticated successfully";
                    response.statusCode = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
                response.statusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        public async Task<Response<List<UserReadDTO>>> GetAllUsersForOrganization(int user_id)
        {
            try
            {
                long? org_id = (await db_context.Users.FirstOrDefaultAsync(x => x.Id == user_id))?.OrgId;
                if (org_id == null)
                {
                    return new Response<List<UserReadDTO>>() { message = "Organization not found", success = false, statusCode = HttpStatusCode.NotFound };
                }
                var users = await db_context.Users.Where(x => x.OrgId == org_id).Select(x => new UserReadDTO(
                $"{x.FirstName} {x.LastName}",
                x.Email,
                x.ProfilePic,
                x.Id.ToString())).ToListAsync();

                return new Response<List<UserReadDTO>>() { data = users, message = "Users fetched successfully" };
            }
            catch (Exception)
            {
                return new Response<List<UserReadDTO>>() { message = "Internal Server Error", statusCode = HttpStatusCode.InternalServerError };
            }
        }

        public async Task<Response<UserProfileReadDTO>> GetUserProfile(int id)
        {
            try
            {
                var user = await db_context.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                {
                    return new Response<UserProfileReadDTO>() { message = "User not found", success = false, statusCode = HttpStatusCode.NotFound };
                }

                var userprofile = new UserProfileReadDTO
                (
                    user_id: user.Id.ToString(),
                    name: $"{user.FirstName} {user.LastName}",
                    email: user.Email,
                    profile_picture: user.ProfilePic,
                    phone_number: user.Phone,
                    isAdmin: user.IsAdmin ?? false
                );

                return new Response<UserProfileReadDTO>() { data = userprofile, message = "User data fetched successfully" };
            }
            catch (Exception)
            {
                return new Response<UserProfileReadDTO>() { message = "Internal Server Error", statusCode = HttpStatusCode.InternalServerError };
            }
        }

        public async Task<Response<UserBankUpdateDTO>> UpdateUserBank(UserBankUpdateDTO userbank, int user_id)
        {
            try
            {
                var user = await db_context.Users.FirstOrDefaultAsync(x => x.Id == user_id);
                if (user == null)
                {
                    return new Response<UserBankUpdateDTO>() { message = "User not found", success = false, statusCode = HttpStatusCode.NotFound };
                }
                user.BankRegion = userbank.bank_region;
                user.BankNumber = userbank.bank_number;
                user.BankName = userbank.bank_name;
                user.BankCode = userbank.bank_code;
                await db_context.SaveChangesAsync();

                return new Response<UserBankUpdateDTO>() { data = null, message = "Successfully created bank account" };
            }
            catch (Exception)
            {
                return new Response<UserBankUpdateDTO>() { message = "Internal Server Error", statusCode = HttpStatusCode.InternalServerError };
            }

        }

        public async Task<Response<UserReadDTO>> SearchForUser(string email)
        {
            try
            {
                var user = await db_context.Users.FirstOrDefaultAsync(x => x.Email == email);
                if (user == null)
                {
                    return new Response<UserReadDTO>() { message = "User not found", success = false, statusCode = HttpStatusCode.NotFound };
                }

                var userReadDto = new UserReadDTO(
                    name: $"{user.FirstName} {user.LastName}",
                    email: user.Email,
                    profile_picture: user.ProfilePic,
                    user_id: user.Id.ToString());

                return new Response<UserReadDTO>() { data = userReadDto, message = "User found" };
            }
            catch (Exception)
            {
                return new Response<UserReadDTO>() { message = "Internal Server Error", statusCode = HttpStatusCode.InternalServerError };
            }
        }
    }
}
