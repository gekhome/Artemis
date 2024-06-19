using Artemis.Dal.Interfaces.Base;

namespace Artemis.Dal.Interfaces
{
    public interface IMusicStudioService : IBaseRepository<MusicStudio>
    {
        IEnumerable<MusicStudio> Read();
    }
}