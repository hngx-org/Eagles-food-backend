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

    public record UserProfileReadDTO(string name, string email, string profile_picture, string phonenumber, string bank_number, string bank_code, string bank_name, bool is_admin);

    public record UserBankUpdateDTO(string bank_number, string bank_code, string bank_name);
    public record UserReadDTO(string name, string email, string profile_picture, string user_id);
}
