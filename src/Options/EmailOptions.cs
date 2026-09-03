namespace Planara.Notifications.Options;

public class EmailOptions
{
    /// <summary>
    /// SMTP-сервер
    /// </summary>
    public required string SmtpServer { get; init; }

    /// <summary>
    /// Порт SMTP-сервера
    /// </summary>
    public int Port { get; init; }

    /// <summary>
    /// Логин для авторизации на SMTP-сервере
    /// </summary>
    public required string Username { get; init; }

    /// <summary>
    /// Пароль для авторизации на SMTP-сервере
    /// </summary>
    public required string Password { get; init; }

    /// <summary>
    /// Адрес отправителя
    /// </summary>
    public required string FromAddress { get; init; }

    /// <summary>
    /// Имя отправителя
    /// </summary>
    public required string FromName { get; init; }
    
    /// <summary>
    /// Путь к директории с email-шаблонами
    /// </summary>
    public required string TemplatesPath { get; init; }
}