using DSharpPlus;
using DSharpPlus.SlashCommands;

namespace TodoBot.Commands;

public class TodoCommands(TodoDbContext db) : ApplicationCommandModule
{
    [SlashCommand("get-todos", "Retrieves a list of todos for the current user")]
    public async Task GetTodosAsync(InteractionContext context)
    {
        var todos = db.Todos.Where(todo => todo.DiscordUserId == context.User.Id).ToList();
        await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
        {
            Content = "Response!"
        });
    }
}