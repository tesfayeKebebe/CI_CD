using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Application.Business;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Web.API.Helpers
{
    public class EmailService : IEmailService
    {
        private  readonly SmtpConfig _config;

        public EmailService(IOptions<AppSettings> config)
        {
            _config = config.Value.SmtpConfig;
        }

        public async Task SendEmail(EmailDto request)
        {
            try
            {

                var build = new StringBuilder();
                var path = build.Append("https://localhost:7241/api").Append("/FileSave/pdf/").Append(request.StoredFileName).ToString();

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_config.Name, _config.EmailAddress));
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                // email.Body = new TextPart(TextFormat.Html) { Text = request.Body };
                var builder = new BodyBuilder{ HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2> <a href={1}>link text</a> ", request.Body,path ) };;
                if (request.Attachments != null)
                {
                    builder.Attachments.Add(request.FileName, request.Attachments, ContentType.Parse(request.ContentType));
                }
                builder.HtmlBody = request.Body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                if (!_config.UseSSL)
                    smtp.ServerCertificateValidationCallback = (object sender2, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
                await smtp.ConnectAsync(_config.Host, _config.Port, _config.UseSSL);
                smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                if (!string.IsNullOrWhiteSpace(_config.Username))
                    await smtp.AuthenticateAsync(_config.Username, _config.Password).ConfigureAwait(false);
                await smtp.SendAsync(email).ConfigureAwait(false);;
                await smtp.DisconnectAsync(true).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
     
        }
    }
}
