using Planara.Common.Configuration;
using Planara.Common.Database;
using Planara.Common.Host;
using Planara.Common.Kafka.Messages.Notifications;
using Planara.Kafka.Extensions;
using Planara.Notifications.Data;
using Planara.Notifications.Options;
using Planara.Notifications.Services;
using Planara.Notifications.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.AddSettingsJson();
builder.Services
    .AddHttpContextAccessor()
    // .AddCors()
    .AddLogging();

builder.Services
    .AddOptions<EmailOptions>()
    .Bind(builder.Configuration.GetSection("Email"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddScoped<IEmailService, EmailService>()
    .AddSingleton<IEmailTemplateService, EmailTemplateService>();

builder.Services.AddDataContext<DataContext>(
    builder.Configuration.GetValue<string>("DbConnections:Postgres:ConnectionString")!,
    builder.Configuration.GetValue<int>("DbConnections:Postgres:MaxRetry"),
    builder.Configuration.GetValue<int>("DbConnections:Postgres:MaxDelaySec")
);

builder.Services
    .AddKafkaConsumer<EmailConfirmationMessage>(builder.Configuration);

builder.Services
    .AddHostedService<EmailConfirmationKafkaConsumerWorker>()
    .AddHostedService<NotificationDeliveryWorker>();

var app = builder.Build();

app.PrepareAndRun<DataContext>(args);