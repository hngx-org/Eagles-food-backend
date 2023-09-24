namespace eagles_food_backend.Domains.Models
{
    public partial class Withdrawal
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Status { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual User? User { get; set; }
    }
}