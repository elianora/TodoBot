using DSharpPlus.EventArgs;
using DSharpPlus;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace TodoBot.Handlers;

public class InteractionHandler
{
    public static IDbContextFactory<TodoDbContext>? DbFactory { get; set; }

    public static async Task OnComponentInteraction(DiscordClient client, ComponentInteractionCreateEventArgs args)
    {
        if (args.Id == "todo_delete_dropdown")
        {
            var todoId = ulong.Parse(args.Interaction.Data.Values.Single());
            if (DbFactory is not null)
            {
                using (var db = await DbFactory.CreateDbContextAsync())
                {
                    var todo = await db.Todos.FindAsync(todoId);
                    if (todo is not null)
                    {
                        db.Remove(todo);
                        await db.SaveChangesAsync();
                        await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new()
                        {
                            Content = "Todo deleted!"
                        });
                    }
                }
            }
        }
    }
}