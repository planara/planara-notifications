using Planara.Common.Database.Domain;

namespace Planara.Notifications.Data.Domain;

/// <summary>
/// Отдельная попытка доставки уведомления получателю
/// </summary>
public class NotificationAttempt : BaseEntity
{
    /// <summary>
    /// ID уведомления, для которого выполнялась попытка отправки
    /// </summary>
    public Guid NotificationDeliveryId { get; set; }

    /// <summary>
    /// Дата и время начала попытки отправки
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// Дата и время завершения попытки отправки
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Результат попытки отправки
    /// </summary>
    public bool? IsSuccess { get; set; }

    /// <summary>
    /// Описание ошибки, возникшей при отправке уведомления
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Уведомление, к которому относится попытка отправки
    /// </summary>
    public NotificationDelivery NotificationDelivery { get; set; } = null!;
}