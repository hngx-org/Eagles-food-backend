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
        public string? InviteCode { get; set; }
    }

    public class UpdateUserDTO
    {
        public string Email { get; set; } = null!;
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Phone { get; set; }
        public string? ProfilePic { get; set; }
        //public IFormFile? Photo { get; set; }
        public string? PhotoString { get; set; }
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

    public class UserProfileReadDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePic { get; set; }
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public bool? IsAdmin { get; set; }
        public int? LunchCreditBalance { get; set; }
        public string? RefreshToken { get; set; }
        public string? ResetToken { get; set; }
        public string? BankNumber { get; set; }
        public string? BankCode { get; set; }
        public string? BankName { get; set; }
        public string? BankRegion { get; set; }
        public string? Currency { get; set; }
        public string? CurrencyCode { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    //public record UserProfileReadDTO(, , , , , , , );

    public record UserBankUpdateDTO([Required] string bank_region, [Required] string bank_number, [Required] string bank_code, [Required] string bank_name);
    public record UserReadDTO(string? name, string? email, string? profile_picture, string? user_id, string? role);

    public class UserDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? ProfilePic { get; set; }
        public string? UserId { get; set; }
        public string? Role { get; set; }
        public string? RefreshToken { get; set; }
        public string? ResetToken { get; set; }
        public string? BankNumber { get; set; }
        public string? BankCode { get; set; }
        public string? BankName { get; set; }
        public string? BankRegion { get; set; }
        public int? OrgId { get; set; }
        public string? Currency { get; set; }
        public string? CurrencyCode { get; set; }
        public string? Organization_Name { get; set; }
        public bool? IsAdmin { get; set; }
        public int? LunchCreditBalance { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }


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