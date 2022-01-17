
namespace GameStore.DAL.Interfaces
{
    interface IWriteOnlyMongoGenericRepository<T> where T : class
    {
        void Add(T item);
        void Update(T item);
        void Delete(T item);
    }
}
