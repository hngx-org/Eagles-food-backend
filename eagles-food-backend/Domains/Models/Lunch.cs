using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.Models
{
    public class Lunch
    {
        [Key] public long LunchId { get; set; }
        public long OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [Required] public long senderId { get; set; }
        public User sender { get; set; }
        [Required] public long recieverId { get; set; }
        public User reciever{ get; set; }
        [Required] public int quantity { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        [Required] public string note { get; set; } = string.Empty;
    }
}
