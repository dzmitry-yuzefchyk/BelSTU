using DataProvider.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.Configuration
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder
                .HasOne(x => x.Creator)
                .WithMany(x => x.CreatedTasks)
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x => x.Assignee)
                .WithMany(x => x.AssignedTasks)
                .HasForeignKey(x=>x.AssigneeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(x => x.Comments)
                .WithOne(x => x.Task)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
