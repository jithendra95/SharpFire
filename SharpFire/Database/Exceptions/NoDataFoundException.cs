namespace SharpFire.Database.Exceptions;

public class NoDataFoundException : Exception
{

    public NoDataFoundException(string message)
        : base(message)
    {
    }

}