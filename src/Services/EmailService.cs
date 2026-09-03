using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Planara.Notifications.DTOs;
using Planara.Notifications.Options;

namespace Planara.Notifications.Services;

public class EmailService(IOptions<EmailOptions> options) : IEmailService
{
    private readonly EmailOptions _options = options.Value;

    public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        using var email = new MimeMessage();

        email.From.Add(new MailboxAddress(_options.FromName, _options.FromAddress));
        email.To.Add(MailboxAddress.Parse(message.To));
        email.Subject = message.Subject;
        email.Body = new BodyBuilder { HtmlBody = message.HtmlContent }.ToMessageBody();

        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(_options.SmtpServer, _options.Port, SecureSocketOptions.SslOnConnect, cancellationToken);
            await client.AuthenticateAsync(_options.Username, _options.Password, cancellationToken);
            await client.SendAsync(email, cancellationToken);
        }
        finally
        {
            if (client.IsConnected)
                await client.DisconnectAsync(true, cancellationToken);
        }
    }
}