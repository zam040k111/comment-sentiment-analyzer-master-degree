
namespace GameStore.DAL.Entities
{
    public class GamePlatformType
    {
        public int GameId { get; set; }

        public Game Game { get; set; }

        public int PlatformTypeId { get; set; }

        public PlatformType PlatformType { get; set; }
    }
}
