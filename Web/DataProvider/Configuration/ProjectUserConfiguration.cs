using DataProvider.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.Configuration
{
    public class ProjectUserConfiguration : IEntityTypeConfiguration<ProjectUser>
    {
        public void Configure(EntityTypeBuilder<ProjectUser> builder)
        {
            builder.HasKey(x => new { x.ProjectId, x.UserId });

            builder
                .HasOne(x => x.Project)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.ProjectId);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Projects)
                .HasForeignKey(x => x.UserId);
        }
    }
}
