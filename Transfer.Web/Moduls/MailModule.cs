using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Common.Settings;

namespace Transfer.Web.Moduls;

public class MailModule : IMailModule
{
    private readonly MailSettings _mailSettings;
    private readonly ILogger _logger;


    public MailModule(IOptions<MailSettings>  settings, ILogger<MailSettings> logger)
    {
        _mailSettings = settings.Value;
        _logger = logger;
    }

    public async Task SendEmailPlainTextAsync(string body, string subject, string recipient, bool isHtml = false)
    {
        var email = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_mailSettings.Mail)
        };
        email.To.Add(MailboxAddress.Parse(recipient));
        email.Sender.Name = _mailSettings.DisplayName;
        email.From.Add(MailboxAddress.Parse(_mailSettings.Mail));
        email.Subject = subject;
        //email.Body = new TextPart(TextFormat) { Text = body };
        if(isHtml)
            email.Body = new TextPart(TextFormat.Html) { Text = body };
        else
            email.Body = new TextPart(TextFormat.Text) { Text = body };
        using var smtp = new SmtpClient();
        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.Auto);
        smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        var resp = await smtp.SendAsync(email);
        _logger?.LogInformation(resp);
        smtp.Disconnect(true);
    }
}
