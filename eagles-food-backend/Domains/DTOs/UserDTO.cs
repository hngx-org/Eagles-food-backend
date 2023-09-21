using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.DTOs
{
    public class UserDTO
    {
    }

    public class CreateUserDTO
    {
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Password { get; set; }
        public string Phone { get; set; }
    }

    public class UserLoginDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
