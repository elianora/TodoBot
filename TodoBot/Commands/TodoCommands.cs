using DSharpPlus;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;
using TodoBot.Entities;

namespace TodoBot.Commands;

public class TodoCommands(IDbContextFactory<TodoDbContext> dbFactory) : ApplicationCommandModule
{
    [SlashCommand("add-todo", "Adds a todo for the current user")]
    public async Task AddTodoAsync(InteractionContext context)
    {
        using var db = dbFactory.CreateDbContext();
        db.Todos.Add(new Todo
        {
            DiscordUserId = context.User.Id,
            Description = "Test"
        });

        db.SaveChanges();

        await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
        {
            Content = "Response!"
        });
    }

    [SlashCommand("get-todos", "Retrieves a list of todos for the current user")]
    public async Task GetTodosAsync(InteractionContext context)
    {
        using var db = dbFactory.CreateDbContext();
        var todos = db.Todos.Where(todo => todo.DiscordUserId == context.User.Id).ToList();
        
        await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
        {
            Content = "Response!"
        });
    }
}