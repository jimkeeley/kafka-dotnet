var builder = Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

//TODO: Add OpenTelemetry (for App Insights, Grafana, etc)
builder.ConfigureLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddConsole();
});

using IHost host = builder.Build();

var serviceProvider = host.Services.GetService<IServiceProvider>();

//need to resolve and start Message bus For Consumer
var bus = serviceProvider.CreateKafkaBus();

await bus.StartAsync();

//block console from exiting so consumer remains 'listening'
Console.Read();
