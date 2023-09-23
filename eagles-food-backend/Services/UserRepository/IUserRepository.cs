using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;

namespace eagles_food_backend.Services.UserServices
{
    public interface IUserRepository
    {
        Task<Response<Dictionary<string, string>>> CreateUser(CreateUserDTO user);
        Task<Response<Dictionary<string, string>>> Login(UserLoginDTO user);
        Task<Response<UserProfileReadDTO>> GetUserProfile(int id);
        Task<Response<UserBankUpdateDTO>> UpdateUserBank(UserBankUpdateDTO userbank, int user_id);
        Task<Response<UserReadAllDTO>> GetAllUsersByOrganization(int user_id);
        Task<Response<UserReadDTO>> SearchForUser(string email);
        Task<Response<Dictionary<string, string>>> UpdateUserProfile(int userId, UpdateUserDTO model);
    }
}
