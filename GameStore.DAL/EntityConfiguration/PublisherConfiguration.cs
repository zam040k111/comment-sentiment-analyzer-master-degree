using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.EntityConfiguration
{
    public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.Property(i => i.CompanyName).HasMaxLength(40);
            builder.Property(i => i.Description).HasColumnType("Ntext");
            builder.Property(i => i.HomePage).HasColumnType("Ntext");
        }
    }
}
