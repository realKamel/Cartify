namespace Cartify.Domain.Exceptions;

public sealed class DuplicateEmailException(string message) : AppBaseException(message)
{
}