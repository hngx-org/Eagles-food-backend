using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.Models
{
    public class Transaction
    {
        public long Id { get; set; }
        [Required]
        public long Sender_Id { get; set; }
        [Required]
        public long Reciever_Id { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public double Token_Price { get; set; }
        public long Type { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
    }
}
