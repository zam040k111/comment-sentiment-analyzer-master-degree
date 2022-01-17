using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities
{
    public abstract class SoftDeletable : ISoftDeletable
    {
        public bool IsDeleted { get; set; }
    }
}
