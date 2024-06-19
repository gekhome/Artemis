namespace Artemis.Dal.Services
{
    public class MusicGenreService : BaseRepository<MusicGenre>, IMusicGenreService
    {
        public MusicGenreService(ArtemisDbContext context) : base(context)
        { }

        public IEnumerable<MusicGenre> Read()
        {
            return GetAll().OrderBy(x => x.GenreName).ToList();
        }
    }
}
