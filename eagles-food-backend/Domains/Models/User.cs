using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.Models
{
    public class User
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Profile_Picture { get; set; }
        [Required]
        public string Email { get; set; }
        public long PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Refresh_Token { get; set; }
        public DateTime Created_At { get; set; } = DateTime.Now;
        public DateTime Updated_At { get; set; } = DateTime.Now;
    }
}
