using Microsoft.EntityFrameworkCore;

namespace TodoBot.Entities;

[Index(nameof(TodoId), IsUnique = true)]
[Index(nameof(TodoId), nameof(DiscordUserId))]
public class Todo
{
    public ulong TodoId { get; set; }
    public ulong DiscordUserId { get; set; }
    public required string Description { get; set; }
}
