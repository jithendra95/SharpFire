namespace SharpFire.Database.Exceptions;

public class NoDataFoundException : Exception
{
    public NoDataFoundException()
    {
    }

    public NoDataFoundException(string message)
        : base(message)
    {
    }

    public NoDataFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}