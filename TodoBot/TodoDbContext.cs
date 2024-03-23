using Microsoft.EntityFrameworkCore;
using TodoBot.Entities;

namespace TodoBot;

public class TodoDbContext : DbContext
{
    public DbSet<Todo> Todos { get; set; }

    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options) { }
}
