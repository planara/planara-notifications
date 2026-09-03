namespace Planara.Notifications.DTOs;

/// <summary>
/// Данные email-сообщения для отправки
/// </summary>
public class EmailMessage
{
    /// <summary>
    /// Получатель письма
    /// </summary>
    public required string To { get; init; }

    /// <summary>
    /// Тема письма
    /// </summary>
    public required string Subject { get; init; }

    /// <summary>
    /// HTML-содержимое письма
    /// </summary>
    public required string HtmlContent { get; init; }
}