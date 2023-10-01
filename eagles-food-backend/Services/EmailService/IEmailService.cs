namespace eagles_food_backend.Services.EmailService
{
    public interface IEmailService
    {
        bool SendEmail(MailData mailData);
    }
}
