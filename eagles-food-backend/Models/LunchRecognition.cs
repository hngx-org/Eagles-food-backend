using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eagles_food_backend.Models
{
    public class LunchRecognition
    {
        public long Id { get; set; }
        [Required]
        public long User_Id { get; set; }
        [ForeignKey("User_Id")]
        public User User { get; set; }
        public long Transaction_Id { get; set; }
        [ForeignKey("Transaction_Id")]
        public Transaction Transaction { get; set; }
        public bool Redeem { get; set; }
    }
}
