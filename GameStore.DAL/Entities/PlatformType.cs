using System.Collections.Generic;

namespace GameStore.DAL.Entities
{
    public class PlatformType : SoftDeletable
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public ICollection<GamePlatformType> GamePlatformTypes { get; set; }

        public PlatformType()
        {
            GamePlatformTypes = new List<GamePlatformType>();
        }
    }
}
