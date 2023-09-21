using System;
using System.Collections.Generic;

namespace eagles_food_backend.Domains.Models
{
    public partial class Lunch
    {
        public int Id { get; set; }
        public int? OrgId { get; set; }
        public int? SenderId { get; set; }
        public int? ReceiverId { get; set; }
        public int Quantity { get; set; }
        public bool? Redeemed { get; set; }
        public string? Note { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Organization? Org { get; set; }
        public virtual User? Receiver { get; set; }
        public virtual User? Sender { get; set; }
    }
}
