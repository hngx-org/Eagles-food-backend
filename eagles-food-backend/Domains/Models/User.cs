using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.Models
{
    public class User
    {
        [Key] public int id { get; set; }
        [Required] public string first_name { get; set; }
        [Required] public string last_name { get; set; }
        public int organizationId { get; set; }
        public Organization Organization { get; set; }
        [Required] public string profile_picture { get; set; }
        [Required] public string email { get; set; }
        public string password_hash { get; set; }
        public string password_salt { get; set; }
        public string refresh_token { get; set; }
        [Required] public string bank_number { get; set; } = string.Empty;
        [Required] public string bank_code { get; set; } = string.Empty;
        [Required] public string bank_name { get; set; } = string.Empty;
        [Required] public string Description { get; set; } = string.Empty;
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime updated_at { get; set; }
        public bool is_admin { get; set; } = false;
        public IEnumerable<Withdrawal>? withdrawals { get; set; }
        
    }
}
