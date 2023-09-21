using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.DTOs
{
    public class UserDTO
    {
    }

    public class CreateUserDTO
    {
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Password { get; set; }
        public string Phone { get; set; }
    }

    public class UserLoginDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public record UserProfileReadDTO(string? user_id, string? name, string? email, string? phone_number, string? profile_picture, bool isAdmin);

    public record UserBankUpdateDTO([Required] string bank_region, [Required] string bank_number, [Required] string bank_code, [Required] string bank_name);
    public record UserReadDTO(string? name, string? email, string? profile_picture, string? user_id);
}
