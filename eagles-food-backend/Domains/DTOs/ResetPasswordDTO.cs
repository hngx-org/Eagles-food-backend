using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.DTOs
{
    public class ResetPasswordDTO
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? ResetToken { get; set; }
        [Required]
        public string? NewPassword { get; set; }
        [Required]
        public string? ConfirmPassword { get; set; }
    }
}
