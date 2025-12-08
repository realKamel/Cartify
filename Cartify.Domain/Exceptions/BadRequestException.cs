namespace Cartify.Domain.Exceptions;

public abstract class BadRequestException(string message) : AppBaseException(message)
{
}
