
namespace Artemis.Dal.Services.Base
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public DbSet<T> Table { get; }
        protected readonly ArtemisDbContext _context;

        public BaseRepository(ArtemisDbContext context)
        {
            _context = context;
            Table = _context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Table.AsQueryable();
        }


        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return Table.Where(expression);
        }

        public virtual T? GetById(int id)
        {
            return Table.Find(id);
        }

        public virtual int Add(T entity)
        {
            Table.Add(entity);
            return _context.SaveChanges();
        }

        public virtual int AddRange(IEnumerable<T> entities)
        {
            Table.AddRange(entities);
            return _context.SaveChanges();
        }

        public virtual int Update(T entity)
        {
            Table.Update(entity);
            return _context.SaveChanges();
        }

        public virtual int UpdateRange(IEnumerable<T> entities)
        {
            Table.UpdateRange(entities);
            return _context.SaveChanges();
        }

        public virtual int Delete(T entity)
        {
            Table.Remove(entity);
            return _context.SaveChanges();
        }

        public virtual int DeleteRange(IEnumerable<T> entities)
        {
            Table.RemoveRange(entities);
            return _context.SaveChanges();
        }

        public IEnumerable<T> ExecuteSqlString(string sql)
            => Table.FromSqlRaw(sql);
    }
}
