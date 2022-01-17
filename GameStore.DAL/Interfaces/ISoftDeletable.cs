namespace GameStore.DAL.Interfaces
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }
}
