namespace eagles_food_backend.Domains.Models
{
    public partial class OrganizationLunchWallet
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public int? OrgId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Organization? Org { get; set; }
    }
}