using eagles_food_backend.Domains.DTOs;
using eagles_food_backend.Domains.Models;

namespace eagles_food_backend.Services.UserServices
{
    public interface IUserRepository
    {
        Task<Response<User>> CreateUser(CreateUserDTO user);
        Task<Response<string>> Login(UserLoginDTO user);

    }
}
