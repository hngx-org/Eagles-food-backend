using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eagles_food_backend.Domains.Models
{
    public class Withdawal
    {
        [Key] public long WithdrawalId { get; set; }
        [Required] public long UserId { get; set; }
        public User User { get; set; }
        public string status { get; set; }
        [Required] public double ammount { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
    }
}
