namespace Artemis.Dal.Interfaces
{
    public interface ILanguageService : IBaseRepository<Languages>
    {
        IEnumerable<Languages> Read();
    }
}