using DataProvider.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.Configuration
{
    class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasKey(x => new { x.UserId, x.ProjectId });

            builder
                .HasOne(x => x.Project)
                .WithMany(x => x.Activities)
                .HasForeignKey(x => x.ProjectId);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Activities)
                .HasForeignKey(x => x.UserId);
        }
    }
}
