namespace GameStore.WEB.Interfaces
{
    public interface IPagenator
    {
        int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; }
    }
}
