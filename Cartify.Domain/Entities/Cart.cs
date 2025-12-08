namespace Cartify.Domain.Entities;

public class Cart
{
	public required string Id { get; set; }
	public required IList<CartItem> CartItems { get; set; }
	public DateTimeOffset? UpdatedAtUtc { get; set; }
}
