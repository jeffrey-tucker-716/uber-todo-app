using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Todo.Database.Models
{
    public partial class TodoContext : DbContext
    {
        public TodoContext()
        {
        }

        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<RememberItem> RememberItems { get; set; }
        public virtual DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Startup.cs will already have configured the context
                // public void ConfigureServices(IServiceCollection services) 
                // { services.AddDbContext<TodoContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TodoDatabase")));
                // the below snippet is only for unit testing.
                optionsBuilder.UseSqlServer("Server=.;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RememberItem>(entity =>
            {
                entity.HasKey(e => e.RememberItemId);

                entity.Property(e => e.RememberItemId).HasMaxLength(40);

                entity.Property(e => e.RememberItemTitle).HasMaxLength(255);

                entity.Property(e => e.TodoItemId)
                    .HasMaxLength(40)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.WhenConverted).HasColumnType("datetime");

                entity.Property(e => e.WhenEntered)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.TodoItem)
                    .WithMany(p => p.RememberItems)
                    .HasForeignKey(d => d.TodoItemId)
                    .HasConstraintName("FK_RememberItems_TodoItems");
            });

            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(e => e.TodoItemId);

                entity.Property(e => e.TodoItemId)
                    .HasMaxLength(40)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TodoTitle).HasMaxLength(255);

                entity.Property(e => e.WhenCompleted).HasColumnType("datetime");

                entity.Property(e => e.WhenEntered).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
