using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial class TodoListContext(DbContextOptions<TodoListContext> options) : DbContext(options)
{
    public virtual DbSet<Core.Entities.Task>? Tasks { get; set; }
    public virtual DbSet<State>? States { get; set; }
    public virtual DbSet<TaskHistory>? TasksHistory { get; set; }
    public virtual DbSet<User>? Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Core.Entities.Task>()
            .HasKey(c => c.Id);  // Define Id as the primary key

        modelBuilder.Entity<Core.Entities.TaskHistory>()
            .HasKey(c => c.Id);  // Define Id as the primary key

        modelBuilder.Entity<Core.Entities.State>()
    .       HasKey(c => c.Id);  // Define Id as the primary key

        modelBuilder.Entity<Core.Entities.User>()
    .       HasKey(c => c.Id);  // Define Id as the primary key
    }
}
