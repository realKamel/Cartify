namespace Cartify.Domain.Exceptions;

public abstract class AppBaseException(string message) : Exception(message)
{
}