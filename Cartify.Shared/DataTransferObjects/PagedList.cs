namespace Cartify.Shared.DataTransferObjects;

public class PagedList<T>
{
    public required IList<T> Data { get; init; }
    public int Page { get; init; }
    public int Limit { get; init; }
    public int Total { get; init; }

    public int TotalPages => (int)Math.Ceiling((double)Total / Limit);
    public bool HasNextPage => Page * Limit < Total;
    public bool HasPreviousPage => Page > 1;
}