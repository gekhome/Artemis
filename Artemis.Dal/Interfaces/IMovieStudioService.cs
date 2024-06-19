namespace Artemis.Dal.Interfaces
{
    public interface IMovieStudioService : IBaseRepository<MovieStudio>
    {
        IEnumerable<MovieStudio> Read();
    }
}