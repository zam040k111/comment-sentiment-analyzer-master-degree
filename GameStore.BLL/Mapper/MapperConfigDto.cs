using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.DAL.Entities;

namespace GameStore.BLL.Mapper
{
    public class MapperConfigDto : Profile
    {
        public MapperConfigDto()
        {
            CreateMap<GameDto, Game>()
                .ForMember(i => i.GameGenres,
                    n => n.MapFrom(x => x.GameGenresId.Select(i =>
                        new GameGenre() { GameId = x.Id, GenreId = i.Value })))
                .ForMember(i => i.GamePlatformTypes,
                    n => n.MapFrom(x => x.GamePlatformTypesId.Select(i =>
                        new GamePlatformType() { GameId = x.Id, PlatformTypeId = i.Value, })));

            CreateMap<Game, GameDto>()
                .ForMember(n => n.GameGenresId,
                    b => b.MapFrom(x => x.GameGenres.Select(p => p.GenreId).ToList()))
                .ForMember(g => g.GamePlatformTypesId,
                    x => x.MapFrom(p => p.GamePlatformTypes.Select(b => b.PlatformTypeId).ToList()));

            CreateMap<CommentDto, Comment>();
            CreateMap<Comment, CommentDto>();

            CreateMap<GenreDto, Genre>()
                .ForMember(i => i.GameGenres,
                    n => n.MapFrom(x => x.GameGenresId.Select(i =>
                        new GameGenre() { GameId = i.Value, GenreId = x.Id })));

            CreateMap<Genre, GenreDto>()
                .ForMember(n => n.GameGenresId,
                    x => x.MapFrom(b => b.GameGenres.Select(p => p.GameId).ToList()));

            CreateMap<PlatformTypeDto, PlatformType>()
                .ForMember(i => i.GamePlatformTypes,
                    n => n.MapFrom(x => x.GamePlatformTypesId.Select(i =>
                        new GamePlatformType() { GameId = i.Value, PlatformTypeId = x.Id, })));

            CreateMap<PlatformType, PlatformTypeDto>()
                .ForMember(n => n.GamePlatformTypesId,
                    x => x.MapFrom(b => b.GamePlatformTypes.Select(p => p.GameId).ToList()));

            CreateMap<GameGenre, GameGenreDto>();
            CreateMap<GameGenreDto, GameGenre>();
            CreateMap<GamePlatformType, GamePlatformTypeDto>();
            CreateMap<GamePlatformTypeDto, GamePlatformType>();
            CreateMap<PublisherDto, Publisher>();
            CreateMap<Publisher, PublisherDto>();
            CreateMap<Order, OrderDto>();

            CreateMap<OrderDto, Order>()
                .ForMember(i => i.TotalPrice,
                    p => p.MapFrom(i => i.OrderDetails.Sum(s => s.Price * s.Quantity)));

            CreateMap<OrderDetail, OrderDetailDto>();
            CreateMap<OrderDetailDto, OrderDetail>();
            CreateMap<VisaModelDto, VisaModel>();
            CreateMap<VisaModel, VisaModelDto>();
            CreateMap<GameFilterDto, GameFilterEntity>();
            CreateMap<GameFilterEntity, GameFilterDto>();
        }
    }
}
