using DataProvider.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.Configuration
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder
                .HasOne(x => x.Settings)
                .WithOne(x => x.Project)
                .HasForeignKey<ProjectSettings>(x => x.Id);

            builder
                .HasOne(x => x.ProjectSecuritySettings)
                .WithOne(x => x.Project)
                .HasForeignKey<ProjectSecuritySettings>(x => x.Id);
        }
    }
}
