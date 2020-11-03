using Chat.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.Server.Repositories.Base
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private ApplicationContext _context;
        protected DbSet<T> _dbSet;

        public BaseRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T item)
        {
            _dbSet.Add(item);
            SaveChanges();
        }

        public void Delete(long id)
        {
            T item = _dbSet.Find(id);
            _dbSet.Remove(item);
            SaveChanges();
        }

        public T Get(long id)
        {
            return _dbSet.Find(id);
        }

        public void Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
