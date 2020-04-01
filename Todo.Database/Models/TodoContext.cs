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

        public virtual DbSet<RememberItems> RememberItems { get; set; }
        public virtual DbSet<TodoItems> TodoItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=Todo;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RememberItems>(entity =>
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

            modelBuilder.Entity<TodoItems>(entity =>
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
