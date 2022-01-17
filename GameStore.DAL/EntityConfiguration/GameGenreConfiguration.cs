using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.EntityConfiguration
{
    public class GameGenreConfiguration : IEntityTypeConfiguration<GameGenre>
    {
        public void Configure(EntityTypeBuilder<GameGenre> builder)
        {
            builder.HasKey(i => new {i.GameId, i.GenreId});

            builder
                .HasOne(sc => sc.Game)
                .WithMany(s => s.GameGenres)
                .HasForeignKey(sc => sc.GameId);

            builder
                .HasOne(sc => sc.Genre)
                .WithMany(c => c.GameGenres)
                .HasForeignKey(sc => sc.GenreId);
        }
    }
}
