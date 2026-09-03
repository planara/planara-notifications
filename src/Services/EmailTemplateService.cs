using Microsoft.Extensions.Options;
using Planara.Notifications.Options;
using Scriban;

namespace Planara.Notifications.Services;

public class EmailTemplateService(IOptions<EmailOptions> options) : IEmailTemplateService
{
    private readonly EmailOptions _options = options.Value;
    
    public async Task<string> RenderAsync(string templateName, object model, CancellationToken cancellationToken = default)
    {
        var templatePath = Path.Combine(_options.TemplatesPath, $"{templateName}.html");

        if (!File.Exists(templatePath))
            throw new FileNotFoundException($"Email template '{templateName}' was not found.", templatePath);

        var content = await File.ReadAllTextAsync(templatePath, cancellationToken);

        var template = Template.Parse(content);

        if (template.HasErrors)
        {
            var errors = string.Join(Environment.NewLine, template.Messages.Select(x => x.Message));

            throw new InvalidOperationException(
                $"Email template '{templateName}' contains errors:{Environment.NewLine}{errors}");
        }

        return await template.RenderAsync(model);
    }
}