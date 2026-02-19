namespace Common.Pagination;

public static class PaginationExtensions
{
    extension<T>(List<T> list)
    {
        public PagedList<T> ToPagedList(int pageNumber, int pageSize,  int totalCount)
        {
            pageSize = pageSize <= 0 ? PaginationConstants.DefaultPageSize : pageSize;
            
            var pageCount = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedList<T>(list, pageNumber, pageSize, pageCount, totalCount);
        }
    }
}
