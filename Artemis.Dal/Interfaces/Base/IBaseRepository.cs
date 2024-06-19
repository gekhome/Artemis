using System.Linq.Expressions;

namespace Artemis.Dal.Interfaces.Base
{
    public interface IBaseRepository<T> where T : class
    {
        DbSet<T> Table { get; }

        int Add(T entity);
        int AddRange(IEnumerable<T> entities);
        int Delete(T entity);
        int DeleteRange(IEnumerable<T> entities);
        IEnumerable<T> ExecuteSqlString(string sql);
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        IEnumerable<T> GetAll();
        T? GetById(int id);
        int Update(T entity);
        int UpdateRange(IEnumerable<T> entities);
    }
}