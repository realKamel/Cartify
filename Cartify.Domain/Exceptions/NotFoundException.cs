namespace Cartify.Domain.Exceptions;

public abstract class NotFoundException(string message) : ApplicationException(message)
{
}