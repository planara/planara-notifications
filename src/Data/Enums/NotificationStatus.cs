namespace Planara.Notifications.Data.Enums;

/// <summary>
/// Статус отправки уведомления
/// </summary>
public enum NotificationStatus
{
    /// <summary>
    /// Ожидание
    /// </summary>
    Pending,
    
    /// <summary>
    /// Отправляется
    /// </summary>
    Processing,
    
    /// <summary>
    /// Отправлено
    /// </summary>
    Sent,
    
    /// <summary>
    /// Ошибка
    /// </summary>
    Failed
}