namespace Cartify.Domain.Entities;

public class CartItem
{
	public required string Id { get; init; }
	public required int ProductId { get; set; }
	public required int Count { get; set; }
	public DateTimeOffset? UpdatedAtUtc { get; set; }
}
