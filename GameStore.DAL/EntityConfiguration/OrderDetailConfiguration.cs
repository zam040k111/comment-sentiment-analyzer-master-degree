
using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.EntityConfiguration
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.Property(p => p.Price).HasColumnType("Money");
            builder.Property(p => p.Discount).HasColumnType("Real");

            builder
                .HasOne(i => i.Order)
                .WithMany(i => i.OrderDetails);
        }
    }
}
