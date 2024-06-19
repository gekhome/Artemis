namespace Artemis.Dal.Services
{
    internal class MovieStudioService : BaseRepository<MovieStudio>, IMovieStudioService
    {
        public MovieStudioService(ArtemisDbContext context) : base(context)
        {
        }

        public IEnumerable<MovieStudio> Read()
        {
            return GetAll().OrderBy(x => x.Name).ToList();
        }
    }
}
