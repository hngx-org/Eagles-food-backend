using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.Models
{
    public class OrganizationWallet
    {
        [Key] public long WalletId { get; set; }
        [Required] public double Balance { get; set; } = 0;
        [Required] public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}
