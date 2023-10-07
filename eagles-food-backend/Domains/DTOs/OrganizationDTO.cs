namespace eagles_food_backend.Domains.DTOs
{
    public class OrganizationDTO
    {
        public string Name { get; set; }
        public decimal LunchPrice { get; set; }
        public string Currency { get; set; }
        public bool Hidden { get; set; }
    }

    public class ModifyOrganizationDTO
    {
        public string OrganizationName { get; set; } = null!;
        public decimal LunchPrice { get; set; }
        public string Currency { get; set; } = null!;
    }

    public class CreateStaffDTO
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }

    public class UpdateOrganizationWalletDTO
    {
        public decimal amount { get; set; }
    }

    public class UpdateOrganizationLunchPriceDTO
    {
        public decimal LunchPrice { get; set; }
    }

    public class InviteToOrganizationDTO
    {
        public string Email { get; set; } = null!;
    }

    public class OrganizationInviteDTO
    {
        public int Id { get; set; }
        public int? OrgId { get; set; }
        public string? Org { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? Status { get; set; }
    }

    public class OrganizationInvitationDTO : OrganizationInviteDTO
    {
        public string? Email { get; set; }

    }

}