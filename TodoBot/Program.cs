using DSharpPlus;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;
using TodoBot;
using TodoBot.Commands;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContextFactory<TodoDbContext>(options =>
{
    options.UseInMemoryDatabase("TodoDb");
});

builder.Services.AddSingleton(serviceProvider =>
{
    var discordConfig = new DiscordConfiguration
    {
        Token = builder.Configuration["DiscordBotToken"],
        TokenType = TokenType.Bot,
        Intents = DiscordIntents.AllUnprivileged,
        MinimumLogLevel = LogLevel.Information,
        LogTimestampFormat = "MMM dd yyyy - hh:mm:ss tt"
    };

    var discordClient = new DiscordClient(discordConfig);
    var slashCommandsConfig = new SlashCommandsConfiguration
    {
        Services = serviceProvider
    };

    var slashCommands = discordClient.UseSlashCommands(slashCommandsConfig);
    slashCommands.RegisterCommands<TodoCommands>();

    return discordClient;
});

var host = builder.Build();
host.Run();
