namespace ASP.Core.Results
{
    public abstract class Error
    {
        public abstract string ErrorType { get; }
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

        public static Error Invalid(string message)
        {
            return new ValidationError(message);
        }

        public static Error NotAllowed(string message)
        {
            return new NotAllowedError(message);
        }

        public override string ToString() => $"{ErrorType}: {Message}";
    }
}
