using System.ComponentModel.DataAnnotations.Schema;

namespace eagles_food_backend.Models
{
    public class Staff
    {
        public long Id { get; set; }
        public string Staff_Id { get; set; }
        public string Role { get; set; }
        public string Org_Id { get; set; }
        [ForeignKey("Org_Id")]
        public Organization Organization { get; set; }
    }
}
