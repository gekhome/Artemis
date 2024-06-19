namespace Artemis.Dal.Services
{
    internal class MusicStudioService : BaseRepository<MusicStudio>, IMusicStudioService
    {
        public MusicStudioService(ArtemisDbContext context) : base(context)
        {
        }

        public IEnumerable<MusicStudio> Read()
        {
            return GetAll().OrderBy(x => x.Name).ToList();
        }
    }
}
