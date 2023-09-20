using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.DTOs
{
    public class LunchDTO
    {
    }
    public class CreateLunchDTO
    {
        [Required]
        public string senderId { get; set; } = null!;
        [Required]
        public string receiverId { get; set; } = null!;
        [Required]
        public string organization_id { get; set; } = null!;
        [Required]
        public int quantity { get; set; }
        [Required]
        public string note { get; set; } = string.Empty;
    }
}
