namespace ClaimsManagementSystem.Contracts.Claims;

public class PagedResponseDto<T>
{
    public IReadOnlyCollection<T> Items { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
}
