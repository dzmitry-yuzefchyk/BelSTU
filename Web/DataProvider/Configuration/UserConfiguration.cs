using DataProvider.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasOne(x => x.Settings)
                .WithOne(x => x.User)
                .HasForeignKey<UserSettings>(x => x.Id);

            builder
                .HasOne(x => x.Profile)
                .WithOne(x => x.User)
                .HasForeignKey<UserProfile>(x => x.Id);
        }
    }
}
