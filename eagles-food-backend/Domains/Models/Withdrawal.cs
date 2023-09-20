using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eagles_food_backend.Domains.Models
{
    public class Withdrawal
    {
        [Key] public int id { get; set; }
        [Required] public int user_id { get; set; }
        public User User { get; set; }
        public string status { get; set; }
        [Required] public double amount { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
    }
}
