using eagles_food_backend.Data;
using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;
using Microsoft.EntityFrameworkCore;

namespace eagles_food_backend.Services
{
    public class UserService : IUserService
    {
        private readonly LunchDbContext _context;
        private readonly IResponseService _responseService;
        public UserService(LunchDbContext context, IResponseService responseService)
        {
            _context = context;
            _responseService = responseService;
        }

        public async Task<ServiceResponse<CreateUserDTO>> CreateUser(CreateUserDTO model)
        {

            if (model == default) return _responseService.ErrorResponse<CreateUserDTO>("Something went Wrong");
            var user = new User()
            {
                email = model.Email,
                first_name = model.FirstName,
                last_name = model.LastName,
                password_hash = model.Password,
                password_salt = "salt",
                profile_picture = "picture",
                refresh_token = "token"
            };
            await _context.Users.AddAsync(user);
            var saved = await _context.SaveChangesAsync() > 0;
            if (saved) return _responseService.SuccessResponse<CreateUserDTO>(model, "User Created Successfully");
            return _responseService.ErrorResponse<CreateUserDTO>("Unable to Create User");
        }

        public async Task<ServiceResponse<List<UserReadDTO>>> GetAllUsersForOrganization(int org_id)
        {
            var users = await _context.Users.Where(x => x.organizationId == org_id).Select(x => new UserReadDTO(
                x.first_name,
                x.email,
                x.profile_picture,
                x.id.ToString()
            )).ToListAsync();

            return _responseService.SuccessResponse(users, "Users fetched successfully");
        }

        public async Task<ServiceResponse<UserProfileReadDTO>> GetUserProfile(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.id == id);
            if (user == null)
            {
                return _responseService.ErrorResponse<UserProfileReadDTO>("User not found");
            }
            var userprofile = new UserProfileReadDTO
            (
                name: user.first_name,
                email: user.email,
                profile_picture: user.profile_picture,
                phonenumber: "",
                bank_number: user.bank_number,
                bank_code: user.bank_code,
                bank_name: user.bank_name,
                is_admin: user.is_admin
            );

            return _responseService.SuccessResponse(userprofile, "User data fetched successfully");
        }

        public async Task<ServiceResponse<UserBankUpdateDTO>> UpdateUserBank(UserBankUpdateDTO userbank, int user_id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.id == user_id);
            if (user == null)
            {
                return _responseService.ErrorResponse<UserBankUpdateDTO>("User not found");
            }
            user.bank_number = userbank.bank_number;
            user.bank_name = userbank.bank_name;
            user.bank_code = userbank.bank_code;
            await _context.SaveChangesAsync();

            return _responseService.SuccessResponse(userbank, "Successfully created bank account");
        }

        public async Task<ServiceResponse<UserReadDTO>> SearchForUser(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.email == email);
            if (user == null)
            {
                return _responseService.ErrorResponse<UserReadDTO>("User not found");
            }

            var userReadDto = new UserReadDTO(
                name: user.first_name,
                email: user.email,
                profile_picture: user.profile_picture,
                user_id: user.id.ToString()
                );

            return _responseService.SuccessResponse(userReadDto, "User found");
        }
    }

    public interface IUserService
    {
        Task<ServiceResponse<CreateUserDTO>> CreateUser(CreateUserDTO model);
        Task<ServiceResponse<UserProfileReadDTO>> GetUserProfile(int id);
        Task<ServiceResponse<UserBankUpdateDTO>> UpdateUserBank(UserBankUpdateDTO userbank, int user_id);
        Task<ServiceResponse<List<UserReadDTO>>> GetAllUsersForOrganization(int org_id);
        Task<ServiceResponse<UserReadDTO>> SearchForUser(string email);
    }
}


