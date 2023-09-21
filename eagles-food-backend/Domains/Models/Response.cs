namespace eagles_food_backend.Domains.Models
{
    public class Response<T>
    {
        public T? data { get; set; }
        public string message { get; set; } = string.Empty;
        public bool success { get; set; }
        public int statusCode { get; set; } = 200;
    }
}
