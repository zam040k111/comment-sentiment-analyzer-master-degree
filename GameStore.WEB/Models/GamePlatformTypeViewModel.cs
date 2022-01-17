
namespace GameStore.WEB.Models
{
    public class GamePlatformTypeViewModel
    {
        public int GameId { get; set; }

        public GameViewModel Game { get; set; }

        public int PlatformTypeId { get; set; }

        public PlatformTypeViewModel PlatformType { get; set; }
    }
}
