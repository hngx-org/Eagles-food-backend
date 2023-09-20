using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.Models
{
    public class User
    {
<<<<<<< HEAD
        [Key] public long UserId { get; set; }
        [Required] public string first_name { get; set; } //Required for registration
        [Required] public string last_name { get; set; } //Required for registration
        [Required] public string username { get; set; }
        public long OrganizationId { get; set; }
=======
        [Key] public int id { get; set; }
        [Required] public string first_name { get; set; }
        [Required] public string last_name { get; set; }
        public int organizationId { get; set; }
>>>>>>> d578b4b9c03e1e86c6cd0805d08b730cfdd32333
        public Organization Organization { get; set; }
        public string? profile_picture { get; set; }
        [Required] public string email { get; set; } //Required for registration
        public byte[] password_hash { get; set; } //
        public byte[] password_salt { get; set; } //
        public string? refresh_token { get; set; }
        [Required] public string currency { get; set; } = string.Empty;
        [Required] public string currency_code { get; set; } = string.Empty;
        [Required] public int lunch_credit_balance { get; set; } = 0;
        [Required] public string bank_number { get; set; } = string.Empty;
        [Required] public string bank_code { get; set; } = string.Empty;
        [Required] public string bank_name { get; set; } = string.Empty;
        [Required] public string bank_region { get; set; } = string.Empty;
        public DateTime created_at { get; set; } = DateTime.Now;
        public DateTime updated_at { get; set; } = DateTime.Now;
        public bool is_admin { get; set; } = false;
<<<<<<< HEAD
        public IEnumerable<Withdawal>? withdrawals { get; set; }
        public IEnumerable<Lunch> sent_lunches { get; set; }
        public IEnumerable<Lunch> recieved_lunches { get; set; }


=======
        public IEnumerable<Withdrawal>? withdrawals { get; set; }
        
>>>>>>> d578b4b9c03e1e86c6cd0805d08b730cfdd32333
    }
}
