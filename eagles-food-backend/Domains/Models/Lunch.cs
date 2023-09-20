using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.Models
{
    public class Lunch
    {
<<<<<<< HEAD
        [Key] public long LunchId { get; set; }
        public long OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [Required] public long senderId { get; set; }
        public User sender { get; set; }
        [Required] public long recieverId { get; set; }
        public User reciever{ get; set; }
=======
        [Key] public int id { get; set; }
        //[Required] public long senderId { get; set; }
        //public Organization sender { get; set; }
        //[Required] public long recieverId { get; set; }
        //public Organization reciever{ get; set; }
>>>>>>> d578b4b9c03e1e86c6cd0805d08b730cfdd32333
        [Required] public int quantity { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        [Required] public string note { get; set; } = string.Empty;
    }
}
