using System;
using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.Models
{
    public class Organization
    {
        [Key] public long OrganizationId { get; set; }
        [Required] public string name { get; set; }
        [Required] public double lunch_price { get; set; }
        public string currency { get; set; }
        public IEnumerable<User>? users { get; set; }
        public IEnumerable<Invite>? invitations { get; set; }
        public IEnumerable<Lunch>? sent_lunches { get; set; }
        public IEnumerable<Lunch>? recieved_lunches { get; set; }


    }
}

