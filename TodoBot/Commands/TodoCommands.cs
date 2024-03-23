using DSharpPlus;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TodoBot.Entities;

namespace TodoBot.Commands;

public class TodoCommands(IDbContextFactory<TodoDbContext> dbFactory) : ApplicationCommandModule
{
    [SlashCommand("add-todo", "Adds a todo for the current user")]
    public async Task AddTodoAsync(InteractionContext context, [Option("description", "Description for the todo")]string description)
    {
        using (var db = dbFactory.CreateDbContext())
        {
            db.Todos.Add(new Todo
            {
                DiscordUserId = context.User.Id,
                Description = description
            });

            db.SaveChanges();
        }

        await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
        {
            Content = "Todo added!"
        });
    }

    [SlashCommand("get-todos", "Retrieves a list of todos for the current user")]
    public async Task GetTodosAsync(InteractionContext context)
    {
        var response = new StringBuilder();
        using (var db = dbFactory.CreateDbContext())
        {
            var todos = db.Todos.Where(todo => todo.DiscordUserId == context.User.Id).ToList();
            foreach (var todo in todos)
            {
                response.AppendLine(todo.Description);
            }
        }

        await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
        {
            Content = response.ToString()
        });
    }
}