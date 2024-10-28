namespace NureTimetableAPI.Exceptions;

public class SQLRepositoryException : Exception
{
    public SQLRepositoryException()
    {
    }

    public SQLRepositoryException(string message) : base(message)
    {
    }

    public SQLRepositoryException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
