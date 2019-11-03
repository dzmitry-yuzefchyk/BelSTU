using DataProvider.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.Configuration
{
    class ProjectSecurityPoliciesConfiguration : IEntityTypeConfiguration<ProjectSecurityPolicy>
    {
        public void Configure(EntityTypeBuilder<ProjectSecurityPolicy> builder)
        {
            builder.HasKey(x => new { x.ProjectSecuritySettingsId, x.UserId });

            builder
                .HasOne(x => x.ProjectSecuritySettings)
                .WithMany(x => x.Policies)
                .HasForeignKey(x => x.ProjectSecuritySettingsId);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Policies)
                .HasForeignKey(x => x.UserId);
        }
    }
}
