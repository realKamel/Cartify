namespace Cartify.Domain.Exceptions;

public class ProductOutOfStockException : ConflictException
{
	public int ProductId { get; }

	public ProductOutOfStockException(int productId, string message)
		: base(message)
	{
		ProductId = productId;
	}
}
