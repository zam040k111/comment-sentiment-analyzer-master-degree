using System;
using System.Collections.Generic;

namespace GameStore.BLL.DTO
{
    public class GameDto
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public short UnitsInStock { get; set; }

        public bool Discontinued { get; set; }

        public DateTime Published { get; set; }

        public int Viewed { get; set; }

        public int? PublisherId { get; set; }

        public PublisherDto Publisher { get; set; }

        public byte[] Image { get; set; }

        public byte[] SmallImage { get; set; }

        public float Score { get; set; }

        public List<int?> GameGenresId { get; set; }

        public List<GameGenreDto> GameGenres { get; set; }

        public List<int?> GamePlatformTypesId { get; set; }

        public List<GamePlatformTypeDto> GamePlatformTypes { get; set; }
    }
}
