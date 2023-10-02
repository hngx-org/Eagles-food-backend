namespace eagles_food_backend.Services.EmailService
{
    public class MailSettings
    {
        public string Server { get; set; } = "sandbox.smtp.mailtrap.io";
        public int Port { get; set; } = 587;
        public string SenderName { get; set; } = "Team Eagles";
        public string SenderEmail { get; set; } = "bolubee95@gmail.com";
        public string UserName { get; set; } = "ec33b51e0a34d6";
        public string Password { get; set; } = "743632b4f773fa";
    }
}
