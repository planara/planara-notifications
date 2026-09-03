namespace Planara.Notifications.Data.Enums;

/// <summary>
/// Тип уведомления
/// </summary>
public enum NotificationType
{
    /// <summary>
    /// Подтверждение почты
    /// </summary>
    EmailConfirmation,
    
    /// <summary>
    /// Сброс пароля
    /// </summary>
    PasswordReset,
    
    /// <summary>
    /// Завершение регистрации
    /// </summary>
    RegistrationCompleted,
    
    /// <summary>
    /// Предупреждение о попытке входа
    /// </summary>
    SecurityAlert
}