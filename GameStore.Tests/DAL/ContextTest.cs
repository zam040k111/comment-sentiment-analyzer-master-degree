using GameStore.DAL;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Tests.DAL
{
    public sealed class ContextTest : GameStoreContext
    {
        public ContextTest(DbContextOptions<GameStoreContext> options) : base(options, null) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildModels(modelBuilder);
        }
    }
}
