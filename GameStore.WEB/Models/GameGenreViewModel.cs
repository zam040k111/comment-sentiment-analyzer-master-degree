
namespace GameStore.WEB.Models
{
    public class GameGenreViewModel
    {
        public int GameId { get; set; }

        public GenreViewModel Genre { get; set; }

        public int GenreId { get; set; }

        public GameViewModel Game { get; set; }
    }
}