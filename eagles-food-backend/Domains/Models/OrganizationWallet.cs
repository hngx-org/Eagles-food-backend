using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.Models
{
    public class OrganizationWallet
    {
        [Key] public long WalletId { get; set; }
        [Required] public double balance { get; set; } = 0;
        [Required] public long OrganizationId { get; set; }
        public Organization organization { get; set; }
    }
}
