using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eagles_food_backend.Models
{
    public class BankAccount
    {
        public long Id { get; set; }
        [Required]
        public long Number { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Bank_Name { get; set; }
        [Required]
        public string Bank_Code { get; set; }
        [Required]
        public long User_Id { get; set; }
        [ForeignKey("User_Id")]
        public User User { get; set; }
    }
}
