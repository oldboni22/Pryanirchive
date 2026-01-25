namespace Common.Pagination;

public record PagedList<T>(List<T> Items, int PageNumber, int PageSize, int PageCount, int TotalCount)
{
    public bool HasPreviousPage => PageNumber > 1;
    
    public bool HasNextPage => PageNumber < TotalCount;
}
