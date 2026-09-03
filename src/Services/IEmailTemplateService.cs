namespace Planara.Notifications.Services;

public interface IEmailTemplateService
{
    /// <summary>
    /// Формирует содержимое email по указанному шаблону
    /// </summary>
    Task<string> RenderAsync(string templateName, object model, CancellationToken cancellationToken = default);
}