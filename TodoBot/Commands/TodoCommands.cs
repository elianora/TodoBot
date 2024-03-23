using DSharpPlus;
using DSharpPlus.Entities;
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
        using (var db = await dbFactory.CreateDbContextAsync())
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

    [SlashCommand("list-todos", "Retrieves a list of todos for the current user")]
    public async Task GetTodosAsync(InteractionContext context)
    {
        var responseBuilder = new StringBuilder();
        using (var db = await dbFactory.CreateDbContextAsync())
        {
            var todos = db.Todos.Where(todo => todo.DiscordUserId == context.User.Id).ToList();
            foreach (var todo in todos)
            {
                responseBuilder.AppendLine($"- {todo.Description}");
            }
        }

        var response = responseBuilder.ToString();
        if (string.IsNullOrWhiteSpace(response))
        {
            response = "You have no todos!";
        }

        await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
        {
            Content = response
        });
    }

    [SlashCommand("mark-todo-done", "Deletes a todo for the current user")]
    public async Task MarkTodoDoneAsync(InteractionContext context)
    {
        using (var db = await dbFactory.CreateDbContextAsync())
        {
            var todos = db.Todos.Where(todo => todo.DiscordUserId == context.User.Id).ToList();
            if (todos.Count == 0)
            {
                await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
                {
                    Content = "You have no todos!"
                });
            }
            else
            {
                var selectOptions = todos.Select(todo => new DiscordSelectComponentOption(todo.Description, $"{todo.TodoId}")).ToList();
                var select = new DiscordSelectComponent("todo_delete_dropdown", "Select a todo to mark as completed...", selectOptions);
                var messageBuilder = new DiscordInteractionResponseBuilder().AddComponents(select);
                await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, messageBuilder);
            }
        }
    }
}