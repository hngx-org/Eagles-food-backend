using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.Models
{

    public class Lunch : BaseEntity
    {
        [Required] public string senderId { get; set; }
        [Required] public string recieverId { get; set; }
        [Required] public string org_id { get; set; }
        [Required] public int quantity { get; set; }
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        [Required] public string note { get; set; } = string.Empty;
        [Required] public bool redeemed { get; set; }
        public Organization Organization { get; set; }
        public User sender { get; set; }
        public User reciever { get; set; }
    }
}
