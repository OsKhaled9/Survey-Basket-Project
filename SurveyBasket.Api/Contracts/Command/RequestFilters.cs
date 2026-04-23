namespace SurveyBasket.Api.Contracts.Command;

public record RequestFilters
{
    // Pagination
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    // Filtering
    public string? SearchValue { get; init; }

    // Sorting
    public string? SortColumn { get; init; }
    public string? SortDirection { get; init; } = "ASC";
}
