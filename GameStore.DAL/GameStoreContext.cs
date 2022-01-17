using System;
using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using GameStore.DAL.EntityConfiguration;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Northwind.Entities;
using Newtonsoft.Json;

namespace GameStore.DAL
{
    public class GameStoreContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<PlatformType> PlatformTypes { get; set; }
        public DbSet<GamePlatformType> GamePlatformTypes { get; set; }
        public DbSet<GameGenre> GameGenres { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }

        private readonly IMongoLogger _logger;

        public GameStoreContext(DbContextOptions<GameStoreContext> options, IMongoLogger logger) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildModels(modelBuilder);

            modelBuilder.Entity<Game>(Initializer.GameSeed);
            modelBuilder.Entity<GameGenre>(Initializer.GameGenreSeed);
            modelBuilder.Entity<GamePlatformType>(Initializer.GamePlatformTypeSeed);
            modelBuilder.Entity<Genre>(Initializer.GenreSeed);
            modelBuilder.Entity<PlatformType>(Initializer.PlatformTypeSeed);
            modelBuilder.Entity<Comment>(Initializer.CommentSeed);
            modelBuilder.Entity<Publisher>(Initializer.PublisherSeed);
        }

        protected void BuildModels(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GameConfiguration());
            modelBuilder.ApplyConfiguration(new GameGenreConfiguration());
            modelBuilder.ApplyConfiguration(new GamePlatformTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GenreConfiguration());
            modelBuilder.ApplyConfiguration(new PlatformTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new PublisherConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new VisaConfiguration());
        }

        public override int SaveChanges()
        {
            if (_logger != null)
            {
                foreach (var entry in ChangeTracker.Entries())
                {
                    var entity = entry.CurrentValues.ToObject();
                    var log = new Log
                    {
                        Date = DateTime.UtcNow.ToString("F"),
                        EntityType = entity.GetType().Name,
                        Operation = entry.State.ToString(),
                        NewEntity = JsonConvert.SerializeObject(entry.CurrentValues.ToObject())
                    };

                    if (entry.State == EntityState.Modified)
                    {
                        log.OldEntity = JsonConvert.SerializeObject(entry.GetDatabaseValues().ToObject());
                    }

                    if (entry.State == EntityState.Deleted)
                    {
                        log.OldEntity = JsonConvert.SerializeObject(entry.CurrentValues.ToObject());
                        log.NewEntity = null;
                    }

                    _logger.AddLog(log);
                }
            }

            return base.SaveChanges();
        }        
    }
}
