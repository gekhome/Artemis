namespace Artemis.Dal.Interfaces
{
    public interface IYearService : IBaseRepository<Years>
    {
        void Initialize();
        IEnumerable<Years> Read();
    }
}