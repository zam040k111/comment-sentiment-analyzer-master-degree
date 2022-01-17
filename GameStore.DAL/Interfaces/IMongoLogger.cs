using GameStore.DAL.Northwind.Entities;

namespace GameStore.DAL.Interfaces
{
    public interface IMongoLogger
    {
        void AddLog(Log log);
    }
}