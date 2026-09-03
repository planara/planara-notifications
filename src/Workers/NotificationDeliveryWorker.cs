using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Planara.Notifications.Data;
using Planara.Notifications.Data.Domain;
using Planara.Notifications.Data.Enums;
using Planara.Notifications.DTOs;
using Planara.Notifications.Services;

namespace Planara.Notifications.Workers;

public class NotificationDeliveryWorker(ILogger<NotificationDeliveryWorker> logger, IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task ProcessAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();

        var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
        var templateService = scope.ServiceProvider.GetRequiredService<IEmailTemplateService>();

        var now = DateTime.UtcNow;

        var deliveries = await dataContext.NotificationDeliveries
            .Where(x => x.Status == NotificationStatus.Pending && (x.NextAttemptAt == null || x.NextAttemptAt <= now))
            .OrderBy(x => x.CreatedAt)
            .Take(10)
            .ToListAsync(cancellationToken);

        foreach (var delivery in deliveries)
        {
            await ProcessDeliveryAsync(delivery, dataContext, emailService, templateService, cancellationToken);
        }
    }

    private async Task ProcessDeliveryAsync(
        NotificationDelivery delivery,
        DataContext dataContext,
        IEmailService emailService,
        IEmailTemplateService templateService,
        CancellationToken cancellationToken)
    {
        var attempt = new NotificationAttempt
        {
            NotificationDeliveryId = delivery.Id,
            StartedAt = DateTime.UtcNow,
        };

        delivery.Status = NotificationStatus.Processing;
        delivery.AttemptCount++;

        dataContext.NotificationAttempts.Add(attempt);

        await dataContext.SaveChangesAsync(cancellationToken);

        try
        {
            switch (delivery.Type)
            {
                case NotificationType.EmailConfirmation:
                    await SendEmailConfirmationAsync(delivery, emailService, templateService, cancellationToken);

                    break;

                default:
                    throw new InvalidOperationException($"Notification type '{delivery.Type}' is not supported.");
            }

            var now = DateTime.UtcNow;

            attempt.CompletedAt = now;
            attempt.IsSuccess = true;
            
            delivery.Status = NotificationStatus.Sent;
            delivery.SentAt = now;
            delivery.NextAttemptAt = null;

            logger.LogInformation(
                "Notification {NotificationId} successfully sent. Type: {NotificationType}, Recipient: {Recipient}.",
                delivery.Id, delivery.Type, delivery.Recipient);
        }
        catch (Exception exception)
        {
            var now = DateTime.UtcNow;

            attempt.CompletedAt = now;
            attempt.IsSuccess = false;
            attempt.Error = exception.ToString();

            if (delivery.AttemptCount >= 3)
            {
                delivery.Status = NotificationStatus.Failed;
                delivery.NextAttemptAt = null;
            }
            else
            {
                delivery.Status = NotificationStatus.Pending;
                delivery.NextAttemptAt = now.AddMinutes(1);
            }

            logger.LogError(exception, "Failed to send notification {NotificationId}. Attempt: {AttemptCount}.", delivery.Id, delivery.AttemptCount);
        }

        await dataContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task SendEmailConfirmationAsync(NotificationDelivery delivery, IEmailService emailService, IEmailTemplateService templateService, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.Deserialize<EmailConfirmationPayload>(delivery.PayloadJson)
            ?? throw new InvalidOperationException($"Unable to deserialize notification payload '{delivery.Id}'.");

        var html = await templateService.RenderAsync("email-confirmation", new { payload.Code }, cancellationToken);

        await emailService.SendAsync(new EmailMessage
            {
                To = delivery.Recipient,
                Subject = "Подтвердите почту",
                HtmlContent = html,
            }, cancellationToken);
    }
}