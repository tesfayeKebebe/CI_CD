namespace Web.API.Helpers
{
    public interface IEmailService
    {
        Task SendEmail(EmailDto request);
    }
}
