
namespace GameStore.BLL.DTO
{
    public class GameGenreDto
    {
        public int GameId { get; set; }

        public GameDto Game { get; set; }

        public int GenreId { get; set; }

        public GenreDto Genre { get; set; }
    }
}
