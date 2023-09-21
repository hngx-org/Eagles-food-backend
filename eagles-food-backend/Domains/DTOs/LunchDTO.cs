using eagles_food_backend.Domains.Models;
using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.DTOs
{
    public class LunchDTO
    {
    }
    public class CreateLunchDTO
    {
        [Required]
        public string senderId { get; set; }
        [Required]
        public string receiverId { get; set; }
        [Required]
        public string org_id { get; set; }
        [Required]
        public int quantity { get; set; }
        [Required]
        public string note { get; set; } = string.Empty;
    }
    public class ResponseLunchDTO : BaseEntity
    {
        [Required]
        public string senderId { get; set; }
        [Required]
        public string receiverId { get; set; }
        [Required]
        public string org_id { get; set; }
        [Required]
        public int quantity { get; set; }
        [Required]
        public string note { get; set; } = string.Empty;
    }
}
