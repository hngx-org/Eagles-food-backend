using eagles_food_backend.Data;
using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

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

                newUser.PasswordHash = hashed;

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
