namespace Artemis.Dal.Interfaces
{
    public interface IMusicGenreService : IBaseRepository<MusicGenre>
    {
        IEnumerable<MusicGenre> Read();
    }
}