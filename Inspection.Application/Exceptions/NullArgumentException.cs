namespace Inspection.Application.Exceptions;
public class NullArgumentException : BaseException
{
    public NullArgumentException()
       : base()
    {
    }

    public NullArgumentException(string message)
        : base(message)
    {
    }

    public NullArgumentException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

