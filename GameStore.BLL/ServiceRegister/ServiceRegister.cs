using GameStore.BLL.Interfaces;
using GameStore.BLL.Logger;
using GameStore.BLL.Services;
using GameStore.DAL;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Logger;
using GameStore.DAL.Northwind;
using GameStore.ML.Interfaces;
using GameStore.ML.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.BLL.ServiceRegister
{
    public static class ServiceRegister
    {
        public static void Register(IServiceCollection services, string dbConnectionString, string mongoConnection)
        {
            services.AddDbContext<GameStoreContext>(i => i.UseSqlServer(dbConnectionString));

            services.AddSingleton<INLog, NLogger>();
            services.AddSingleton(new NorthwindContext(mongoConnection));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IPlatformTypeService, PlatformTypeService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPublisherService, PublisherService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMongoLogger, MongoLogger>();
            services.AddScoped<ISentimentService, SentimentService>();
        }
    }
}
