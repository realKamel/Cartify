namespace Cartify.Domain.Exceptions;

public sealed class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException() : base($"No Product was Found")
    {
    }

    public ProductNotFoundException(int id) : base($"No Product was found with id: {id}")
    {
    }
}