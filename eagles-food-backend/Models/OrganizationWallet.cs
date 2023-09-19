using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eagles_food_backend.Models
{
    public class OrganizationWallet
    {
        public long Id { get; set; }
        [Required]
        public double Balance { get; set; }
        [Required]
        public long Org_Id { get; set; }
        [ForeignKey("Org_Id")]
        public Organization Organization { get; set; }
      
    }
}
