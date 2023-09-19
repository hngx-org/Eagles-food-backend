using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eagles_food_backend.Models
{
    public class StaffWallet
    {
        public long Id { get; set; }
        [Required]
        public double Balance { get; set; }
        public long Staff_Id { get; set; }
        [ForeignKey("Staff_Id")]
        public Staff Staff { get; set; }
    }
}
