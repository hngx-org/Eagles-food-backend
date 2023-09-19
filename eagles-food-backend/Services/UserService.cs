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
            await Task.Delay(1000);
            if (model == default) return _responseService.ErrorResponse<CreateUserDTO>("Something went Wrong");
            return _responseService.SuccessResponse<CreateUserDTO>(model, "Successful Operation");
        }
    }

    public interface IUserService
    {
        Task<ServiceResponse<CreateUserDTO>> CreateUser(CreateUserDTO model);
    }
}
