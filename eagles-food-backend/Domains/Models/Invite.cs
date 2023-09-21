using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.Models
{
    public class Invite : BaseEntity
    {
        [Required] public string email { get; set; }
        [Required] public string token { get; set; }
        [Required] DateTime TTL { get; set; }
        [Required] public int OrganizationId { get; set; }
        [Required] public Organization organization { get; set; }
    }
}
