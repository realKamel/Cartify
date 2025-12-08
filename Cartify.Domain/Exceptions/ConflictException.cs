namespace Cartify.Domain.Exceptions;

public abstract class ConflictException(string message) : AppBaseException(message)
{
}