using System;
using System.Collections.Generic;
using GameStore.DAL.Services.Filters;

namespace GameStore.BLL.DTO
{
    public class GameFilterDto
    {
        public List<int?> Genres { get; set; }
        
        public List<int?> Platforms { get; set; }
        
        public List<int?> Publishers { get; set; }
        
        public SortBy SortBy { get; set; }
        
        public decimal PriceFrom { get; set; }
        
        public decimal PriceTo { get; set; }
        
        public TimeSpan Published { get; set; }
        
        public string Name { get; set; }

        public List<GameDto> GamesPerPage { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int TotalItems { get; set; }
    }
}
