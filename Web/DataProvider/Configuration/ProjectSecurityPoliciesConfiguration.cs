using DataProvider.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.Configuration
{
    class ProjectSecurityPoliciesConfiguration : IEntityTypeConfiguration<ProjectSecurityPolicy>
    {
        public void Configure(EntityTypeBuilder<ProjectSecurityPolicy> builder)
        {
            builder.HasKey(x => new { x.Id, x.ProjectSettingsId, x.UserId });

            builder
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne(x => x.ProjectSettings)
                .WithMany(x => x.Policies)
                .HasForeignKey(x => x.ProjectSettingsId);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Policies)
                .HasForeignKey(x => x.UserId);
        }
    }
}
