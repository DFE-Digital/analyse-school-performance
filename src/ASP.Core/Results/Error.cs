namespace ASP.Core.Results
{
    public abstract class Error
    {
        public string Message { get; }

        public Error(string message)
        {
            Message = message;
        }

        public static Error NotFound(string message)
        {
            return new NotFoundError(message);
        }

        public static Error Unexpected(string message)
        {
            return new UnexpectedError(message);
        }

        public static Error Validation(string message)
        {
            return new ValidationError(message);
        }
    }
}
