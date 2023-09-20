using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.Models
{
    public class Organization
    {
        [Key]
        public long id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public double lunch_price { get; set; }
        public string currency_code { get; set; }
        public IEnumerable<User>? users { get; set; }
        public IEnumerable<Invite>? invitations { get; set; }
        public IEnumerable<Lunch>? lunches { get; set; }
        public DateTime updated_at { get; set; } = DateTime.Now;
        public bool is_deleted { get; set; } = false;
    }
}

