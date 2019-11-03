using Microsoft.EntityFrameworkCore;
using DataProvider.Entities;
using System.Reflection;

namespace DataProvider
{
    class TaskboardContext : DbContext
    {
        public TaskboardContext(DbContextOptions<TaskboardContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectSettings> ProjectSettings { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<BoardSettings> BoardSettings { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
    }
}
