namespace Cartify.Shared.DataTransferObjects.User;

public record CartResponse
{
	public required string Id { get; set; }
	public required IList<CartItemDto> CartItems { get; set; }
	public int NumOfCartItems => CartItems.Sum(p => p.Count);
	public decimal TotalCartPrice => CartItems.Sum(i => i.Price);
}
