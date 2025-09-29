namespace E_commerce.Emails
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlBody);
    }
}