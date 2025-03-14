using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Data
{
    public partial class TodoListContext(DbContextOptions<TodoListContext> options) : DbContext(options)
    {
        public virtual DbSet<Core.Entities.Task>? Tasks { get; set; }
        public virtual DbSet<State>? States { get; set; }
        public virtual DbSet<TaskHistory>? TasksHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Core.Entities.Task>()
                .HasKey(c => c.Id);  // Define Id as the primary key
        }
    }
}
