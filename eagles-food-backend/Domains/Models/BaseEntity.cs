using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.Models
{
    public class BaseEntity
    {
        [Key] public string Id { get; set; }
    }
}
