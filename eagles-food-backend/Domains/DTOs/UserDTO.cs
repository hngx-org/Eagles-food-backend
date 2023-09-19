using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.DTOs
{
    public class UserDTO
    {
    }

    public class CreateUserDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
