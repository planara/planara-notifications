namespace Planara.Notifications.DTOs;

public class EmailConfirmationPayload
{
    /// <summary>
    /// Код подтверждения почты
    /// </summary>
    public required string Code { get; init; }
}