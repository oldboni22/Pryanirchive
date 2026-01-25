namespace Common.Pagination;

public sealed record PaginationParameters(
    int PageNumber = PaginationConstants.DefaultPageNumber, int PageSize = PaginationConstants.DefaultPageSize);
