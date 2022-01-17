using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.DAL.Entities;
using GameStore.DAL.Northwind.Entities;

namespace GameStore.BLL.Mapper
{
    public class MapperConfigSynchronizerDb : Profile
    {
        public MapperConfigSynchronizerDb()
        {
            CreateMap<Game, Product>()
                .ForMember(i => i.Id,
                    p => p.MapFrom(i => i.Id.ToString()))
                .ForMember(n => n.CategoryId,
                    b => b.MapFrom(x => x.GameGenres.FirstOrDefault().GenreId))
                .ForMember(g => g.SupplierId,
                    x => x.MapFrom(p => p.PublisherId.Value));

            CreateMap<Product, Game>()
                .ForMember(i => i.Id,
                    p => p.MapFrom(i => i.ProductId))
                .ForMember(i => i.GameGenres,
                    n => n.MapFrom(x =>
                        new List<GameGenre> { new GameGenre { GameId = x.ProductId, GenreId = x.CategoryId } }))
                .ForMember(i => i.PublisherId,
                    p => p.MapFrom(i => i.SupplierId));

            CreateMap<Genre, Category>()
                .ForMember(i => i.Id,
                    p => p.MapFrom(i => i.Id.ToString()));
            CreateMap<Category, Genre>()
                .ForMember(i => i.Id,
                    p => p.MapFrom(i => i.CategoryId));

            CreateMap<Order, Orders>()
                .ForMember(i => i.Id,
                    p => p.MapFrom(i => i.Id.ToString()));
            CreateMap<Orders, Order>()
                .ForMember(i => i.Id,
                    p => p.MapFrom(i => i.OrderId));

            CreateMap<OrderDetail, OrderDetails>()
                .ForMember(i => i.Id,
                    p => p.MapFrom(i => i.Id.ToString()));
            CreateMap<OrderDetails, OrderDetail>()
                .ForMember(i => i.Id,
                    p => p.MapFrom(i => 0));

            CreateMap<Publisher, Supplier>()
                .ForMember(i => i.Id,
                    p => p.MapFrom(i => i.Id.ToString()));
            CreateMap<Supplier, Publisher>()
                .ForMember(i => i.Id,
                    p => p.MapFrom(i => i.SupplierId));
        }
    }
}
