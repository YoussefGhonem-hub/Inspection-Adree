namespace Inspection.Application.Exceptions
{
    public class BaseException : Exception
    {
        public int StatusCode { get; protected set; }

        public BaseException()
            : base()
        {
        }

        public BaseException(string message)
            : base(message)
        {
        }

        public BaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public BaseException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}