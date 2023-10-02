using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.DTOs
{
    public class CreateUserDTO
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }

    public class UpdateUserDTO
    {
        public string Email { get; set; } = null!;
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Phone { get; set; }
        public string? ProfilePic { get; set; }
    }

    public class UserLoginDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class ChangePasswordDTO
    {
        public string Email { get; set; } = null!;
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }

    public record UserProfileReadDTO(string? user_id, string? name, string? email, string? phone_number, string? profile_picture, bool isAdmin, string organization, string balance);

    public record UserBankUpdateDTO([Required] string bank_region, [Required] string bank_number, [Required] string bank_code, [Required] string bank_name);
    public record UserReadDTO(string? name, string? email, string? profile_picture, string? user_id, string? role);

    public struct UserReadAllDTO
    {
        public List<UserReadDTO> org { get; set; }
        public List<UserReadDTO> others { get; set; }
    }

    public class ToggleInviteDTO
    {
        public int InviteId { get; set; }
        public bool Status { get; set; }
    }
}