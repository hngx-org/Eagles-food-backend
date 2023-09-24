using eagles_food_backend.Domains.Models;
using System.ComponentModel.DataAnnotations;

namespace eagles_food_backend.Domains.DTOs
{
    public class CreateLunchDTO
    {
        [Required]
        public int[] receivers { get; set; }
        [Required]
        public int quantity { get; set; }
        [Required]
        public string note { get; set; } = string.Empty;
    }
    public class ResponseLunchDTO
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string? SenderName { get; set; }
        public int ReceiverId { get; set; }
        public string? ReceiverName { get; set; }
        public int Quantity { get; set; }
        public bool Redeemed { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class WithdrawLunchDTO{
        public int Quantity{get; set;}
    }

    public class ResponseLunchWithdrawalDTO{
        public decimal WithdrawalAmount{get; set;} 
        
    }
}
