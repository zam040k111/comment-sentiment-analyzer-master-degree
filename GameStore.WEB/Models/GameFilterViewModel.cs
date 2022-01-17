using System;
using System.Collections.Generic;
using GameStore.WEB.Interfaces;

namespace GameStore.WEB.Models
{
    public class GameFilterViewModel : IPagenator
    {
        public List<int?> Genres { get; set; }
        
        public List<int?> Platforms { get; set; }
        
        public List<int?> Publishers { get; set; }
        
        public int SortBy { get; set; }
        
        public decimal PriceFrom { get; set; }
        
        public decimal PriceTo { get; set; }
        
        public TimeSpan Published { get; set; }
        
        public string Name { get; set; }

        public GameViewModel GameItems { get; set; } = new GameViewModel();

        public List<GameViewModel> GamesPerPage { get; set; }

        public int PageNumber { get; set; } = 1;
        
        public int PageSize { get; set; } = 10;

        public int TotalItems { get; set; }

        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
    }
}
