namespace Cartify.Domain.Exceptions;

public sealed class CategoryNotFoundException(string message) : NotFoundException(message)
{
}