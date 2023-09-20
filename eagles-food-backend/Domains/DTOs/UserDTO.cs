using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.DTOs
{
    public class UserLoginDTO
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class CreateUserDTO
    {
<<<<<<< HEAD
        [Required] public string first_name { get; set; }
        [Required] public string last_name { get; set; }
        [Required ]public string username { get; set; }
        [Required] public string password { get; set; }
        [Required] public string email { get; set; }
=======
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
>>>>>>> d578b4b9c03e1e86c6cd0805d08b730cfdd32333
    }
}
