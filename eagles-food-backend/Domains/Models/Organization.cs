using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.Models
{
    public class Organization
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Admin_User { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public double Plate_Price { get; set; }
    }
}
