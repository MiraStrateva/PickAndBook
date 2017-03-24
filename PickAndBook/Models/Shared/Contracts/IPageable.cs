namespace PickAndBook.Models.Shared.Contracts
{
    public interface IPageable
    {
        int TotalCount { get; set; }

        int PageSize { get; set; }

        int CurrentPage { get; set; }
    }
}
