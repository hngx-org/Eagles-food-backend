using System.ComponentModel.DataAnnotations.Schema;

namespace eagles_food_backend.Domains.Models
{
    public class DevicePushNotification
    {
        public long Id { get; set; }
        public string Device_Id { get; set; }
        public string Token { get; set; }
        public long User_Id { get; set; }
        [ForeignKey("User_Id")]
        public User User { get; set; }

    }
}
