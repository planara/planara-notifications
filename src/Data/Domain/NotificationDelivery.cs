using System.ComponentModel.DataAnnotations;
using Planara.Common.Database.Domain;
using Planara.Notifications.Data.Enums;

namespace Planara.Notifications.Data.Domain;

/// <summary>
/// Уведомление, ожидающее отправки или уже обработанное сервисом уведомлений
/// </summary>
public class NotificationDelivery : BaseEntity
{
    /// <summary>
    /// ID исходного события, по которому создано уведомление
    /// </summary>
    public Guid EventId { get; set; }

    /// <summary>
    /// Тип уведомления
    /// </summary>
    public NotificationType Type { get; set; }

    /// <summary>
    /// Канал доставки уведомления
    /// </summary>
    public NotificationChannel Channel { get; set; }

    /// <summary>
    /// Получатель уведомления
    /// </summary>
    [MaxLength(320)]
    public string Recipient { get; set; } = null!;

    /// <summary>
    /// Данные, необходимые для формирования содержимого уведомления, сериализованные в JSON
    /// </summary>
    public string PayloadJson { get; set; } = null!;

    /// <summary>
    /// Текущий статус доставки уведомления
    /// </summary>
    public NotificationStatus Status { get; set; }

    /// <summary>
    /// Количество выполненных попыток отправки уведомления
    /// </summary>
    public int AttemptCount { get; set; }

    /// <summary>
    /// Дата и время, после которых разрешена следующая попытка отправки
    /// </summary>
    public DateTime? NextAttemptAt { get; set; }

    /// <summary>
    /// Дата и время успешной отправки уведомления
    /// </summary>
    public DateTime? SentAt { get; set; }
    
    /// <summary>
    /// История попыток отправки уведомления
    /// </summary>
    public ICollection<NotificationAttempt> Attempts { get; set; } = [];
}