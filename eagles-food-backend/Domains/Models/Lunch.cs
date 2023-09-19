using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.Models
{
    public class Lunch
    {
        [Key] public long LunchId { get; set; }
        [Required] public long senderId { get; set; }
        public Organization sender { get; set; }
        [Required] public long recieverId { get; set; }
        public Organization reciever{ get; set; }
        [Required] public int quantity { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        [Required] public string note { get; set; } = string.Empty;
    }
}
