namespace DailyTaskTracker.Application.DTOs.Common;

/// <summary>
/// Generic paginated envelope for returning paginated API responses.
/// </summary>
public record PagedResponse<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage
)
{
    public static PagedResponse<T> Create(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        return new PagedResponse<T>(
            items,
            totalCount,
            pageNumber,
            pageSize,
            totalPages,
            pageNumber > 1,
            pageNumber < totalPages
        );
    }
}
