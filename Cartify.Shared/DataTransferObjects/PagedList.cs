namespace Cartify.Shared.DataTransferObjects;

public record PagedList<T>
{
	public required IList<T> Data { get; init; }
	public required int Page { get; init; }
	public required int Limit { get; init; }
	public required int Total { get; init; }

	public int TotalPages => ( int ) Math.Ceiling(( double ) Total / Limit);
	public bool HasNextPage => Page * Limit < Total;
	public bool HasPreviousPage => Page > 1;
}