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
        [Required] public string first_name { get; set; }
        [Required] public string last_name { get; set; }
        [Required ]public string username { get; set; }
        [Required] public string password { get; set; }
        [Required] public string email { get; set; }

    }
}
