using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.EntityConfiguration
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasIndex(p => p.Key).IsUnique();
            builder
                .HasMany(n => n.GameGenres)
                .WithOne(n => n.Game);
            builder
                .HasMany(b => b.GamePlatformTypes)
                .WithOne(p => p.Game);
            builder.Property(i => i.Price).HasColumnType("Money");
        }
    }
}
