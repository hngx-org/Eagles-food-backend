namespace eagles_food_backend.Domains.DTOs
{
    public class OrganizationDTO
    {
        public string Name { get; set; }
        public double LunchPrice { get; set; }
        public string Currency { get; set; }
    }

    public class CreateOrganizationDTO
    {
        public string Name { get; set; }
        public double LunchPrice { get; set; }
        public string Currency { get; set; }
    }

}
