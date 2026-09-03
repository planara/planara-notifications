using Planara.Notifications.DTOs;

namespace Planara.Notifications.Services;

public interface IEmailService
{
    /// <summary>
    /// Отправляет email-сообщение
    /// </summary>
    Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
}