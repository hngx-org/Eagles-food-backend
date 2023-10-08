namespace eagles_food_backend.Domains.Models
{
    public class InvitationRequest
    {
        public int Id { get; set; }
        public string UserEmail { get; set; } = null!;
        public string Token { get; set; } = null!;
        public int? OrgId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Status { get; set; }
        public virtual Organization? Org { get; set; }
    }
}
