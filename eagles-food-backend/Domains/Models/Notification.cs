using System.ComponentModel.DataAnnotations.Schema;

namespace eagles_food_backend.Domains.Models
{
    public class Activity
    {
        public long Id { get; set; }
        public long Staff_Id { get; set; }
        [ForeignKey("Staff_Id")]
        public Staff Staff { get; set; }
        public long Org_Id { get; set; }
        [ForeignKey("Org_Id")]
        public Organization Organization { get; set; }
        public long Is_Read { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
