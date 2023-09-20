using eagles_food_backend.Data;
using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;

namespace eagles_food_backend.Services
{
    public class UserService : IUserService
    {
        private readonly LunchDbContext _context;
        private readonly IResponseService _responseService;
        public UserService(LunchDbContext context, IResponseService responseService) {
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
            if(saved)return _responseService.SuccessResponse<CreateUserDTO>(model, "User Created Successfully");
            return _responseService.ErrorResponse<CreateUserDTO>("Unable to Create User");
        }

        //public async Task<ServiceResponse<object>> Login(string email, string password){
            
        //}
    }

    public interface IUserService
    {
        Task<ServiceResponse<CreateUserDTO>> CreateUser(CreateUserDTO model);
    }
}
