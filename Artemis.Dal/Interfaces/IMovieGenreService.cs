namespace Artemis.Dal.Interfaces
{
    public interface IMovieGenreService : IBaseRepository<MovieGenre>
    {
        IEnumerable<MovieGenre> Read();
    }
}