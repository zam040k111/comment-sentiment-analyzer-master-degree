using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.EntityConfiguration
{
    public class VisaConfiguration : IEntityTypeConfiguration<VisaModel>
    {
        public void Configure(EntityTypeBuilder<VisaModel> builder)
        {
        }
    }
}
