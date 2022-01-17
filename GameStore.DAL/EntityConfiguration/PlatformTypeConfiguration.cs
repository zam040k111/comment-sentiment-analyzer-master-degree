using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.EntityConfiguration
{
    public class PlatformTypeConfiguration : IEntityTypeConfiguration<PlatformType>
    {
        public void Configure(EntityTypeBuilder<PlatformType> builder)
        {
            builder
                .HasMany(n => n.GamePlatformTypes)
                .WithOne(n => n.PlatformType);
        }
    }
}
