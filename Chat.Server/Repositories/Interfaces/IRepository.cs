namespace Chat.Server.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Get(long id);
        void Add(T item);
        void Update(T item);
        void Delete(long id);
        void SaveChanges();
    }
}
