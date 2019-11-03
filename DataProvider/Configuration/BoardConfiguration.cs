using DataProvider.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProvider.Configuration
{
    public class BoardConfiguration : IEntityTypeConfiguration<Board>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            builder
                .HasOne(x => x.Settings)
                .WithOne(x => x.Board)
                .HasForeignKey<BoardSettings>(x => x.Id);
        }
    }
}
