namespace eagles_food_backend.Domains.Models
{
    public partial class OrganizationInvite
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime Ttl { get; set; }
        public int? OrgId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Organization? Org { get; set; }
    }
}