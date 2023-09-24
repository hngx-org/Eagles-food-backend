namespace eagles_food_backend.Domains.Models
{
    public partial class User
    {
        public User()
        {
            LunchReceivers = new HashSet<Lunch>();
            LunchSenders = new HashSet<Lunch>();
            Withdrawals = new HashSet<Withdrawal>();
        }

        public int Id { get; set; }
        public int? OrgId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePic { get; set; }
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string PasswordHash { get; set; } = null!;
        public bool? IsAdmin { get; set; }
        public int? LunchCreditBalance { get; set; }
        public string? RefreshToken { get; set; }
        public string? BankNumber { get; set; }
        public string? BankCode { get; set; }
        public string? BankName { get; set; }
        public string? BankRegion { get; set; }
        public string? Currency { get; set; }
        public string? CurrencyCode { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Organization? Org { get; set; }
        public virtual ICollection<Lunch> LunchReceivers { get; set; }
        public virtual ICollection<Lunch> LunchSenders { get; set; }
        public virtual ICollection<Withdrawal> Withdrawals { get; set; }
    }
}