namespace eagles_food_backend.Domains.Models
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public bool Status { get; set; }
        public string? Message { get; set; }
    }
}