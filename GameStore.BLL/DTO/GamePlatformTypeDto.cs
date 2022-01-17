
namespace GameStore.BLL.DTO
{
    public class GamePlatformTypeDto
    {
        public int GameId { get; set; }

        public GameDto Game { get; set; }
        
        public int PlatformTypeId { get; set; }

        public PlatformTypeDto PlatformType { get; set; }
    }
}
