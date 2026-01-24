namespace Common.Pagination;

public record PaginationParameters(
    int PageNumber = PaginationConstants.DefaultPageNumber, int PageSize = PaginationConstants.DefaultPageSize);
