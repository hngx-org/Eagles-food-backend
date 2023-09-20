using eagles_food_backend.Domains.Models;

namespace eagles_food_backend.Services.ResponseServce
{
    public class ResponseService : IResponseService
    {
        public ServiceResponse<T> SuccessResponse<T>(T Data, string? message)
        {
            return new ServiceResponse<T> { Data = Data, Message = message ?? "Successful Operation", Status = true };
        }

        public ServiceResponse<T> ErrorResponse<T>(string? message)
        {
            return new ServiceResponse<T> { Message = message ?? "Something went wrong", Status = false };
        }
    }


    public interface IResponseService
    {
        ServiceResponse<T> SuccessResponse<T>(T Data, string message = null);
        ServiceResponse<T> ErrorResponse<T>(string message = null);
    }
}
