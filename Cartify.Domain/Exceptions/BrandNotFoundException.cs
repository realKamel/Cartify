namespace Cartify.Domain.Exceptions;

public sealed class BrandNotFoundException(string message) : NotFoundException(message)
{
}