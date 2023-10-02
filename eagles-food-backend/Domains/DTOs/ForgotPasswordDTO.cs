using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.DTOs
{
    public class ForgotPasswordDTO
    {
        [Required]
        public string? Email { get; set; }
    }
}
