namespace Artemis.Dal.Services
{
    public class MovieGenreService : BaseRepository<MovieGenre>, IMovieGenreService
    {
        public MovieGenreService(ArtemisDbContext context) : base(context)
        { }

        public IEnumerable<MovieGenre> Read()
        {
            return GetAll().OrderBy(x => x.GenreName).ToList();
        }
    }
}
