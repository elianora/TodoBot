using DSharpPlus;

namespace TodoBot;

public class Worker(DiscordClient client) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        client.Logger.LogInformation("Starting TodoBot...");
        await client.ConnectAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        client.Logger.LogInformation("Stopping TodoBot...");
        await client.DisconnectAsync();
        client.Dispose();
    }
}