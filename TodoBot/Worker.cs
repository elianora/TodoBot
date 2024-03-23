using DSharpPlus;

namespace TodoBot;

public class Worker(DiscordClient client) : BackgroundService
{
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await client.ConnectAsync();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await client.DisconnectAsync();
        client.Dispose();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
        => Task.CompletedTask;
}