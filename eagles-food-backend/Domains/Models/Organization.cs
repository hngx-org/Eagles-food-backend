using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eagles_food_backend.Domains.Models
{
    public class Organization : BaseEntity
    {

        [Required]
        public string name { get; set; }
        [Required]
        public double lunch_price { get; set; }
        public string currency { get; set; }
        public IEnumerable<User>? users { get; set; }
        public IEnumerable<Invite>? invitations { get; set; }
        public IEnumerable<Lunch>? lunches { get; set; }


    }
}

