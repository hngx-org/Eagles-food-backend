using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.Models
{
    public class OrganizationInvite
    {
        public long Id { get; set; }
        [Required]
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime TTL { get; set; }
    }
}
