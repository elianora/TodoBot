using DSharpPlus;

namespace TodoBot;

public class Worker(DiscordClient client) : BackgroundService
{
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        client.Logger.LogInformation("Starting TodoBot...");
        await client.ConnectAsync();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        client.Logger.LogInformation("Stopping TodoBot...");
        await client.DisconnectAsync();
        client.Dispose();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
        => Task.CompletedTask;
}