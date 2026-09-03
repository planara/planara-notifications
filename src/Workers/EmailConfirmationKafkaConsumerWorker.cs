using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Planara.Common.Kafka;
using Planara.Common.Kafka.Messages.Notifications;
using Planara.Common.Workers;
using Planara.Kafka.Interfaces;
using Planara.Notifications.Data;
using Planara.Notifications.Data.Domain;
using Planara.Notifications.Data.Enums;

namespace Planara.Notifications.Workers;

public class EmailConfirmationKafkaConsumerWorker(
    ILogger<EmailConfirmationKafkaConsumerWorker> logger,
    IKafkaConsumer<EmailConfirmationMessage> consumer,
    IServiceScopeFactory scopeFactory)
    : KafkaConsumerWorkerBase<EmailConfirmationMessage>(logger, consumer, scopeFactory)
{
    protected override string TopicKey => KafkaTopicKeys.EmailConfirmation;

    protected override async Task HandleMessage(EmailConfirmationMessage message, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var dataContext = serviceProvider.GetRequiredService<DataContext>();

        var exists = await dataContext.NotificationDeliveries
            .AnyAsync(x => x.EventId == message.Id, cancellationToken);

        if (exists)
        {
            logger.LogInformation("Notification event {Id} already processed. Skipping.", message.Id);

            return;
        }

        var now = DateTime.UtcNow;

        var delivery = new NotificationDelivery
        {
            EventId = message.Id,
            Type = NotificationType.EmailConfirmation,
            Channel = NotificationChannel.Email,
            Recipient = message.Email,
            PayloadJson = JsonSerializer.Serialize(new { message.Code, }),
            Status = NotificationStatus.Pending,
            AttemptCount = 0,
            NextAttemptAt = now,
            CreatedAt = now,
            UpdatedAt = now,
        };

        dataContext.NotificationDeliveries.Add(delivery);
        await dataContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Notification {NotificationId} created from event {EventId}.", delivery.Id, message.Id);
    }
}