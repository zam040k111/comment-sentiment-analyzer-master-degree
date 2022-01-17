using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.EntityConfiguration
{
    public class GamePlatformTypeConfiguration : IEntityTypeConfiguration<GamePlatformType>
    {
        public void Configure(EntityTypeBuilder<GamePlatformType> builder)
        {
            builder.HasKey(i => new {i.PlatformTypeId, i.GameId});

            builder
                .HasOne(sc => sc.Game)
                .WithMany(s => s.GamePlatformTypes)
                .HasForeignKey(sc => sc.GameId);

            builder
                .HasOne(sc => sc.PlatformType)
                .WithMany(c => c.GamePlatformTypes)
                .HasForeignKey(sc => sc.PlatformTypeId);
        }
    }
}
