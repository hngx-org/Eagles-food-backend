using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.DTOs
{
    public class UserLoginDTO
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class CreateUserDTO
    {
        [Required] public string first_name { get; set; }
        [Required] public string last_name { get; set; }
        [Required] public string username { get; set; }
        [Required] public string password { get; set; }
        [Required] public string email { get; set; }

    }

    public record UserProfileReadDTO(string? name, string? email, string? profile_picture, string? phonenumber, string? bank_number, string? bank_code, string? bank_name, bool is_admin);

    public record UserBankUpdateDTO([Required] string bank_number, [Required] string bank_code, [Required] string bank_name);
    public record UserReadDTO(string? name, string? email, string? profile_picture, string? user_id);
}
